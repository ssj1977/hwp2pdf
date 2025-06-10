using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
// using HwpObjectLib; // Removed as FormMain no longer directly uses HwpObjectLib types
using System.Reflection;
using System.Diagnostics;

namespace hwp2pdf
{
    public partial class FormMain : Form
    {
        string m_strPrinter = ""; //PDF 변환용 가상 프린터 이름
        int m_nPrintMethod = 1;
        bool m_bUseCurrentPath = true;
        string m_strSavePath = "";
        static bool st_bConverting = false;
        int option_overwrite = 0; // 0:새이름으로 저장, 1:변환스킵, 2:덮어쓰기
        int option_source_ext_flag = (1 | 2 | 4); //Source 확장자에 대한 비트플래그 타입
        bool option_PDF_print = false; //true 면 가상인쇄 방식 사용
        // HwpObject hwp_object = null; // Removed
        // bool filecheckdll_ok = false; // Removed
        //쓰레드에서 사용할 변수들
        static int st_convert_target_index = 0;
        //static string[] target_type_array = new string[] { "PDF", "HWP", "HWPX", "HWPML2X", "HTML+", "ODT", "OOXML", "MSWORD", "UNICODE", "RTF" };
        //static string[] target_ext_array = new string[] { ".pdf", ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".doc", ".txt", ".rtf" };
        static string[] target_type_array = new string[] { "PDF", "HWP", "HWPX", "HWPML2X", "HTML+", "ODT", "OOXML", "UNICODE", "RTF" };
        static string[] target_ext_array = new string[] { ".pdf", ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".txt", ".rtf" };
        //public static string[] source_type_array = new string[] { "HWP", "HWPX", "HWPML2X", "HTML+", "ODT", "OOXML", "MSWORD", "UNICODE", "RTF" };
        public static string[] source_ext_array = new string[] { ".hwp", ".hwpx", ".hml", ".html", ".odt", ".docx", ".doc", ".txt", ".rtf" };
        public FormMain()
        {
            // ConversionEngine HWP availability check
            try
            {
                using (var tempEngine = new ConversionEngine(message => {})) // Dummy logger for check
                {
                    // HWP automation seems available if no exception.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"한컴오토메이션 초기화 중 오류 발생 (ConversionEngine 생성 실패):\n{ex.Message}\n프로그램을 종료합니다.", "hwp2pdf Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Load += (s, e) => Close(); // Use lambda for cleaner event handler attachment
                return;
            }

            InitializeComponent();

#if HWP2PDF_WINDOWS
            //ini 파일에서 정보 불러오기
            String ini_path = System.Windows.Forms.Application.StartupPath + "\\hwp2pdf.ini";
            StringBuilder strTemp = new StringBuilder(1024, 1024);
            GetPrivateProfileString("Main", "SavePath", "", strTemp, strTemp.Capacity, ini_path);
            m_strSavePath = strTemp.ToString();
            strTemp.Clear();
            GetPrivateProfileString("Main", "SaveToCurrentPath", "true", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) bool.TryParse(strTemp.ToString(), out m_bUseCurrentPath); else m_bUseCurrentPath = true;
            strTemp.Clear();
            GetPrivateProfileString("Main", "OptionOverwrite", "0", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) int.TryParse(strTemp.ToString(), out option_overwrite); else option_overwrite = 0;
            strTemp.Clear();
            GetPrivateProfileString("Main", "OptionExtFlags", (1 | 2 | 4).ToString(), strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) int.TryParse(strTemp.ToString(), out option_source_ext_flag); else option_source_ext_flag = (1 | 2 | 4);
            strTemp.Clear();
            GetPrivateProfileString("Main", "OptionPDFPrint", "false", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) bool.TryParse(strTemp.ToString(), out option_PDF_print); else option_PDF_print = false;
            strTemp.Clear();
            GetPrivateProfileString("Main", "CurrentTargetType", "0", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) int.TryParse(strTemp.ToString(), out st_convert_target_index); else st_convert_target_index = 0;
            strTemp.Clear();
            GetPrivateProfileString("Main", "PrinterName", "", strTemp, strTemp.Capacity, ini_path);
            m_strPrinter = strTemp.ToString();
            strTemp.Clear();
            GetPrivateProfileString("Main", "PrintMethod", "1", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) int.TryParse(strTemp.ToString(), out m_nPrintMethod); else m_nPrintMethod = 1;
            strTemp.Clear();
            GetPrivateProfileString("Main", "Bounds", "", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0)
            {
                String strLocation = strTemp.ToString();
                String[] strTokens = strLocation.Split(',');
                int count = 0, 
                    x = this.Location.X, y= this.Location.Y, 
                    w = this.Size.Width, h = this.Size.Height;
                foreach(String strToken in strTokens)
                {
                    if (count == 0) x = int.Parse(strToken);
                    else if (count == 1) y = int.Parse(strToken);
                    else if (count == 2) w = int.Parse(strToken);
                    else if (count == 3) h = int.Parse(strToken);
                    else break;
                    count++;
                }
                Rectangle temp_rect = Rectangle.FromLTRB(x, y, x+w-1, y+h-1);
                if (SystemInformation.VirtualScreen.IntersectsWith(temp_rect) == true)
                {
                    this.SetDesktopBounds(x, y, w, h);
                }
            }
#else
            // On non-Windows, log skipping INI and rely on class member initializers for defaults.
            // add_log might not be safe before full UI initialization, consider Console.WriteLine if needed for debugging.
            // Console.WriteLine("[FormMain Constructor] INI file loading is skipped (non-Windows or HWP2PDF_WINDOWS not defined).");
#endif
            // FilePathCheckerModuleExample.DLL related logic is now handled within ConversionEngine's constructor.
            // HWP object visibility and initial SetMessageBoxMode are also handled by ConversionEngine.
            // The original logic for finding DLL path and registering it is simplified in ConversionEngine for now.

            // PDF 변환용 프린터가 설치되어 있는지 확인 (This logic runs on all platforms)
            System.Collections.ArrayList printer_names
                = new System.Collections.ArrayList(System.Drawing.Printing.PrinterSettings.InstalledPrinters);
            bool bPrinterInstalled = false;
            bool bSetDefault = false;
            if (m_strPrinter == "") bSetDefault = true;
            for (int i=0; i<printer_names.Count; i++)
            {
                string name = printer_names[i].ToString();
                if (name == m_strPrinter)
                {
                    bPrinterInstalled = true;
                    break;
                }
                else if (name.ToLower().Contains("pdf"))
                {
                    if (name.ToLower().Contains("hancom"))
                    {
                        bPrinterInstalled = true;
                        if (bSetDefault == true)
                        {   //프린터가 설정되지 않은 상태에서 한컴PDF가 있으면 기본 프린터로 사용 
                            m_strPrinter = name;
                            break;  
                        }
                    }
                    if (name.ToLower().Contains("microsoft"))
                    {
                        bPrinterInstalled = true;
                        //프린터가 설정되지 않은 상태에서 한컴PDF가 없는 경우 MS PDF 사용
                        if (bSetDefault == true) m_strPrinter = name; 
                    }
                }
            }
            if (bPrinterInstalled == false)
            {
                MessageBox.Show("한컴 PDF 또는 Micosoft Print to PDF가 설치되어 있지 않습니다.", "hwp2pdf");
            }
            if (m_bUseCurrentPath == true) m_strSavePath = System.IO.Directory.GetCurrentDirectory();
            update_path();
            int nCount = Math.Min(target_ext_array.GetLength(0), target_type_array.GetLength(0));
            // 변환가능 파일 형식 콤보박스 초기화하기
            combo_target_format.Items.Clear();
            for (int i = 0; i < nCount; i++)
            {
                string str_temp = target_type_array[i] + " (" + target_ext_array[i] + ")";
                combo_target_format.Items.Add(str_temp);
            }
            if (nCount > st_convert_target_index) combo_target_format.SelectedIndex = st_convert_target_index;
            else if (nCount > 0) combo_target_format.SelectedIndex = 0;
        }
        private delegate void add_log_delegate(string text);
        private delegate void show_convert_state_delegate(int nRow, string text);
        private delegate void enable_controls_delegate(bool bEnable);
        private void add_log(string text)
        {
            if (text_log.InvokeRequired)
            {
                var d = new add_log_delegate(add_log);
                Invoke(d, new object[] { text });
            }
            else
            {
                String time_str = DateTime.Now.ToString("(HH:mm:ss) ");
                text_log.AppendText("\r\n" + time_str + text);
            }
        }
        private void show_convert_state(int nRow, string text)
        {
            if (list_file.InvokeRequired)
            {
                var d = new show_convert_state_delegate(show_convert_state);
                Invoke(d, new object[] { nRow, text });
            }
            else
            {
                list_file.Items[nRow].SubItems[CONST.COL_STATUS].Text = text;
                list_file.RedrawItems(nRow, nRow, false);
                list_file.EnsureVisible(nRow);
            }
        }
        private void enable_controls(bool bEnable)
        {
            if (list_file.InvokeRequired)
            {
                var d = new enable_controls_delegate(enable_controls);
                Invoke(d, new object[] {bEnable});
            }
            else
            {
                list_file.Enabled = bEnable;
                btnSavePath.Enabled = bEnable;
                btn_clear.Enabled = bEnable;
                btn_close.Enabled = bEnable;
                btn_convert.Enabled = true;
                if (bEnable)    btn_convert.Text = "변환 시작";
                else            btn_convert.Text = "변환 중단";
            }
        }
        private void convert_files()
        {
            if (st_bConverting == true)
            {
                st_bConverting = false;
                btn_convert.Enabled = false;
                return;
            }
            if (list_file.Items.Count <= 0) return;
            String[] paths = new string[list_file.Items.Count];
            int nIndex = 0;
            foreach (ListViewItem item in list_file.Items)
            {
                paths[nIndex] = Path.Combine(item.SubItems[CONST.COL_PATH].Text, item.SubItems[CONST.COL_NAME].Text);
                item.SubItems[CONST.COL_STATUS].Text = "";
                nIndex++;
            }
            Thread th = new Thread(() => convert_thread(paths, m_bUseCurrentPath, m_strSavePath));
            th.SetApartmentState(ApartmentState.STA);
            st_bConverting = true;
            enable_controls(false);
            th.Start();
            st_convert_target_index = combo_target_format.SelectedIndex;
        }
        private void convert_thread(string[] paths, bool bUseCurrentPathLocal, string strSavePathLocal)
        {
            int nConverted = 0;
            add_log("파일 변환을 시작합니다. (ConversionEngine 사용)");

            string selectedTargetType = target_type_array[st_convert_target_index];
            string selectedTargetExt = target_ext_array[st_convert_target_index];

            try
            {
                using (ConversionEngine engine = new ConversionEngine(this.add_log))
                {
                    engine.TargetType = selectedTargetType;
                    engine.TargetExtension = selectedTargetExt;
                    engine.OverwriteOption = this.option_overwrite;
                    engine.UsePdfPrint = this.option_PDF_print;
                    engine.PrinterName = this.m_strPrinter;
                    engine.PrintMethod = this.m_nPrintMethod;

                    for (int nRow = 0; nRow < paths.Length; nRow++)
                    {
                        if (!st_bConverting) { add_log("변환 작업을 중단합니다."); break; }

                        string file_path = paths[nRow];
                        string currentOutputDirectory = bUseCurrentPathLocal ? Path.GetDirectoryName(file_path) : strSavePathLocal;

                        show_convert_state(nRow, "변환 준비중..."); // Initial status for UI

                        bool success = engine.ConvertFile(file_path, currentOutputDirectory);
                        // engine.LastStatusMessageForUI will be set by ConvertFile

                        show_convert_state(nRow, engine.LastStatusMessageForUI); // Update UI with detailed status

                        if (success) // Success here means file was processed without critical error, even if skipped
                        {
                            // Check if it was a genuine conversion for counting purposes
                            if (engine.LastStatusMessageForUI.Contains("완료")) // "완료", "완료(덮어씀)", "완료(이름바꿈)"
                            {
                               nConverted++;
                            }
                        }
                        // Error logging is now primarily handled by ConversionEngine via add_log

                        if (!st_bConverting) { add_log("변환 작업을 중단합니다."); break; }
                    }
                } // Engine disposed here
            }
            catch (Exception ex) // Catch errors from engine instantiation or unexpected issues
            {
                add_log($"[FormMain.convert_thread] 중대한 오류 발생: {ex.Message}");
                add_log($"스택 트레이스: {ex.StackTrace}");
            }

            add_log(String.Format("{0}개 파일 중 {1}개 파일을 변환 완료 또는 성공적으로 처리했습니다.", paths.Length, nConverted));
            st_bConverting = false;
            enable_controls(true);
        }
        private void btn_convert_Click(object sender, EventArgs e)
        {
            convert_files();
        }
        private void list_menu_convert_Click(object sender, EventArgs e)
        {
            convert_files();
        }
        private void list_file_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private bool CheckDuplication(string filepath)
        {
            foreach (ListViewItem item in list_file.Items)
            {
                string anotherpath = Path.Combine(item.SubItems[CONST.COL_PATH].Text, item.SubItems[CONST.COL_NAME].Text);
                if (filepath.Equals(anotherpath, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private void list_file_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Array.Sort(files, new FileNameComparer());
            int nAdded = add_files(files, option_source_ext_flag);
            add_log(String.Format("{0}개를 목록에 추가하였습니다.", nAdded));
        }
        private string GetFileSizeString(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";
            return size;
        }
        private int add_files(string[] files, int extflag)
        {
            int nAdded = 0;
            foreach (string file in files)
            {
                FileAttributes attr = System.IO.File.GetAttributes(file);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    string[] dirs_only = Directory.GetDirectories(file);
                    string[] files_only = Directory.GetFiles(file);
                    nAdded += add_files(dirs_only, option_source_ext_flag); 
                    nAdded += add_files(files_only, option_source_ext_flag);
                }
                else
                {
                    FileInfo fInfo = new FileInfo(System.IO.Path.GetFullPath(file));
                    string file_size = GetFileSizeString(fInfo.Length);
                    string file_ext = fInfo.Extension.ToLower();

                    int flag_compare = 1;
                    bool is_correct_ext = false;
                    foreach (string ext_item in source_ext_array )
                    {
                        if ((file_ext.Equals(ext_item.ToLower())) && ((extflag & flag_compare) != 0))
                        {
                            is_correct_ext = true;
                            break;
                        }
                        flag_compare = flag_compare * 2;
                    }
                    if (is_correct_ext)
                    {
                        if (CheckDuplication(fInfo.FullName) == false)
                        {
                            Icon iconForFile = SystemIcons.WinLogo;
                            if (!imageList_file.Images.ContainsKey(file_ext))
                            {
                                iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file);
                                imageList_file.Images.Add(file_ext, iconForFile);
                            }
                            ListViewItem newItem = list_file.Items.Add(fInfo.Name, file_ext);
                            for (int i=1; i< CONST.COL_TOTAL; i++) newItem.SubItems.Add(""); //칼럼 초기화
                            //newItem.SubItems[CONST.COL_NAME].Text = fInfo.Name;
                            newItem.SubItems[CONST.COL_STATUS].Text = "";
                            newItem.SubItems[CONST.COL_SIZE].Text = GetFileSizeString(fInfo.Length);
                            newItem.SubItems[CONST.COL_PATH].Text = fInfo.DirectoryName;
                            nAdded++;
                        }
                    }
                }
            }
            return nAdded;
        }
        private void list_file_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }
        private void clear_files()
        {
            if (list_file.Items.Count > 0)
            {
                list_file.Items.Clear();
                add_log("목록을 초기화하였습니다.");
            }
        }
        private void list_menu_clear_Click(object sender, EventArgs e)
        {
            clear_files();
        }
        private void btn_clear_Click(object sender, EventArgs e)
        {
            clear_files();
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void list_menu_exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if (hwp_object != null) hwp_object.Quit(); // Removed, ConversionEngine handles its own HwpObject
#if HWP2PDF_WINDOWS
            String ini_path_closing = System.Windows.Forms.Application.StartupPath + "\\hwp2pdf.ini"; // Use different var name
            WritePrivateProfileString("Main", "SaveToCurrentPath", m_bUseCurrentPath.ToString(), ini_path_closing);
            if (m_bUseCurrentPath == true) m_strSavePath = ""; // Don't save path if using current path
            WritePrivateProfileString("Main", "SavePath", m_strSavePath, ini_path_closing);
            WritePrivateProfileString("Main", "OptionOverwrite", option_overwrite.ToString(), ini_path_closing);
            WritePrivateProfileString("Main", "OptionExtFlags", option_source_ext_flag.ToString(), ini_path_closing);
            WritePrivateProfileString("Main", "OptionPDFPrint", option_PDF_print.ToString(), ini_path_closing);
            WritePrivateProfileString("Main", "CurrentTargetType", st_convert_target_index.ToString(), ini_path_closing);
            WritePrivateProfileString("Main", "PrinterName", m_strPrinter, ini_path_closing);
            WritePrivateProfileString("Main", "PrintMethod", m_nPrintMethod.ToString(), ini_path_closing);
            String strBoundsValue_closing; // Use different var name
            if (WindowState == FormWindowState.Maximized || WindowState==FormWindowState.Minimized)
            {
                strBoundsValue_closing = RestoreBounds.Location.X.ToString() + ',' + RestoreBounds.Location.Y.ToString()
                    + ',' + RestoreBounds.Size.Width.ToString() + ',' + RestoreBounds.Size.Height.ToString();
            }
            else
            {
                strBoundsValue_closing = this.Location.X.ToString() + ',' + this.Location.Y.ToString()
                    + ',' + this.Size.Width.ToString() + ',' + this.Size.Height.ToString();
            }
            WritePrivateProfileString("Main", "Bounds", strBoundsValue_closing, ini_path_closing);
#endif
        }
        private void btnSavePath_Click(object sender, EventArgs e)
        {
            FormSetSavePath dlg = new FormSetSavePath();
            dlg.setOption(m_bUseCurrentPath, m_strSavePath);
                if (dlg.ShowDialog() == DialogResult.OK)
            {
                m_bUseCurrentPath = dlg.IsUseCurrentPath();
                m_strSavePath = dlg.getSavePath();
                update_path();
            }

        }
        private void update_path()
        {
            if (m_bUseCurrentPath == true)  textSavePath.Text = "변환된 파일을 원본 파일과 같은 폴더에 저장합니다.";
            else textSavePath.Text = "변환된 파일 저장 경로: "+ m_strSavePath;
            textSavePath.Update();
        }
        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            /*switch (e.KeyCode)
            {
            }*/
        }
        private void list_menu_add_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "한컴오피스 파일|*.hwp;*.hwpx;*.hml;|기타 변환가능 파일|*.docx;*.doc;*.rtf;*.txt;|All File(*.*)|*.*";
            dlg.Multiselect = true;
            dlg.Title = "추가할 HWP 파일을 선택해 주세요";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = dlg.FileNames;
                Array.Sort(files, new FileNameComparer());
                int nAdded = add_files(files, 0xFFFF);
                add_log(String.Format("{0}개를 목록에 추가하였습니다.", nAdded));
            }
        }
        private void list_menu_delete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in list_file.SelectedItems)
            {
                list_file.Items.Remove(item);
            }
        }
        private void contextMenu_list_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextMenu_list.Items[1].Enabled = (list_file.SelectedItems.Count > 0);
            contextMenu_list.Items[2].Enabled = (list_file.Items.Count > 0);
            contextMenu_list.Items[4].Enabled = (list_file.Items.Count > 0);
            contextMenu_list.Items[5].Enabled = (list_file.Items.Count > 0);
        }

