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
    [Table(Name = "KHENTHUONG")]
    public class KHENTHUONG
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "HOIVIEN_ID", DbType = "INTEGER")]
        public int HOIVIEN_ID { get; set; }

        [Column(Name = "LOAIKHENTHUONG", DbType = "VARCHAR")]
        public string LOAIKHENTHUONG { get; set; }

        [Column(Name = "NAMKHENTHUONG", DbType = "INTEGER")]
        public decimal NAMKHENTHUONG { get; set; }

        [Column(Name = "NOIDUNGKHENTHUONG", DbType = "VARCHAR")]
        public string NOIDUNGKHENTHUONG { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string TENIKHENTHUONG { get; set; }
    }
}
