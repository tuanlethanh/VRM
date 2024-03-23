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
    [Table(Name = "QUATRINHCHIENDAU")]
    public class QUATRINHCHIENDAU
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "HOIVIEN_ID", DbType = "INTEGER")]
        public int HOIVIEN_ID { get; set; }

        [Column(Name = "LOAIKHANGCHIEN", DbType = "VARCHAR")]
        public string LOAIKHANGCHIEN { get; set; }

        [Column(Name = "CHIENDICH", DbType = "VARCHAR")]
        public string CHIENDICH { get; set; }

        [Column(Name = "DONVI", DbType = "VARCHAR")]
        public string DONVI { get; set; }

        [Column(Name = "CAPBAC", DbType = "VARCHAR")]
        public string CAPBAC { get; set; }

        [Column(Name = "CHUCVU", DbType = "VARCHAR")]
        public string CHUCVU { get; set; }

        [Column(Name = "THOIGIAN", DbType = "VARCHAR")]
        public string THOIGIAN { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string TENKHANGCHIEN { get; set; }
    }
}
