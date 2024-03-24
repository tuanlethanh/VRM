using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRM.Database;
using VRM.Utilities;

namespace VRM.Forms.Authentication
{
    public partial class frmChangePassword : Form
    {
        public frmChangePassword()
        {
            InitializeComponent();
        }
        private readonly DatabaseContext databaseContext = new DatabaseContext();
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text) 
                || string.IsNullOrEmpty(txtNewPassword.Text)
                || string.IsNullOrEmpty(txtOldPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin!");
                return;
            }

            string oldPass = CalculateMD5(txtOldPassword.Text);
            string newPass = CalculateMD5(txtNewPassword.Text);
            string confirmPass = CalculateMD5(txtConfirmPassword.Text);

            if (!newPass.Equals(confirmPass))
            {
                MessageBox.Show("Xác nhận mật khẩu mới không khớp, vui lòng kiểm tra lại!");
                return;
            }

            if (Constant.LoginUser.Password != oldPass)
            {
                MessageBox.Show("Mật khẩu cũ không đúng!");
                return;
            }

            var user = databaseContext.USERs.FirstOrDefault(x => x.ID == Constant.LoginUser.ID);
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy người dùng này!");
                return;
            }

            user.Password = newPass;
            databaseContext.SaveChanges();
            Constant.LoginUser = user;  

            DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }


        public string CalculateMD5(string input)
        {
            // Create an MD5 hash object
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);

                // Compute the hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
