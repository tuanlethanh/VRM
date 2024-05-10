namespace VRM.Forms
{
    partial class frmKhenThuong
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboLoaiKhenThuong = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNamKhenThuong = new System.Windows.Forms.NumericUpDown();
            this.txtNoiDungKhenThuong = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtNamKhenThuong)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Image = global::VRM.Properties.Resources.logout;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(275, 169);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 35);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Đóng";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::VRM.Properties.Resources.diskette;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(194, 169);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 35);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Lưu";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboLoaiKhenThuong
            // 
            this.cboLoaiKhenThuong.FormattingEnabled = true;
            this.cboLoaiKhenThuong.Location = new System.Drawing.Point(133, 9);
            this.cboLoaiKhenThuong.Name = "cboLoaiKhenThuong";
            this.cboLoaiKhenThuong.Size = new System.Drawing.Size(217, 21);
            this.cboLoaiKhenThuong.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Nội dung khen thưởng:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Năm khen thưởng:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Loại khen thưởng:";
            // 
            // txtNamKhenThuong
            // 
            this.txtNamKhenThuong.Location = new System.Drawing.Point(133, 36);
            this.txtNamKhenThuong.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.txtNamKhenThuong.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.txtNamKhenThuong.Name = "txtNamKhenThuong";
            this.txtNamKhenThuong.Size = new System.Drawing.Size(217, 20);
            this.txtNamKhenThuong.TabIndex = 2;
            this.txtNamKhenThuong.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            // 
            // txtNoiDungKhenThuong
            // 
            this.txtNoiDungKhenThuong.Location = new System.Drawing.Point(133, 68);
            this.txtNoiDungKhenThuong.Name = "txtNoiDungKhenThuong";
            this.txtNoiDungKhenThuong.Size = new System.Drawing.Size(217, 96);
            this.txtNoiDungKhenThuong.TabIndex = 3;
            this.txtNoiDungKhenThuong.Text = "";
            // 
            // frmKhenThuong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 214);
            this.Controls.Add(this.txtNoiDungKhenThuong);
            this.Controls.Add(this.txtNamKhenThuong);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cboLoaiKhenThuong);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKhenThuong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thi đua khen thưởng";
            this.Load += new System.EventHandler(this.frmKhenThuong_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtNamKhenThuong)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboLoaiKhenThuong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtNamKhenThuong;
        private System.Windows.Forms.RichTextBox txtNoiDungKhenThuong;
    }
}