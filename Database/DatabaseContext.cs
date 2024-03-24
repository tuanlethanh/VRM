using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRM.Entities;
using System.Windows.Forms;
using System.IO;

namespace VRM.Database
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = Path.Combine(Application.StartupPath, "Database", "VRM.db"), ForeignKeys = true }.ConnectionString
            }, true)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HOIVIEN> HOIVIENs { get; set; }

        public DbSet<CHIHOI> CHIHOIs { get; set; }

        public DbSet<QUATRINHCHIENDAU> QUATRINHCHIENDAUs { get; set; }

        public DbSet<KHENTHUONG> KHENTHUONGs { get; set; }

        public DbSet<THONGTINGIADINH> THONGTINGIADINHs { get; set; }
        public DbSet<USER> USERs { get; set; }

    }
}
