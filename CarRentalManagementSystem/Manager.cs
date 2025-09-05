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
    public partial class Manager : Form
    {
        
        public Manager()
        {
            InitializeComponent();
            
        }

        

        private void btnEmployeeList_Click(object sender, EventArgs e)
        {
            if (!panel2.Controls.Contains(EmployeeList.Instance))
            {
                panel2.Controls.Add(EmployeeList.Instance);
                EmployeeList.Instance.Dock = DockStyle.Fill;
                EmployeeList.Instance.BringToFront();
            }
            else
            {
                EmployeeList.Instance.BringToFront();
            }
        }
       
        private void btnCustomerList_Click(object sender, EventArgs e)
        {
            if (!panel2.Controls.Contains(CustomerList.Instance))
            {
                panel2.Controls.Add(CustomerList.Instance);
                CustomerList.Instance.Dock = DockStyle.Fill;
                CustomerList.Instance.BringToFront();
            }
            else
            {
                CustomerList.Instance.BringToFront();
            }
        }

        

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogIn l=new LogIn();
            l.Show();
        }

       
        private void btnCarDetails_Click(object sender, EventArgs e)
        {
            if (!panel2.Controls.Contains(CarDetails.Instance))
            {
                panel2.Controls.Add(CarDetails.Instance);
                CarDetails.Instance.Dock = DockStyle.Fill;
                CarDetails.Instance.BringToFront();
            }
            else
            {
                CarDetails.Instance.BringToFront();
            }
        }
    }
}
