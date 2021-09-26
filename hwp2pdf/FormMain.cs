using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using AxHWPCONTROLLib;


namespace hwp2pdf
{
    public partial class FormMain : Form
    {
        bool m_bUseCurrentPath = true;
        string m_strSavePath = "";
        static bool st_bConverting = false;
        static string st_format = "PDF";
        int option_overwrite = 0; // 0:새이름으로 저장, 1:변환스킵, 2:덮어쓰기
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
            // FilePathCheckerModuleExample.DLL이 있어야 OCX컨트롤이 파일에 바로 접근 가능
            // 초기화를 위해서는 레지스트리 "\HKEY_CURRENT_USER\SOFTWARE\HNC\HwpCtrl\Modules"에 
            // FilePathCheckerModuleExample 값으로 DLL의 위치가 등록되어 있어야 함
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("HNC", true);
            if (reg == null)
            {
                MessageBox.Show("한글(2010 이상 버전)이 설치되어 있지 않습니다.");
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
                MessageBox.Show("레지스트리 항목(Modules) 추가 중 오류가 발생했습니다..");
            }

            if (axHwpCtrl1.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample"))
                add_log("파일 접근권한 획득에 성공하였습니다.");
            else
                add_log("파일 접근권한 획득에 실패했습니다. 경고창이 뜨면 [모두 허용]을 클릭하세요.");

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
                list_file.Items[nRow].SubItems[2].Text = text;
                list_file.RedrawItems(nRow, nRow, false);
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
                if (bEnable)    btn_convert_pdf.Text = "변환";
                else            btn_convert_pdf.Text = "변환 중단";
            }
        }
        private void convert_files()
        {
            if (st_bConverting == true)
            {
                st_bConverting = false;
                btn_convert_pdf.Enabled = false;
                return;
            }
            if (list_file.Items.Count <= 0) return;
            String[] paths = new string[list_file.Items.Count];
            int nIndex = 0;
            foreach (ListViewItem item in list_file.Items)
            {
                paths[nIndex] = item.SubItems[3].Text;
                item.SubItems[2].Text = "";
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
            AxHwpCtrl temp_hwp = new AxHwpCtrl();
            temp_hwp.CreateControl();
            bool bReg = temp_hwp.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample");
            int nRow =0 ;
            int nConverted = 0;
            add_log("PDF 변환을 시작합니다. 잠시 기다려 주세요......");
            foreach (string file_path in paths)
            {
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
                else if (temp_hwp.Open(file_path, file_type, ""))
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
                        if (temp_hwp.SaveAs(save_path, st_format, ""))
                        {
                            if (bOverwirte == true) show_convert_state(nRow, "완료 : 덮어씀");
                            else if (bChanged == true) show_convert_state(nRow, "완료 : " + System.IO.Path.GetFileName(save_path));
                            else show_convert_state(nRow, "완료");
                            nConverted++;
                        }
                        else show_convert_state(nRow, "열기실패");
                    }
                }
                else show_convert_state(nRow, "변환실패");
                temp_hwp.Clear();
                nRow++;
                if (st_bConverting == false)
                {
                    add_log("변환 작업을 중단합니다.");
                    break;
                }
            }
            add_log(String.Format("{0}개 파일 중 {1}개 파일을 PDF로 변환하였습니다.", paths.Length, nConverted));
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
        private void list_file_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        private bool CheckDuplication(string filepath)
        {
            foreach (ListViewItem item in list_file.Items)
            {
                if (filepath.Equals(item.SubItems[3].Text, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        private void list_file_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Array.Sort(files, new FileNameComparer());
            add_files(files);
        }
        private void add_files(string[] files)
        {
            int nTotal = 0, nAdded = 0;
            foreach (string file in files)
            {
                nTotal++;
                string file_ext = System.IO.Path.GetExtension(file).ToUpper();
                if (file_ext.Equals(".HWP") 
                    || file_ext.Equals(".HWPX")
                    || file_ext.Equals(".HML")
                    || file_ext.Equals(".DOC")
                    || file_ext.Equals(".DOCX")
                    || file_ext.Equals(".RTF")
                    || file_ext.Equals(".TXT"))
                {
                    if (CheckDuplication(System.IO.Path.GetFullPath(file)) == false)
                    {
                        ListViewItem newItem = list_file.Items.Add(System.IO.Path.GetFileName(file), 0);
                        newItem.SubItems.Add(System.IO.Path.GetDirectoryName(file));
                        newItem.SubItems.Add("");
                        newItem.SubItems.Add(System.IO.Path.GetFullPath(file));
                        nAdded++;
                    }
                }
            }
            add_log(String.Format("{0}개 파일 중 {1}개를 목록에 추가하였습니다.", nTotal, nAdded));
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
            else textSavePath.Text = "PDF 저장 경로: "+ m_strSavePath;
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
                add_files(files);
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
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private void btn_config_Click(object sender, EventArgs e)
        {
            FormConfig dlg = new FormConfig();
            dlg.setOption(option_overwrite);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                option_overwrite = dlg.get_option_overwrite();
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
