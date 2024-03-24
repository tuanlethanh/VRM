using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Drawing;
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

            //Chua login:
            listButtonUpdate.Enabled = false;
            listButtonExport.Enabled = false;
            listUserManager.Enabled = false;
            listDanhMuc.Enabled = false;
            btnSearch.Enabled = false;

            frmLogin frm = new frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                listButtonUpdate.Enabled = true;
                listButtonExport.Enabled = true;
                listUserManager.Enabled = true;
                listDanhMuc.Enabled = true;
                btnSearch.Enabled = true;


                oldSize = base.Size;
                refreshDataGridView();

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

        void refreshDataGridView()
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
                query.Where(s => s.HOTEN.ToLower().Contains(keyword)
                || keyword.Equals(s.NAMSINH) || s.QUEQUAN.ToLower().Contains(keyword)
                || s.DIACHI.ToLower().Contains(keyword) || s.SODIENTHOAI.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(txtTimKiemNamSinh.Text))
            {
                var keyword = txtTimKiemNamSinh.Text;
                query.Where(s => keyword.Equals(s.NAMSINH));
            }

            var data = query.Join(databaseContext.CHIHOIs, hoivien => hoivien.CHIHOI_ID, chihoi => chihoi.ID,
                (hoivien, chihoi) => new { hoivien, chihoi }).ToList();

            var listDat = new List<HoiVienModel>();
            foreach (var item in data)
            {
                listDat.Add(new HoiVienModel
                {
                    ID = item.hoivien.ID,
                    HOTEN = item.hoivien.HOTEN,
                    MAHOIVIEN = item.hoivien.MAHOIVIEN,
                    NAMSINH = item.hoivien.NAMSINH,
                    TENCHIHOI = item.chihoi.TENCHIHOI
                });
            }

            daMembers.DataSource = listDat;
        }

        private void btnNewMember_Click(object sender, EventArgs e)
        {
            var frm = new frmModifyMember();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Tạo hội viên thành công");
                refreshDataGridView();
            }
        }

        private void btnUpdateMember_Click(object sender, EventArgs e)
        {
            currentSelectedRow = this.daMembers.CurrentRow;
            if (currentSelectedRow != null)
            {
                var frm = new frmModifyMember();
                var id = int.Parse(currentSelectedRow.Cells["ID"].Value.ToString());
                var member = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == id);
                frm.hoivien = member;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Chỉnh sửa viên thành công");
                    refreshDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng");
            }
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
                    var hoivien = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == id);

                    if (hoivien != null)
                    {
                        #region Thong tin chung
                        cboBranch.SelectedValue = hoivien.CHIHOI_ID;
                        cboGender.SelectedValue = hoivien.GIOITINH;
                        cboThuongBenhBinh.SelectedValue = hoivien.THUONGBENHBINH;
                        cboCapBac.SelectedValue = hoivien.CAPBAC;

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
                        pbAvatar.ImageLocation = hoivien.HINHANH;
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
            }
            catch (Exception ex)
            {

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
                if (ck != null) s.TENIKHENTHUONG = ck.Name;
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

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow != null)
            {
                var dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa hội viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    var id = int.Parse(currentSelectedRow.Cells["ID"].Value.ToString());
                    var member = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == id);
                    databaseContext.HOIVIENs.Remove(member);
                    databaseContext.SaveChanges();
                    currentSelectedRow = null;
                    refreshDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng");
            }

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
            refreshDataGridView();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
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
                    query.Where(s => s.HOTEN.ToLower().Contains(keyword)
                    || keyword.Equals(s.NAMSINH) || s.QUEQUAN.ToLower().Contains(keyword)
                    || s.DIACHI.ToLower().Contains(keyword) || s.SODIENTHOAI.Contains(keyword));
                }

                if (!string.IsNullOrEmpty(txtTimKiemNamSinh.Text))
                {
                    var keyword = txtTimKiemNamSinh.Text;
                    query.Where(s => keyword.Equals(s.NAMSINH));
                }

                var listHoiVien = query.Join(databaseContext.CHIHOIs, hv => hv.CHIHOI_ID, ch => ch.ID, (hv, ch) => new
                {
                    HoiVien = hv,
                    TenChiHoi = ch.TENCHIHOI
                }).ToList();

                var listHoiVienIds = listHoiVien.Select(s => s.HoiVien.ID).ToList();
                var listQuaTrinhChienDaus = databaseContext.QUATRINHCHIENDAUs.Where(s => listHoiVienIds.Contains(s.HOIVIEN_ID)).ToList();
                //var listKhenThuongs = databaseContext.KHENTHUONGs.Where(s => listHoiVienIds.Contains(s.HOIVIEN_ID)).ToList();
                var listTTGiaDinhs = databaseContext.THONGTINGIADINHs.Where(s => listHoiVienIds.Contains(s.HOIVIEN_ID)).ToList();


                var listReport = new List<ReportListModel>();
                listHoiVien.AsParallel().ForAll(s =>
                {
                    var hoivien = new ReportListModel
                    {
                        HoTen = s.HoiVien.HOTEN,
                        NamSinh = s.HoiVien.NAMSINH,
                        NgayNhapNgu = s.HoiVien.NGAYNHAPNGU == null ? "" : s.HoiVien.NGAYNHAPNGU.ToString("MM/yyyy"),
                        NgayXuatNgu = s.HoiVien.NGAYXUATNGU == null ? "" : s.HoiVien.NGAYXUATNGU.ToString("MM/yyyy"),
                        DangVien = s.HoiVien.DANGVIEN ? "x" : "",
                        CapTuong = !string.IsNullOrEmpty(s.HoiVien.CAPBAC) ? s.HoiVien.CAPBAC.Contains("CAPTUONG") ? "x" : "" : "",
                        BonGach = s.HoiVien.CAPBAC == "CAPTA1" ? "x" : "",
                        BaGach = s.HoiVien.CAPBAC == "CAPTA2" ? "x" : "",
                        HaiGach = s.HoiVien.CAPBAC == "CAPTA3" ? "x" : "",
                        MotGach = s.HoiVien.CAPBAC == "CAPTA4" ? "x" : "",
                        CapUy = string.IsNullOrEmpty(s.HoiVien.CAPBAC) ? s.HoiVien.CAPBAC.Contains("CAPUY") ? "x" : "" : "",
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
                        NgayVaoHoi = s.HoiVien.NGAYVAOHOI == null ? "" : s.HoiVien.NGAYVAOHOI.ToString("dd/MM/yyyy"),
                        NamCapTheHoiVien = s.HoiVien.NGAYCAPTHE == null ? "" : s.HoiVien.NGAYCAPTHE.ToString("yyyy"),
                        SoTheHoiVien = s.HoiVien.MAHOIVIEN,
                        TenChiHoi = s.TenChiHoi
                    };
                    hoivien.ChatDocDaCam_Con = listTTGiaDinhs.Any(k => k.HOIVIEN_ID == s.HoiVien.ID && k.CHATDOCDACAM && k.QUANHE == "CON") ? "x" : "";
                    var chienDichDienBienPhu = listQuaTrinhChienDaus.FirstOrDefault(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "CHIEN_DICH_DIEN_BIEN_PHU");
                    if (chienDichDienBienPhu != null)
                    {
                        hoivien.ChucVu_CDDienBienPhu = string.Format("Cấp bậc: {0}, Chức vụ: {1}", chienDichDienBienPhu.CAPBAC, chienDichDienBienPhu.CHUCVU);
                        hoivien.DonVi_CDDienBienPhu = string.Format("Đơn vị: {0}, Thời gian: {1}", chienDichDienBienPhu.DONVI, chienDichDienBienPhu.THOIGIAN);
                    }
                    var GPThuDo = listQuaTrinhChienDaus.FirstOrDefault(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "GIAI_PHONG_THU_DO");
                    if (GPThuDo != null)
                    {
                        hoivien.ChucVu_GPThuDo = string.Format("Cấp bậc: {0}, Chức vụ: {1}", GPThuDo.CAPBAC, GPThuDo.CHUCVU);
                        hoivien.DonVi_GPThuDo = string.Format("Đơn vị: {0}, Thời gian: {1}", GPThuDo.DONVI, GPThuDo.THOIGIAN);
                    }
                    hoivien.ChongMy = listQuaTrinhChienDaus.Any(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "KHANG_CHIEN_CHONG_MY") ? "x" : "";
                    hoivien.CCB_sau304 = listQuaTrinhChienDaus.Any(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "CCCB_SAU_30_4") ? "x" : "";

                    var CHIEN_DICH_HO_CHI_MINH = listQuaTrinhChienDaus.FirstOrDefault(k => k.HOIVIEN_ID == s.HoiVien.ID && k.LOAIKHANGCHIEN == "CHIEN_DICH_HO_CHI_MINH");
                    if (CHIEN_DICH_HO_CHI_MINH != null)
                    {
                        hoivien.ChucVu_HCM = string.Format("Cấp bậc: {0}, Chức vụ: {1}", CHIEN_DICH_HO_CHI_MINH.CAPBAC, CHIEN_DICH_HO_CHI_MINH.CHUCVU);
                        hoivien.DonVi_HCM = string.Format("Đơn vị: {0}, Thời gian: {1}", CHIEN_DICH_HO_CHI_MINH.DONVI, CHIEN_DICH_HO_CHI_MINH.THOIGIAN);
                    }
                    listReport.Add(hoivien);
                });

                var templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVienFULL.xlsx";
                if (cboTimKiemChiHoi.SelectedValue != null
                    && decimal.Parse(cboTimKiemChiHoi.SelectedValue.ToString()) != -1)
                {
                    templateFile = $"{Application.StartupPath}\\Templates\\DanhSachHoiVien.xlsx";
                }


                var aWorkBook = ExcelHelper.GetWorkbook(templateFile);
                aWorkBook = ExcelHelper.ExportExcel(listReport, aWorkBook, 0, new List<ColumnValue>() {
                new ColumnValue{FieldName = "CHIHOI", FieldValue = listReport.Count > 0 ? listReport[0].TenChiHoi : "", IsValue = true, ValueType = ValueDataType.String},
            });
                MemoryStream mstream = new MemoryStream();
                OoxmlSaveOptions opts1 = new OoxmlSaveOptions(SaveFormat.Xlsx);
                aWorkBook.Save(mstream, opts1);
                SaveFileDialog save = new SaveFileDialog();
                if (save.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(save.FileName, mstream.ToArray());
                    Process.Start(save.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra trong quá trình xử lý");
            }


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            currentSelectedRow = this.daMembers.CurrentRow;
            if (currentSelectedRow == null)
            {
                MessageBox.Show("Vui lòng lựa chọn một bản ghi");
                return;
            }

            decimal hoiVienID = 0;
            decimal.TryParse(currentSelectedRow.Cells["HVID"].Value.ToString(), out hoiVienID);
            var hoivien = databaseContext.HOIVIENs.FirstOrDefault(s => s.ID == hoiVienID);

            if (hoivien == null)
            {
                MessageBox.Show("Không tìm thấy thông tin hội viên");
                return;
            }

            var printModel = new PrintModel
            {
                CapBac = hoivien.CAPBAC,
                CapBacKhiNghiHuu = hoivien.CAPBAC,
                QueQuan = hoivien.QUEQUAN,
                HoTen = hoivien.HOTEN,
                NguoiKhai = hoivien.HOTEN,
                GioiTinh = hoivien.GIOITINH == "MALE" ? "Nam" : "Nữ",
                NgaySinh = hoivien.NAMSINH.ToString(),
                DanToc = hoivien.DANTOC,
                TonGiao = hoivien.TONGIAO,
                HoKhauThuongTru = hoivien.NOICUTRU,
                DiaChi = hoivien.DIACHI,
                NgayNhapNgu = hoivien.NGAYNHAPNGU != null ? hoivien.NGAYNHAPNGU.ToString("dd/MM/yyyy") : "",
                NgayXuatNgu = hoivien.NGAYXUATNGU != null ? hoivien.NGAYXUATNGU.ToString("dd/MM/yyyy") : "",
                TrinhDoVanHoa = hoivien.TRINHDOHOCVAN,
                SoDienThoai = hoivien.SODIENTHOAI,
                TrinhDoChuyenMon = hoivien.TRINHDOCHUYENMON,
                LyLuanChinhTri = hoivien.LYLUANCHINHTRI,
                NgayVaoDang = hoivien.NGAYVAODANG != null ? hoivien.NGAYVAODANG.ToString("dd/MM/yyyy") : "",
                DonVi = hoivien.COQUANDONVI,
                NgayNghiHuu = hoivien.NGAYNGHIHUU != null ? hoivien.NGAYNGHIHUU.ToString("dd/MM/yyyy") : "",
                CoQuanKhiNghiHuu = hoivien.COQUANDONVI,
                ThuongBinh = hoivien.THUONGBENHBINH,
                GiaDinhLietSi = "",
                TinhTrangSucKhoe = hoivien.TINHTRANGSUCKHOE,
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
                new ColumnValue{FieldName = "CacCon", FieldValue = printModel.CacCon, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "ChatDocMauDaCam", FieldValue = printModel.ChatDocMauDaCam, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "KhenThuong", FieldValue = printModel.ChatDocMauDaCam, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "TinhHinhDoiSongGiaDinh", FieldValue = printModel.TinhHinhDoiSongGiaDinh, IsValue = true, ValueType = ValueDataType.String},
                new ColumnValue{FieldName = "QuaTrinhCongTac", FieldValue = printModel.QuaTrinhCongTac, IsValue = true, ValueType = ValueDataType.String},
            });
            MemoryStream mstream = new MemoryStream();
            OoxmlSaveOptions opts1 = new OoxmlSaveOptions(SaveFormat.Xlsx);
            aWorkBook.Save(mstream, opts1);
            SaveFileDialog save = new SaveFileDialog();
            if (save.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(save.FileName, mstream.ToArray());
                Process.Start(save.FileName);
            }

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
                refreshDataGridView();

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
                MessageBox.Show("Thay đổi mật khẩu thành công, vui lòng đăng nhập lại để tiếp tục sử dụng chương trình");
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
    }
}
