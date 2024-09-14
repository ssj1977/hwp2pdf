using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hwp2pdf
{
    public partial class FormSetSavePath : Form
    {
        bool m_bUseCurrentPath = true;
        string m_strSavePath;

        public FormSetSavePath()
        {
            InitializeComponent();
        }
        public void setOption(bool bUseCurrentPath, string strSavePath)
        {
            m_bUseCurrentPath = bUseCurrentPath;
            m_strSavePath = strSavePath;
        }
        public bool IsUseCurrentPath()
        {
            return m_bUseCurrentPath;
        }
        public string getSavePath()
        {
            return m_strSavePath;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            m_bUseCurrentPath = radioCurrentPath.Checked;
            if (m_bUseCurrentPath==false)
            {
                System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(textSavePath.Text);
                // 있다면 레지스트리에 값을 추가
                if (info.Exists)
                {
                    m_strSavePath = textSavePath.Text;
                }
                else
                {
                    MessageBox.Show(textSavePath.Text + "\n지정된 경로를 찾을 수 없습니다.");
                    return;
                }
            }
     
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textSavePath.Text = dlg.SelectedPath;
                textSavePath.Update();
            }

        }
        private void FormSetSavePath_Load(object sender, EventArgs e)
        {
            radioCurrentPath.Checked = m_bUseCurrentPath;
            radioSpecificPath.Checked = !m_bUseCurrentPath;
            textSavePath.Text = m_strSavePath;
        }
        private void radioCurrentPath_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCurrentPath.Checked == true)
            {
                btnSavePath.Enabled = false;
                textSavePath.Enabled = false;
            }
        }
        private void radioSpecificPath_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCurrentPath.Checked == false)
            {
                btnSavePath.Enabled = true;
                textSavePath.Enabled = true;
            }
        }
    }
}
