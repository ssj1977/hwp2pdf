using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices; // For DllImport
using System.Text; // For StringBuilder

namespace hwp2pdf
{
    public static class CommandLineRunner
    {
#if HWP2PDF_WINDOWS
        // P/Invoke for reading INI files - Only compiled if HWP2PDF_WINDOWS is defined
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
#endif

        // We don't need WritePrivateProfileString for command-line reading.

        public static void Run(string[] args)
        {
            Console.WriteLine("[CommandLineRunner] Execution started.");
            Action<string> consoleLogger = message => Console.WriteLine(message);

            // --- Default settings & INI loading ---
            List<string> inputPaths = new List<string>();
            string outputDirectory = string.Empty;
            string targetFormat = "PDF";
            string targetExtension = ".pdf"; // Will be derived from targetFormat
            int overwriteOption = 0; // 0: new name, 1: skip, 2: overwrite
            bool usePdfPrint = false;
            string printerName = string.Empty;
            int printMethod = 1;
            bool useCurrentPathForOutput = true; // Corresponds to m_bUseCurrentPath
            string iniSavePath = string.Empty; // Corresponds to m_strSavePath

            string iniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hwp2pdf.ini");
            consoleLogger($"[CommandLineRunner] Attempting to load settings from: {iniFilePath}");

#if HWP2PDF_WINDOWS
            if (File.Exists(iniFilePath))
            {
                StringBuilder tempBuilder = new StringBuilder(1024);

                GetPrivateProfileString("Main", "SavePath", "", tempBuilder, tempBuilder.Capacity, iniFilePath);
                iniSavePath = tempBuilder.ToString();
                tempBuilder.Clear();

                GetPrivateProfileString("Main", "SaveToCurrentPath", "true", tempBuilder, tempBuilder.Capacity, iniFilePath);
                if (tempBuilder.Length > 0) bool.TryParse(tempBuilder.ToString(), out useCurrentPathForOutput);
                tempBuilder.Clear();

                GetPrivateProfileString("Main", "OptionOverwrite", "0", tempBuilder, tempBuilder.Capacity, iniFilePath);
                if (tempBuilder.Length > 0) int.TryParse(tempBuilder.ToString(), out overwriteOption);
                tempBuilder.Clear();

                GetPrivateProfileString("Main", "OptionPDFPrint", "false", tempBuilder, tempBuilder.Capacity, iniFilePath);
                if (tempBuilder.Length > 0) bool.TryParse(tempBuilder.ToString(), out usePdfPrint);
                tempBuilder.Clear();

                GetPrivateProfileString("Main", "CurrentTargetType", "0", tempBuilder, tempBuilder.Capacity, iniFilePath);
                if (tempBuilder.Length > 0 && int.TryParse(tempBuilder.ToString(), out int targetTypeIndex))
                {
                    string[] iniTargetTypes = { "PDF", "HWP", "HWPX", "HWPML2X", "HTML", "ODT", "OOXML", "UNICODE", "RTF" };
                    string[] iniTargetExtensions = { ".pdf", ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".txt", ".rtf" };
                    if (targetTypeIndex >= 0 && targetTypeIndex < iniTargetTypes.Length)
                    {
                        targetFormat = iniTargetTypes[targetTypeIndex];
                        targetExtension = iniTargetExtensions[targetTypeIndex];
                    }
                }
                tempBuilder.Clear();

                GetPrivateProfileString("Main", "PrinterName", "", tempBuilder, tempBuilder.Capacity, iniFilePath);
                printerName = tempBuilder.ToString();
                tempBuilder.Clear();

                GetPrivateProfileString("Main", "PrintMethod", "1", tempBuilder, tempBuilder.Capacity, iniFilePath);
                if (tempBuilder.Length > 0) int.TryParse(tempBuilder.ToString(), out printMethod);
                tempBuilder.Clear();

                consoleLogger("[CommandLineRunner] Loaded settings from INI file (Windows).");
            }
            else
            {
                consoleLogger("[CommandLineRunner] INI file not found (Windows). Using default settings.");
            }
#else
            consoleLogger("[CommandLineRunner] INI reading is skipped (non-Windows or HWP2PDF_WINDOWS not defined). Using default settings.");
            // Ensure defaults are set as if INI was not there or empty for consistent testing
            useCurrentPathForOutput = true; // Default if INI is skipped
            iniSavePath = string.Empty;     // Default if INI is skipped
            overwriteOption = 0;            // Default if INI is skipped
            usePdfPrint = false;            // Default if INI is skipped
            targetFormat = "PDF";           // Default if INI is skipped
            targetExtension = ".pdf";       // Default if INI is skipped
            printerName = string.Empty;     // Default if INI is skipped
            printMethod = 1;                // Default if INI is skipped
#endif // Correctly placed #endif

            // consoleLogger("[CommandLineRunner] Proceeding with default/command-line settings."); // Redundant with above

            // --- Argument Parsing (Overrides INI settings) ---
            bool outputDirArgSet = false; // To help decide final output directory logic

            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        case "-i":
                            if (i + 1 < args.Length) inputPaths.Add(args[++i]);
                            else { consoleLogger("[CommandLineRunner] Error: Missing value for -i."); return; }
                            break;
                        case "-o":
                            if (i + 1 < args.Length) { outputDirectory = args[++i]; outputDirArgSet = true; }
                            else { consoleLogger("[CommandLineRunner] Error: Missing value for -o."); return; }
                            break;
                        case "-f":
                            if (i + 1 < args.Length)
                            {
                                string fmtArg = args[++i].ToUpper();
                                if (fmtArg == "DOCX") // Add alias for DOCX
                                {
                                    fmtArg = "OOXML";
                                    consoleLogger($"[CommandLineRunner] Info: Using format 'OOXML' for requested 'DOCX'.");
                                }
                                // Mapping from FormMain's target_type_array and target_ext_array
                                string[] availableFormats = { "PDF", "HWP", "HWPX", "HWPML2X", "HTML", "ODT", "OOXML", "UNICODE", "RTF" };
                                string[] availableExtensions = { ".pdf", ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".txt", ".rtf" };
                                int fmtIndex = Array.IndexOf(availableFormats, fmtArg);
                                if (fmtIndex != -1)
                                {
                                    targetFormat = availableFormats[fmtIndex];
                                    targetExtension = availableExtensions[fmtIndex];
                                }
                                else
                                {
                                    consoleLogger($"[CommandLineRunner] Warning: Unknown target format '{fmtArg}'. Using current default '{targetFormat}'.");
                                }
                            }
                            else { consoleLogger("[CommandLineRunner] Error: Missing value for -f."); return; }
                            break;
                        case "-ow":
                            if (i + 1 < args.Length && int.TryParse(args[++i], out int owOption))
                            { overwriteOption = owOption; }
                            else { consoleLogger("[CommandLineRunner] Error: Missing or invalid value for -ow."); return; }
                            break;
                        case "-pdfprint":
                            if (i + 1 < args.Length && bool.TryParse(args[++i], out bool pdfPrintVal))
                            { usePdfPrint = pdfPrintVal; }
                            else { consoleLogger("[CommandLineRunner] Error: Missing or invalid value for -pdfprint."); return; }
                            break;
                        case "-printer":
                            if (i + 1 < args.Length) { printerName = args[++i]; }
                            else { consoleLogger("[CommandLineRunner] Error: Missing value for -printer."); return; }
                            break;
                        case "-pm":
                             if (i + 1 < args.Length && int.TryParse(args[++i], out int pMethod))
                            { printMethod = pMethod;}
                            else { consoleLogger("[CommandLineRunner] Error: Missing or invalid value for -pm."); return; }
                            break;
                        case "-help": case "--help": case "/?":
                            PrintHelp(consoleLogger); return;
                        default:
                            consoleLogger($"[CommandLineRunner] Warning: Unknown argument '{args[i]}'."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                consoleLogger($"[CommandLineRunner] Error parsing arguments: {ex.Message}");
                PrintHelp(consoleLogger); return;
            }

            if (!inputPaths.Any())
            {
                consoleLogger("[CommandLineRunner] Error: No input files specified. Use -i <path>.");
                PrintHelp(consoleLogger); return;
            }

            // Determine final output directory
            if (!outputDirArgSet) // If -o was not used by command line
            {
                if (useCurrentPathForOutput) // Check INI setting for "SaveToCurrentPath"
                {
                    // Default to the directory of the first input file.
                    // This might need adjustment if inputPaths contains directories or mixed sources.
                    try
                    {
                        outputDirectory = Path.GetDirectoryName(Path.GetFullPath(inputPaths.First()));
                        consoleLogger($"[CommandLineRunner] Output directory not set by -o, INI SaveToCurrentPath is true. Outputting to first input's directory: {outputDirectory}");
                    }
                    catch (Exception ex) // Handle cases where GetFullPath or GetDirectoryName might fail for an odd path
                    {
                        consoleLogger($"[CommandLineRunner] Warning: Could not determine directory from first input path. Using default. Error: {ex.Message}");
                        outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output_cmd");
                    }
                }
                else if (!string.IsNullOrEmpty(iniSavePath)) // Check INI "SavePath"
                {
                    outputDirectory = iniSavePath;
                    consoleLogger($"[CommandLineRunner] Output directory not set by -o, using INI SavePath: {outputDirectory}");
                }
                else // Fallback if no other output directory is set
                {
                    outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output_cmd");
                    consoleLogger($"[CommandLineRunner] Output directory not set by -o or INI. Defaulting to: {outputDirectory}");
                }
            }

            if (string.IsNullOrEmpty(outputDirectory)) // Final safety check if outputDirectory is still null or empty
            {
                 outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output_cmd");
                 consoleLogger($"[CommandLineRunner] Output directory was empty, defaulting to: {outputDirectory}");
            }


            if (!Directory.Exists(outputDirectory))
            {
                try { Directory.CreateDirectory(outputDirectory); consoleLogger($"[CommandLineRunner] Created output directory: {outputDirectory}"); }
                catch (Exception ex) { consoleLogger($"[CommandLineRunner] Error: Could not create output directory '{outputDirectory}': {ex.Message}"); return; }
            }

            consoleLogger($"[CommandLineRunner] Final Configuration: Inputs='{string.Join(", ", inputPaths)}', OutputDir='{outputDirectory}', Format='{targetFormat}', Ext='{targetExtension}', Overwrite='{overwriteOption}', PDFPrint='{usePdfPrint}', Printer='{printerName}', PrintMethod='{printMethod}'");

            // --- Conversion Logic ---
            try
            {
                using (ConversionEngine engine = new ConversionEngine(consoleLogger))
                {
                    engine.TargetType = targetFormat;
                    engine.TargetExtension = targetExtension;
                    engine.OverwriteOption = overwriteOption;
                    engine.UsePdfPrint = usePdfPrint;
                    engine.PrinterName = printerName;
                    engine.PrintMethod = printMethod;

                    int successCount = 0;
                    int totalFilesAttempted = 0;
                    foreach (string pathArg in inputPaths)
                    {
                        if (Directory.Exists(pathArg))
                        {
                            consoleLogger($"[CommandLineRunner] Processing directory: {pathArg}");
                            string[] sourceExtensions = { ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".doc", ".txt", ".rtf" };
                            List<string> filesInDir = new List<string>();
                            foreach(var ext in sourceExtensions)
                            {
                                filesInDir.AddRange(Directory.GetFiles(pathArg, $"*{ext}", SearchOption.TopDirectoryOnly));
                            }

                            if (!filesInDir.Any()) {
                                consoleLogger($"[CommandLineRunner] No convertible files found in directory: {pathArg}");
                                continue;
                            }

                            foreach (string filePath in filesInDir)
                            {
                                totalFilesAttempted++;
                                consoleLogger($"[CommandLineRunner] Attempting to convert '{filePath}'");
                                bool success = engine.ConvertFile(filePath, outputDirectory);
                                if (success) successCount++;
                                consoleLogger($"[CommandLineRunner] Conversion of '{filePath}' {(success ? "succeeded" : "failed or skipped")}.");
                            }
                        }
                        else if (File.Exists(pathArg))
                        {
                            totalFilesAttempted++;
                            consoleLogger($"[CommandLineRunner] Attempting to convert '{pathArg}'");
                            bool success = engine.ConvertFile(pathArg, outputDirectory);
                            if (success) successCount++;
                            consoleLogger($"[CommandLineRunner] Conversion of '{pathArg}' {(success ? "succeeded" : "failed or skipped")}.");
                        }
                        else
                        {
                            consoleLogger($"[CommandLineRunner] Error: Input path not found: {pathArg}");
                        }
                    }
                    consoleLogger($"[CommandLineRunner] Processed {inputPaths.Count} input path argument(s), {totalFilesAttempted} file(s) attempted. {successCount} file(s) reported as successfully converted/handled.");
                }
            }
            catch (Exception ex)
            {
                consoleLogger($"[CommandLineRunner] An error occurred during conversion: {ex.Message}");
                consoleLogger($"[CommandLineRunner] Stack Trace: {ex.StackTrace}");
            }

            Console.WriteLine("[CommandLineRunner] Execution finished.");
        }

