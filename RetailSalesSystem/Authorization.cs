using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RetailSalesSystem
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection test = new MySqlConnection("server=" + textBox1.Text + ";database=" + textBox2.Text + ";uid=" + textBox3.Text + ";pwd=" + textBox4.Text);
                test.Open();
                Program.connectionString = "server=" + textBox1.Text + ";database=" + textBox2.Text + ";uid=" + textBox3.Text + ";pwd=" + textBox4.Text;
                MainFrom form = new MainFrom();
                form.Show();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
