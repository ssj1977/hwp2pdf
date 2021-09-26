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
        public FormConfig()
        {
            InitializeComponent();
        }
        public void setOption(int option_overwrite)
        {
            m_option_overwite = option_overwrite;
        }
        public int get_option_overwrite()
        {
            return m_option_overwite;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (radioButton_newname.Checked == true) m_option_overwite = 0;
            else if (radioButton_skip.Checked == true) m_option_overwite = 1;
            else if (radioButton_overwrite.Checked == true) m_option_overwite = 2;
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
        }
    }
}
