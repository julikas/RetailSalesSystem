using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.Net.Http.Headers;

namespace RetailSalesSystem
{
    public partial class Overhead : Form
    {
        MySqlDataAdapter masterDataAdapter;
        MySqlDataAdapter detailsDataAdapter;
        MySqlConnection connection;
        MySqlCommandBuilder masterCommandBuilder;
        MySqlCommandBuilder detailsCommandBuilder;
        BindingSource masterBinding;
        BindingSource binding;
        public Overhead()
        {
            InitializeComponent();
            GetData();
        }

        private void GetData()
        {
            masterBinding = new BindingSource();
            binding = new BindingSource();
            connection = new MySqlConnection(Program.connectionString);
            DataSet data = new DataSet();
            data.Locale = System.Globalization.CultureInfo.InvariantCulture;

            masterDataAdapter = new
                MySqlDataAdapter("select * from `заголовки накладной`;", connection);
            masterDataAdapter.Fill(data, "заголовки накладной");
            masterCommandBuilder = new MySqlCommandBuilder(masterDataAdapter);

            detailsDataAdapter = new
                MySqlDataAdapter("select * from `табличная часть накладной`;", connection);
            detailsDataAdapter.Fill(data, "табличная часть накладной");
            detailsCommandBuilder = new MySqlCommandBuilder(detailsDataAdapter);

            DataRelation relation = new DataRelation("CustomersOrders",
                data.Tables["заголовки накладной"].Columns["Номер"],
                data.Tables["табличная часть накладной"].Columns["Номер ЗН"]);
            data.Relations.Add(relation);

            masterBinding.DataSource = data;
            masterBinding.DataMember = "заголовки накладной";
            dataGridView1.DataSource = masterBinding;

            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;

            binding.DataSource = masterBinding;
            binding.DataMember = "CustomersOrders";
            dataGridView2.DataSource = binding;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            masterDataAdapter.UpdateCommand = masterCommandBuilder.GetUpdateCommand();
            masterDataAdapter.Update(((DataSet)masterBinding.DataSource).Tables["заголовки накладной"]);
            detailsDataAdapter.UpdateCommand = detailsCommandBuilder.GetUpdateCommand();
            detailsDataAdapter.Update(((DataSet)((BindingSource)binding.DataSource).DataSource).Tables["табличная часть накладной"]);
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null)
                {
                    continue;
                }

                var nomer = (int)dataGridView1.Rows[i].Cells[0].Value;

                var sumOpt = 0;
                var sumRozn = 0;
                for (int j = 0; j < dataGridView2.Rows.Count; j++)
                {
                    if (Enumerable.Range(0, dataGridView2.Columns.Count)
                        .Any(ind => dataGridView2.Rows[j].Cells[ind].Value == null
                            || dataGridView2.Rows[j].Cells[ind].Value as string == string.Empty))
                    {
                        continue;
                    };

                    if ((int)dataGridView2.Rows[j].Cells[2].Value != nomer)
                    {
                        continue;
                    }

                    sumOpt += (int)dataGridView2.Rows[j].Cells[3].Value * (int)dataGridView2.Rows[j].Cells[4].Value;
                    sumRozn += (int)dataGridView2.Rows[j].Cells[3].Value * (int)dataGridView2.Rows[j].Cells[5].Value;
                }

                dataGridView1.Rows[i].Cells[5].Value = sumOpt;
                dataGridView1.Rows[i].Cells[6].Value = sumRozn;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet sheet1 = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                int StartCol = 1;
                int StartRow = 1;
                int j = 0, i = 0;

                //Write Headers
                for (j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow, StartCol + j];
                    myRange.Value2 = dataGridView1.Columns[j].HeaderText;
                }

                StartRow++;

                //Write datagridview content
                for (i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        try
                        {
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow + i, StartCol + j];
                            myRange.Value2 = dataGridView2[j, i].Value == null ? "" : dataGridView2[j, i].Value;
                        }
                        catch
                        {
                            ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 0;
            int y = 20;
            int cell_height = 0;

            int colCount = dataGridView2.ColumnCount;
            int rowCount = dataGridView2.RowCount - 1;

            Font font = new Font("Tahoma", 9, FontStyle.Bold, GraphicsUnit.Point);

            int[] widthC = new int[colCount];

            int current_col = 0;
            int current_row = 0;

            while (current_col < colCount)
            {
                if (g.MeasureString(dataGridView2.Columns[current_col].HeaderText.ToString(), font).Width > widthC[current_col])
                {
                    widthC[current_col] = (int)g.MeasureString(dataGridView2.Columns[current_col].HeaderText.ToString(), font).Width;
                }
                current_col++;
            }
            current_col = 0;
            while (current_row < rowCount)
            {
                while (current_col < colCount)
                {
                    if (g.MeasureString(dataGridView2[current_col, current_row].Value.ToString(), font).Width > widthC[current_col])
                    {
                        widthC[current_col] = (int)g.MeasureString(dataGridView2[current_col, current_row].Value.ToString(), font).Width;
                    }
                    current_col++;
                }
                current_col = 0;
                current_row++;
            }

            current_col = 0;
            current_row = 0;

            string value = "";

            int width = widthC[current_col];
            int height = dataGridView2[current_col, current_row].Size.Height;

            Rectangle cell_border;
            SolidBrush brush = new SolidBrush(Color.Black);


            while (current_col < colCount)
            {
                width = widthC[current_col];
                cell_height = dataGridView2[current_col, current_row].Size.Height;
                cell_border = new Rectangle(x, y, width, height);
                value = dataGridView2.Columns[current_col].HeaderText.ToString();
                g.DrawRectangle(new Pen(Color.Black), cell_border);
                g.DrawString(value, font, brush, x, y);
                x += widthC[current_col];
                current_col++;
            }
            current_col = 0;
            current_row = 0;
            x = 0;
            y += dataGridView2[current_col, current_row].Size.Height;
            while (current_row < rowCount)
            {
                while (current_col < colCount)
                {
                    width = widthC[current_col];
                    cell_height = dataGridView2[current_col, current_row].Size.Height;
                    cell_border = new Rectangle(x, y, width, height);
                    value = dataGridView2[current_col, current_row].Value.ToString();
                    g.DrawRectangle(new Pen(Color.Black), cell_border);
                    g.DrawString(value, font, brush, x, y);
                    x += widthC[current_col];
                    current_col++;
                }
                current_col = 0;
                current_row++;
                x = 0;
                y += cell_height;
            }
        }

        private void PrintButton_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument1;
            if (printDialog.ShowDialog() == DialogResult.OK)
                printDialog.Document.Print();
        }
    }
}
