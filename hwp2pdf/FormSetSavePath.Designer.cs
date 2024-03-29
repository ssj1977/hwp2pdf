﻿namespace hwp2pdf
{
    partial class FormSetSavePath
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetSavePath));
            this.radioCurrentPath = new System.Windows.Forms.RadioButton();
            this.radioSpecificPath = new System.Windows.Forms.RadioButton();
            this.textSavePath = new System.Windows.Forms.TextBox();
            this.btnSavePath = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioCurrentPath
            // 
            this.radioCurrentPath.AutoSize = true;
            this.radioCurrentPath.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioCurrentPath.Location = new System.Drawing.Point(12, 12);
            this.radioCurrentPath.Name = "radioCurrentPath";
            this.radioCurrentPath.Size = new System.Drawing.Size(296, 25);
            this.radioCurrentPath.TabIndex = 0;
            this.radioCurrentPath.TabStop = true;
            this.radioCurrentPath.Text = "원본 파일과 같은 경로에 저장합니다.";
            this.radioCurrentPath.UseVisualStyleBackColor = true;
            this.radioCurrentPath.CheckedChanged += new System.EventHandler(this.radioCurrentPath_CheckedChanged);
            // 
            // radioSpecificPath
            // 
            this.radioSpecificPath.AutoSize = true;
            this.radioSpecificPath.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioSpecificPath.Location = new System.Drawing.Point(12, 53);
            this.radioSpecificPath.Name = "radioSpecificPath";
            this.radioSpecificPath.Size = new System.Drawing.Size(312, 25);
            this.radioSpecificPath.TabIndex = 1;
            this.radioSpecificPath.TabStop = true;
            this.radioSpecificPath.Text = "아래 지정한 폴더에 모아서 저장합니다.";
            this.radioSpecificPath.UseVisualStyleBackColor = true;
            this.radioSpecificPath.CheckedChanged += new System.EventHandler(this.radioSpecificPath_CheckedChanged);
            // 
            // textSavePath
            // 
            this.textSavePath.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textSavePath.Location = new System.Drawing.Point(12, 88);
            this.textSavePath.Name = "textSavePath";
            this.textSavePath.Size = new System.Drawing.Size(452, 29);
            this.textSavePath.TabIndex = 3;
            // 
            // btnSavePath
            // 
            this.btnSavePath.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSavePath.Location = new System.Drawing.Point(334, 53);
            this.btnSavePath.Name = "btnSavePath";
            this.btnSavePath.Size = new System.Drawing.Size(130, 29);
            this.btnSavePath.TabIndex = 2;
            this.btnSavePath.Text = "폴더 찾기...";
            this.btnSavePath.UseVisualStyleBackColor = true;
            this.btnSavePath.Click += new System.EventHandler(this.btnSavePath_Click);
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.Location = new System.Drawing.Point(217, 136);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(111, 40);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "확인";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(353, 136);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 40);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormSetSavePath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(477, 191);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSavePath);
            this.Controls.Add(this.textSavePath);
            this.Controls.Add(this.radioSpecificPath);
            this.Controls.Add(this.radioCurrentPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetSavePath";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "변환한 파일을 저장할 경로를 설정합니다.";
            this.Load += new System.EventHandler(this.FormSetSavePath_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioCurrentPath;
        private System.Windows.Forms.RadioButton radioSpecificPath;
        private System.Windows.Forms.TextBox textSavePath;
        private System.Windows.Forms.Button btnSavePath;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}