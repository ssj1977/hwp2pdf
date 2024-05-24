using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using HwpObjectLib;

namespace hwp2pdf
{
    public partial class FormMain : Form
    {
        string m_strPrinter = ""; //PDF 변환용 가상 프린터 이름
        int m_nPrintMethod = 1;
        bool m_bUseCurrentPath = true;
        string m_strSavePath = "";
        static bool st_bConverting = false;
        static string st_format = "PDF";
        int option_overwrite = 0; // 0:새이름으로 저장, 1:변환스킵, 2:덮어쓰기
        int option_extflag = (1 | 2 | 4); //비트플래그 타입 : 1 = hwp / 2 = hwpx / 4 = hml / 8 = doc / 16 =docx / 32 = rtf / 64 = txt
        bool option_PDF_print = true;
        public FormMain()
        {
            InitializeComponent();
            //ini 파일에서 정보 불러오기
            String ini_path = System.Windows.Forms.Application.StartupPath + "\\hwp2pdf.ini";
            StringBuilder strTemp = new StringBuilder(1024, 1024);
            GetPrivateProfileString("Main", "SavePath", "", strTemp, strTemp.Capacity, ini_path);
            m_strSavePath = strTemp.ToString();
            GetPrivateProfileString("Main", "SaveToCurrentPath", "", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) m_bUseCurrentPath = bool.Parse(strTemp.ToString());
            GetPrivateProfileString("Main", "OptionOverwrite", "", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) option_overwrite = int.Parse(strTemp.ToString());
            GetPrivateProfileString("Main", "OptionExtFlag", "", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) option_extflag = int.Parse(strTemp.ToString());
            GetPrivateProfileString("Main", "OptionPDFPrint", "", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) option_PDF_print = bool.Parse(strTemp.ToString());
            GetPrivateProfileString("Main", "PrinterName", "", strTemp, strTemp.Capacity, ini_path);
            m_strPrinter = strTemp.ToString();
            GetPrivateProfileString("Main", "PrintMethod", "", strTemp, strTemp.Capacity, ini_path);
            if (strTemp.Length > 0) m_nPrintMethod = int.Parse(strTemp.ToString());
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

            // FilePathCheckerModuleExample.DLL이 있어야 OCX컨트롤이 파일에 바로 접근 가능
            // 초기화를 위해서는 레지스트리 "\HKEY_CURRENT_USER\SOFTWARE\HNC\HwpCtrl\Modules"에 
            // FilePathCheckerModuleExample 값으로 DLL의 위치가 등록되어 있어야 함
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("HNC", true);
            if (reg == null)
            {
                MessageBox.Show("한컴오피스 한글2010 이상 버전이 설치되어 있지 않습니다.");
                return;
            }
            reg = reg.CreateSubKey("HwpCtrl");
            if (reg == null)
            {
                MessageBox.Show("레지스트리 항목(HwpCtrl) 추가 중 오류가 발생했습니다.");
                return;
            }
            else
            {
                reg = reg.CreateSubKey("Modules");
            }
            if (reg != null)
            {
                Object temp = reg.GetValue("FilePathCheckerModuleExample");
                bool bRegisterCurrentPath = false;
                // 먼저 레지스트리 값이 있는지 확인하고
                if (temp == null)
                {
                    add_log("레지스트리에 FilePathCheckerModuleExample.DLL이 등록되어 있지 않습니다.");
                    //레지스트리 값이 없다면 현재 실행경로에 DLL이 있는지 찾아서 등록 시도
                    bRegisterCurrentPath = true;
                }
                else
                {
                    //레지스트리 값이 있는 경우 지정된 위치에 DLL이 존재하는지 확인
                    String dll_path = temp.ToString();
                    FileInfo file_info = new FileInfo(dll_path);
                    //파일이 없다면 실행파일과 같은 경로에 DLL이 있는지 재확인
                    if (file_info.Exists == false)
                    {
                        add_log("레지스트리에 지정된 경로에 FilePathCheckerModuleExample.DLL이 없습니다.");
                        bRegisterCurrentPath = true;
                    }
                }
                if (bRegisterCurrentPath)
                {
                    // 레지스트리 값이 없거나 지정된 위치에 DLL 파일이 없는 경우, 현재 실행파일과 같은 경로에 DLL이 있는지 확인
                    String dll_path = System.Windows.Forms.Application.StartupPath;
                    dll_path += "\\" + "FilePathCheckerModuleExample.DLL";
                    FileInfo file_info = new FileInfo(dll_path);
                    // 있다면 레지스트리에 값을 추가
                    if (file_info.Exists)
                    {
                        add_log("실행파일과 같은 경로의 FilePathCheckerModuleExample.DLL을 등록합니다.");
                        reg.SetValue("FilePathCheckerModuleExample", dll_path);
                    }
                    else
                    {
                        add_log("실행파일과 같은 경로에 FilePathCheckerModuleExample.DLL이 없습니다.");
                    }
                }
            }
            else
            {
                MessageBox.Show("레지스트리 항목(Modules) 추가 중 오류가 발생했습니다.");
            }
            // PDF 변환용 프린터가 설치되어 있는지 확인
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
                else if (name.Contains("PDF"))
                {
                    if (name.Contains("Hancom PDF"))
                    {
                        bPrinterInstalled = true;
                        if (bSetDefault == true)
                        {   //프린터가 설정되지 않은 상태에서 한컴PDF가 있으면 기본 프린터로 사용 
                            m_strPrinter = name;
                            break;  
                        }
                    }
                    if (name.Contains("Microsoft Print to PDF"))
                    {
                        bPrinterInstalled = true;
                        //프린터가 설정되지 않은 상태에서 한컴PDF가 없는 경우 MS PDF 사용
                        if (bSetDefault == true) m_strPrinter = name; 
                    }
                }
            }
            if (bPrinterInstalled == false)
            {
                MessageBox.Show("한컴 PDF 또는 Micosoft Print to PDF가 설치되어 있지 않습니다.");
            }

//            if (axHwpCtrl1.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample"))
//                add_log("파일 접근권한 획득에 성공하였습니다.");
//            else
//               add_log("파일 접근권한 획득에 실패했습니다. 경고창이 뜨면 [모두 허용]을 클릭하세요.");

