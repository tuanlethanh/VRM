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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private readonly DatabaseContext databaseContext = new DatabaseContext();

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Constant.LoginUser.ID == 0)
            {
                var listUser = databaseContext.USERs.ToList();
                if (listUser.Count == 0)
                {
                    databaseContext.USERs.Add(new Entities.USER
                    {
                        UserName = "admin",
                        Password = CalculateMD5("123456"),
                        Role = "ADMIN",
                        FullName = "Administrator"
                    });
                    databaseContext.SaveChanges();
                }
            }
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

        private void frmLogin_Enter(object sender, EventArgs e)
        {
            Login();
        }

        void Login()
        {
            if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!");
                return;
            }

            string userName = txtUserName.Text;
            string password = CalculateMD5(txtPassword.Text);

            var user = databaseContext.USERs.FirstOrDefault(s => s.UserName.ToLower().Equals(userName) && s.Password.Equals(password));
            if (user == null)
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!");
                return;
            }
            Constant.LoginUser = user;
            DialogResult = DialogResult.OK;
        }
    }
}
