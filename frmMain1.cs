using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRM.Database;
using VRM.Forms;

namespace VRM
{
    public partial class frmMain1 : Form
    {
        public frmMain1()
        {
            InitializeComponent();
        }

        private readonly DatabaseContext databaseContext = new DatabaseContext();


        private void frmMain_Load(object sender, EventArgs e)
        {
            RefreshMemberList();
        }

        void RefreshMemberList()
        {
            var data = databaseContext.Members.AsQueryable();

            if (!string.IsNullOrEmpty(txtSearchByName.Text))
            {
                data = data.Where(s => s.FULLNAME.Contains(txtSearchByName.Text));
            }

            if (!string.IsNullOrEmpty(txtSearchByDob.Text))
            {
                data = data.Where(s => s.DOB.Year.Equals(int.Parse(txtSearchByDob.Text)));
            }

            if (!string.IsNullOrEmpty(txtSearchByArmyDate.Text))
            {
                data = data.Where(s => s.DOB.Year.Equals(int.Parse(txtSearchByArmyDate.Text)));
            }

            if (!string.IsNullOrEmpty(txtSearchByCode.Text))
            {
                data = data.Where(s => s.CODE.Contains(txtSearchByName.Text));
            }

            daMemberList.DataSource = data.ToList();
            daMemberList.Refresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshMemberList();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            var frm = new frmModifyMember();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Tạo hội viên thành công");
            }
        }

        private void daMemberList_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
