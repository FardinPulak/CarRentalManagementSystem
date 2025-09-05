using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalManagementSystem
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }


        

        private void btnRentCar_Click(object sender, EventArgs e)
        {
            if (!panel2.Controls.Contains(RentCar.Instance))
            {
                panel2.Controls.Add(RentCar.Instance);
                RentCar.Instance.Dock = DockStyle.Fill;
                RentCar.Instance.BringToFront();
            }
            else
            {
                RentCar.Instance.BringToFront();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            LogIn l = new LogIn();
            l.Show();
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

        private void btnReceive_Click(object sender, EventArgs e)
        {
            if (!panel2.Controls.Contains(ReceiveCar.Instance))
            {
                panel2.Controls.Add(ReceiveCar.Instance);
                ReceiveCar.Instance.Dock = DockStyle.Fill;
                ReceiveCar.Instance.BringToFront();
            }
            else
            {
                ReceiveCar.Instance.BringToFront();
            }

        }
    }
}