            if (m_bUseCurrentPath == true) m_strSavePath = System.IO.Directory.GetCurrentDirectory();
            update_path();
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
                list_file.Items[nRow].SubItems[3].Text = text;
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
                btn_convert_pdf.Enabled = true;
                btn_convert_hwpx.Enabled = true;
                btn_convert_hwp.Enabled = true;
                if (bEnable)    btn_convert_pdf.Text = "PDF로 변환";
                else            btn_convert_pdf.Text = "변환 중단";
                if (bEnable)    btn_convert_hwpx.Text = "HWPX로 변환";
                else            btn_convert_hwpx.Text = "변환 중단";
                if (bEnable)    btn_convert_hwp.Text = "HWP로 변환";
                else            btn_convert_hwp.Text = "변환 중단";
            }
        }
        private void convert_files()
        {
            if (st_bConverting == true)
            {
                st_bConverting = false;
                btn_convert_pdf.Enabled = false;
                btn_convert_hwpx.Enabled = false;
                btn_convert_hwp.Enabled = false;
                return;
            }
            if (list_file.Items.Count <= 0) return;
            String[] paths = new string[list_file.Items.Count];
            int nIndex = 0;
            foreach (ListViewItem item in list_file.Items)
            {
                paths[nIndex] = Path.Combine(item.SubItems[1].Text, item.SubItems[0].Text);
                item.SubItems[3].Text = "";
                nIndex++;
            }
            Thread th = new Thread(() => convert_thread(paths, m_bUseCurrentPath, m_strSavePath));
            th.SetApartmentState(ApartmentState.STA);
            st_bConverting = true;
            enable_controls(false);
            th.Start();
        }
        private void convert_thread(string[] paths, bool bUseCurrentPath, string strSavePath)
        {
            HwpObject temp_hwp = new HwpObject();
            temp_hwp.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample");
            int nRow =0 ;
            int nConverted = 0;
            add_log("파일 변환을 시작합니다. 잠시 기다려 주세요......");
            foreach (string file_path in paths)
            {
                temp_hwp.SetMessageBoxMode(0x00224421);
                string file_ext = System.IO.Path.GetExtension(file_path).ToUpper();
                string file_type = "";
                if (file_ext.Equals(".HWP")) file_type = "HWP";
                else if (file_ext.Equals(".HWPX")) file_type = "HWPX";
                else if (file_ext.Equals(".HML")) file_type = "HWPML2X";
                else if (file_ext.Equals(".DOCX")) file_type = "OOXML";
                else if (file_ext.Equals(".DOC")) file_type = "MSWORD";
                else if (file_ext.Equals(".TXT")) file_type = "UNICODE";
                else if (file_ext.Equals(".RTF")) file_type = "RTF";
                if (file_type == st_format)
                {
                    show_convert_state(nRow, "변환안함(같은형식)");
                }
                else if (temp_hwp.Open(file_path, file_type, "lock:false;forceopen:true;")) //
                {
                    show_convert_state(nRow, "변환중");
                    string save_path = "";
                    if (bUseCurrentPath == true)  save_path = System.IO.Path.GetDirectoryName(file_path);
                    else                          save_path = strSavePath;
                    save_path += "\\" + System.IO.Path.GetFileNameWithoutExtension(file_path) + "." + st_format.ToLower();
                    //저장할 파일 이름과 겹치는 파일이 이미 있는지 확인하고 설정에 따라 처리
                    bool bSkip = false;
                    bool bChanged = false;
                    bool bOverwirte = false;
                    int temp_num = 0; 
                    while (File.Exists(save_path))
                    {
                        temp_num += 1;
                        if (option_overwrite == 0) // 이름 바꾸기
                        {
                            if (bUseCurrentPath == true) save_path = System.IO.Path.GetDirectoryName(file_path);
                            else save_path = strSavePath;
                            save_path += "\\" + System.IO.Path.GetFileNameWithoutExtension(file_path) + "(" + temp_num.ToString() + ")." + st_format.ToLower();
                            bChanged = true;
                        }
                        else if (option_overwrite == 1) //건너뛰기
                        {
                            bSkip = true;
                            break;
                        }
                        else if (option_overwrite == 2)  //덮어쓰기
                        {
                            bOverwirte = true;
                            bSkip = false;
                            break;
                        }
                        else //예상치 못한 값의 경우 건너뛰기로 처리
                        {
                            bSkip = true;
                            break;
                        }
                    }
                    if (bSkip == true)
                    {
                        show_convert_state(nRow, "변환안함(이름겹침)");
                    }
                    else
                    {
                        bool bSuccess = false;
                        if (st_format == "PDF" && option_PDF_print == true && m_strPrinter != "")
                        {
                            //PDF 파일의 경우 가상 프린터를 사용하는 방식으로 변환 가능
                            //인쇄 모아쓰기 설정을 변경할 수 있지만 인쇄 팝업이 잠시 떴다 사라짐
                            //가상 프린터를 쓰지 않는 경우는 기존의 SaveAS 방식으로 변환
                            /*temp_hwp.HAction.GetDefault("Print", temp_hwp.HParameterSet.HFileOpenSave.HSet);
                            pset.SetItem("PrintMethod", m_nPrintMethod); //인쇄방식
                            pset.SetItem("Collate", 1);
                            pset.SetItem("NumCopy", 1);
                            pset.SetItem("UserOrder", 0);
                            pset.SetItem("PrintToFile", 1);
                            pset.SetItem("Range", 0);
                            pset.SetItem("FileName", save_path); // 경로
                            pset.SetItem("PrinterName", m_strPrinter); //프린터
                            pset.SetItem("UsingPagenum", 1);
                            pset.SetItem("ReverseOrder", 0);
                            pset.SetItem("Pause", 0);
                            pset.SetItem("PrintImage", 1);
                            pset.SetItem("PrintDrawObj", 1);
                            pset.SetItem("PrintClickHere ", 0);
                            pset.SetItem("PrintFormObj", 1);
                            pset.SetItem("PrintMarkPen", 0);
                            pset.SetItem("PrintMemo", 0);
                            pset.SetItem("PrintMemoContents", 0);
                            pset.SetItem("PrintRevision", 1);
                            pset.SetItem("PrintBarcode", 1);
                            pset.SetItem("Flags", 8192);
                            pset.SetItem("Device", 3);
                            pset.SetItem("PrintPronounce", 0);
                            if (act.Execute(pset) == 1)
                            {
                                bSuccess = true;
                                Thread.Sleep(1000); //기다리지 않으면 가끔 '다른이름으로 저장' 창이 뜸
                            }*/
                        }
                        else
                        {
                            //SaveAs의 경우 PDF 변환시 HWP파일의 모아찍기 설정은 그대로 유지됨
                            bSuccess = temp_hwp.SaveAs(save_path, st_format, "");
                        }
                        if (bSuccess)
                        {
                            if (bOverwirte == true) show_convert_state(nRow, "완료(덮어씀)");
                            else if (bChanged == true) show_convert_state(nRow, "완료(이름바꿈) - " + System.IO.Path.GetFileName(save_path));
                            else show_convert_state(nRow, "완료");
                            nConverted++;
                        }
                        else show_convert_state(nRow, "변환 시도 실패");
                    }
                    temp_hwp.Clear(1);
                }
                else show_convert_state(nRow, "원본파일 열기 실패");
                nRow++;
                if (st_bConverting == false)
                {
                    add_log("변환 작업을 중단합니다.");
                    break;
                }
            }
            temp_hwp.Quit();
            add_log(String.Format("{0}개 파일 중 {1}개 파일을 변환하였습니다.", paths.Length, nConverted));
            st_bConverting = false;
            enable_controls(true);
        }
        private void btn_convert_pdf_Click(object sender, EventArgs e)
        {
            st_format = "PDF";
            convert_files();
        }
        private void list_menu_convert_pdf_Click(object sender, EventArgs e)
        {
            st_format = "PDF";
            convert_files();
        }
        private void btn_convert_hwpx_Click(object sender, EventArgs e)
        {
            st_format = "HWPX";
            convert_files();
        }
        private void list_menu_convert_hwpx_Click(object sender, EventArgs e)
        {
            st_format = "HWPX";
            convert_files();
        }
        private void btn_convert_hwp_Click(object sender, EventArgs e)
        {
            st_format = "HWP";
            convert_files();
        }
        private void list_menu_convert_hwp_Click(object sender, EventArgs e)
        {
            st_format = "HWP";
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
                string anotherpath = Path.Combine(item.SubItems[0].Text, item.SubItems[1].Text);
                if (filepath.Equals(anotherpath, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private void list_file_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Array.Sort(files, new FileNameComparer());
            int nAdded = add_files(files, 0xFFFF); //처음 파일 추가시는 모든 확장자를 인식하도록
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
                    nAdded += add_files(dirs_only, option_extflag); //폴더 내의 파일을 모두 가져올때는 정해진 확장자만
                    nAdded += add_files(files_only, option_extflag);
                }
                else
                {
                    FileInfo fInfo = new FileInfo(System.IO.Path.GetFullPath(file));
                    string file_size = GetFileSizeString(fInfo.Length);
                    string file_ext = fInfo.Extension.ToUpper();
                    if ((file_ext.Equals(".HWP") && ((extflag & 1) != 0))
                        || (file_ext.Equals(".HWPX") && ((extflag & 2) != 0))
                        || (file_ext.Equals(".HML") && ((extflag & 4) != 0))
                        || (file_ext.Equals(".DOC") && ((extflag & 8) != 0))
                        || (file_ext.Equals(".DOCX") && ((extflag & 16) != 0))
                        || (file_ext.Equals(".RTF") && ((extflag & 32) != 0))
                        || (file_ext.Equals(".TXT") && ((extflag & 64) != 0)))
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
                            newItem.SubItems.Add(fInfo.DirectoryName);
                            newItem.SubItems.Add(GetFileSizeString(fInfo.Length)); // 크기
                            newItem.SubItems.Add(""); // 처리상태
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
            String ini_path = System.Windows.Forms.Application.StartupPath + "\\hwp2pdf.ini";
            WritePrivateProfileString("Main", "SaveToCurrentPath", m_bUseCurrentPath.ToString(), ini_path);
            if (m_bUseCurrentPath == true) m_strSavePath = "";
            WritePrivateProfileString("Main", "SavePath", m_strSavePath, ini_path);
            WritePrivateProfileString("Main", "OptionOverwrite", option_overwrite.ToString(), ini_path);
            WritePrivateProfileString("Main", "OptionExtFlag", option_extflag.ToString(), ini_path);
            WritePrivateProfileString("Main", "OptionPDFPrint", option_PDF_print.ToString(), ini_path);
            WritePrivateProfileString("Main", "PrinterName", m_strPrinter, ini_path);
            WritePrivateProfileString("Main", "PrintMethod", m_nPrintMethod.ToString(), ini_path);
            String strTemp;
            if (WindowState == FormWindowState.Maximized || WindowState==FormWindowState.Minimized)
            {
                strTemp = RestoreBounds.Location.X.ToString() + ',' + RestoreBounds.Location.Y.ToString()
                    + ',' + RestoreBounds.Size.Width.ToString() + ',' + RestoreBounds.Size.Height.ToString();
            }
            else
            {
                strTemp = this.Location.X.ToString() + ',' + this.Location.Y.ToString()
                    + ',' + this.Size.Width.ToString() + ',' + this.Size.Height.ToString();
            }
            WritePrivateProfileString("Main", "Bounds", strTemp, ini_path);
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

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private void btn_config_Click(object sender, EventArgs e)
        {
            FormConfig dlg = new FormConfig();
            dlg.setOption(option_overwrite, option_extflag, option_PDF_print, m_strPrinter, m_nPrintMethod);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                option_overwrite = dlg.get_option_overwrite();
                option_extflag = dlg.get_option_extflag();
                option_PDF_print = dlg.get_option_PDF_print();
                m_strPrinter = dlg.get_option_printer();
                m_nPrintMethod = dlg.get_option_printmethod();
            }
        }
    }
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
