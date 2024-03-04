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
using VRM.Forms;

namespace VRM
{
    public partial class frmMain : RibbonForm
    {
        private Size oldSize;
        private readonly DatabaseContext databaseContext = new DatabaseContext();
        DataGridViewRow currentSelectedRow;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            oldSize = base.Size;
            refreshDataGridView();
        }

        void refreshDataGridView()
        {
            var query = databaseContext.Members.AsQueryable();

            daMembers.DataSource = query.ToList();
        }

        private void btnNewMember_Click(object sender, EventArgs e)
        {
            var frm = new frmModifyMember();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Tạo hội viên thành công");
                refreshDataGridView();
            }
        }

        private void btnUpdateMember_Click(object sender, EventArgs e)
        {
            currentSelectedRow = this.daMembers.CurrentRow;
        }

        private void daMembers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = this.daMembers.CurrentRow;
            bindingData(currentSelectedRow);
        }

        void bindingData(DataGridViewRow row)
        {
            if (row != null)
            {
                txtCode.Text = row.Cells["CODE"].Value.ToString();
                txtFullName.Text = row.Cells["FULLNAME"].Value.ToString();
                cboGender.Text = row.Cells["GENDER"].Value.ToString();
                txtDateOfBirth.Value = (DateTime)row.Cells["DOB"].Value;
                txtethnic.Text = row.Cells["ETHNIC"].Value.ToString();
                txtReligon.Text = row.Cells["RELIGION"].Value.ToString();
                txtHomtown.Text = row.Cells["HOMETOWN"].Value.ToString();
                txtResidence.Text = row.Cells["RESIDENCE"].Value.ToString();
                txtAddress.Text = row.Cells["ADDRESS"].Value.ToString();
                txtPhoneNumber.Text = row.Cells["MOBILE_NO"].Value.ToString();
                txtEmail.Text = row.Cells["EMAIL"].Value.ToString();
                txtQuanlify.Text = row.Cells["QUALIFICATION"].Value.ToString();
                txtPoliticalTheory.Text = row.Cells["POLITICAL_THEORY"].Value.ToString();
                pbAvatar.ImageLocation = row.Cells["IMAGE"].Value.ToString();
            }
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            if (currentSelectedRow != null)
            {
                var dialog = MessageBox.Show("Bạn có chắc chắn muốn xóa hội viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    var id = int.Parse(currentSelectedRow.Cells["ID"].ToString());
                    var member = databaseContext.Members.FirstOrDefault(s => s.ID == id);
                    databaseContext.Members.Remove(member);
                    databaseContext.SaveChanges();
                    currentSelectedRow = null;
                }
            } else
            {
                MessageBox.Show("Vui lòng chọn một dòng");
            }
            
        }

        private void daMembers_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void btnBranchManage_Click(object sender, EventArgs e)
        {
            var branch = new frmBranch();
            branch.ShowDialog();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {

            foreach (Control cnt in this.Controls)
                ResizeAll(cnt, base.Size);

            oldSize = base.Size;
        }

        private void ResizeAll(Control control, Size newSize)
        {
            try
            {
                int width = newSize.Width - oldSize.Width;
                control.Left += (control.Left * width) / oldSize.Width;
                control.Width += (control.Width * width) / oldSize.Width;

                int height = newSize.Height - oldSize.Height;
                control.Top += (control.Top * height) / oldSize.Height;
                control.Height += (control.Height * height) / oldSize.Height;
            }
            catch (Exception)
            {

                //throw;
            }
        }
    }
}