#if HWP2PDF_WINDOWS
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)] // Added SetLastError
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)] // Added SetLastError
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
#endif

        private void btn_config_Click(object sender, EventArgs e)
        {
            FormConfig dlg = new FormConfig();
            dlg.setOption(option_overwrite, option_source_ext_flag, option_PDF_print, m_strPrinter, m_nPrintMethod);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                option_overwrite = dlg.get_option_overwrite();
                option_source_ext_flag = dlg.get_option_extflag();
                option_PDF_print = dlg.get_option_PDF_print();
                m_strPrinter = dlg.get_option_printer();
                m_nPrintMethod = dlg.get_option_printmethod();
            }
        }

        private void combo_target_format_SelectionChangeCommitted(object sender, EventArgs e)
        {
            st_convert_target_index = combo_target_format.SelectedIndex;
        }
    }
}

static class CONST
{
    public const int COL_NAME = 0;
    public const int COL_STATUS = 1;
    public const int COL_SIZE = 2;
    public const int COL_PATH = 3;
    public const int COL_TOTAL = 4; //전체 개수
}


public class FileNameComparer : IComparer<string>
{

    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    static extern int StrCmpLogicalW(String x, String y);

    public int Compare(string x, string y)
    {
        return StrCmpLogicalW(x, y);
    }

}
