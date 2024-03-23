using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRM.Database;
using VRM.Entities;
using VRM.Models;
using VRM.Utilities;

namespace VRM.Forms
{
    public partial class frmKhenThuong : Form
    {
        public frmKhenThuong()
        {
            InitializeComponent();
        }

        public delegate void SaveChangedEventHandler(object sender);

        public event SaveChangedEventHandler SaveChanged;

        private readonly DatabaseContext databaseContext = new DatabaseContext();
        public KHENTHUONG KhenThuong { get; set; }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (KhenThuong == null)
            {
                KhenThuong = new KHENTHUONG();
            }
            KhenThuong.NAMKHENTHUONG = txtNamKhenThuong.Value;
            KhenThuong.LOAIKHENTHUONG = cboLoaiKhenThuong.SelectedValue.ToString();
            KhenThuong.NOIDUNGKHENTHUONG = txtNoiDungKhenThuong.Text;
            SaveChanged(KhenThuong);
            DialogResult = DialogResult.OK;
        }

        private void frmKhenThuong_Load(object sender, EventArgs e)
        {
            cboLoaiKhenThuong.DataSource = Constant.DanhMucLoaiKhenThuong;
            cboLoaiKhenThuong.DisplayMember = "Name";
            cboLoaiKhenThuong.ValueMember = "Id";

            if (KhenThuong == null)
            {
                KhenThuong = new KHENTHUONG();
            }

            txtNamKhenThuong.Value = KhenThuong.NAMKHENTHUONG == 0 ? DateTime.Now.Year : KhenThuong.NAMKHENTHUONG;
            txtNoiDungKhenThuong.Text = KhenThuong.NOIDUNGKHENTHUONG;
            if (!string.IsNullOrEmpty(KhenThuong.LOAIKHENTHUONG))
                cboLoaiKhenThuong.SelectedValue = KhenThuong.LOAIKHENTHUONG;
        }
    }
}
