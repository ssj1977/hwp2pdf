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
        bool m_option_PDF_print = true;
        string m_strPrinter = "";
        int m_nPrintMethod = 0; 

        public FormConfig()
        {
            InitializeComponent();
        }
        public void setOption(int option_overwrite, int option_extflag, bool option_PDF_print, string strPrinter, int nPrintMethod)
        {
            m_option_overwite = option_overwrite;
            m_option_extflag = option_extflag;
            m_option_PDF_print = option_PDF_print;
            m_strPrinter = strPrinter;
            m_nPrintMethod = nPrintMethod;
        }
        public int get_option_overwrite()
        {
            return m_option_overwite;
        }
        public int get_option_extflag()
        {
            return m_option_extflag;
        }
        public bool get_option_PDF_print()
        {
            return m_option_PDF_print;
        }
        public string get_option_printer()
        {
            return m_strPrinter;
        }
        public int get_option_printmethod()
        {
            return m_nPrintMethod;
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
            
            m_option_PDF_print = radio_PDF_Print.Checked;
            m_strPrinter = comboBox_PDF_Printer.SelectedItem.ToString();
            int nTemp = comboBox_PDF_PrintMethod.SelectedIndex;
            if (nTemp == 0)
            {
                m_nPrintMethod = 0;
            }
            else if (nTemp > 0)
            {   // 1 = 용지맞춤, 2 = 나눠찍기, 3 = 자동 모아찍기 설정은 무시
                m_nPrintMethod = nTemp + 3;
            }
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

            if (m_option_PDF_print == true) radio_PDF_Print.Checked = true;
            else radio_PDF_SaveAs.Checked = true;
            UpdatePDFOption(m_option_PDF_print);
            // PDF 변환용 프린터를 콤보박스에 추가
            System.Collections.ArrayList printer_names
                = new System.Collections.ArrayList(System.Drawing.Printing.PrinterSettings.InstalledPrinters);
            for (int i = 0; i < printer_names.Count; i++)
            {
                string name = printer_names[i].ToString();
                if (name.Contains("PDF"))
                {
                    int nIndex = comboBox_PDF_Printer.Items.Add(name);
                    if (name == m_strPrinter) comboBox_PDF_Printer.SelectedIndex = nIndex;
                }
            }
            if (comboBox_PDF_Printer.Items.Count > 0 && comboBox_PDF_Printer.SelectedIndex <= 0) 
                comboBox_PDF_Printer.SelectedIndex = 0;

            //모아찍기 설정값 선택
            if (m_nPrintMethod == 0)
            {
                comboBox_PDF_PrintMethod.SelectedIndex = 0;
            }
            else if (m_nPrintMethod > 3)
            {   // 1 = 용지맞춤, 2 = 나눠찍기, 3 = 자동 모아찍기 설정은 무시
                comboBox_PDF_PrintMethod.SelectedIndex = m_nPrintMethod - 3;
            }
        }
        private void UpdatePDFOption(bool bPDF)
        {
            label_PDF_Printer.Enabled = bPDF;
            label_PDF_PrintMethod.Enabled = bPDF;
            comboBox_PDF_Printer.Enabled = bPDF;
            comboBox_PDF_PrintMethod.Enabled = bPDF;
        }
        private void radio_PDF_SaveAs_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePDFOption(false);
        }

        private void radio_PDF_Print_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePDFOption(true);
        }
    }
}
