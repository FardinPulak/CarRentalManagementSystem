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
    public partial class EmployeeList : UserControl
    {
        string newId;
        DataAccess da = new DataAccess();
        public EmployeeList()
        {
            InitializeComponent();
            this.da = new DataAccess();

            this.PopulateGridView();
            this.AutoIdGenerate();
        }
        
        public static EmployeeList _instance;

        public static EmployeeList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EmployeeList();
                }
                return _instance;
            }
        }
        private void PopulateGridView(string sql = "select * from UserID where Post = 'Employee';")
        {
            var ds = this.da.ExecuteQuery(sql);

            this.dgvEmployeeList.AutoGenerateColumns = true;
            this.dgvEmployeeList.DataSource = ds.Tables[0];
        }
        private String AutoIdGenerate()
        {
            var sql = "select Id from UserID where Post='Employee' order by Id desc;";
            var dt = this.da.ExecuteQueryTable(sql);
            string oldId = dt.Rows[0][0].ToString();
            string[] temp = oldId.Split('-');
            int num = Convert.ToInt32(temp[1]);
            newId = "e-" + (++num).ToString("d3");
            this.txtUserId.Text = newId;
            return newId;
        }
        private bool IsValidToSave()
        {
            if (String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtUserId.Text) ||
                String.IsNullOrEmpty(this.txtPhone.Text) || String.IsNullOrEmpty(this.txtMail.Text) ||
                String.IsNullOrEmpty(this.txtSalary.Text)||String.IsNullOrEmpty(this.txtPassword.Text) ||
                String.IsNullOrEmpty(this.cmbGender.Text))
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

                var query = "select * from UserID where Id = '" + this.txtUserId.Text + "'and Post = 'Employee';";
                var ds = this.da.ExecuteQuery(query);

                if (ds.Tables[0].Rows.Count == 1)
                {
                    var sql = @"update UserID set  
                                Name = '" + this.txtName.Text + @"',
                                [Date Of Birth] = '" + this.dtpDOB.Text + @"',
                                Gender = '" + this.cmbGender.Text + @"',
                                Phone = '" + this.txtPhone.Text + @"',
                                Mail = '" + this.txtMail.Text + @"',
                                Post= 'Employee',
                                Salary= '"+txtSalary +@"',
                                Password='"+txtPassword+ @"'
                                where Id = '"+ this.txtUserId.Text +@"';";
                    int count = this.da.ExecuteDMLQuery(sql);

                    if (count == 1)
                        MessageBox.Show("Data Updated");
                    else
                        MessageBox.Show("Data Upgradation Failure");
                }
                else
                {
                    var sql = @"insert into UserID values('"+ this.txtUserId.Text + "','"+ this.txtName.Text + "','"+ this.dtpDOB.Text +"','"+ this.cmbGender.Text + "','"+ this.txtPhone.Text + "','"+this.txtMail.Text +"','Employee','"+this.txtSalary.Text +"','"+ this.txtPassword.Text +"');";
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
            this.txtUserId.Clear();
            this.dtpDOB.Text = "";
            this.txtPhone.Clear();
            this.txtMail.Text = "";
            this.cmbGender.SelectedIndex = -1;
            this.txtPassword.Clear();
            this.txtSalary.Clear();

            this.txtSearch.Clear();

            this.dgvEmployeeList.ClearSelection();
            this.AutoIdGenerate();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.PopulateGridView();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sql = "select * from UserID where Post = 'Employee' and Id = '" + this.txtSearch.Text + "';";
            this.PopulateGridView(sql);
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text=string.Empty;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvEmployeeList.SelectedRows.Count < 1)
                {
                    MessageBox.Show("Please select a row first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (this.dgvEmployeeList.CurrentRow == null)
                {
                    MessageBox.Show("Invalid current row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                    return;

                var idCell = this.dgvEmployeeList.CurrentRow.Cells[0].Value;
                var nameCell = this.dgvEmployeeList.CurrentRow.Cells[1].Value;

                if (idCell == null || nameCell == null)
                {
                    MessageBox.Show("Invalid cell values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var id = idCell.ToString();
                var name = nameCell.ToString();

                var sql = "DELETE FROM UserID WHERE Id = '" + id + "';";

               
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }

        private void dgvEmployeeList_DoubleClick(object sender, EventArgs e)
        {
            this.txtUserId.Text = this.dgvEmployeeList.CurrentRow.Cells[0].Value.ToString();
            this.txtName.Text = this.dgvEmployeeList.CurrentRow.Cells[1].Value.ToString();
            this.dtpDOB.Text = this.dgvEmployeeList.CurrentRow.Cells[2].Value.ToString();
            this.cmbGender.Text = this.dgvEmployeeList.CurrentRow.Cells[3].Value.ToString();
            this.txtPhone.Text = this.dgvEmployeeList.CurrentRow.Cells[4].Value.ToString();
            this.txtMail.Text = this.dgvEmployeeList.CurrentRow.Cells[5].Value.ToString();
            this.txtSalary.Text = this.dgvEmployeeList.CurrentRow.Cells[7].Value.ToString();
            this.txtPassword.Text = this.dgvEmployeeList.CurrentRow.Cells[8].Value.ToString();
        }
    }
}
