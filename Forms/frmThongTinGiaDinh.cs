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
using VRM.Utilities;

namespace VRM.Forms
{
    public partial class frmThongTinGiaDinh : Form
    {
        public frmThongTinGiaDinh()
        {
            InitializeComponent();
        }

        public delegate void SaveChangedEventHandler(object sender);

        public event SaveChangedEventHandler SaveChanged;

        private readonly DatabaseContext databaseContext = new DatabaseContext();
        public THONGTINGIADINH GiaDinh { get; set; }

        private void frmThongTinGiaDinh_Load(object sender, EventArgs e)
        {
            cboQuanHe.DataSource = Constant.DanhMucMoiQuanHe;
            cboQuanHe.DisplayMember = "Name";
            cboQuanHe.ValueMember = "Id";

            if (GiaDinh == null)
            {
                GiaDinh = new THONGTINGIADINH();
            }
            txtHoTen.Text = GiaDinh.HOTEN;
            txtDiaChi.Text = GiaDinh.DIACHIHIENNAY;
            txtNamSinh.Value = GiaDinh.NAMSINH == 0 ? 1900 : GiaDinh.NAMSINH;
            txtQueQuan.Text = GiaDinh.QUEQUAN;
            chkChatDocDaCam.Checked = GiaDinh.CHATDOCDACAM;
            if (!string.IsNullOrEmpty(GiaDinh.QUANHE))
                cboQuanHe.SelectedValue = GiaDinh.QUANHE;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (GiaDinh == null || GiaDinh.ID == 0)
            {
                GiaDinh = new THONGTINGIADINH();
            }
            else
            {
                GiaDinh = databaseContext.THONGTINGIADINHs.FirstOrDefault(s => s.ID == GiaDinh.ID);
            }

            GiaDinh.DIACHIHIENNAY = txtDiaChi.Text;
            GiaDinh.QUEQUAN = txtQueQuan.Text;
            GiaDinh.NAMSINH = txtNamSinh.Value;
            GiaDinh.HOTEN = txtHoTen.Text;
            GiaDinh.CHATDOCDACAM = chkChatDocDaCam.Checked;
            GiaDinh.QUANHE = cboQuanHe.SelectedValue.ToString();
            SaveChanged(GiaDinh);
            DialogResult = DialogResult.OK;
        }
    }
}
