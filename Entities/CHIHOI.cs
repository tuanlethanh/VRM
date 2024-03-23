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
    [Table(Name = "CHIHOI")]
    public class CHIHOI
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "MACHIHOI", DbType = "VARCHAR")]
        public string MACHIHOI { get; set; }

        [Column(Name = "TENCHIHOI", DbType = "VARCHAR")]
        public string TENCHIHOI { get; set; }
    }
}
