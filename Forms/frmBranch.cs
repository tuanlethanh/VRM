﻿using System;
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
            var query = databaseContext.Branches.AsQueryable();
            if (!string.IsNullOrEmpty(txtKeyword.Text))
            {
                query.Where(s => txtKeyword.Text.Contains(s.NAME)
                || txtKeyword.Text.Contains(s.CODE));
            }

            daBranch.DataSource = query.ToList();
        }
        void showEditForm(Branch branch)
        {
            var editBranch = new frmEditBranch();
            editBranch.branch = branch;
            if (editBranch.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Cập nhật thành công", "Thông báo");
                refreshGrid();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            showEditForm(null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa bản ghi này", "Xác nhận xóa", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var id = int.Parse(currentSelectedRow.Cells["ID"].Value.ToString());
                var branch = databaseContext.Branches.FirstOrDefault(r => r.ID == id);
                if (branch != null)
                {
                    databaseContext.Branches.Remove(branch);
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
            showEditForm(new Branch
            {
                CODE = currentSelectedRow.Cells["CODE"].Value.ToString(),
                NAME = currentSelectedRow.Cells["NAME"].Value.ToString(),
                ID = int.Parse(currentSelectedRow.Cells["ID"].Value.ToString())
            });
        }
    }
}
