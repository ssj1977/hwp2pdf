using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace hwp2pdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // FilePathCheckerModuleExample.DLL이 있어야 OCX컨트롤이 파일에 바로 접근 가능
            // 초기화를 위해서는 레지스트리 "\HKEY_CURRENT_USER\SOFTWARE\HNC\HwpCtrl\Modules"에 
            // FilePathCheckerModuleExample 값으로 DLL의 위치가 등록되어 있어야 함
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("HNC", true);
            if (reg == null)
            {
                MessageBox.Show("한/글이 설치되어 있지 않습니다.");
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
        }

        private void btn_convert_Click(object sender, EventArgs e)
        {
            int nTotal = 0, nConverted = 0;
            add_log("PDF 변환을 시작합니다. 잠시 기다려 주세요......");
            btn_convert.Enabled = false;
            btn_clear.Enabled = false;
            foreach (ListViewItem item in list_file.Items)
            {
                nTotal++;
                string file_path = item.SubItems[3].Text;
                string file_ext = System.IO.Path.GetExtension(file_path).ToUpper();
                string file_type = "";
                if (file_ext.Equals(".HWP")) file_type = "HWP";
                else if (file_ext.Equals(".TXT")) file_type = "UNICODE";
                if (axHwpCtrl1.Open(file_path, file_type, ""))
                {
                    list_file.Items[item.Index].SubItems[2].Text = "변환중";
                    list_file.RedrawItems(item.Index, item.Index, false);
                    string pdf_path = System.IO.Path.GetDirectoryName(file_path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(file_path) + ".PDF";
                    if (axHwpCtrl1.SaveAs(pdf_path, "PDF", ""))
                    {
                        list_file.Items[item.Index].SubItems[2].Text = "완료";
                        nConverted++;
                    }
                    else list_file.Items[item.Index].SubItems[2].Text = "변환실패";
                }
                else
                {
                    list_file.Items[item.Index].SubItems[2].Text = "열기실패";
                }
                list_file.RedrawItems(item.Index,item.Index, false);
                 axHwpCtrl1.Clear();
            }
            add_log(String.Format("{0}개 파일 중 {1}개 파일을 PDF로 변환하였습니다.", nTotal, nConverted));
            btn_convert.Enabled = true;
            btn_clear.Enabled = true;
        }

        private void list_file_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void list_file_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            int nTotal = 0, nAdded = 0;
            foreach (string file in files)
            {
                nTotal++;
                string file_ext = System.IO.Path.GetExtension(file).ToUpper();
                if (file_ext.Equals(".HWP") || file_ext.Equals(".TXT"))
                {
                    ListViewItem newItem = list_file.Items.Add(System.IO.Path.GetFileName(file), 0);
                    newItem.SubItems.Add(System.IO.Path.GetDirectoryName(file));
                    newItem.SubItems.Add("");
                    newItem.SubItems.Add(System.IO.Path.GetFullPath(file));
                    nAdded++;
                }
            }
            add_log(String.Format("{0}개 파일 중 {1}개를 목록에 추가하였습니다.",nTotal, nAdded));
        }

        private void list_file_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (list_file.Items.Count > 0)
            {
                list_file.Items.Clear();
                add_log("목록을 초기화하였습니다.");
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void add_log(String text)
        {
            String time_str = DateTime.Now.ToString("(HH:mm:ss) ");
            text_log.AppendText("\r\n" + time_str + text);
        }

        private void text_title_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
