using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetailSalesSystem
{
    public partial class MainFrom : Form
    {
        public MainFrom()
        {
            InitializeComponent();
            reloadData();
            Console.WriteLine(Program.data1.ToString("yyyy-MM-dd"));
        }
        private void reloadData()
        {
            Program.data1 = dateTimePicker1.Value;
            Program.data2 = dateTimePicker2.Value;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Overhead form = new Overhead();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StockItems form = new StockItems();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SalesDp form = new SalesDp();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Nomenclature form = new Nomenclature();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainFrom_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Departments form = new Departments();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Counterparties form = new Counterparties();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            reloadData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            reloadData();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                RDDepartmen form = new RDDepartmen();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                RDCounterpaties form = new RDCounterpaties();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                ComingProduct form = new ComingProduct();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                СonsumptionProduct form = new СonsumptionProduct();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                WriteoffProduct form = new WriteoffProduct();
                form.Show();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
