using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VRM.Entities;

namespace VRM
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //if (!System.IO.File.Exists("D:\\SQLiteWithEF.db"))
            //{
            //    System.IO.File.Create("D:\\SQLiteWithEF.db");
            //}
            //var context = new Database.DatabaseContext();

            //Veteran employee = new Veteran()
            //{
            //    FULLNAME = "Nguyễn Văn A",
            //    DOB = DateTime.Today,
            //    GENDER = "M"
            //};
            //context.Veterans.Add(employee);
            //context.SaveChanges();



            //MessageBox.Show(context.Veterans.ToList().Count.ToString());
        }
    }
}
