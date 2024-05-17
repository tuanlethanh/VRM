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
    public partial class frmQuaTrinhChienDau : Form
    {
        public frmQuaTrinhChienDau()
        {
            InitializeComponent();
        }

        public delegate void SaveChangedEventHandler(object sender);

        public event SaveChangedEventHandler SaveChanged;

        private readonly DatabaseContext databaseContext = new DatabaseContext();
        public QUATRINHCHIENDAU KhangChien { get; set; }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (KhangChien == null || KhangChien.ID == 0)
            {
                KhangChien = new QUATRINHCHIENDAU();
            }
            else
            {
                KhangChien = databaseContext.QUATRINHCHIENDAUs.FirstOrDefault(s => s.ID == KhangChien.ID);
            }
            KhangChien.THOIGIAN = txtThoiGian.Text;
            KhangChien.CHUCVU = txtChucVu.Text;
            KhangChien.CAPBAC = txtCapBac.Text;
            KhangChien.DONVI = txtDonVi.Text;
            KhangChien.CHIENDICH = cboChienDich.SelectedValue.ToString();
            KhangChien.LOAIKHANGCHIEN = cboLoaiKhangChien.SelectedValue.ToString();
            KhangChien.TENKHANGCHIEN = Constant.DanhMucLoaiKhangChien.FirstOrDefault(s => s.Id.ToString() == KhangChien.LOAIKHANGCHIEN)?.Name;
            KhangChien.TENCHIENDICH = Constant.DanhMucChienDich.FirstOrDefault(s => s.Id.ToString() == KhangChien.CHIENDICH)?.Name;
            SaveChanged(KhangChien);
            DialogResult = DialogResult.OK;
        }

        private void frmKhangChien_Load(object sender, EventArgs e)
        {
            cboLoaiKhangChien.DataSource = Constant.DanhMucLoaiKhangChien;
            cboLoaiKhangChien.DisplayMember = "Name";
            cboLoaiKhangChien.ValueMember = "Id";
            cboLoaiKhangChien.SelectedValue = "KHANG_CHIEN_CHONG_PHAP";

            cboChienDich.DataSource = Constant.DanhMucChienDich;
            cboChienDich.DisplayMember = "Name";
            cboChienDich.ValueMember = "Id";

            if (KhangChien == null)
            {
                KhangChien = new QUATRINHCHIENDAU();
            }
            txtThoiGian.Text = KhangChien.THOIGIAN;
            txtChucVu.Text = KhangChien.CHUCVU;
            txtCapBac.Text = KhangChien.CAPBAC;
            txtDonVi.Text = KhangChien.DONVI;
            if (!string.IsNullOrEmpty(KhangChien.LOAIKHANGCHIEN))
                cboLoaiKhangChien.SelectedValue = KhangChien.LOAIKHANGCHIEN;
            if (!string.IsNullOrEmpty(KhangChien.CHIENDICH))
                cboChienDich.SelectedValue = KhangChien.CHIENDICH;
        }

        private void cboLoaiKhangChien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiKhangChien.SelectedValue != null)
            {
                
                cboChienDich.DataSource = Constant.DanhMucChienDich.Where(s => s.ParentId == cboLoaiKhangChien.SelectedValue).ToList();
                cboChienDich.DisplayMember = "Name";
                cboChienDich.ValueMember = "Id";
            }
        }
    }
}
