using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFACRUD;

namespace CarRentalManagementSystem
{
    public partial class CarDetails : UserControl
    {
        string newId;
        DataAccess da = new DataAccess();
        public CarDetails()
        {
            InitializeComponent();
            this.da = new DataAccess();

            this.PopulateGridView();
            //this.AutoIdGenerate();
        }

        public static CarDetails _instance;

        public static CarDetails Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CarDetails();
                }
                return _instance;
            }
        }
        private void PopulateGridView(string sql = "select * from CarDetails;")
        {
            var ds = this.da.ExecuteQuery(sql);

            this.dgvCarDetails.AutoGenerateColumns = true;
            this.dgvCarDetails.DataSource = ds.Tables[0];
        }
        private void AutoIdGenerate()
        {
            var sql = "select Car Id from CarDetails order by Car Id desc;";
            var dt = this.da.ExecuteQueryTable(sql);
            string oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            newId = "car-" + (++num).ToString("d3");
            this.txtCarId.Text = newId;
        }

        private bool IsValidToSave()
        {
            if (String.IsNullOrEmpty(this.txtCarId.Text) || String.IsNullOrEmpty(this.txtModel.Text) ||
                String.IsNullOrEmpty(this.txtBrand.Text) || String.IsNullOrEmpty(this.txtBodyColor.Text) ||
                String.IsNullOrEmpty(this.cmbSeat.Text))
                return false;
            else
                return true;
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

                var query = "select * from CarDetails where Car Id = '" + this.txtCarId.Text + "';";
                var ds = this.da.ExecuteQuery(query);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    var sql = @"update CarDetails set  
                                Brand = '" + this.txtBrand.Text + @"',
                                Model = '" + this.txtModel.Text + @"',
                                Body Color = '" + this.txtBodyColor.Text + @"',
                                Seat = '"+this.cmbSeat.Text+@"'
                                where Car Id = '" + this.txtCarId.Text + @"';";
                    int count = this.da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data Updated");
                    else
                        MessageBox.Show("Data Upgradation Failure");
                }
                else
                {
                    var sql = @"insert into CArDetails values('" + this.txtCarId.Text + "','" + this.txtBrand.Text + "','" + this.txtModel.Text + "','" + this.txtBodyColor.Text + "','" + this.cmbSeat.Text + "');";
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
            this.txtCarId.Clear();
            this.txtBrand.Clear();
            this.txtModel.Text = "";
            this.txtBodyColor.Clear();
            this.cmbSeat.SelectedIndex = -1;

            this.txtSearch.Clear();

            this.dgvCarDetails.ClearSelection();
            this.AutoIdGenerate();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sql = "select * from CarDetails where Car Id = '" + this.txtSearch.Text + "';";
            this.PopulateGridView(sql);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.PopulateGridView();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarDetails.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a row first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (this.dgvCarDetails.CurrentRow == null)
                {
                    MessageBox.Show("Invalid current row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                var caridCell = this.dgvCarDetails.CurrentRow.Cells[0].Value;
                var brandCell = this.dgvCarDetails.CurrentRow.Cells[1].Value;

                if (caridCell == null || brandCell == null)
                {
                    MessageBox.Show("Invalid cell values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var carid = caridCell.ToString();
                var brand = brandCell.ToString();

                var sql = "DELETE FROM CarDetails WHERE Car Id = '" + carid + "';";


                int count = this.da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show(brand.ToUpper() + " has been successfully removed from the user list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Failed to delete user data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.PopulateGridView();
                this.ClearAll();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL Error: " + sqlEx.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred: " + exc.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }

        private void dgvCarDetails_DoubleClick(object sender, EventArgs e)
        {
            this.txtCarId.Text = this.dgvCarDetails.CurrentRow.Cells[0].Value.ToString();
            this.txtBrand.Text = this.dgvCarDetails.CurrentRow.Cells[1].Value.ToString();
            this.txtModel.Text = this.dgvCarDetails.CurrentRow.Cells[2].Value.ToString();
            this.txtBodyColor.Text = this.dgvCarDetails.CurrentRow.Cells[3].Value.ToString();
            this.cmbSeat.Text = this.dgvCarDetails.CurrentRow.Cells[4].Value.ToString();
        }
    }
       
}
