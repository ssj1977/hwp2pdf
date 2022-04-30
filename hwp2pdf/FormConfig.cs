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
    public partial class FormConfig : Form
    {
        int m_option_overwite = 0;
        int m_option_extflag = 0; //비트플래그 타입 : 1 = hwp / 2 = hwpx / 4 = hml / 8 = doc / 16 =docx / 32 = rtf / 64 = txt
        public FormConfig()
        {
            InitializeComponent();
        }
        public void setOption(int option_overwrite, int option_extflag)
        {
            m_option_overwite = option_overwrite;
            m_option_extflag = option_extflag;
        }
        public int get_option_overwrite()
        {
            return m_option_overwite;
        }
        public int get_option_extflag()
        {
            return m_option_extflag;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (radioButton_newname.Checked == true) m_option_overwite = 0;
            else if (radioButton_skip.Checked == true) m_option_overwite = 1;
            else if (radioButton_overwrite.Checked == true) m_option_overwite = 2;

            m_option_extflag = 0;
            if (cb_hwp.Checked == true) m_option_extflag = m_option_extflag | 1;
            if (cb_hwpx.Checked == true) m_option_extflag = m_option_extflag | 2;
            if (cb_hml.Checked == true) m_option_extflag = m_option_extflag | 4;
            if (cb_doc.Checked == true) m_option_extflag = m_option_extflag | 8;
            if (cb_docx.Checked == true) m_option_extflag = m_option_extflag | 16;
            if (cb_rtf.Checked == true) m_option_extflag = m_option_extflag | 32;
            if (cb_txt.Checked == true) m_option_extflag = m_option_extflag | 64;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            if (m_option_overwite == 0) radioButton_newname.Checked = true;
            else if (m_option_overwite == 1) radioButton_skip.Checked = true;
            else if (m_option_overwite == 2) radioButton_overwrite.Checked = true;

            if ((m_option_extflag & 1) != 0) cb_hwp.Checked = true;
            if ((m_option_extflag & 2) != 0) cb_hwpx.Checked = true;
            if ((m_option_extflag & 4) != 0) cb_hml.Checked = true;
            if ((m_option_extflag & 8) != 0) cb_doc.Checked = true;
            if ((m_option_extflag & 16) != 0) cb_docx.Checked = true;
            if ((m_option_extflag & 32) != 0) cb_rtf.Checked = true;
            if ((m_option_extflag & 64) != 0) cb_txt.Checked = true;
        }
    }
}
