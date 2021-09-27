
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
            this.groupBox_overwrite_option.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_overwrite_option
            // 
            this.groupBox_overwrite_option.Controls.Add(this.radioButton_overwrite);
            this.groupBox_overwrite_option.Controls.Add(this.radioButton_skip);
            this.groupBox_overwrite_option.Controls.Add(this.radioButton_newname);
            this.groupBox_overwrite_option.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox_overwrite_option.Location = new System.Drawing.Point(12, 17);
            this.groupBox_overwrite_option.Name = "groupBox_overwrite_option";
            this.groupBox_overwrite_option.Size = new System.Drawing.Size(535, 146);
            this.groupBox_overwrite_option.TabIndex = 0;
            this.groupBox_overwrite_option.TabStop = false;
            this.groupBox_overwrite_option.Text = "변환할 파일과 같은 이름의 파일이 있을때 처리 방법";
            // 
            // radioButton_overwrite
            // 
            this.radioButton_overwrite.AutoSize = true;
            this.radioButton_overwrite.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_overwrite.Location = new System.Drawing.Point(18, 104);
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
            this.radioButton_skip.Location = new System.Drawing.Point(18, 73);
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
            this.radioButton_newname.Location = new System.Drawing.Point(18, 42);
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
            this.btnCancel.Location = new System.Drawing.Point(437, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 42);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.Location = new System.Drawing.Point(318, 179);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(110, 42);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(559, 235);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_overwrite_option;
        private System.Windows.Forms.RadioButton radioButton_overwrite;
        private System.Windows.Forms.RadioButton radioButton_skip;
        private System.Windows.Forms.RadioButton radioButton_newname;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}