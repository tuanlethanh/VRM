using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRM.Database;
using VRM.Entities;
using VRM.Forms;
using VRM.Forms.Authentication;
using VRM.Models;
using VRM.Utilities;
using VRM.Utilities.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;
using static VRM.Utilities.Excel.Emums;

namespace VRM
{
    public partial class frmMain : RibbonForm
    {
        private Size oldSize;
        private readonly DatabaseContext databaseContext = new DatabaseContext();
        DataGridViewRow currentSelectedRow;
        public frmMain()
        {
            InitializeComponent();
        }
        public List<QUATRINHCHIENDAU> DanhSachQuaTrinhChienDau { get; set; } = new List<QUATRINHCHIENDAU>();
        public List<KHENTHUONG> DanhSachKhenThuong { get; set; } = new List<KHENTHUONG>();
        public List<THONGTINGIADINH> DanhSachThanhVien { get; set; } = new List<THONGTINGIADINH>();
        private void frmMain_Load(object sender, EventArgs e)
        {
            clearData();
            //Chua login:
            listButtonUpdate.Enabled = false;
            listButtonExport.Enabled = false;
            listUserManager.Enabled = false;
            listDanhMuc.Enabled = false;
            btnSearch.Enabled = false;
            btnEdit.Visible = false;
            btnDelete.Visible = false;

           frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                listButtonUpdate.Enabled = true;
                listButtonExport.Enabled = true;
                listUserManager.Enabled = true;
                listDanhMuc.Enabled = true;
                btnSearch.Enabled = true;


                oldSize = base.Size;
                daMembers.DataSource = searchData();

                var listChiHoi = databaseContext.CHIHOIs.ToList();
                listChiHoi.Insert(0, new CHIHOI { ID = -1, TENCHIHOI = "------Tất cả------" });
                cboTimKiemChiHoi.DataSource = listChiHoi;
                cboTimKiemChiHoi.DisplayMember = "TENCHIHOI";
                cboTimKiemChiHoi.ValueMember = "ID";

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
            }
        }

