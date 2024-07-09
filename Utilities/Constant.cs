using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRM.Entities;
using VRM.Models;

namespace VRM.Utilities
{
    public class Constant
    {
        public static List<DropdownModel> DanhMucLoaiKhangChien = new List<DropdownModel>
            {
                new DropdownModel {Id = "KHANG_CHIEN_CHONG_PHAP", Name = "Chống pháp" },
                new DropdownModel {Id = "KHANG_CHIEN_CHONG_MY", Name = "Chống Mỹ" },
                new DropdownModel {Id = "CCCB_SAU_30_4", Name = "Sau 30/4/1975" },
            };

        public static List<DropdownModel> DanhMucChienDich = new List<DropdownModel>
            {
                new DropdownModel {Id = "CHIEN_DICH_DIEN_BIEN_PHU", Name = "Chiến dịch điện biên phủ", ParentId = "KHANG_CHIEN_CHONG_PHAP" },
                new DropdownModel {Id = "GIAI_PHONG_THU_DO", Name = "Giải phóng thủ đô", ParentId = "KHANG_CHIEN_CHONG_PHAP" },
                new DropdownModel {Id = "KHANG_CHIEN_CHONG_PHAP_KHAC", Name = "Chiến dịch khác", ParentId = "KHANG_CHIEN_CHONG_PHAP" },
                new DropdownModel {Id = "CHIEN_DICH_HO_CHI_MINH", Name = "Chiến dịch hồ Chí Minh" , ParentId = "KHANG_CHIEN_CHONG_MY"},
                new DropdownModel {Id = "KHANG_CHIEN_CHONG_MY_KHAC", Name = "Chiến dịch khác" , ParentId = "KHANG_CHIEN_CHONG_MY"},
                new DropdownModel {Id = "CHIEN_DICH_SAU_30_4", Name = "Chiến dịch sau 30/4/1975", ParentId = "CCCB_SAU_30_4" },
            };

        public static List<DropdownModel> DanhMucLoaiKhenThuong = new List<DropdownModel>
            {
                new DropdownModel {Id = "HUANCHUONG", Name = "Huân chương" },
                new DropdownModel {Id = "HUYCHUONG", Name = "Huy chương" },
                new DropdownModel {Id = "KHAC", Name = "Khác" },
            };

        public static List<DropdownModel> DanhMucGioiTinh = new List<DropdownModel>
            {
                new DropdownModel {Id = "NAM", Name = "Nam" },
                new DropdownModel {Id = "NU", Name = "Nữ" },
                new DropdownModel {Id = "KHAC", Name = "Khác" },
            };

        public static List<DropdownModel> DanhMucMoiQuanHe = new List<DropdownModel>
            {
                new DropdownModel {Id = "VO", Name = "Vợ" },
                new DropdownModel {Id = "CHONG", Name = "Chồng" },
                new DropdownModel {Id = "CON", Name = "Con" },
                new DropdownModel {Id = "KHAC", Name = "Khác" },
            };

        public static List<DropdownModel> DanhMucHangThuongBinh = new List<DropdownModel>
            {
                new DropdownModel {Id = "KHONG", Name = "Không" },
                new DropdownModel {Id = "BENHBINH", Name = "Bệnh binh" },
                new DropdownModel {Id = "HANG1", Name = "Thương binh hạng 1/4" },
                new DropdownModel {Id = "HANG2", Name = "Thương binh hạng 2/4" },
                new DropdownModel {Id = "HANG3", Name = "Thương binh hạng 3/4" },
                new DropdownModel {Id = "HANG4", Name = "Thương binh hạng 4/4" },
            };

        public static List<DropdownModel> DanhMucCapBac = new List<DropdownModel>
            {
                new DropdownModel {Id = "CAPTUONG1", Name = "Đại tướng" },
                new DropdownModel {Id = "CAPTUONG2", Name = "Thượng tướng" },
                new DropdownModel {Id = "CAPTUONG3", Name = "Trung tướng" },
                new DropdownModel {Id = "CAPTUONG4", Name = "Thiếu tướng" },
                new DropdownModel {Id = "CAPTA1", Name = "Đại tá" },
                new DropdownModel {Id = "CAPTA2", Name = "Thượng tá" },
                new DropdownModel {Id = "CAPTA3", Name = "Trung tá" },
                new DropdownModel {Id = "CAPTA4", Name = "Thiếu tá" },
                new DropdownModel {Id = "CAPUY1", Name = "Đại úy" },
                new DropdownModel {Id = "CAPUY2", Name = "Thượng úy" },
                new DropdownModel {Id = "CAPUY3", Name = "Trung úy" },
                new DropdownModel {Id = "CAPUY4", Name = "Thiếu úy" },
                new DropdownModel {Id = "HSQCS", Name = "HSQCS" },
                new DropdownModel {Id = "QNCN", Name = "Quân nhân chuyên nghiệp" },
                new DropdownModel {Id = "CNVQP", Name = "Công nhân viên Quốc phòng" }
            };

        public static List<DropdownModel> DanhMucDoiTuongKetNap = new List<DropdownModel>
            {
                new DropdownModel {Id = "DOITUONG1", Name = "Đối tượng 1" },
                new DropdownModel {Id = "DOITUONG2", Name = "Đối tượng 2" },
                new DropdownModel {Id = "DOITUONG3", Name = "Đối tượng 3" },
                new DropdownModel {Id = "DOITUONG4", Name = "Đối tượng 4" },
                new DropdownModel {Id = "DOITUONG5", Name = "Đối tượng 5" },
                new DropdownModel {Id = "DOITUONG6", Name = "Đối tượng 6" },
                new DropdownModel {Id = "DOITUONG7", Name = "Đối tượng 7" },
            };

        public static USER LoginUser = new USER();

        public static string PRINT_MEMBER_LIST_FULL = "FULL";
        public static string PRINT_MEMBER_LIST_SHORT = "SHORT";
        public static string PRINT_MEMBER_LIST_DS_HOIVIEN = "DS_HOIVIEN";
        public static string PRINT_MEMBER_LIST_DS_HOIVIEN_TT = "DS_HOIVIEN_TT";
    }
}
