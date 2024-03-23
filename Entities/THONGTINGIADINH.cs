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

    [Table(Name = "THONGTINGIADINH")]
    public class THONGTINGIADINH
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "HOIVIEN_ID", DbType = "INTEGER")]
        public int HOIVIEN_ID { get; set; }

        [Column(Name = "QUANHE", DbType = "VARCHAR")]
        public string QUANHE { get; set; }

        [Column(Name = "NAMSINH", DbType = "INTEGER")]
        public decimal NAMSINH { get; set; }

        [Column(Name = "HOTEN", DbType = "VARCHAR")]
        public string HOTEN { get; set; }
        
        [Column(Name = "QUEQUAN", DbType = "VARCHAR")]
        public string QUEQUAN { get; set; }
        
        [Column(Name = "DIACHIHIENNAY", DbType = "VARCHAR")]
        public string DIACHIHIENNAY { get; set; }

        [Column(Name = "CHATDOCDACAM", DbType = "BOOLEAN")]
        public bool CHATDOCDACAM { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string TENQUANHE { get; set; }
    }
}
