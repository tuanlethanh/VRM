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
    public partial class frmBranch : Form
    {

        private readonly DatabaseContext databaseContext = new DatabaseContext();
        DataGridViewRow currentSelectedRow;
        public frmBranch()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            refreshGrid();
        }

        private void frmBranch_Load(object sender, EventArgs e)
        {
            refreshGrid();
        }

        void refreshGrid()
        {
            var query = databaseContext.CHIHOIs.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(txtKeyword.Text))
            {
                query.Where(s => txtKeyword.Text.Contains(s.TENCHIHOI)
                || txtKeyword.Text.Contains(s.MACHIHOI));
            }

            daBranch.DataSource = query.ToList();
        }
        void showEditForm(CHIHOI branch)
        {
            var editBranch = new frmEditBranch();
            editBranch.branch = branch;
            if (editBranch.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                refreshGrid();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            showEditForm(null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var id = int.Parse(currentSelectedRow.Cells["ID"].Value.ToString());

                var hoiviens = databaseContext.HOIVIENs.Where(s => s.CHIHOI_ID == id).Count();
                if (hoiviens > 0)
                {
                    MessageBox.Show("Chi hội này đã có hội viên, vui lòng xóa các hội viên thuộc chi hội hoặc chuyển hội viên sang chi hội khác", "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var branch = databaseContext.CHIHOIs.FirstOrDefault(r => r.ID == id);
                if (branch != null)
                {
                    databaseContext.CHIHOIs.Remove(branch);
                    databaseContext.SaveChanges();
                    
                }
                refreshGrid();
            }
        }

        private void daBranch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = this.daBranch.CurrentRow;
        }

        private void daBranch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            currentSelectedRow = this.daBranch.CurrentRow;
            showEditForm(new CHIHOI
            {
                MACHIHOI = currentSelectedRow.Cells["CODE"].Value.ToString(),
                TENCHIHOI = currentSelectedRow.Cells["NAME"].Value.ToString(),
                ID = int.Parse(currentSelectedRow.Cells["ID"].Value.ToString())
            });
        }

        private void frmBranch_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
