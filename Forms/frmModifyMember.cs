﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
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
    public partial class frmModifyMember : Form
    {
        public frmModifyMember()
        {
            InitializeComponent();
        }

        public delegate void SaveChangedEventHandler(object sender);

        public event SaveChangedEventHandler SaveChanged;

        private readonly DatabaseContext databaseContext = new DatabaseContext();

        public HOIVIEN hoivien { get; set; }
        public List<QUATRINHCHIENDAU> DanhSachQuaTrinhChienDau { get; set; } = new List<QUATRINHCHIENDAU>();
        public List<KHENTHUONG> DanhSachKhenThuong { get; set; } = new List<KHENTHUONG>();
        public List<THONGTINGIADINH> DanhSachThanhVien { get; set; } = new List<THONGTINGIADINH>();

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (hoivien == null)
            {
                hoivien = new HOIVIEN();
            }
            
            hoivien.MAHOIVIEN = txtCode.Text;
            hoivien.HOTEN = txtFullName.Text;
            hoivien.NAMSINH = txtDateOfBirth.Value;
            hoivien.GIOITINH = cboGender.SelectedText;
            hoivien.DANTOC = txtethnic.Text;
            hoivien.TONGIAO = txtCode.Text;
            hoivien.QUEQUAN = txtHomtown.Text;
            hoivien.NOICUTRU = txtResidence.Text;
            hoivien.DIACHI = txtAddress.Text;
            hoivien.SODIENTHOAI = txtPhoneNumber.Text;
            hoivien.EMAIL = txtEmail.Text;
            hoivien.TRINHDOCHUYENMON = txtQuanlify.Text;
            hoivien.TRINHDOHOCVAN = txtAcademic.Text;
            hoivien.LYLUANCHINHTRI = txtPoliticalTheory.Text;
            hoivien.TRINHDOHOCVAN = txtAcademic.Text;
            hoivien.NGAYVAOHOI = txtNgayVaoHoi.Value;
            hoivien.NGAYCAPTHE = txtIssueCardDate.Value;
            hoivien.DANGVIEN = chkDangVien.Checked;
            hoivien.CONGGIAO = chkCongGiao.Checked;
            hoivien.DANTOCITNGUOI = chkDanTocItNguoi.Checked;
            hoivien.CONLIETSI = chkConLietSi.Checked;
            hoivien.THUONGBINH = !cboThuongBenhBinh.SelectedValue.ToString().Equals("BENHBINH");
            hoivien.BENHBINH = cboThuongBenhBinh.SelectedValue.ToString().Equals("BENHBINH");
            hoivien.THUONGBENHBINH = cboThuongBenhBinh.SelectedValue.ToString();
            hoivien.TINHTRANGSUCKHOE = txtTinhTrangSucKhoe.Text;
            hoivien.CHATDOCDACAM = chkChatDocDaCam.Checked;
            hoivien.COQUANDONVI = txtCoQuanDonViKhiNghiHuu.Text;
            hoivien.CAPBAC = cboCapBac.SelectedValue.ToString();
            hoivien.CHUCVU = txtChucVu.Text;
            hoivien.NGAYNGHIHUU = txtNgayNghiHuu.Value;
            hoivien.NGAYNHAPNGU = txtNgayNhapNgu.Value;
            hoivien.NGAYXUATNGU = txtNgayXuatNgu.Value;
            hoivien.NGAYVAODANG = txtNgayVaoDang.Value;
            hoivien.COQUANKHIXUATNGU = txtCoQuanKhiXuatNgu.Text;

            hoivien.CHIHOI_ID = cboBranch.SelectedValue != null ? int.Parse(cboBranch.SelectedValue.ToString()) : 0;
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Images")))
            {
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Images"));
            }
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Images", hoivien.MAHOIVIEN)))
            {
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Images", hoivien.MAHOIVIEN));
            }
            if (pbAvatar.ImageLocation != null)
            {
                var savePath = Path.Combine(Application.StartupPath, "Images", hoivien.MAHOIVIEN, Path.GetFileName(pbAvatar.ImageLocation));
                if (!File.Exists(savePath))
                {
                    File.Copy(pbAvatar.ImageLocation, savePath);
                }
                hoivien.HINHANH = savePath;
            }
            
           
            if (hoivien.ID == 0)
            {
                hoivien.CREATED_AT = DateTime.Now;
                hoivien.CREATED_BY = Constant.LoginUser.ID;
                hoivien = databaseContext.HOIVIENs.Add(hoivien);
                databaseContext.SaveChanges();
            }
            else
            {
                hoivien.UPDATED_AT = DateTime.Now;
                hoivien.UPDATED_BY = Constant.LoginUser.ID;
            }
            
            DanhSachQuaTrinhChienDau.ForEach(s => s.HOIVIEN_ID = hoivien.ID);
            databaseContext.QUATRINHCHIENDAUs.AddOrUpdate(DanhSachQuaTrinhChienDau.ToArray());
            DanhSachThanhVien.ForEach(s => s.HOIVIEN_ID = hoivien.ID);
            databaseContext.THONGTINGIADINHs.AddOrUpdate(DanhSachThanhVien.ToArray());
            DanhSachKhenThuong.ForEach(s => s.HOIVIEN_ID = hoivien.ID);
            databaseContext.KHENTHUONGs.AddOrUpdate(DanhSachKhenThuong.ToArray());
            databaseContext.SaveChanges();
            DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbAvatar.ImageLocation = openFileDialog.FileName;
            }
        }

        private void frmModifyMember_Load(object sender, EventArgs e)
        {
            fillFormData();
        }

        void fillFormData()
        {
            
            var branchs = databaseContext.CHIHOIs.ToList();
            cboBranch.DataSource = branchs;
            cboBranch.DisplayMember = "TENCHIHOI";
            cboBranch.ValueMember = "ID";

            cboGender.DataSource = Constant.DanhMucGioiTinh;
            cboGender.DisplayMember = "Name";
            cboGender.ValueMember = "Id";

            cboThuongBenhBinh.DataSource = Constant.DanhMucHangThuongBinh;
            cboThuongBenhBinh.DisplayMember = "Name";
            cboThuongBenhBinh.ValueMember = "Id";

            cboCapBac.DataSource = Constant.DanhMucCapBac;
            cboCapBac.DisplayMember = "Name";
            cboCapBac.ValueMember = "Id";

            if (hoivien != null)
            {
                #region Thong tin chung
                if (branchs.Count > 0)
                {
                    cboBranch.SelectedValue = hoivien.CHIHOI_ID;
                }
                if (!String.IsNullOrEmpty(hoivien.GIOITINH))
                {
                    cboGender.SelectedValue = hoivien.GIOITINH;
                }

                if (!String.IsNullOrEmpty(hoivien.THUONGBENHBINH))
                {
                    cboThuongBenhBinh.SelectedValue = hoivien.THUONGBENHBINH;
                }

                if (!String.IsNullOrEmpty(hoivien.CAPBAC))
                {

                    cboCapBac.SelectedValue = hoivien.CAPBAC;
                }

                txtCode.Text = hoivien.MAHOIVIEN;
                txtFullName.Text = hoivien.HOTEN;
                txtDateOfBirth.Value = hoivien.NAMSINH;
                txtethnic.Text = hoivien.DANTOC;
                txtReligon.Text = hoivien.TONGIAO;
                txtHomtown.Text = hoivien.QUEQUAN;
                txtResidence.Text = hoivien.NOICUTRU;
                txtAddress.Text = hoivien.DIACHI;
                txtPhoneNumber.Text = hoivien.SODIENTHOAI;
                txtEmail.Text = hoivien.EMAIL;
                txtQuanlify.Text = hoivien.TRINHDOCHUYENMON;
                txtPoliticalTheory.Text = hoivien.LYLUANCHINHTRI;
                if (!String.IsNullOrEmpty(hoivien.HINHANH))
                {
                    pbAvatar.ImageLocation = hoivien.HINHANH;
                }
               
                txtAcademic.Text = hoivien.TRINHDOHOCVAN;

                txtNgayVaoHoi.Value = hoivien.NGAYVAOHOI;
                txtIssueCardDate.Value = hoivien.NGAYCAPTHE;
                chkDangVien.Checked = hoivien.DANGVIEN;
                chkCongGiao.Checked = hoivien.CONGGIAO;
                chkDanTocItNguoi.Checked = hoivien.DANTOCITNGUOI;
                chkConLietSi.Checked = hoivien.CONLIETSI;
                txtNgayNghiHuu.Value = hoivien.NGAYNGHIHUU;
                txtNgayNhapNgu.Value = hoivien.NGAYNHAPNGU;
                txtNgayVaoDang.Value = hoivien.NGAYVAODANG;
                txtNgayXuatNgu.Value = hoivien.NGAYXUATNGU;
                txtCoQuanKhiXuatNgu.Text = hoivien.COQUANKHIXUATNGU;
                txtChucVu.Text = hoivien.CHUCVU;
                txtCoQuanDonViKhiNghiHuu.Text = hoivien.COQUANDONVI;
                txtTinhTrangSucKhoe.Text = hoivien.TINHTRANGSUCKHOE;
                chkChatDocDaCam.Checked = hoivien.CHATDOCDACAM;
                #endregion

                #region Thông tin kháng chiên
                DanhSachQuaTrinhChienDau = databaseContext.QUATRINHCHIENDAUs.Where(s => s.HOIVIEN_ID == hoivien.ID).ToList();
                DanhSachKhenThuong = databaseContext.KHENTHUONGs.Where(s => s.HOIVIEN_ID == hoivien.ID).ToList();
                DanhSachThanhVien = databaseContext.THONGTINGIADINHs.Where(s => s.HOIVIEN_ID == hoivien.ID).ToList();
                reloadQuaTrinhChienDau();
                reloadKhenThuong();
                reloadGiaDinh();
                #endregion
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmQuaTrinhChienDau frm = new frmQuaTrinhChienDau();
            frm.SaveChanged += KhangChien_SaveChanged;
            frm.ShowDialog();
        }

        private void KhangChien_SaveChanged(object sender)
        {
            var quatrinh = sender as QUATRINHCHIENDAU;
            if (hoivien != null)
            {
                quatrinh.HOIVIEN_ID = hoivien.ID;
            }
            DanhSachQuaTrinhChienDau.Add(quatrinh);
            reloadQuaTrinhChienDau();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedItem = dagKhangChien.CurrentRow;
            var selectedRowIndex = selectedItem.Index;
            var quatrinh = DanhSachQuaTrinhChienDau[selectedRowIndex];
            
            if (quatrinh != null)
            {
                var dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa quá trình chiến đấu này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    DanhSachQuaTrinhChienDau.RemoveAt(selectedItem.Index);
                    if (quatrinh.ID != 0)
                    {
                        databaseContext.QUATRINHCHIENDAUs.Remove(quatrinh);
                        databaseContext.SaveChanges();
                    }
                    reloadQuaTrinhChienDau();
                }
            }
            
        }

       void reloadQuaTrinhChienDau()
        {
            DanhSachQuaTrinhChienDau.ForEach(s =>
            {
                var ck = Constant.DanhMucLoaiKhenThuong.FirstOrDefault(k => k.Id.Equals(s.LOAIKHANGCHIEN));
                if (ck != null) s.TENKHANGCHIEN = ck.Name;
            });
            var bindingList = new BindingList<QUATRINHCHIENDAU>(DanhSachQuaTrinhChienDau);
            var source = new BindingSource(bindingList, null);
            dagKhangChien.DataSource = source;
        }

        void reloadKhenThuong()
        {
            DanhSachKhenThuong.ForEach(s =>
            {
                var ck = Constant.DanhMucLoaiKhenThuong.FirstOrDefault(k => k.Id.Equals(s.LOAIKHENTHUONG));
                if (ck != null) s.TENKHENTHUONG = ck.Name;
            });
            var bindingList = new BindingList<KHENTHUONG>(DanhSachKhenThuong);
            var source = new BindingSource(bindingList, null);
            dagKhenThuong.DataSource = source;
        }

        void reloadGiaDinh()
        {
            DanhSachThanhVien.ForEach(s =>
            {
                var ck = Constant.DanhMucMoiQuanHe.FirstOrDefault(k => k.Id.Equals(s.QUANHE));
                if (ck != null) s.TENQUANHE = ck.Name;
            });
            var bindingList = new BindingList<THONGTINGIADINH>(DanhSachThanhVien);
            var source = new BindingSource(bindingList, null);
            dagGiaDinh.DataSource = source;
        }

        private void dagKhangChien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //currentSelectedRow = this.dagKhangChien.CurrentRow;
        }

        private void btnAddKhenThuong_Click(object sender, EventArgs e)
        {
            frmKhenThuong frm = new frmKhenThuong();
            frm.SaveChanged += KhenThuong_SaveChanged;
            frm.ShowDialog();
        }

        private void KhenThuong_SaveChanged(object sender)
        {
            var khenthuong = sender as KHENTHUONG;
            if (hoivien != null)
            {
                khenthuong.HOIVIEN_ID = hoivien.ID;
            }
            DanhSachKhenThuong.Add(khenthuong);
            reloadKhenThuong();
        }

        private void btnDelKhenThuong_Click(object sender, EventArgs e)
        {
            var selectedItem = dagKhenThuong.CurrentRow;
            var selectedRowIndex = selectedItem.Index;
            var khenthuong = DanhSachKhenThuong[selectedRowIndex];

            if (khenthuong != null)
            {
                var dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin khen thưởng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    DanhSachQuaTrinhChienDau.RemoveAt(selectedItem.Index);
                    if (khenthuong.ID != 0)
                    {
                        databaseContext.KHENTHUONGs.Remove(khenthuong);
                        databaseContext.SaveChanges();
                    }
                    reloadQuaTrinhChienDau();
                }
            }
        }

        private void btnDelPeople_Click(object sender, EventArgs e)
        {
            var selectedItem = dagKhenThuong.CurrentRow;
            var selectedRowIndex = selectedItem.Index;
            var thanhvien = DanhSachThanhVien[selectedRowIndex];

            if (thanhvien != null)
            {
                var dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin thành viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    DanhSachQuaTrinhChienDau.RemoveAt(selectedItem.Index);
                    if (thanhvien.ID != 0)
                    {
                        databaseContext.THONGTINGIADINHs.Remove(thanhvien);
                        databaseContext.SaveChanges();
                    }
                    reloadGiaDinh();
                }
            }
        }

        private void btnAddPeople_Click(object sender, EventArgs e)
        {

            var frm = new frmThongTinGiaDinh();
            frm.SaveChanged += ThongTinGiaDinh_SaveChanged;

            frm.ShowDialog();
        }

        private void ThongTinGiaDinh_SaveChanged(object sender)
        {
            var thongtin = sender as THONGTINGIADINH;
            if (hoivien != null)
            {
                thongtin.HOIVIEN_ID = hoivien.ID;
            }
            DanhSachThanhVien.Add(thongtin);
            reloadGiaDinh();
        }
    }
}
