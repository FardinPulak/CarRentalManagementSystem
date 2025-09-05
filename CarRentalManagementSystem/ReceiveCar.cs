using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFACRUD;

namespace CarRentalManagementSystem
{
    public partial class ReceiveCar : UserControl
    {
        public ReceiveCar()
        {
            InitializeComponent();
        }

        DataAccess da = new DataAccess();
        string newId;

        public static ReceiveCar _instance;
        public static ReceiveCar Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReceiveCar();
                }
                return _instance;
            }
        }
        private void PopulateGridView(string sql = "select * from ReceiveCar;")
        {
            var ds = this.da.ExecuteQuery(sql);

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = ds.Tables[0];
        }
        private String AutoIdGenerate()
        {
            var sql = "select Rent_Id from  ReceiveCar order by Rent_Id desc" +
                " desc;";
            var dt = this.da.ExecuteQueryTable(sql);
            string oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            newId = "r-" + (++num).ToString("d3");
            this.txtCustomerId.Text = newId;
            return newId;
        }
        private bool IsValidToSave()
        {
            if (String.IsNullOrEmpty(this.txtTotal.Text) || String.IsNullOrEmpty(this.txtCustomerId.Text) ||
                String.IsNullOrEmpty(this.txtRentCost.Text) || String.IsNullOrEmpty(this.txtCarId.Text) ||
                String.IsNullOrEmpty(this.dtpRentalDay.Text) || String.IsNullOrEmpty(this.txtAdvance.Text) || String.IsNullOrEmpty(this.txtDue.Text))
                return false;
            else
                return true;
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sql = "select * from RentCar where Rent_Id = '" + this.txtSearch.Text + "';";
            this.PopulateGridView(sql);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.PopulateGridView();
        }

     
    }
}
