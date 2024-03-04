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
    [Table(Name = "Member")]
    public class Member
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ID { get; set; }

        [Column(Name = "BRANCH_ID", DbType = "INTEGER")]
        public int BRANCH_ID { get; set; }

        [Column(Name = "CODE", DbType = "VARCHAR")]
        public string CODE { get; set; }
        
        [Column(Name = "FULLNAME", DbType = "VARCHAR")]
        public string FULLNAME { get; set; }

        [Column(Name = "GENDER", DbType = "VARCHAR")]
        public string GENDER { get; set; }

        [Column(Name = "DOB", DbType = "DATE")]
        public DateTime DOB { get; set; }

        [Column(Name = "ETHNIC", DbType = "VARCHAR")]
        public string ETHNIC { get; set; }

        [Column(Name = "RELIGION", DbType = "VARCHAR")]
        public string RELIGION { get; set; }

        [Column(Name = "HOMETOWN", DbType = "VARCHAR")]
        public string HOMETOWN { get; set; }

        [Column(Name = "RESIDENCE", DbType = "VARCHAR")]
        public string RESIDENCE { get; set; }

        [Column(Name = "ADDRESS", DbType = "VARCHAR")]
        public string ADDRESS { get; set; }

        [Column(Name = "MOBILE_NO", DbType = "VARCHAR")]
        public string MOBILE_NO { get; set; }

        [Column(Name = "EMAIL", DbType = "VARCHAR")]
        public string EMAIL { get; set; }

        [Column(Name = "QUALIFICATION", DbType = "VARCHAR")]
        public string QUALIFICATION { get; set; }

        [Column(Name = "ACADEMIC", DbType = "VARCHAR")]
        public string ACADEMIC { get; set; }

        [Column(Name = "POLITICAL_THEORY", DbType = "VARCHAR")]
        public string POLITICAL_THEORY { get; set; }

        [Column(Name = "IMAGE", DbType = "VARCHAR")]
        public string IMAGE { get; set; }

        [Column(Name = "CREATED_AT", DbType = "DATETIME")]
        public DateTime CREATED_AT { get; set; }

        [Column(Name = "UPDATED_AT", DbType = "DATETIME")]
        public DateTime UPDATED_AT { get; set; }

        [Column(Name = "CREATED_BY", DbType = "INTEGER")]
        public DateTime CREATED_BY { get; set; }

        [Column(Name = "UPDATED_BY", DbType = "INTEGER")]
        public DateTime UPDATED_BY { get; set; }
    }
}
