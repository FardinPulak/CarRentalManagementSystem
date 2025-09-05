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
    public partial class LogIn : Form
    {
        DataAccess da = new DataAccess();
        public LogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string sql = "select * from UserID where ID = '" + this.txtUserId.Text + "' and Password = '" + this.txtPassword.Text + "';";
            var ds = this.da.ExecuteQuery(sql);
            
            if (ds.Tables[0].Rows.Count == 1)
            {
                MessageBox.Show("Welcome...");

                if (ds.Tables[0].Rows[0][6].ToString() == "Manager")
                {
                    this.Hide();
                    new Manager().Show();
                }
                else if (ds.Tables[0].Rows[0][6].ToString() == "Employee")
                {
                    this.Hide();
                    new Employee().Show();
                }
            }
            else
                MessageBox.Show("Invalid User Id or Password!!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(100,0,0,0);
        }
    }
}
