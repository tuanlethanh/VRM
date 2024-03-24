using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRM.Entities
{
    [Table(Name = "HOIVIEN")]
    public class HOIVIEN
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "CHIHOI_ID", DbType = "INTEGER")]
        public int CHIHOI_ID { get; set; }

        [Column(Name = "MAHOIVIEN", DbType = "VARCHAR")]
        public string MAHOIVIEN { get; set; }
        
        [Column(Name = "HOTEN", DbType = "VARCHAR")]
        public string HOTEN { get; set; }

        [Column(Name = "GIOITINH", DbType = "VARCHAR")]
        public string GIOITINH { get; set; }

        [Column(Name = "NAMSINH", DbType = "INTEGER")]
        public decimal NAMSINH { get; set; }

        [Column(Name = "DANTOC", DbType = "VARCHAR")]
        public string DANTOC { get; set; }

        [Column(Name = "TONGIAO", DbType = "VARCHAR")]
        public string TONGIAO { get; set; }

        [Column(Name = "QUEQUAN", DbType = "VARCHAR")]
        public string QUEQUAN { get; set; }

        [Column(Name = "NOICUTRU", DbType = "VARCHAR")]
        public string NOICUTRU { get; set; }

        [Column(Name = "DIACHI", DbType = "VARCHAR")]
        public string DIACHI { get; set; }

        [Column(Name = "SODIENTHOAI", DbType = "VARCHAR")]
        public string SODIENTHOAI { get; set; }

        [Column(Name = "EMAIL", DbType = "VARCHAR")]
        public string EMAIL { get; set; }

        [Column(Name = "TRINHDOCHUYENMON", DbType = "VARCHAR")]
        public string TRINHDOCHUYENMON { get; set; }

        [Column(Name = "TRINHDOHOCVAN", DbType = "VARCHAR")]
        public string TRINHDOHOCVAN { get; set; }

        [Column(Name = "LYLUANCHINHTRI", DbType = "VARCHAR")]
        public string LYLUANCHINHTRI { get; set; }

        [Column(Name = "HINHANH", DbType = "VARCHAR")]
        public string HINHANH { get; set; }

        [Column(Name = "NGAYCAPTHE", DbType = "DATE")]
        public DateTime NGAYCAPTHE { get; set; }

        [Column(Name = "DANGVIEN", DbType = "BOOLEAN")]
        public bool DANGVIEN { get; set; }

        [Column(Name = "CONLIETSI", DbType = "BOOLEAN")]
        public bool CONLIETSI { get; set; }

        [Column(Name = "THUONGBINH", DbType = "BOOLEAN")]
        public bool THUONGBINH { get; set; }

        [Column(Name = "BENHBINH", DbType = "BOOLEAN")]
        public bool BENHBINH { get; set; }

        [Column(Name = "CONGGIAO", DbType = "BOOLEAN")]
        public bool CONGGIAO { get; set; }

        [Column(Name = "DANTOCITNGUOI", DbType = "BOOLEAN")]
        public bool DANTOCITNGUOI { get; set; }
        
        [Column(Name = "CHATDOCDACAM", DbType = "BOOLEAN")]
        public bool CHATDOCDACAM { get; set; }

        [Column(Name = "NGAYVAOHOI", DbType = "DATE")]
        public DateTime NGAYVAOHOI { get; set; }

        [Column(Name = "CREATED_AT", DbType = "DATETIME")]
        public DateTime CREATED_AT { get; set; }

        [Column(Name = "UPDATED_AT", DbType = "DATETIME")]
        public DateTime UPDATED_AT { get; set; }

        [Column(Name = "CREATED_BY", DbType = "INTEGER")]
        public int CREATED_BY { get; set; }

        [Column(Name = "UPDATED_BY", DbType = "INTEGER")]
        public int UPDATED_BY { get; set; }

        [Column(Name = "NGAYNHAPNGU", DbType = "DATE")]
        public DateTime NGAYNHAPNGU { get; set; }

        [Column(Name = "NGAYXUATNGU", DbType = "DATE")]
        public DateTime NGAYXUATNGU { get; set; }

        [Column(Name = "NGAYNGHIHUU", DbType = "DATE")]
        public DateTime NGAYNGHIHUU { get; set; }
        
        [Column(Name = "NGAYVAODANG", DbType = "DATE")]
        public DateTime NGAYVAODANG { get; set; }

        [Column(Name = "CAPBAC", DbType = "VARCHAR")]
        public string CAPBAC { get; set; }

        [Column(Name = "CHUCVU", DbType = "VARCHAR")]
        public string CHUCVU { get; set; }

        [Column(Name = "COQUANDONVI", DbType = "VARCHAR")]
        public string COQUANDONVI { get; set; }

        [Column(Name = "TINHTRANGSUCKHOE", DbType = "VARCHAR")]
        public string TINHTRANGSUCKHOE { get; set; }

        [Column(Name = "THUONGBENHBINH", DbType = "VARCHAR")]
        public string THUONGBENHBINH { get; set; }

        [Column(Name = "COQUANKHIXUATNGU", DbType = "VARCHAR")]
        public string COQUANKHIXUATNGU { get; set; }
    }
}