        private static void PrintHelp(Action<string> logger)
        {
            logger("[CommandLineRunner] Help:");
            logger("  hwp2pdf (command-line mode)");
            logger("  Arguments:");
            logger("    -i <path>         : Input file or folder. Can be specified multiple times.");
            logger("                        If folder, processes supported files in that folder (not recursive).");
            logger("    -o <directory>    : Output directory. Overrides INI. (Defaults based on INI or to 'output_cmd')");
            logger("    -f <format>       : Target format. Overrides INI. (e.g., PDF, HWPX, DOCX). (Default: PDF or from INI)");
            logger("    -ow <option>      : Overwrite option. Overrides INI. (0:new name, 1:skip, 2:overwrite). (Default: 0 or from INI)");
            logger("    -pdfprint <bool>  : Use PDF printing. Overrides INI. (true/false). (Default: false or from INI)");
            logger("    -printer <name>   : PDF printer name. Overrides INI.");
            logger("    -pm <method>      : Print method. Overrides INI. (Default: 1 or from INI)");
            logger("    -help or /?       : Show this help message.");
            logger("  INI file (hwp2pdf.ini) is loaded for defaults if present next to the executable.");
            logger("  Example:");
            logger("    hwp2pdf -i document.hwp -o converted_files -f PDF");
        }
    }
}
