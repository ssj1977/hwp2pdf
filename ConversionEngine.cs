using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices; // For DllImport if ini reading is moved here later
using System.Text; // For StringBuilder if ini reading is moved here later
using System.Threading; // For Thread.Sleep
// using HwpObjectLib; // Assuming this will be available in the execution environment // Temporarily commented for testing CLI

namespace hwp2pdf
{
    public class ConversionEngine : IDisposable
    {
        // private HwpObject m_hwpObject; // Temporarily commented for testing CLI
        private bool m_fileCheckDllOk = false; // This might still be useful if we simulate its effect
        private Action<string> m_logger;

        // Add this property to ConversionEngine class
        public string LastStatusMessageForUI { get; private set; }

        // Configuration settings - to be populated by constructor or properties
        public string TargetType { get; set; } = "PDF"; // Default
        public string TargetExtension { get; set; } = ".pdf"; // Default
        public bool UsePdfPrint { get; set; } = false;
        public string PrinterName { get; set; } = "";
        public int PrintMethod { get; set; } = 1;
        public int OverwriteOption { get; set; } = 0; // 0: new name, 1: skip, 2: overwrite

        public ConversionEngine(Action<string> logger)
        {
            m_logger = logger ?? (message => {}); // Default to no-op logger if null

            try
            {
                Log("ConversionEngine Constructor: HWP Object initialization is TEMPORARILY COMMENTED OUT for CLI testing.");
                // // Basic HWP Object Initialization (similar to FormMain constructor)
                // // More advanced registry/DLL checks might be added later or passed in
                // m_hwpObject = new HwpObject();
                // if (m_hwpObject == null)
                // {
                //     Log("Error: HwpObject creation failed.");
                //     throw new Exception("HwpObject creation failed.");
                // }

                // // Attempt to register FilePathCheckerModuleExample.DLL
                // // This logic is simplified from FormMain and might need refinement
                // // or to be made configurable.
                // // For now, we assume it's either present in path or pre-registered.
                // // A more robust solution would involve checking registry as in FormMain.
                // m_fileCheckDllOk = m_hwpObject.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample");
                // if (!m_fileCheckDllOk)
                // {
                //     Log("Warning: FilePathCheckerModuleExample.DLL registration failed. HWP UI might pop up.");
                // }
                // else
                // {
                //     Log("FilePathCheckerModuleExample.DLL registered successfully.");
                // }
                //  // Ensure HWP is not visible if DLL is working, hide if not.
                // SetHwpVisibility(!m_fileCheckDllOk);
                this.LastStatusMessageForUI = "Engine Initialized (HWP Calls Disabled)";


            }
            catch (Exception ex)
            {
                Log($"Error initializing HWP engine: {ex.Message}");
                throw; // Re-throw to indicate initialization failure
            }
        }

        private void SetHwpVisibility(bool visible)
        {
            // if (m_hwpObject != null) // Temporarily commented for testing CLI
            // {
            //     try
            //     {
            //         IXHwpWindows hwpWindows = (IXHwpWindows)m_hwpObject.XHwpWindows;
            //         if (hwpWindows != null && hwpWindows.Count > 0)
            //         {
            //             IXHwpWindow hwpWindow = (IXHwpWindow)hwpWindows.Item[0];
            //             if (hwpWindow != null)
            //             {
            //                 hwpWindow.Visible = visible;
            //                 Log($"Set HWP window visibility to: {visible}");
            //             }
            //         }
            //     }
            //     catch (Exception ex)
            //     {
            //         Log($"Warning: Could not set HWP window visibility: {ex.Message}");
            //     }
            // }
            Log($"SetHwpVisibility called with: {visible} (HWP Calls Disabled)");
        }


        private void Log(string message)
        {
            m_logger($"[ConversionEngine] {message}");
        }

