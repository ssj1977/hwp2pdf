using HwpObjectLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace hwp2pdf
{
    internal static class CliRunner
    {
        private static readonly string[] TargetTypes = { "PDF", "HWP", "HWPX", "HWPML2X", "HTML+", "ODT", "OOXML", "UNICODE", "RTF" };
        private static readonly string[] TargetExts = { ".pdf", ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".txt", ".rtf" };
        private static readonly string[] SourceExts = { ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".doc", ".txt", ".rtf" };

        private enum OverwriteMode
        {
            Rename = 0,
            Skip = 1,
            Overwrite = 2
        }

        private sealed class CliOptions
        {
            public readonly List<string> Inputs = new List<string>();
            public string OutputDirectory;
            public string TargetType = "PDF";
            public OverwriteMode Overwrite = OverwriteMode.Rename;
            public bool UsePdfPrint;
            public string PrinterName = "";
            public int PrintMethod = 1;
        }

        public static bool ShouldRunCli(string[] args)
        {
            if (args == null || args.Length == 0) return false;
            foreach (string arg in args)
            {
                if (string.Equals(arg, "--gui", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public static int Run(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CliOptions options;
            string parseError;
            if (!TryParse(args, out options, out parseError))
            {
                if (!string.IsNullOrEmpty(parseError)) Console.Error.WriteLine(parseError);
                PrintUsage();
                return 1;
            }

            if (options.Inputs.Count == 0)
            {
                Console.Error.WriteLine("입력 파일 또는 폴더를 지정해 주세요.");
                PrintUsage();
                return 1;
            }

            if (!RegistryHasHancom())
            {
                Console.Error.WriteLine("한컴오피스 한글 2010 이상 버전이 설치되어 있지 않습니다.");
                return 2;
            }

            int targetIndex = GetTargetIndex(options.TargetType);
            if (targetIndex < 0)
            {
                Console.Error.WriteLine("지원하지 않는 대상 형식입니다: " + options.TargetType);
                return 1;
            }
            string targetType = TargetTypes[targetIndex];
            string targetExt = TargetExts[targetIndex];

            var files = CollectInputFiles(options.Inputs);
            if (files.Count == 0)
            {
                Console.Error.WriteLine("변환할 파일을 찾지 못했습니다.");
                return 1;
            }

            string outputDirectory = options.OutputDirectory;
            bool useCurrentPath = string.IsNullOrEmpty(outputDirectory);
            if (!useCurrentPath)
            {
                outputDirectory = Path.GetFullPath(outputDirectory);
                Directory.CreateDirectory(outputDirectory);
            }

            HwpObject hwpObject = null;
            string printerName = options.PrinterName;
            try
            {
                hwpObject = new HwpObject();
                bool filecheckdllOk = RegisterSecurityModule(hwpObject);

                if (targetType == "PDF" && options.UsePdfPrint)
                {
                    if (!TryResolvePdfPrinter(ref printerName))
                    {
                        Console.Error.WriteLine("한컴 PDF 또는 Microsoft Print to PDF 프린터를 찾지 못했습니다.");
                        return 2;
                    }
                }

                if (!filecheckdllOk)
                {
                    Console.Error.WriteLine("경고: FilePathCheckerModuleExample.DLL 연결에 실패했습니다.");
                }

                int converted = 0;
                int skipped = 0;
                int failed = 0;

                Console.WriteLine("총 {0}개 파일 변환을 시작합니다.", files.Count);
                for (int i = 0; i < files.Count; i++)
                {
                    string sourcePath = files[i];
                    string sourceExt = Path.GetExtension(sourcePath);
                    if (string.Equals(sourceExt, targetExt, StringComparison.OrdinalIgnoreCase))
                    {
                        skipped++;
                        Console.WriteLine("[{0}/{1}] 건너뜀(같은 형식): {2}", i + 1, files.Count, sourcePath);
                        continue;
                    }

                    string savePath = BuildTargetPath(sourcePath, outputDirectory, useCurrentPath, targetExt);
                    bool changedName;
                    bool shouldSkip;
                    bool overwrite;
                    ResolveOverwrite(ref savePath, options.Overwrite, outputDirectory, useCurrentPath, sourcePath, targetExt, out changedName, out shouldSkip, out overwrite);
                    if (shouldSkip)
                    {
                        skipped++;
                        Console.WriteLine("[{0}/{1}] 건너뜀(이름 겹침): {2}", i + 1, files.Count, sourcePath);
                        continue;
                    }

                    bool success = ConvertOne(hwpObject, filecheckdllOk, sourcePath, savePath, targetType, options.UsePdfPrint, printerName, options.PrintMethod);
                    if (success)
                    {
                        converted++;
                        if (overwrite) Console.WriteLine("[{0}/{1}] 완료(덮어씀): {2}", i + 1, files.Count, savePath);
                        else if (changedName) Console.WriteLine("[{0}/{1}] 완료(이름 바꿈): {2}", i + 1, files.Count, savePath);
                        else Console.WriteLine("[{0}/{1}] 완료: {2}", i + 1, files.Count, savePath);
                    }
                    else
                    {
                        failed++;
                        Console.WriteLine("[{0}/{1}] 실패: {2}", i + 1, files.Count, sourcePath);
                    }
                }

                Console.WriteLine("완료: 성공 {0}, 건너뜀 {1}, 실패 {2}", converted, skipped, failed);
                return failed > 0 ? 3 : 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("실행 중 오류가 발생했습니다: " + ex.Message);
                return 2;
            }
            finally
            {
                if (hwpObject != null) hwpObject.Quit();
            }
        }

        private static bool ConvertOne(HwpObject hwpObject, bool filecheckdllOk, string sourcePath, string savePath, string targetType, bool usePdfPrint, string printerName, int printMethod)
        {
            try
            {
                if (filecheckdllOk) hwpObject.SetMessageBoxMode(0x00211411);
                if (!hwpObject.Open(sourcePath, "", "lock:false;forceopen:true;suspendpassword:true;"))
                {
                    return false;
                }

                bool success;
                if (targetType == "PDF" && usePdfPrint && !string.IsNullOrEmpty(printerName))
                {
                    HAction hwpAction = (HAction)hwpObject.HAction;
                    HParameterSet hwpPset = (HParameterSet)hwpObject.HParameterSet;
                    HPrint hwpPrint = (HPrint)hwpPset.HPrint;
                    HSet hwpSet = (HSet)hwpPrint.HSet;
                    hwpAction.GetDefault("Print", hwpSet);
                    hwpPrint.PrintMethod = (ushort)printMethod;
                    hwpPrint.Collate = 1;
                    hwpPrint.NumCopy = 1;
                    hwpPrint.PrintToFile = 1;
                    hwpPrint.filename = savePath;
                    hwpPrint.PrinterName = printerName;
                    hwpPrint.Flags = 8192;
                    hwpPrint.Device = 3;
                    success = hwpAction.Execute("Print", hwpSet);
                    if (success)
                    {
                        success = WaitForFileWriteCompletion(savePath);
                    }
                }
                else
                {
                    success = hwpObject.SaveAs(savePath, targetType, "");
                }

                return success;
            }
            finally
            {
                hwpObject.Clear(1);
            }
        }

        private static bool WaitForFileWriteCompletion(string savePath)
        {
            const int maxWaitSeconds = 60;
            const int maxRetryCount = maxWaitSeconds * 2; // 500ms 간격
            for (int retry = 0; retry < maxRetryCount; retry++)
            {
                try
                {
                    // 배타적으로 열리면 가상 프린터의 파일 쓰기가 끝난 상태
                    using (var stream = new FileStream(savePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    { }
                    return true;
                }
                catch (IOException)
                {
                    if (retry > 0 && retry % 10 == 0)
                    {
                        Console.WriteLine("파일 쓰기 완료 대기 중: " + Path.GetFileName(savePath));
                    }
                    Thread.Sleep(500);
                }
            }
            Console.Error.WriteLine("파일 쓰기 완료 대기 시간이 초과되었습니다: " + savePath);
            return false;
        }

        private static string BuildTargetPath(string sourcePath, string outputDirectory, bool useCurrentPath, string targetExt)
        {
            string dir = useCurrentPath ? Path.GetDirectoryName(sourcePath) : outputDirectory;
            return Path.Combine(dir, Path.GetFileNameWithoutExtension(sourcePath) + targetExt);
        }

        private static void ResolveOverwrite(ref string savePath, OverwriteMode overwriteMode, string outputDirectory, bool useCurrentPath, string sourcePath, string targetExt, out bool changedName, out bool shouldSkip, out bool overwrite)
        {
            changedName = false;
            shouldSkip = false;
            overwrite = false;
            int tempNum = 0;

            while (File.Exists(savePath))
            {
                tempNum++;
                if (overwriteMode == OverwriteMode.Rename)
                {
                    string dir = useCurrentPath ? Path.GetDirectoryName(sourcePath) : outputDirectory;
                    savePath = Path.Combine(dir, Path.GetFileNameWithoutExtension(sourcePath) + "(" + tempNum.ToString(CultureInfo.InvariantCulture) + ")" + targetExt);
                    changedName = true;
                }
                else if (overwriteMode == OverwriteMode.Skip)
                {
                    shouldSkip = true;
                    break;
                }
                else if (overwriteMode == OverwriteMode.Overwrite)
                {
                    overwrite = true;
                    break;
                }
                else
                {
                    shouldSkip = true;
                    break;
                }
            }
        }

        private static bool RegistryHasHancom()
        {
            using (RegistryKey software = Registry.CurrentUser.OpenSubKey("SOFTWARE"))
            {
                if (software == null) return false;
                using (RegistryKey hnc = software.OpenSubKey("HNC"))
                {
                    return hnc != null;
                }
            }
        }

        private static bool RegisterSecurityModule(HwpObject hwpObject)
        {
            using (RegistryKey software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true))
            {
                if (software == null) return false;

                using (RegistryKey hnc = software.OpenSubKey("HNC", true))
                {
                    if (hnc == null) return false;

                    using (RegistryKey automation = hnc.CreateSubKey("HwpAutomation"))
                    {
                        if (automation == null) return false;

                        using (RegistryKey modules = automation.CreateSubKey("Modules"))
                        {
                            if (modules == null) return false;

                            const string valueName = "FilePathCheckerModuleExample";
                            object current = modules.GetValue(valueName);
                            bool registerCurrentPath = false;
                            if (current == null)
                            {
                                registerCurrentPath = true;
                            }
                            else
                            {
                                string dllPath = current.ToString();
                                if (!File.Exists(dllPath))
                                {
                                    registerCurrentPath = true;
                                }
                            }

                            if (registerCurrentPath)
                            {
                                string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FilePathCheckerModuleExample.DLL");
                                if (File.Exists(dllPath))
                                {
                                    modules.SetValue(valueName, dllPath);
                                }
                            }
                        }
                    }
                }
            }

            return hwpObject.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample");
        }

        private static bool TryResolvePdfPrinter(ref string printerName)
        {
            var printerNames = new System.Collections.ArrayList(System.Drawing.Printing.PrinterSettings.InstalledPrinters);
            bool installed = false;
            bool setDefault = string.IsNullOrEmpty(printerName);
            for (int i = 0; i < printerNames.Count; i++)
            {
                string name = printerNames[i].ToString();
                if (string.Equals(name, printerName, StringComparison.OrdinalIgnoreCase))
                {
                    installed = true;
                    break;
                }

                string lower = name.ToLowerInvariant();
                if (!lower.Contains("pdf")) continue;

                if (lower.Contains("hancom"))
                {
                    installed = true;
                    if (setDefault)
                    {
                        printerName = name;
                        break;
                    }
                }

                if (lower.Contains("microsoft"))
                {
                    installed = true;
                    if (setDefault) printerName = name;
                }
            }
            return installed;
        }

        private static int GetTargetIndex(string targetType)
        {
            for (int i = 0; i < TargetTypes.Length; i++)
            {
                if (string.Equals(TargetTypes[i], targetType, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        private static List<string> CollectInputFiles(List<string> inputs)
        {
            var result = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string input in inputs)
            {
                if (string.IsNullOrWhiteSpace(input)) continue;
                string full = Path.GetFullPath(input);
                if (Directory.Exists(full))
                {
                    CollectFromDirectory(full, result, seen);
                }
                else if (File.Exists(full))
                {
                    string ext = Path.GetExtension(full);
                    if (IsSupportedSourceExt(ext) && seen.Add(full))
                    {
                        result.Add(full);
                    }
                }
            }

            result.Sort(new FileNameComparer());
            return result;
        }

        private static void CollectFromDirectory(string directory, List<string> result, HashSet<string> seen)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                string ext = Path.GetExtension(file);
                if (IsSupportedSourceExt(ext))
                {
                    string full = Path.GetFullPath(file);
                    if (seen.Add(full)) result.Add(full);
                }
            }

            foreach (string subDir in Directory.GetDirectories(directory))
            {
                CollectFromDirectory(subDir, result, seen);
            }
        }

        private static bool IsSupportedSourceExt(string ext)
        {
            foreach (string sourceExt in SourceExts)
            {
                if (string.Equals(sourceExt, ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool TryParse(string[] args, out CliOptions options, out string error)
        {
            options = new CliOptions();
            error = "";
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (string.Equals(arg, "--help", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "-h", StringComparison.OrdinalIgnoreCase))
                {
                    PrintUsage();
                    return false;
                }

                if (string.Equals(arg, "--gui", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (string.Equals(arg, "--input", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "-i", StringComparison.OrdinalIgnoreCase))
                {
                    string value;
                    if (!TryReadValue(args, ref i, out value))
                    {
                        error = "--input 옵션에 값이 필요합니다.";
                        return false;
                    }
                    options.Inputs.Add(value);
                    continue;
                }

                if (string.Equals(arg, "--output", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "-o", StringComparison.OrdinalIgnoreCase))
                {
                    string value;
                    if (!TryReadValue(args, ref i, out value))
                    {
                        error = "--output 옵션에 값이 필요합니다.";
                        return false;
                    }
                    options.OutputDirectory = value;
                    continue;
                }

                if (string.Equals(arg, "--target", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "-t", StringComparison.OrdinalIgnoreCase))
                {
                    string value;
                    if (!TryReadValue(args, ref i, out value))
                    {
                        error = "--target 옵션에 값이 필요합니다.";
                        return false;
                    }
                    options.TargetType = value.ToUpperInvariant();
                    continue;
                }

                if (string.Equals(arg, "--overwrite", StringComparison.OrdinalIgnoreCase))
                {
                    string value;
                    if (!TryReadValue(args, ref i, out value))
                    {
                        error = "--overwrite 옵션에 값이 필요합니다. (rename|skip|overwrite)";
                        return false;
                    }
                    OverwriteMode mode;
                    if (!TryParseOverwrite(value, out mode))
                    {
                        error = "지원하지 않는 --overwrite 값입니다: " + value;
                        return false;
                    }
                    options.Overwrite = mode;
                    continue;
                }

                if (string.Equals(arg, "--pdf-print", StringComparison.OrdinalIgnoreCase))
                {
                    options.UsePdfPrint = true;
                    continue;
                }

                if (string.Equals(arg, "--printer", StringComparison.OrdinalIgnoreCase))
                {
                    string value;
                    if (!TryReadValue(args, ref i, out value))
                    {
                        error = "--printer 옵션에 값이 필요합니다.";
                        return false;
                    }
                    options.PrinterName = value;
                    continue;
                }

                if (string.Equals(arg, "--print-method", StringComparison.OrdinalIgnoreCase))
                {
                    string value;
                    if (!TryReadValue(args, ref i, out value))
                    {
                        error = "--print-method 옵션에 값이 필요합니다.";
                        return false;
                    }
                    int method;
                    if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out method))
                    {
                        error = "--print-method 값은 숫자여야 합니다.";
                        return false;
                    }
                    options.PrintMethod = method;
                    continue;
                }

                if (arg.StartsWith("-", StringComparison.Ordinal))
                {
                    error = "알 수 없는 옵션입니다: " + arg;
                    return false;
                }

                options.Inputs.Add(arg);
            }

            return true;
        }

        private static bool TryReadValue(string[] args, ref int i, out string value)
        {
            value = "";
            int next = i + 1;
            if (next >= args.Length) return false;
            value = args[next];
            i = next;
            return true;
        }

        private static bool TryParseOverwrite(string value, out OverwriteMode mode)
        {
            mode = OverwriteMode.Rename;
            if (string.Equals(value, "rename", StringComparison.OrdinalIgnoreCase))
            {
                mode = OverwriteMode.Rename;
                return true;
            }

            if (string.Equals(value, "skip", StringComparison.OrdinalIgnoreCase))
            {
                mode = OverwriteMode.Skip;
                return true;
            }

            if (string.Equals(value, "overwrite", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "replace", StringComparison.OrdinalIgnoreCase))
            {
                mode = OverwriteMode.Overwrite;
                return true;
            }

            return false;
        }

        public static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  hwp2pdf.exe <file-or-dir> [more paths...] [options]");
            Console.WriteLine("  hwp2pdf.exe --input <path> [--input <path> ...] [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help              도움말 출력");
            Console.WriteLine("  --gui                   GUI 모드로 실행");
            Console.WriteLine("  -o, --output <dir>      출력 폴더(기본: 원본 파일 폴더)");
            Console.WriteLine("  -t, --target <type>     출력 형식 (PDF, HWP, HWPX, HWPML2X, HTML+, ODT, OOXML, UNICODE, RTF)");
            Console.WriteLine("  --overwrite <mode>      rename|skip|overwrite (기본: rename)");
            Console.WriteLine("  --pdf-print             PDF 출력 시 가상 프린터 방식 사용");
            Console.WriteLine("  --printer <name>        PDF 프린터 이름 지정");
            Console.WriteLine("  --print-method <num>    HWP 인쇄 방식 번호(기본: 1)");
        }
    }
}
