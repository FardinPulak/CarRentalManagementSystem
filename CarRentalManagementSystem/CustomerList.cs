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
     
    public partial class CustomerList : UserControl
    {
        string newId;
        DataAccess da = new DataAccess();
        public CustomerList()
        {
            InitializeComponent();


        }
        public static CustomerList _instance;
        public static CustomerList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CustomerList();
                }
                return _instance;
            }
        }

        private void PopulateGridView(string sql = "select * from CustomerList;")
        {
            var ds = this.da.ExecuteQuery(sql);

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = ds.Tables[0];
        }
        private String AutoIdGenerate()
        {
            var sql = "select Customer_ID from  CustomerList order by Customer_ID desc;";
            var dt = this.da.ExecuteQueryTable(sql);
            string oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            newId = "cus-" + (++num).ToString("d3");
            this.txtCustomerId.Text = newId;
            return newId;
        }
        private bool IsValidToSave()
        {
            if (String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtCustomerId.Text) ||
                String.IsNullOrEmpty(this.txtPhone.Text) || String.IsNullOrEmpty(this.txtCarId.Text) ||
                String.IsNullOrEmpty(this.txtRentCost.Text) || String.IsNullOrEmpty(this.txtAdvance.Text) ||
                String.IsNullOrEmpty(this.dtpRentalDay.Text)|| String.IsNullOrEmpty(this.txtTotal.Text)||
                String.IsNullOrEmpty(this.txtDue.Text)|| String.IsNullOrEmpty(this.txtDays.Text))
                return false;
            else
                return true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sql = "select * from CustomerList where Customer_ID = '" + this.txtSearch.Text + "';";
            this.PopulateGridView(sql);
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

                var query = "select * from CustomerList where Id = '" + this.txtCustomerId.Text + "';";
                var ds = this.da.ExecuteQuery(query);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    var sql = @"update CustomerList set  
                                Name = '" + this.txtName.Text + @"',
                                Rental Day = '" + this.dtpRentalDay.Text + @"',
                                Car Id = '" + this.txtCarId.Text + @"',
                                Phone = '" + this.txtPhone.Text + @"',
                                Days = " + this.txtDays.Text + @",
                                Rent Cost= '"+this.txtRentCost.Text+ @"',
                                Total = '" + this.txtTotal.Text + @"',
                                Advance='" + this.txtAdvance.Text+ @"',
                                Due = '"+this.txtDue.Text + @"'
                                where Customer_ID = '" + this.txtCustomerId.Text + @"';";
                    int count = this.da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data Updated");
                    else
                        MessageBox.Show("Data Upgradation Failure");
                }
                else
                {
                    var sql = @"insert into CustomerList values('"+this.txtCustomerId.Text+"','"+this.txtName.Text+"','"+this.txtPhone.Text+"','"+this.txtCarId.Text+"','"+this.dtpRentalDay.Text+"',"+this.txtDays.Text+",'"+this.txtRentCost.Text+"','"+this.txtTotal.Text+"','"+this.txtAdvance.Text+"','"+this.txtDue.Text+"');";
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
            this.txtName.Clear();
            this.txtCustomerId.Clear();
            this.txtAdvance.Clear();
            this.txtPhone.Clear();
            this.txtDays.Clear();
            this.txtDue.Clear();
            this.txtRentCost.Clear();
            this.txtTotal.Clear();
            this.txtCarId.Clear();

            this.txtSearch.Clear();

            this.dataGridView1.ClearSelection();
            this.AutoIdGenerate();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            
            this.PopulateGridView();
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGridView1.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a row first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (this.dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Invalid current row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                var idCell = this.dataGridView1.CurrentRow.Cells[0].Value;
                var nameCell = this.dataGridView1.CurrentRow.Cells[1].Value;

                if (idCell == null || nameCell == null)
                {
                    MessageBox.Show("Invalid cell values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var id = idCell.ToString();
                var name = nameCell.ToString();

                var sql = "DELETE FROM CustomerList WHERE Customer_ID = '" + id + "';";


                int count = this.da.ExecuteDMLQuery(sql);

                if (count == 1)
                    MessageBox.Show(name.ToUpper() + " has been successfully removed from the user list.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