        public bool ConvertFile(string inputFilePath, string outputDirectory)
        {
            Log($"Starting conversion for: {inputFilePath}");
            this.LastStatusMessageForUI = "처리 시작..."; // Initialize status

            // if (m_hwpObject == null) // Temporarily commented for testing CLI
            // {
            //     Log("Error: HwpObject not initialized.");
            //     this.LastStatusMessageForUI = "엔진 초기화 안됨 (HWP 비활성)";
            //     return false;
            // }

            if (!File.Exists(inputFilePath))
            {
                Log($"Error: Input file not found: {inputFilePath}");
                return false;
            }

            // if (m_fileCheckDllOk) // Temporarily commented for testing CLI
            // {
            //     // SetMessageBoxMode to suppress dialogs if DLL is correctly loaded and registered
            //     // m_hwpObject.SetMessageBoxMode(0x00211411); // Value from FormMain
            // }


            string fileExt = Path.GetExtension(inputFilePath).ToLower();
            if (fileExt == TargetExtension)
            {
                Log($"Skipping conversion: File is already in target format ({TargetExtension}).");
                this.LastStatusMessageForUI = "변환안함(같은형식)";
                return true; // Or indicate skipped
            }

            Log("[ConvertFile] HWP Open and SaveAs/Print calls are TEMPORARILY DISABLED for CLI testing.");
            this.LastStatusMessageForUI = "변환 건너뜀 (HWP 비활성)";
            // Simulate a successful file operation for testing flow if needed, or return false
            // For now, let's simulate a "success" to test the rest of CommandLineRunner's logic
            // return false; // If we want to simulate conversion failure

            // Simulate determining save path for logging/UI feedback testing
            string savePath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputFilePath) + TargetExtension);
            bool skipFile = false;
            int tempNum = 0;
            string originalSavePathBase = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputFilePath));

            while (File.Exists(savePath))
            {
                if (OverwriteOption == 0) // New name
                {
                    tempNum++;
                    savePath = $"{originalSavePathBase}({tempNum}){TargetExtension}";
                }
                else if (OverwriteOption == 1) // Skip
                {
                    this.LastStatusMessageForUI = "변환안함(이름겹침)";
                    skipFile = true;
                    break;
                }
                else if (OverwriteOption == 2) // Overwrite
                {
                    // UI message will be set later
                    break;
                }
                else // Default to skip
                {
                    this.LastStatusMessageForUI = "변환안함(옵션오류)";
                    skipFile = true;
                    break;
                }
            }

            if (skipFile)
            {
                Log($"Skipping file {inputFilePath} due to overwrite option.");
                return true; // Indicate skipped but handled as per options
            }

            // Simulate success for flow testing
            if (OverwriteOption == 2 && tempNum == 0) this.LastStatusMessageForUI = "완료(덮어씀)";
            else if (tempNum > 0) this.LastStatusMessageForUI = $"완료(이름바꿈) - {Path.GetFileName(savePath)}";
            else this.LastStatusMessageForUI = "완료";
            Log($"Simulated conversion: {inputFilePath} to {savePath}");
            return true;


            // Original HWP logic below is commented out:
            /*
            if (m_hwpObject.Open(inputFilePath, "", "lock:false;forceopen:true;suspendpassword:true;"))
            {
                Log($"Opened '{inputFilePath}' successfully.");
                string savePath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputFilePath) + TargetExtension);

                // Handle filename conflicts based on OverwriteOption
                bool skipFile = false;
                int tempNum = 0;
                string originalSavePathBase = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputFilePath));

                while (File.Exists(savePath))
                {
                    if (OverwriteOption == 0) // New name
                    {
                        tempNum++;
                        savePath = $"{originalSavePathBase}({tempNum}){TargetExtension}";
                        Log($"File exists, trying new name: {savePath}");
                    }
                    else if (OverwriteOption == 1) // Skip
                    {
                        Log($"Skipping conversion for '{inputFilePath}' as output file '{savePath}' exists and overwrite option is 'skip'.");
                        this.LastStatusMessageForUI = "변환안함(이름겹침)";
                        skipFile = true;
                        break;
                    }
                    else if (OverwriteOption == 2) // Overwrite
                    {
                        Log($"Output file '{savePath}' exists and will be overwritten.");
                        // LastStatusMessageForUI will be set to "완료(덮어씀)" after successful conversion
                        break;
                    }
                    else // Default to skip for unknown option
                    {
                        Log($"Unknown overwrite option {OverwriteOption}, skipping.");
                        this.LastStatusMessageForUI = "변환안함(옵션오류)"; // More specific status
                        skipFile = true;
                        break;
                    }
                }

                if (skipFile)
                {
                    m_hwpObject.Clear(1); // Clear document
                    return true; // Indicate skipped
                }

                bool success = false;
                if (TargetType == "PDF" && UsePdfPrint && !string.IsNullOrEmpty(PrinterName))
                {
                    Log($"Converting to PDF using printer: {PrinterName}, Method: {PrintMethod}");
                    HAction hwpAction = (HAction)m_hwpObject.HAction; // Requires HwpObjectLib
                    HParameterSet hwpPset = (HParameterSet)m_hwpObject.HParameterSet; // Corrected type // Requires HwpObjectLib
                    HPrint hwpPrint = (HPrint)hwpPset.HPrint; // Corrected access // Requires HwpObjectLib
                    HSet hwpSet = (HSet)hwpPrint.HSet; // Corrected access // Requires HwpObjectLib

                    hwpAction.GetDefault("Print", hwpSet); // Requires HwpObjectLib
                    hwpPrint.PrintMethod = (ushort)PrintMethod;
                    hwpPrint.Collate = 1;
                    hwpPrint.NumCopy = 1;
                    hwpPrint.PrintToFile = 1;
                    hwpPrint.filename = savePath;
                    hwpPrint.PrinterName = PrinterName;
                    hwpPrint.Flags = 8192; // From FormMain
                    hwpPrint.Device = 3;   // From FormMain

                    success = hwpAction.Execute("Print", hwpSet);
                    Log(success ? "Print action executed." : "Print action failed to execute.");

                    if (success)
                    {
                        Log($"File '{savePath}' print command issued. Verifying file creation...");
                        // File write verification loop (simplified from FormMain)
                        int attempts = 0;
                        bool fileVerified = false;
                        while (!fileVerified && attempts < 10) // Max 5 seconds wait
                        {
                            try
                            {
                                using (FileStream stream = new FileStream(savePath, FileMode.Open, FileAccess.Read, FileShare.None))
                                {
                                    fileVerified = true;
                                }
                            }
                            catch (IOException)
                            {
                                Thread.Sleep(500); // Wait for file to be released
                                attempts++;
                            }
                        }
                        if(fileVerified)
                        {
                            Log($"File '{savePath}' verified.");
                            this.LastStatusMessageForUI = "쓰기 완료"; // Fallback for print path
                        }
                        else
                        {
                            Log($"Warning: File '{savePath}' could not be verified after printing.");
                            this.LastStatusMessageForUI = "변환 실패(파일검증실패)";
                            success = false;
                        }
                    }
                    else // Print action failed
                    {
                        this.LastStatusMessageForUI = "변환 실패(인쇄명령)";
                    }
                }
                else // SaveAs path
                {
                    Log($"Converting to {TargetType} using SaveAs. Output: {savePath}");
                    success = m_hwpObject.SaveAs(savePath, TargetType, ""); // Requires HwpObjectLib
                }

                if (success)
                {
                    Log($"Successfully converted '{inputFilePath}' to '{savePath}'.");
                    if (OverwriteOption == 2 && File.Exists(savePath) && tempNum == 0) // tempNum == 0 means original name was used
                    {
                        this.LastStatusMessageForUI = "완료(덮어씀)";
                    }
                    else if (tempNum > 0)
                    {
                        this.LastStatusMessageForUI = $"완료(이름바꿈) - {Path.GetFileName(savePath)}";
                    }
                    else
                    {
                        this.LastStatusMessageForUI = "완료";
                    }
                }
                else
                {
                    // If success is false and no specific message set yet (e.g. from print failure)
                    if (this.LastStatusMessageForUI == "처리 시작...")
                    {
                         this.LastStatusMessageForUI = "변환 시도 실패";
                    }
                    Log($"Failed to convert '{inputFilePath}'. Current status: {this.LastStatusMessageForUI}");
                }
                m_hwpObject.Clear(1); // Clear document // Requires HwpObjectLib
                return success;
            }
            else // m_hwpObject.Open failed
            {
                Log($"Error: Failed to open input file: {inputFilePath}");
                this.LastStatusMessageForUI = "원본파일 열기 실패";
                m_hwpObject.Clear(1); // Try to clear in case of partial open // Requires HwpObjectLib
                return false;
            }
            */
        }

        public void Dispose()
        {
            // if (m_hwpObject != null) // Temporarily commented for testing CLI
            // {
            //     Log("Quitting HWP object.");
            //     m_hwpObject.Quit();
            //     Marshal.ReleaseComObject(m_hwpObject); // Ensure COM object is released
            //     m_hwpObject = null;
            // }
            Log("ConversionEngine Disposed (HWP Calls Disabled).");
        }
    }
}
