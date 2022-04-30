﻿
namespace hwp2pdf
{
    partial class FormConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfig));
            this.groupBox_overwrite_option = new System.Windows.Forms.GroupBox();
            this.radioButton_overwrite = new System.Windows.Forms.RadioButton();
            this.radioButton_skip = new System.Windows.Forms.RadioButton();
            this.radioButton_newname = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_rtf = new System.Windows.Forms.CheckBox();
            this.cb_docx = new System.Windows.Forms.CheckBox();
            this.cb_doc = new System.Windows.Forms.CheckBox();
            this.cb_hwpx = new System.Windows.Forms.CheckBox();
            this.cb_hwp = new System.Windows.Forms.CheckBox();
            this.cb_txt = new System.Windows.Forms.CheckBox();
            this.cb_hml = new System.Windows.Forms.CheckBox();
            this.groupBox_overwrite_option.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_overwrite_option
            // 
            this.groupBox_overwrite_option.Controls.Add(this.radioButton_overwrite);
            this.groupBox_overwrite_option.Controls.Add(this.radioButton_skip);
            this.groupBox_overwrite_option.Controls.Add(this.radioButton_newname);
            this.groupBox_overwrite_option.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox_overwrite_option.Location = new System.Drawing.Point(13, 17);
            this.groupBox_overwrite_option.Name = "groupBox_overwrite_option";
            this.groupBox_overwrite_option.Size = new System.Drawing.Size(469, 146);
            this.groupBox_overwrite_option.TabIndex = 0;
            this.groupBox_overwrite_option.TabStop = false;
            this.groupBox_overwrite_option.Text = "변환할 파일과 같은 이름의 파일이 있을때 처리 방법";
            // 
            // radioButton_overwrite
            // 
            this.radioButton_overwrite.AutoSize = true;
            this.radioButton_overwrite.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_overwrite.Location = new System.Drawing.Point(18, 102);
            this.radioButton_overwrite.Name = "radioButton_overwrite";
            this.radioButton_overwrite.Size = new System.Drawing.Size(204, 25);
            this.radioButton_overwrite.TabIndex = 2;
            this.radioButton_overwrite.TabStop = true;
            this.radioButton_overwrite.Text = "기존 파일에 덮어씁니다.";
            this.radioButton_overwrite.UseVisualStyleBackColor = true;
            // 
            // radioButton_skip
            // 
            this.radioButton_skip.AutoSize = true;
            this.radioButton_skip.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_skip.Location = new System.Drawing.Point(18, 70);
            this.radioButton_skip.Name = "radioButton_skip";
            this.radioButton_skip.Size = new System.Drawing.Size(258, 25);
            this.radioButton_skip.TabIndex = 1;
            this.radioButton_skip.TabStop = true;
            this.radioButton_skip.Text = "해당 파일을 변환하지 않습니다.";
            this.radioButton_skip.UseVisualStyleBackColor = true;
            // 
            // radioButton_newname
            // 
            this.radioButton_newname.AutoSize = true;
            this.radioButton_newname.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_newname.Location = new System.Drawing.Point(18, 38);
            this.radioButton_newname.Name = "radioButton_newname";
            this.radioButton_newname.Size = new System.Drawing.Size(356, 25);
            this.radioButton_newname.TabIndex = 0;
            this.radioButton_newname.TabStop = true;
            this.radioButton_newname.Text = "이름 뒤에 번호를 붙여서 새 파일을 만듭니다.";
            this.radioButton_newname.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(371, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 42);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.Location = new System.Drawing.Point(252, 342);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 42);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_hml);
            this.groupBox1.Controls.Add(this.cb_txt);
            this.groupBox1.Controls.Add(this.cb_rtf);
            this.groupBox1.Controls.Add(this.cb_docx);
            this.groupBox1.Controls.Add(this.cb_doc);
            this.groupBox1.Controls.Add(this.cb_hwpx);
            this.groupBox1.Controls.Add(this.cb_hwp);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(13, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(469, 146);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "폴더를 불러올 때 폴더 안의 파일 중에서 추가할 확장자";
            // 
            // cb_rtf
            // 
            this.cb_rtf.AutoSize = true;
            this.cb_rtf.Location = new System.Drawing.Point(288, 40);
            this.cb_rtf.Name = "cb_rtf";
            this.cb_rtf.Size = new System.Drawing.Size(56, 25);
            this.cb_rtf.TabIndex = 5;
            this.cb_rtf.Text = "RTF";
            this.cb_rtf.UseVisualStyleBackColor = true;
            // 
            // cb_docx
            // 
            this.cb_docx.AutoSize = true;
            this.cb_docx.Location = new System.Drawing.Point(159, 71);
            this.cb_docx.Name = "cb_docx";
            this.cb_docx.Size = new System.Drawing.Size(73, 25);
            this.cb_docx.TabIndex = 4;
            this.cb_docx.Text = "DOCX";
            this.cb_docx.UseVisualStyleBackColor = true;
            // 
            // cb_doc
            // 
            this.cb_doc.AutoSize = true;
            this.cb_doc.Location = new System.Drawing.Point(159, 40);
            this.cb_doc.Name = "cb_doc";
            this.cb_doc.Size = new System.Drawing.Size(63, 25);
            this.cb_doc.TabIndex = 3;
            this.cb_doc.Text = "DOC";
            this.cb_doc.UseVisualStyleBackColor = true;
            // 
            // cb_hwpx
            // 
            this.cb_hwpx.AutoSize = true;
            this.cb_hwpx.Location = new System.Drawing.Point(18, 71);
            this.cb_hwpx.Name = "cb_hwpx";
            this.cb_hwpx.Size = new System.Drawing.Size(77, 25);
            this.cb_hwpx.TabIndex = 1;
            this.cb_hwpx.Text = "HWPX";
            this.cb_hwpx.UseVisualStyleBackColor = true;
            // 
            // cb_hwp
            // 
            this.cb_hwp.AutoSize = true;
            this.cb_hwp.Location = new System.Drawing.Point(18, 40);
            this.cb_hwp.Name = "cb_hwp";
            this.cb_hwp.Size = new System.Drawing.Size(67, 25);
            this.cb_hwp.TabIndex = 0;
            this.cb_hwp.Text = "HWP";
            this.cb_hwp.UseVisualStyleBackColor = true;
            // 
            // cb_txt
            // 
            this.cb_txt.AutoSize = true;
            this.cb_txt.Location = new System.Drawing.Point(288, 71);
            this.cb_txt.Name = "cb_txt";
            this.cb_txt.Size = new System.Drawing.Size(57, 25);
            this.cb_txt.TabIndex = 6;
            this.cb_txt.Text = "TXT";
            this.cb_txt.UseVisualStyleBackColor = true;
            // 
            // cb_hml
            // 
            this.cb_hml.AutoSize = true;
            this.cb_hml.Location = new System.Drawing.Point(18, 102);
            this.cb_hml.Name = "cb_hml";
            this.cb_hml.Size = new System.Drawing.Size(64, 25);
            this.cb_hml.TabIndex = 2;
            this.cb_hml.Text = "HML";
            this.cb_hml.UseVisualStyleBackColor = true;
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(498, 398);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox_overwrite_option);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "설정";
            this.Load += new System.EventHandler(this.FormConfig_Load);
            this.groupBox_overwrite_option.ResumeLayout(false);
            this.groupBox_overwrite_option.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_overwrite_option;
        private System.Windows.Forms.RadioButton radioButton_overwrite;
        private System.Windows.Forms.RadioButton radioButton_skip;
        private System.Windows.Forms.RadioButton radioButton_newname;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_docx;
        private System.Windows.Forms.CheckBox cb_doc;
        private System.Windows.Forms.CheckBox cb_hwpx;
        private System.Windows.Forms.CheckBox cb_hwp;
        private System.Windows.Forms.CheckBox cb_rtf;
        private System.Windows.Forms.CheckBox cb_hml;
        private System.Windows.Forms.CheckBox cb_txt;
    }
}