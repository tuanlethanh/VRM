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
using VRM.Entities;

namespace VRM.Forms
{
    public partial class frmEditBranch : Form
    {
        private readonly DatabaseContext databaseContext = new DatabaseContext();

        public frmEditBranch()
        {
            InitializeComponent();
        }

        public CHIHOI branch { get; set; }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Lỗi nhập liệu");
                return;
            }

            var existingBranch = databaseContext.CHIHOIs.FirstOrDefault(s => s.MACHIHOI ==  txtCode.Text);
            if (existingBranch != null)
            {
                MessageBox.Show("Mã chi bộ đã tồn tại, vui lòng kiểm tra lại", "Lỗi nhập liệu");
                return;
            }

            if (branch == null || branch.ID == 0)
            {
                branch = new CHIHOI();
            }

            branch.MACHIHOI = txtCode.Text;
            branch.TENCHIHOI = txtName.Text;
            databaseContext.CHIHOIs.Add(branch);
            databaseContext.SaveChanges();
            DialogResult = DialogResult.OK;
        }

        private void frmEditBranch_Load(object sender, EventArgs e)
        {
            if (branch != null)
            {
                txtCode.Text = branch.MACHIHOI;
                txtName.Text = branch.TENCHIHOI;
            }
        }
    }
}
