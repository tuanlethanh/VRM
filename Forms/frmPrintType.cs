using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VRM.Models;
using VRM.Utilities;

namespace VRM.Forms
{
    public partial class frmPrintType : Form
    {
        public frmPrintType()
        {
            InitializeComponent();
        }
        public delegate void SaveChangedEventHandler(object sender);

        public event SaveChangedEventHandler SaveChanged;

        public String PrintType {  get; set; }

        private void frmPrintType_Load(object sender, EventArgs e)
        {
            List<DropdownModel> printType = new List<DropdownModel>
            {
                new DropdownModel {Id = Constant.PRINT_MEMBER_LIST_FULL, Name = "Danh sách hội viên đầy đủ"},
                new DropdownModel {Id = Constant.PRINT_MEMBER_LIST_SHORT, Name = "Danh sách hội viên rút gọn"},
                new DropdownModel {Id = Constant.PRINT_MEMBER_LIST_DS_HOIVIEN_TT, Name = "Danh sách thông tin hội viên"},
                new DropdownModel {Id = Constant.PRINT_MEMBER_LIST_DS_HOIVIEN, Name = "Danh sách hội viên"},
            };

            cboPrintType.DataSource = printType;
            cboPrintType.DisplayMember = "Name";
            cboPrintType.ValueMember = "Id";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintType = cboPrintType.SelectedValue.ToString();
            //SaveChanged(cboPrintType.SelectedValue);
            DialogResult = DialogResult.OK;
        }
    }
}
