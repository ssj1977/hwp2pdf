namespace hwp2pdf
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.col_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_fullpath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.list_file = new System.Windows.Forms.ListView();
            this.contextMenu_list = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.list_menu_add = new System.Windows.Forms.ToolStripMenuItem();
            this.list_menu_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.list_menu_clear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.list_menu_convert = new System.Windows.Forms.ToolStripMenuItem();
            this.list_menu_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList_file = new System.Windows.Forms.ImageList(this.components);
            this.btn_convert = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.text_log = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.text_title = new System.Windows.Forms.TextBox();
            this.axHwpCtrl1 = new AxHWPCONTROLLib.AxHwpCtrl();
            this.btnSavePath = new System.Windows.Forms.Button();
            this.textSavePath = new System.Windows.Forms.TextBox();
            this.contextMenu_list.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axHwpCtrl1)).BeginInit();
            this.SuspendLayout();
            // 
            // col_name
            // 
            this.col_name.Text = "파일이름";
            this.col_name.Width = 220;
            // 
            // col_path
            // 
            this.col_path.Text = "파일위치";
            this.col_path.Width = 180;
            // 
            // col_status
            // 
            this.col_status.Text = "처리상태";
            this.col_status.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.col_status.Width = 80;
            // 
            // col_fullpath
            // 
            this.col_fullpath.Text = "전체경로";
            this.col_fullpath.Width = 0;
            // 
            // list_file
            // 
            this.list_file.AllowDrop = true;
            this.list_file.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list_file.AutoArrange = false;
            this.list_file.BackColor = System.Drawing.SystemColors.Window;
            this.list_file.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.list_file.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_name,
            this.col_path,
            this.col_status,
            this.col_fullpath});
            this.list_file.ContextMenuStrip = this.contextMenu_list;
            this.list_file.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.list_file.FullRowSelect = true;
            this.list_file.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list_file.HideSelection = false;
            this.list_file.Location = new System.Drawing.Point(12, 98);
            this.list_file.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.list_file.MinimumSize = new System.Drawing.Size(300, 200);
            this.list_file.Name = "list_file";
            this.list_file.Size = new System.Drawing.Size(525, 259);
            this.list_file.SmallImageList = this.imageList_file;
            this.list_file.TabIndex = 1;
            this.list_file.UseCompatibleStateImageBehavior = false;
            this.list_file.View = System.Windows.Forms.View.Details;
            this.list_file.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.list_file_DrawColumnHeader);
            this.list_file.DragDrop += new System.Windows.Forms.DragEventHandler(this.list_file_DragDrop);
            this.list_file.DragEnter += new System.Windows.Forms.DragEventHandler(this.list_file_DragEnter);
            // 
            // contextMenu_list
            // 
            this.contextMenu_list.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.list_menu_add,
            this.list_menu_delete,
            this.list_menu_clear,
            this.toolStripSeparator1,
            this.list_menu_convert,
            this.list_menu_exit});
            this.contextMenu_list.Name = "contextMenu_list";
            this.contextMenu_list.Size = new System.Drawing.Size(197, 142);
            this.contextMenu_list.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_list_Opening);
            // 
            // list_menu_add
            // 
            this.list_menu_add.Name = "list_menu_add";
            this.list_menu_add.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.list_menu_add.Size = new System.Drawing.Size(196, 22);
            this.list_menu_add.Text = "항목 추가하기...";
            this.list_menu_add.Click += new System.EventHandler(this.list_menu_add_Click);
            // 
            // list_menu_delete
            // 
            this.list_menu_delete.Name = "list_menu_delete";
            this.list_menu_delete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.list_menu_delete.Size = new System.Drawing.Size(196, 22);
            this.list_menu_delete.Text = "항목 지우기";
            this.list_menu_delete.Click += new System.EventHandler(this.list_menu_delete_Click);
            // 
            // list_menu_clear
            // 
            this.list_menu_clear.Name = "list_menu_clear";
            this.list_menu_clear.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Delete)));
            this.list_menu_clear.Size = new System.Drawing.Size(196, 22);
            this.list_menu_clear.Text = "목록 초기화";
            this.list_menu_clear.Click += new System.EventHandler(this.list_menu_clear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // list_menu_convert
            // 
            this.list_menu_convert.Name = "list_menu_convert";
            this.list_menu_convert.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.list_menu_convert.Size = new System.Drawing.Size(196, 22);
            this.list_menu_convert.Text = "변환 시작";
            this.list_menu_convert.Click += new System.EventHandler(this.list_menu_convert_Click);
            // 
            // list_menu_exit
            // 
            this.list_menu_exit.Name = "list_menu_exit";
            this.list_menu_exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.list_menu_exit.Size = new System.Drawing.Size(196, 22);
            this.list_menu_exit.Text = "종료";
            this.list_menu_exit.Click += new System.EventHandler(this.list_menu_exit_Click);
            // 
            // imageList_file
            // 
            this.imageList_file.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_file.ImageStream")));
            this.imageList_file.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_file.Images.SetKeyName(0, "hwp.png");
            // 
            // btn_convert
            // 
            this.btn_convert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_convert.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_convert.Location = new System.Drawing.Point(549, 100);
            this.btn_convert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_convert.Name = "btn_convert";
            this.btn_convert.Size = new System.Drawing.Size(120, 50);
            this.btn_convert.TabIndex = 2;
            this.btn_convert.Text = "변환";
            this.btn_convert.UseVisualStyleBackColor = true;
            this.btn_convert.Click += new System.EventHandler(this.btn_convert_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_clear.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_clear.Location = new System.Drawing.Point(549, 161);
            this.btn_clear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(120, 50);
            this.btn_clear.TabIndex = 3;
            this.btn_clear.Text = "목록 초기화";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // text_log
            // 
            this.text_log.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_log.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.text_log.Location = new System.Drawing.Point(12, 364);
            this.text_log.MinimumSize = new System.Drawing.Size(300, 4);
            this.text_log.Multiline = true;
            this.text_log.Name = "text_log";
            this.text_log.ReadOnly = true;
            this.text_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.text_log.Size = new System.Drawing.Size(525, 50);
            this.text_log.TabIndex = 6;
            this.text_log.WordWrap = false;
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(550, 363);
            this.btn_close.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(120, 50);
            this.btn_close.TabIndex = 4;
            this.btn_close.Text = "종료";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // text_title
            // 
            this.text_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text_title.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.text_title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.text_title.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.text_title.Location = new System.Drawing.Point(12, 12);
            this.text_title.MinimumSize = new System.Drawing.Size(300, 4);
            this.text_title.Multiline = true;
            this.text_title.Name = "text_title";
            this.text_title.ReadOnly = true;
            this.text_title.Size = new System.Drawing.Size(658, 50);
            this.text_title.TabIndex = 8;
            this.text_title.TabStop = false;
            this.text_title.Text = "설치된 \'한글\'을 활용하여 HWP 파일을 PDF 파일로 변환합니다.\r\n아래 목록에 HWP 파일을 끌어서 놓기로 추가한 후 \'변환\' 버튼을 클릭해 " +
    "주세요.";
            // 
            // axHwpCtrl1
            // 
            this.axHwpCtrl1.Enabled = true;
            this.axHwpCtrl1.Location = new System.Drawing.Point(555, 228);
            this.axHwpCtrl1.Name = "axHwpCtrl1";
            this.axHwpCtrl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axHwpCtrl1.OcxState")));
            this.axHwpCtrl1.Size = new System.Drawing.Size(114, 62);
            this.axHwpCtrl1.TabIndex = 9;
            this.axHwpCtrl1.TabStop = false;
            this.axHwpCtrl1.Visible = false;
            // 
            // btnSavePath
            // 
            this.btnSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavePath.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSavePath.Location = new System.Drawing.Point(549, 68);
            this.btnSavePath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSavePath.Name = "btnSavePath";
            this.btnSavePath.Size = new System.Drawing.Size(120, 24);
            this.btnSavePath.TabIndex = 10;
            this.btnSavePath.Text = "저장경로 변경...";
            this.btnSavePath.UseVisualStyleBackColor = true;
            this.btnSavePath.Click += new System.EventHandler(this.btnSavePath_Click);
            // 
            // textSavePath
            // 
            this.textSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSavePath.Location = new System.Drawing.Point(12, 68);
            this.textSavePath.Name = "textSavePath";
            this.textSavePath.ReadOnly = true;
            this.textSavePath.Size = new System.Drawing.Size(525, 23);
            this.textSavePath.TabIndex = 12;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.ClientSize = new System.Drawing.Size(684, 426);
            this.Controls.Add(this.textSavePath);
            this.Controls.Add(this.btnSavePath);
            this.Controls.Add(this.axHwpCtrl1);
            this.Controls.Add(this.text_title);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.text_log);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_convert);
            this.Controls.Add(this.list_file);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(580, 420);
            this.Name = "FormMain";
            this.Text = "HWP -> PDF 변환기";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.contextMenu_list.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axHwpCtrl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader col_name;
        private System.Windows.Forms.ColumnHeader col_path;
        private System.Windows.Forms.ColumnHeader col_status;
        private System.Windows.Forms.ColumnHeader col_fullpath;
        private System.Windows.Forms.ListView list_file;
        private System.Windows.Forms.ImageList imageList_file;
        private System.Windows.Forms.Button btn_convert;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.TextBox text_log;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.TextBox text_title;
        private AxHWPCONTROLLib.AxHwpCtrl axHwpCtrl1;
        private System.Windows.Forms.Button btnSavePath;
        private System.Windows.Forms.TextBox textSavePath;
        private System.Windows.Forms.ContextMenuStrip contextMenu_list;
        private System.Windows.Forms.ToolStripMenuItem list_menu_add;
        private System.Windows.Forms.ToolStripMenuItem list_menu_delete;
        private System.Windows.Forms.ToolStripMenuItem list_menu_clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem list_menu_convert;
        private System.Windows.Forms.ToolStripMenuItem list_menu_exit;
    }
}

