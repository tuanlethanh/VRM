using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRM.Database;
using VRM.Entities;

namespace VRM.Forms
{
    public partial class frmModifyMember : Form
    {
        public frmModifyMember()
        {
            InitializeComponent();
        }

        public delegate void SaveChangedEventHandler(object sender);

        public event SaveChangedEventHandler SaveChanged;

        private readonly DatabaseContext databaseContext = new DatabaseContext();

        public Member member { get; set; }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var member = new Member();
            member.CODE = txtCode.Text;
            member.FULLNAME = txtFullName.Text;
            member.DOB = txtDateOfBirth.Value;
            member.GENDER = cboGender.SelectedText;
            member.ETHNIC = txtethnic.Text;
            member.RELIGION = txtCode.Text;
            member.HOMETOWN = txtHomtown.Text;
            member.RESIDENCE = txtResidence.Text;
            member.ADDRESS = txtAddress.Text;
            member.MOBILE_NO = txtPhoneNumber.Text;
            member.EMAIL = txtEmail.Text;
            member.QUALIFICATION = txtQuanlify.Text;
            member.ACADEMIC = txtAcademic.Text;
            member.POLITICAL_THEORY = txtPoliticalTheory.Text;
            member.BRANCH_ID = cboBranch.SelectedValue != null ? int.Parse(cboBranch.SelectedValue.ToString()) : 0;
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Images")))
            {
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Images"));
            }
            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Images", member.CODE)))
            {
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Images", member.CODE));
            }

            var savePath = Path.Combine(Application.StartupPath, "Images", member.CODE, Path.GetFileName(pbAvatar.ImageLocation));
            File.Copy(pbAvatar.ImageLocation, savePath);
            member.IMAGE = pbAvatar.ImageLocation;

            databaseContext.Members.Add(member);
            databaseContext.SaveChanges();
            DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbAvatar.ImageLocation = openFileDialog.FileName;
            }
        }

        private void frmModifyMember_Load(object sender, EventArgs e)
        {
            fillFormData();
        }

        void fillFormData()
        {
            var branchs = databaseContext.Branches.ToList();
            cboBranch.DataSource = branchs;
            cboBranch.ValueMember = "ID";
            cboBranch.DisplayMember = "NAME";
            if (member != null)
            {
                if (branchs.Count > 0)
                {
                    cboBranch.SelectedValue = member.ID;
                }
                txtCode.Text = member.CODE;
                txtFullName.Text = member.FULLNAME;
                cboGender.Text = member.GENDER;
                txtDateOfBirth.Value = member.DOB;
                txtethnic.Text = member.ETHNIC;
                txtReligon.Text = member.RELIGION;
                txtHomtown.Text = member.HOMETOWN;
                txtResidence.Text = member.RESIDENCE;
                txtAddress.Text = member.ADDRESS;
                txtPhoneNumber.Text = member.MOBILE_NO;
                txtEmail.Text = member.EMAIL;
                txtQuanlify.Text = member.QUALIFICATION;
                txtPoliticalTheory.Text = member.POLITICAL_THEORY;
                pbAvatar.ImageLocation = member.IMAGE;
            }
        }
    }
}