        List<HoiVienModel> searchData()
        {
            var query = databaseContext.HOIVIENs.AsQueryable();
            if (cboTimKiemChiHoi.SelectedValue != null
                && decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString()) != -1)
            {
                var id = decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString());
                query = query.Where(s => s.CHIHOI_ID == id);
            }

            if (!string.IsNullOrEmpty(txtTimKiemTuKhoa.Text))
            {
                var keyword = txtTimKiemTuKhoa.Text.ToLower();

                query = query.Where(s => s.HOTEN.ToLower().Contains(keyword)
                || s.QUEQUAN.ToLower().Contains(keyword)
                || s.DIACHI.ToLower().Contains(keyword) || s.SODIENTHOAI.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(txtTimKiemNamSinh.Text))
            {
                var keyword = txtTimKiemNamSinh.Text;
                int nam = 0;
                int.TryParse(keyword, out nam);
                query = query.Where(s => s.NAMSINH.HasValue && nam.Equals(s.NAMSINH.Value.Year));
            }

            if (!string.IsNullOrEmpty(txtNamNhapNguSearch.Text))
            {
                var keyword = txtNamNhapNguSearch.Text;
                int nam = 0;
                int.TryParse(keyword, out nam);
                query = query.Where(s => s.NGAYNHAPNGU.HasValue && nam.Equals(s.NGAYNHAPNGU.Value.Year));
            }

            if (chkNNSau1975.Checked)
            {
                var date = new DateTime(1975, 4, 30);
                query = query.Where(s => s.NGAYNHAPNGU.HasValue && s.NGAYNHAPNGU.Value >= date);
            }

            if (chkNNTruoc1975.Checked)
            {
                var date = new DateTime(1975, 4, 30);
                query = query.Where(s => s.NGAYNHAPNGU.HasValue && s.NGAYNHAPNGU.Value < date);
            }

            if (!string.IsNullOrEmpty(txtNamXuatNguSearch.Text))
            {
                var keyword = txtNamXuatNguSearch.Text;
                int nam = 0;
                int.TryParse(keyword, out nam);
                query = query.Where(s => s.NGAYXUATNGU.HasValue && nam.Equals(s.NGAYXUATNGU.Value.Year));
            }

            if (chkDangVienSearch.Checked)
            {
                query = query.Where(s => s.DANGVIEN == true);
            }

            if (chkChatDocDaCamSearch.Checked)
            {
                query = query.Where(s => s.CHATDOCDACAM == true);
            } 
            
            if (chKThuongBinh.Checked)
            {
                query = query.Where(s => s.THUONGBINH == true);
            }
    
            var data = query.Join(databaseContext.CHIHOIs, hoivien => hoivien.CHIHOI_ID, chihoi => chihoi.ID,
                (hoivien, chihoi) => new { hoivien, chihoi }).ToList();

            var listDat = new List<HoiVienModel>();
            var lsIds = data.Select(s => s.hoivien.ID).ToList();
            var chiendichs = new List<string> { "CHIEN_DICH_HO_CHI_MINH", "KHANG_CHIEN_CHONG_MY_KHAC" };
            var lstKhangChiens = databaseContext.QUATRINHCHIENDAUs.Where(s => lsIds.Contains(s.HOIVIEN_ID) && chiendichs.Contains(s.CHIENDICH)).ToList();
            foreach (var item in data)
            {
                listDat.Add(new HoiVienModel
                {
                    ID = item.hoivien.ID,
                    HOTEN = item.hoivien.HOTEN,
                    MAHOIVIEN = item.hoivien.MAHOIVIEN,
                    NAMSINH = item.hoivien.NAMSINH,
                    TENCHIHOI = item.chihoi.TENCHIHOI,
                    NAMNGHIHUU = item.hoivien.NGAYNGHIHUU?.Year.ToString(),
                    NAMNHAPNGU = item.hoivien.NGAYNHAPNGU?.Year.ToString(),
                    NAMXUATNGU = item.hoivien.NGAYXUATNGU?.Year.ToString(),
                    QUEQUAN = item.hoivien.QUEQUAN,
                    CHONGMY = lstKhangChiens.Any(s => s.HOIVIEN_ID == item.hoivien.ID),
                    KYNIEMCHUONG = item.hoivien.KYNIEMCHUONG,
                    MACHIHOI = item.chihoi.MACHIHOI
                });
            }

           return listDat.ToList().OrderBy(s => s.MACHIHOI).ToList();
        }

        private void btnNewMember_Click(object sender, EventArgs e)
        {
            var frm = new frmModifyMember();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Tạo hội viên thành công", "Lưu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                daMembers.DataSource = searchData();
            }
        }

        private void btnUpdateMember_Click(object sender, EventArgs e)
        {
            UpdateMember();
        }

        private void daMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = this.daMembers.CurrentRow;
            bindingData(currentSelectedRow);
        }

        void bindingData(DataGridViewRow row)
        {
            try
            {
                if (row != null)
                {
                    var id = int.Parse(row.Cells["HVID"].Value.ToString());
                    var hoivien = databaseContext.HOIVIENs.AsNoTracking().FirstOrDefault(s => s.ID == id);

                    if (hoivien != null)
                    {
                        #region Thong tin chung
                        cboBranch.SelectedValue = hoivien.CHIHOI_ID;
                        cboGender.SelectedValue = hoivien.GIOITINH;
                        cboThuongBenhBinh.SelectedValue = hoivien.THUONGBENHBINH;
                        cboCapBac.SelectedValue = hoivien.CAPBAC;

                        txtCode.Text = hoivien.MAHOIVIEN;
                        txtFullName.Text = hoivien.HOTEN;
                        txtDateOfBirth.Text = hoivien.NAMSINH?.ToString("dd/MM/yyyy");
                        txtethnic.Text = hoivien.DANTOC;
                        txtReligon.Text = hoivien.TONGIAO;
                        txtHomtown.Text = hoivien.QUEQUAN;
                        txtResidence.Text = hoivien.NOICUTRU;
                        txtAddress.Text = hoivien.DIACHI;
                        txtPhoneNumber.Text = hoivien.SODIENTHOAI;
                        txtBHYT.Text = hoivien.BHYT;
                        txtCCCD.Text = hoivien.CCCD;
                        txtQuanlify.Text = hoivien.TRINHDOCHUYENMON;
                        txtPoliticalTheory.Text = hoivien.LYLUANCHINHTRI;
                        txtKyNiemChuong.Text = hoivien.KYNIEMCHUONG;
                        if(!String.IsNullOrEmpty(hoivien.HINHANH))
                        {
                            pbAvatar.ImageLocation = hoivien.HINHANH;
                        }
                        
                        txtAcademic.Text = hoivien.TRINHDOHOCVAN;

                        txtNgayVaoHoi.Text = hoivien.NGAYVAOHOI?.ToString("dd/MM/yyyy");
                        txtIssueCardDate.Text = hoivien.NGAYCAPTHE?.ToString("dd/MM/yyyy");
                        chkDangVien.Checked = hoivien.DANGVIEN;
                        chkCongGiao.Checked = hoivien.CONGGIAO;
                        chkDanTocItNguoi.Checked = hoivien.DANTOCITNGUOI;
                        chkConLietSi.Checked = hoivien.CONLIETSI;
                        txtNgayNghiHuu.Text = hoivien.NGAYNGHIHUU?.ToString("dd/MM/yyyy");
                        txtNgayNhapNgu.Text = hoivien.NGAYNHAPNGU?.ToString("dd/MM/yyyy"); ;
                        txtNgayVaoDang.Text = hoivien.NGAYVAODANG?.ToString("dd/MM/yyyy");
                        txtNgayXuatNgu.Text = hoivien.NGAYXUATNGU?.ToString("dd/MM/yyyy");
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

                        btnEdit.Visible = true;
                        btnDelete.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        void reloadQuaTrinhChienDau()
        {
            DanhSachQuaTrinhChienDau.ForEach(s =>
            {
                s.TENKHANGCHIEN = Constant.DanhMucLoaiKhangChien.FirstOrDefault(k => k.Id.Equals(s.LOAIKHANGCHIEN))?.Name;
                s.TENCHIENDICH = Constant.DanhMucChienDich.FirstOrDefault(k => k.Id.Equals(s.CHIENDICH))?.Name;
            });
            var bindingList = new BindingList<QUATRINHCHIENDAU>(DanhSachQuaTrinhChienDau);
            var source = new BindingSource(bindingList, null);
            dagKhangChien.DataSource = source;
        }

        void reloadKhenThuong()
        {
            DanhSachKhenThuong.ForEach(s =>
            {
                s.TENKHENTHUONG = Constant.DanhMucLoaiKhenThuong.FirstOrDefault(k => k.Id.Equals(s.LOAIKHENTHUONG))?.Name;
            });
            var bindingList = new BindingList<KHENTHUONG>(DanhSachKhenThuong);
            var source = new BindingSource(bindingList, null);
            dagKhenThuong.DataSource = source;
        }

        void reloadGiaDinh()
        {
            DanhSachThanhVien.ForEach(s =>
            {
                s.TENQUANHE = Constant.DanhMucMoiQuanHe.FirstOrDefault(k => k.Id.Equals(s.QUANHE))?.Name;
            });
            var bindingList = new BindingList<THONGTINGIADINH>(DanhSachThanhVien);
            var source = new BindingSource(bindingList, null);
            dagGiaDinh.DataSource = source;
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            DeleteMember();
            clearData();
        }

        private void daMembers_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void btnBranchManage_Click(object sender, EventArgs e)
        {
            var branch = new frmBranch();
            if (branch.ShowDialog() == DialogResult.OK)
            {
                var listChiHoi = databaseContext.CHIHOIs.ToList();
                listChiHoi.Insert(0, new CHIHOI { ID = -1, TENCHIHOI = "------Tất cả------" });
                cboTimKiemChiHoi.DataSource = listChiHoi;
                cboTimKiemChiHoi.DisplayMember = "TENCHIHOI";
                cboTimKiemChiHoi.ValueMember = "ID";
            }

        }

        private void frmMain_Resize(object sender, EventArgs e)
        {

            foreach (Control cnt in this.Controls)
                ResizeAll(cnt, base.Size);

            oldSize = base.Size;
        }

        private void ResizeAll(Control control, Size newSize)
        {
            try
            {
                int width = newSize.Width - oldSize.Width;
                control.Left += (control.Left * width) / oldSize.Width;
                control.Width += (control.Width * width) / oldSize.Width;

                int height = newSize.Height - oldSize.Height;
                control.Top += (control.Top * height) / oldSize.Height;
                control.Height += (control.Height * height) / oldSize.Height;
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            daMembers.DataSource = searchData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var frmPrintType = new frmPrintType();
            if (frmPrintType.ShowDialog() ==  DialogResult.OK)
            {
                var printType = frmPrintType.PrintType;


                try
                {
                    var query = databaseContext.HOIVIENs.AsNoTracking().AsQueryable();
                    if (cboTimKiemChiHoi.SelectedValue != null
                        && decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString()) != -1)
                    {
                        var id = decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString());
                        query = query.Where(s => s.CHIHOI_ID == id);
                    }

                    if (!string.IsNullOrEmpty(txtTimKiemTuKhoa.Text))
                    {
                        var keyword = txtTimKiemTuKhoa.Text.ToLower();

                        query = query.Where(s => s.HOTEN.ToLower().Contains(keyword)
                        || s.QUEQUAN.ToLower().Contains(keyword)
                        || s.DIACHI.ToLower().Contains(keyword) || s.SODIENTHOAI.Contains(keyword));
                    }

                    if (!string.IsNullOrEmpty(txtTimKiemNamSinh.Text))
                    {
                        var keyword = txtTimKiemNamSinh.Text;
                        int nam = 0;
                        int.TryParse(keyword, out nam);
                        query = query.Where(s => s.NAMSINH.HasValue && nam.Equals(s.NAMSINH.Value.Year));
                    }

                    if (!string.IsNullOrEmpty(txtNamNhapNguSearch.Text))
                    {
                        var keyword = txtNamNhapNguSearch.Text;
                        int nam = 0;
                        int.TryParse(keyword, out nam);
                        query = query.Where(s => s.NGAYNHAPNGU.HasValue && nam.Equals(s.NGAYNHAPNGU.Value.Year));
                    }

                    if (!string.IsNullOrEmpty(txtNamXuatNguSearch.Text))
                    {
                        var keyword = txtNamXuatNguSearch.Text;
                        int nam = 0;
                        int.TryParse(keyword, out nam);
                        query = query.Where(s => s.NGAYXUATNGU.HasValue && nam.Equals(s.NGAYXUATNGU.Value.Year));
                    }

                    if (chkDangVienSearch.Checked)
                    {
                        query = query.Where(s => s.DANGVIEN == true);
                    }

                    if (chkChatDocDaCamSearch.Checked)
                    {
                        query = query.Where(s => s.CHATDOCDACAM == true);
                    }

                    if (chkNNSau1975.Checked)
                    {
                        var date = new DateTime(1975, 4, 30);
                        query = query.Where(s => s.NGAYNHAPNGU.HasValue && s.NGAYNHAPNGU.Value >= date);
                    }

                    if (chkNNTruoc1975.Checked)
                    {
                        var date = new DateTime(1975, 4, 30);
                        query = query.Where(s => s.NGAYNHAPNGU.HasValue && s.NGAYNHAPNGU.Value < date);
                    }

                    if (chKThuongBinh.Checked)
                    {
                        query = query.Where(s => s.THUONGBINH == true);
                    }

                    var listHoiVien = query.Join(databaseContext.CHIHOIs, hv => hv.CHIHOI_ID, ch => ch.ID, (hv, ch) => new
                    {
                        HoiVien = hv,
                        ChiHoi = ch
                    }).ToList();

                    var listHoiVienIds = listHoiVien.Select(s => s.HoiVien.ID).ToList();
                    var listQuaTrinhChienDaus = databaseContext.QUATRINHCHIENDAUs.AsNoTracking().Where(s => listHoiVienIds.Contains(s.HOIVIEN_ID)).ToList();
                    //var listKhenThuongs = databaseContext.KHENTHUONGs.Where(s => listHoiVienIds.Contains(s.HOIVIEN_ID)).ToList();
                    var listTTGiaDinhs = databaseContext.THONGTINGIADINHs.AsNoTracking().Where(s => listHoiVienIds.Contains(s.HOIVIEN_ID)).ToList();


                    var listReport = new List<ReportListModel>();
                    listHoiVien.AsParallel().ForAll(s =>
                    {
                        var hoivien = new ReportListModel
                        {
                            ID = s.HoiVien.ID,
                            HoTen = s.HoiVien.HOTEN,
                            NamSinh = s.HoiVien.NAMSINH?.ToString("dd/MM/yyyy"),
                            NgayNhapNgu = s.HoiVien.NGAYNHAPNGU == null ? "" : s.HoiVien.NGAYNHAPNGU?.ToString("MM/yyyy"),
                            NgayXuatNgu = s.HoiVien.NGAYXUATNGU == null ? "" : s.HoiVien.NGAYXUATNGU?.ToString("MM/yyyy"),
                            DangVien = s.HoiVien.DANGVIEN ? "x" : "",
                            CapTuong = !string.IsNullOrEmpty(s.HoiVien.CAPBAC) ? s.HoiVien.CAPBAC.Contains("CAPTUONG") ? "x" : "" : "",
                            BonGach = s.HoiVien.CAPBAC == "CAPTA1" ? "x" : "",
                            BaGach = s.HoiVien.CAPBAC == "CAPTA2" ? "x" : "",
                            HaiGach = s.HoiVien.CAPBAC == "CAPTA3" ? "x" : "",
                            MotGach = s.HoiVien.CAPBAC == "CAPTA4" ? "x" : "",
                            CapUy = !string.IsNullOrEmpty(s.HoiVien.CAPBAC) ? s.HoiVien.CAPBAC.Contains("CAPUY") ? "x" : "" : "",
                            HSQCS = s.HoiVien.CAPBAC == "HSQCS" ? "x" : "",
                            QNCN = s.HoiVien.CAPBAC == "QNCN" ? "x" : "",
                            CNVQP = s.HoiVien.CAPBAC == "CNVQP" ? "x" : "",
                            ChucVu = s.HoiVien.CHUCVU,
                            ThuongBinh = s.HoiVien.THUONGBINH ? "x" : "",
                            BenhBinh = s.HoiVien.BENHBINH ? "x" : "",
                            ChatDocDaCam_BanThan = s.HoiVien.CHATDOCDACAM ? "x" : "",
                            ConLietSi = s.HoiVien.CONLIETSI ? "x" : "",
                            DanTocItNguoi = s.HoiVien.DANTOCITNGUOI ? "x" : "",
                            CongGiao = s.HoiVien.CONGGIAO ? "x" : "",
                            NgayVaoHoi = s.HoiVien.NGAYVAOHOI == null ? "" : s.HoiVien.NGAYVAOHOI?.ToString("dd/MM/yyyy"),
                            NamCapTheHoiVien = s.HoiVien.NGAYCAPTHE == null ? "" : s.HoiVien.NGAYCAPTHE?.ToString("yyyy"),
                            SoTheHoiVien = s.HoiVien.MAHOIVIEN,
                            TenChiHoi = s.ChiHoi.TENCHIHOI,
                            MaChiHoi = s.ChiHoi.MACHIHOI
                        };
                        hoivien.ChatDocDaCam_Con = listTTGiaDinhs.Any(k => k.HOIVIEN_ID == s.HoiVien.ID && k.CHATDOCDACAM && k.QUANHE == "CON") ? "x" : "";
                        var chienDichDienBienPhu = listQuaTrinhChienDaus.FirstOrDefault(k => k.HOIVIEN_ID == s.HoiVien.ID && k.CHIENDICH == "CHIEN_DICH_DIEN_BIEN_PHU");
                        if (chienDichDienBienPhu != null)
                        {
                            hoivien.ChucVu_CDDienBienPhu = string.Format("Cấp bậc: {0}, Chức vụ: {1}", chienDichDienBienPhu.CAPBAC, chienDichDienBienPhu.CHUCVU);
                            hoivien.DonVi_CDDienBienPhu = string.Format("Đơn vị: {0}, Thời gian: {1}", chienDichDienBienPhu.DONVI, chienDichDienBienPhu.THOIGIAN);
                        }
                        var GPThuDo = listQuaTrinhChienDaus.FirstOrDefault(k => k.HOIVIEN_ID == s.HoiVien.ID && k.CHIENDICH == "GIAI_PHONG_THU_DO");
                        if (GPThuDo != null)
                        {
                            hoivien.ChucVu_GPThuDo = string.Format("Cấp bậc: {0}, Chức vụ: {1}", GPThuDo.CAPBAC, GPThuDo.CHUCVU);
                            hoivien.DonVi_GPThuDo = string.Format("Đơn vị: {0}, Thời gian: {1}", GPThuDo.DONVI, GPThuDo.THOIGIAN);
                        }
                        hoivien.ChongMy = listQuaTrinhChienDaus.Any(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "KHANG_CHIEN_CHONG_MY") ? "x" : "";
                        hoivien.CCB_sau304 = listQuaTrinhChienDaus.Any(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "CCCB_SAU_30_4") ? "x" : "";

                        var CHIEN_DICH_HO_CHI_MINH = listQuaTrinhChienDaus.FirstOrDefault(k => k.HOIVIEN_ID == s.HoiVien.ID && k.CHIENDICH == "CHIEN_DICH_HO_CHI_MINH");
                        if (CHIEN_DICH_HO_CHI_MINH != null)
                        {
                            hoivien.ChucVu_HCM = string.Format("Cấp bậc: {0}, Chức vụ: {1}", CHIEN_DICH_HO_CHI_MINH.CAPBAC, CHIEN_DICH_HO_CHI_MINH.CHUCVU);
                            hoivien.DonVi_HCM = string.Format("Đơn vị: {0}, Thời gian: {1}", CHIEN_DICH_HO_CHI_MINH.DONVI, CHIEN_DICH_HO_CHI_MINH.THOIGIAN);
                        }
                        listReport.Add(hoivien);
                    });
                    listReport = listReport.ToList().OrderBy(s => s.MaChiHoi).ToList();
                    var templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVienFULL.xlsx";
                    switch (printType)
                    {
                        case "FULL":
                            templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVienFULL.xlsx";
                            if (cboTimKiemChiHoi.SelectedValue != null
                                && decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString()) != -1)
                            {
                                templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVien.xlsx";
                            }
                            break;
                        case "SHORT":
                            templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVienRUTGON.xlsx";
                            if (cboTimKiemChiHoi.SelectedValue != null
                                && decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString()) != -1)
                            {
                                templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVienSHORT.xlsx";
                            }
                            break;
                    }
                    


                    var aWorkBook = ExcelHelper.GetWorkbook(templateFile);
                    aWorkBook = ExcelHelper.ExportExcel(listReport, aWorkBook, 0, new List<ColumnValue>() {
                new ColumnValue{FieldName = "CHIHOI", FieldValue = listReport.Count > 0 ? listReport[0].TenChiHoi : "", IsValue = true, ValueType = ValueDataType.String},
            });
                    MemoryStream mstream = new MemoryStream();
                    OoxmlSaveOptions opts1 = new OoxmlSaveOptions(SaveFormat.Xlsx);
                    aWorkBook.Save(mstream, opts1);
                    SaveFileDialog saveDlg = new SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export Excel File To";
                    if (saveDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveDlg.FileName, mstream.ToArray());
                        Process.Start(saveDlg.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra trong quá trình xử lý", "Lỗi cập nhật thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print();

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                listButtonUpdate.Enabled = true;
                listButtonExport.Enabled = true;
                listUserManager.Enabled = true;
                listDanhMuc.Enabled = true;
                btnSearch.Enabled = true;


                oldSize = base.Size;
                daMembers.DataSource = searchData();

                var listChiHoi = databaseContext.CHIHOIs.ToList();
                listChiHoi.Insert(0, new CHIHOI { ID = -1, TENCHIHOI = "------Tất cả------" });
                cboTimKiemChiHoi.DataSource = listChiHoi;
                cboTimKiemChiHoi.DisplayMember = "TENCHIHOI";
                cboTimKiemChiHoi.ValueMember = "ID";

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
            }
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Thay đổi mật khẩu thành công, vui lòng đăng nhập lại để tiếp tục sử dụng chương trình", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                listButtonUpdate.Enabled = false;
                listButtonExport.Enabled = false;
                listUserManager.Enabled = false;
                listDanhMuc.Enabled = false;
                btnSearch.Enabled = false;

                frmLogin frmLogin = new frmLogin();
                if (frmLogin.ShowDialog() == DialogResult.OK)
                {
                    listButtonUpdate.Enabled = true;
                    listButtonExport.Enabled = true;
                    listUserManager.Enabled = true;
                    listDanhMuc.Enabled = true;
                    btnSearch.Enabled = true;

                }
            }
        }

        private void daMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateMember();
        }

        void Print()
        {
            currentSelectedRow = this.daMembers.CurrentRow;
            if (currentSelectedRow == null)
            {
                MessageBox.Show("Vui lòng lựa chọn một bản ghi", "Lỗi thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal hoiVienID = 0;
            decimal.TryParse(currentSelectedRow.Cells["HVID"].Value.ToString(), out hoiVienID);
            var hoivien = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == hoiVienID);

            if (hoivien == null)
            {
                MessageBox.Show("Không tìm thấy thông tin hội viên", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var printModel = new PrintModel
            {
                CapBac = !string.IsNullOrEmpty(hoivien.CAPBAC) ? Constant.DanhMucCapBac
                            .FirstOrDefault(s => s.Id.ToString().Equals(hoivien.CAPBAC))?.Name : "",
                CapBacKhiNghiHuu = !string.IsNullOrEmpty(hoivien.CAPBAC) ? Constant.DanhMucCapBac
                            .FirstOrDefault(s => s.Id.ToString().Equals(hoivien.CAPBAC))?.Name : "",
                QueQuan = hoivien.QUEQUAN,
                HoTen = hoivien.HOTEN,
                NguoiKhai = hoivien.HOTEN,
                GioiTinh = hoivien.GIOITINH == "MALE" ? "Nam" : "Nữ",
                NgaySinh = hoivien.NAMSINH.ToString(),
                DanToc = hoivien.DANTOC,
                TonGiao = hoivien.TONGIAO,
                HoKhauThuongTru = hoivien.NOICUTRU,
                DiaChi = hoivien.DIACHI,
                NgayNhapNgu = hoivien.NGAYNHAPNGU != null ? hoivien.NGAYNHAPNGU?.ToString("dd/MM/yyyy") : "",
                NgayXuatNgu = hoivien.NGAYXUATNGU != null ? hoivien.NGAYXUATNGU?.ToString("dd/MM/yyyy") : "",
                TrinhDoVanHoa = hoivien.TRINHDOHOCVAN,
                SoDienThoai = hoivien.SODIENTHOAI,
                TrinhDoChuyenMon = hoivien.TRINHDOCHUYENMON,
                LyLuanChinhTri = hoivien.LYLUANCHINHTRI,
                NgayVaoDang = hoivien.NGAYVAODANG != null ? hoivien.NGAYVAODANG?.ToString("dd/MM/yyyy") : "",
                NgayChinhThuc = hoivien.NGAYVAODANG != null ? hoivien.NGAYVAODANG?.ToString("dd/MM/yyyy") : "",
                DonVi = hoivien.COQUANDONVI,
                NgayNghiHuu = hoivien.NGAYNGHIHUU != null ? hoivien.NGAYNGHIHUU?.ToString("dd/MM/yyyy") : "",
                CoQuanKhiNghiHuu = hoivien.COQUANDONVI,
                ThuongBinh = !string.IsNullOrEmpty(hoivien.THUONGBENHBINH) ? Constant.DanhMucHangThuongBinh
                            .FirstOrDefault(s => s.Id.ToString().Equals(hoivien.THUONGBENHBINH))?.Name : "",
                GiaDinhLietSi = "",
                TinhTrangSucKhoe = hoivien.TINHTRANGSUCKHOE,
                KyLuat = "",
            };

            var listThongTinGiaDinh = databaseContext.THONGTINGIADINHs.Where(s => s.HOIVIEN_ID == hoiVienID).ToList();
            var voChong = listThongTinGiaDinh.FirstOrDefault(s => s.QUANHE == "VO" || s.QUANHE == "CHONG");
            if (voChong != null)
            {
                printModel.HoTenVoChong = voChong.HOTEN;
                printModel.NamSinhVoChong = voChong.NAMSINH.ToString();
                printModel.QueQuanVoChong = voChong.QUEQUAN;
                printModel.ChoOHienNayVoChong = voChong.DIACHIHIENNAY;
            }

            StringBuilder cacCon = new StringBuilder();
            var cacConList = listThongTinGiaDinh.Where(s => s.QUANHE == "CON").ToList();
            int i = 1;
            foreach (var item in cacConList)
            {
                cacCon.AppendLine(string.Format("{0}. {1} - Sinh năm: {2}, Nơi ở hiện nay: {3}", i++, item.HOTEN, item.NAMSINH, item.DIACHIHIENNAY));
            }
            printModel.CacCon = cacCon.ToString();

            List<string> chatDocDaCam = new List<string>();
            if (hoivien.CHATDOCDACAM)
            {
                chatDocDaCam.Add("Bản thân");
            }



            if (listThongTinGiaDinh.Any(s => s.CHATDOCDACAM && s.QUANHE == "CON"))
            {
                chatDocDaCam.Add("Con");
            }
            printModel.ChatDocMauDaCam = string.Join(" và ", chatDocDaCam);

            var listKhenThuong = databaseContext.KHENTHUONGs.Where(s => s.HOIVIEN_ID == hoiVienID).ToList();
            StringBuilder khenthuong = new StringBuilder();
            foreach (var item in listKhenThuong)
            {
                khenthuong.AppendLine(string.Format("Năm {0}: {1}", item.NAMKHENTHUONG, item.NOIDUNGKHENTHUONG));
            }
            printModel.KhenThuong = khenthuong.ToString();

            var listChienDau = databaseContext.QUATRINHCHIENDAUs.Where(s => s.HOIVIEN_ID == hoiVienID).ToList();
            var listQuaTrinhCongTac = new StringBuilder();
            foreach (var item in listChienDau)
            {
                listQuaTrinhCongTac.AppendLine(string.Format("Thời gian: {0}, Cấp bậc: {1}, Chức vụ: {2}, Đơn vị công tác: {3}, chiến trường chiến đấu: {4}",
                    item.THOIGIAN, item.CAPBAC, item.CHUCVU, item.DONVI, item.CHIENDICH));
            }
            printModel.QuaTrinhCongTac = listQuaTrinhCongTac.ToString();

            var templateFile = $"{Application.StartupPath}\\Templates\\DonXinVaoHoiCCB.xlsx";

            var aWorkBook = ExcelHelper.GetWorkbook(templateFile);
            aWorkBook = ExcelHelper.ExportExcel(new List<PrintModel>(), aWorkBook, 0, new List<ColumnValue>() {
                new ColumnValue{FieldName = "CapBac", FieldValue = printModel.CapBac, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "CapBacKhiNghiHuu", FieldValue = printModel.CapBacKhiNghiHuu, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "QueQuan", FieldValue = printModel.QueQuan, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "HoTen", FieldValue = printModel.HoTen, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "GioiTinh", FieldValue = printModel.GioiTinh, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NgaySinh", FieldValue = printModel.NgaySinh, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "DanToc", FieldValue = printModel.DanToc, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TonGiao", FieldValue = printModel.TonGiao, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "HoKhauThuongTru", FieldValue = printModel.HoKhauThuongTru, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "DiaChi", FieldValue = printModel.DiaChi, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NgayNhapNgu", FieldValue = printModel.NgayNhapNgu, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NgayXuatNgu", FieldValue = printModel.NgayXuatNgu, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TrinhDoVanHoa", FieldValue = printModel.TrinhDoVanHoa, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "SoDienThoai", FieldValue = printModel.SoDienThoai, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TrinhDoChuyenMon", FieldValue = printModel.TrinhDoChuyenMon, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "LyLuanChinhTri", FieldValue = printModel.LyLuanChinhTri, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NgayVaoDang", FieldValue = printModel.NgayVaoDang, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "DonVi", FieldValue = printModel.DonVi, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NgayNghiHuu", FieldValue = printModel.NgayNghiHuu, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "CoQuanKhiNghiHuu", FieldValue = printModel.CoQuanKhiNghiHuu, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "ThuongBinh", FieldValue = printModel.ThuongBinh, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "GiaDinhLietSi", FieldValue = printModel.GiaDinhLietSi, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TinhTrangSucKhoe", FieldValue = printModel.TinhTrangSucKhoe, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "HoTenVoChong", FieldValue = printModel.HoTenVoChong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NamSinhVoChong", FieldValue = printModel.NamSinhVoChong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "ChoOHienNayVoChong", FieldValue = printModel.ChoOHienNayVoChong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TrinhDoVanHoaVoChong", FieldValue = printModel.ChoOHienNayVoChong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NgheNghiepVoChong", FieldValue = printModel.ChoOHienNayVoChong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "QueQuanVoChong", FieldValue = printModel.QueQuanVoChong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "ChatDocMauDaCam", FieldValue = printModel.ChatDocMauDaCam, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "KhenThuong", FieldValue = printModel.KhenThuong, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TinhHinhDoiSongGiaDinh", FieldValue = printModel.TinhHinhDoiSongGiaDinh, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "QuaTrinhCongTac", FieldValue = printModel.QuaTrinhCongTac, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "KyLuat", FieldValue = printModel.KyLuat, IsValue = true, ValueType = ValueDataType.String},
            },
            new List<ColumnValue>() {
               
                new ColumnValue{FieldName = "TTCacCon", FieldValue = printModel.CacCon, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "NguoiKhai", FieldValue = printModel.NguoiKhai, IsValue = true, ValueType = ValueDataType.String},
            });
            MemoryStream mstream = new MemoryStream();
            OoxmlSaveOptions opts1 = new OoxmlSaveOptions(SaveFormat.Xlsx);
            aWorkBook.Save(mstream, opts1);
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllBytes(save.FileName, mstream.ToArray());
                    Process.Start(save.FileName);
                }
                catch
                {
                    MessageBox.Show("Vui lòng đóng file đang mở, sau đó thực hiện lại.", "Lỗi xử lý dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        void UpdateMember()
        {
            currentSelectedRow = this.daMembers.CurrentRow;
            if (currentSelectedRow != null)
            {
                var frm = new frmModifyMember();
                var id = int.Parse(currentSelectedRow.Cells["HVID"].Value.ToString());
                var member = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == id);
                frm.hoivien = member;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Chỉnh sửa viên thành công", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    daMembers.DataSource = searchData();
                    currentSelectedRow = this.daMembers.CurrentRow;
                    bindingData(currentSelectedRow);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng", "Lỗi thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void DeleteMember()
        {
            if (currentSelectedRow != null)
            {
                var dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa hội viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    var id = int.Parse(currentSelectedRow.Cells["HVID"].Value.ToString());
                    var member = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == id);
                    databaseContext.HOIVIENs.Remove(member);
                    databaseContext.SaveChanges();
                    currentSelectedRow = null;
                    daMembers.DataSource = searchData();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng");
            }
        }

        void clearData()
        {
            DanhSachQuaTrinhChienDau = new List<QUATRINHCHIENDAU>();
            var bindingList = new BindingList<QUATRINHCHIENDAU>(DanhSachQuaTrinhChienDau);
            var source = new BindingSource(bindingList, null);
            dagKhangChien.DataSource = source;

            DanhSachKhenThuong = new List<KHENTHUONG>();
            var bindingList1 = new BindingList<KHENTHUONG>(DanhSachKhenThuong);
            var source1 = new BindingSource(bindingList1, null);
            dagKhenThuong.DataSource = source1;


            DanhSachThanhVien = new List<THONGTINGIADINH>();
            var bindingList2 = new BindingList<THONGTINGIADINH>(DanhSachThanhVien);
            var source2 = new BindingSource(bindingList2, null);
            dagGiaDinh.DataSource = source2;

            foreach (var item in this.groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    var control = item as TextBox;
                    control.Text = "";
                }

                if (item is DateTimePicker)
                {
                    var control = item as DateTimePicker;
                    control.Value = new DateTime(1900,1,1);
                }

                if (item is CheckBox)
                {
                    var control = item as CheckBox;
                    control.Checked = false;
                }

                if (item is ComboBox)
                {
                    var control = item as ComboBox;
                    control.SelectedItem = null;
                }

                if (item is PictureBox)
                {
                    var control = item as PictureBox;
                    control.Image = null;
                }
            }

            foreach (var item in this.groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    var control = item as TextBox;
                    control.Text = "";
                }

                if (item is DateTimePicker)
                {
                    var control = item as DateTimePicker;
                    control.Value = new DateTime(1900, 1, 1);
                }

                if (item is CheckBox)
                {
                    var control = item as CheckBox;
                    control.Checked = false;
                }

                if (item is ComboBox)
                {
                    var control = item as ComboBox;
                    control.SelectedItem = null;
                }

                if (item is PictureBox)
                {
                    var control = item as PictureBox;
                    control.Image = null;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            UpdateMember();
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteMember();
            clearData();
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Mục từ khóa có thể tìm kiếm theo các trường:\n Họ tên; Quê quán; Địa chỉ hiện tại; Số điện thoại liên hệ", "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtTimKiemNamSinh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&  (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtNamXuatNguSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtNamNhapNguSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnExportDs_Click(object sender, EventArgs e)
        {
            var data = searchData();
            data.ForEach(s =>
            {
                s.CHONGMY = s.CHONGMY.ToString() == "True" ? "x" : "-";
            });

            var templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVienNew.xlsx";
            
            try
            {
                var aWorkBook = ExcelHelper.GetWorkbook(templateFile);
                aWorkBook = ExcelHelper.ExportExcel(data, aWorkBook, 0, new List<ColumnValue>() { });
                MemoryStream mstream = new MemoryStream();
                OoxmlSaveOptions opts1 = new OoxmlSaveOptions(SaveFormat.Xlsx);
                aWorkBook.Save(mstream, opts1);
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.InitialDirectory = @"C:\";
                saveDlg.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDlg.FilterIndex = 0;
                saveDlg.RestoreDirectory = true;
                saveDlg.Title = "Export Excel File To";
                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveDlg.FileName, mstream.ToArray());
                    Process.Start(saveDlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra trong quá trình xử lý", "Lỗi cập nhật thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
                
    }
}
