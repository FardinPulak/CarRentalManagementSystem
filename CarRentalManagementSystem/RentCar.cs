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
    public partial class RentCar : UserControl
    {
        public static RentCar _instance;
        public RentCar()
        {
            InitializeComponent();
        }
        
        
        string newId;
        DataAccess da = new DataAccess();
        private void PopulateGridView(string sql = "select * from RentCar;")
        {
            var ds = this.da.ExecuteQuery(sql);

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = ds.Tables[0];
        }
        private String AutoIdGenerate()
        {
            var sql = "select Rent_Id from  RentCar order by Rent_Id desc" +
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
            if (String.IsNullOrEmpty(this.txtRentId.Text)   || String.IsNullOrEmpty(this.txtCustomerId.Text)||
                String.IsNullOrEmpty(this.txtRentCost.Text) || String.IsNullOrEmpty(this.txtCarId.Text)     || 
                String.IsNullOrEmpty(this.dtpRentalDay.Text)|| String.IsNullOrEmpty(this.txtDays.Text))
                return false;
            else
                return true;
        }

        public static RentCar Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RentCar();
                }
                return _instance;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidToSave())
                {
                    MessageBox.Show("Please fill all the information");
                    return;
                }

                var query = "select * from RentCar where Rent_Id = '" + this.txtRentId.Text + "';";
                var ds = this.da.ExecuteQuery(query);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    var sql = @"update RentCar set  
                                
                                Car Id = '" + this.txtCarId.Text + @"'
                                Customer_ID = '" + this.txtCustomerId.Text + @"',
                                Rental Day = '" + this.dtpRentalDay.Text + @"',
                                Days = " + this.txtDays.Text + @",
                                Rent Cost= '" + this.txtRentCost.Text + @"',
                                where Rent_Id = '" + this.txtRentId.Text + @"';";
                    int count = this.da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data Updated");
                    else
                        MessageBox.Show("Data Upgradation Failure");
                }
                else
                {
                    var sql = @"insert into RentCar values( '" + this.txtCustomerId.Text + "', '" + this.dtpRentalDay.Text + "'," + this.txtDays.Text + ", '" + this.txtRentCost.Text + "','" + this.txtRentId.Text +"');";
                    int count = this.da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data Added");
                    else
                        MessageBox.Show("Data Addition Failure");
                }

                this.PopulateGridView();
                this.ClearAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show("There is an error in your input: " + exc.Message);
            }
        }
        private void ClearAll()
        {
            this.txtRentId.Clear();
            this.txtCustomerId.Clear();
            this.txtDays.Clear();
            this.txtRentCost.Clear();
            this.txtCarId.Clear();

            this.txtSearch.Clear();

            this.dataGridView1.ClearSelection();
            this.AutoIdGenerate();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll(); 
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
