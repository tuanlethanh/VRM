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
    [Table(Name = "User")]
    public class USER
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "UserName", DbType = "VARCHAR")]
        public string UserName { get; set; }

        [Column(Name = "Password", DbType = "VARCHAR")]
        public string Password { get; set; }

        [Column(Name = "FullName", DbType = "VARCHAR")]
        public string FullName { get; set; }

        [Column(Name = "Role", DbType = "VARCHAR")]
        public string Role { get; set; }
    }
}
