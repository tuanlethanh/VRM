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
    [Table(Name = "Branch")]
    public class Branch
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "CODE", DbType = "VARCHAR")]
        public string CODE { get; set; }

        [Column(Name = "NAME", DbType = "VARCHAR")]
        public string NAME { get; set; }
    }
}
