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
using Excel = Microsoft.Office.Interop.Excel;

namespace RetailSalesSystem
{
    public partial class StockItems : Form
    {
        MySqlDataAdapter dataAdapter;
        MySqlConnection connection;
        MySqlCommandBuilder commandBuilder;
        BindingSource binding;
        public StockItems()
        {
            InitializeComponent();
            GetData();
        }

        private void GetData()
        {
            string selectCommand = "select `sum coming`.Номенклатор, (COALESCE(`sum coming`.`приход количество`,0) - COALESCE(`sum consumption`.`расход количество`,0) - COALESCE(`sum write-off`.`списание количество`,0)) as Количество,(COALESCE(`sum coming`.`приход опт`, 0) - COALESCE(`sum consumption`.`расход опт`, 0) - COALESCE(`sum write-off`.`списание опт`, 0)) as `Суммарная оптовая цена`,(COALESCE(`sum coming`.`приход розн`, 0) - COALESCE(`sum consumption`.`расход розн`, 0) - COALESCE(`sum write-off`.`списание розн`, 0)) as `Суммарная розничная цена` from(`sum coming` LEFT JOIN `sum write-off` on `sum coming`.Номенклатор = `sum write-off`.Номенклатор) left join `sum consumption` on `sum coming`.Номенклатор = `sum consumption`.Номенклатор; ";
            try
            {

                using (connection = new MySqlConnection(Program.connectionString))
                {
                    dataAdapter = new MySqlDataAdapter();
                    dataAdapter.SelectCommand = new MySqlCommand(selectCommand, connection);
                    commandBuilder = new MySqlCommandBuilder(dataAdapter);
                    connection.Open();
                    DataTable table = new DataTable();
                    dataAdapter.Fill(table);
                    binding = new BindingSource();
                    binding.DataSource = table;
                    dataGridView1.DataSource = binding;
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportExel_Click(object sender, EventArgs e)
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
                for (j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow, StartCol + j];
                    myRange.Value2 = dataGridView1.Columns[j].HeaderText;
                }

                StartRow++;

                //Write datagridview content
                for (i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        try
                        {
                            Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[StartRow + i, StartCol + j];
                            myRange.Value2 = dataGridView1[j, i].Value == null ? "" : dataGridView1[j, i].Value;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            dataAdapter.Update((DataTable)binding.DataSource);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int x = 0;
            int y = 20;
            int cell_height = 0;

            int colCount = dataGridView1.ColumnCount;
            int rowCount = dataGridView1.RowCount - 1;

            Font font = new Font("Tahoma", 9, FontStyle.Bold, GraphicsUnit.Point);

            int[] widthC = new int[colCount];

            int current_col = 0;
            int current_row = 0;

            while (current_col < colCount)
            {
                if (g.MeasureString(dataGridView1.Columns[current_col].HeaderText.ToString(), font).Width > widthC[current_col])
                {
                    widthC[current_col] = (int)g.MeasureString(dataGridView1.Columns[current_col].HeaderText.ToString(), font).Width;
                }
                current_col++;
            }
            current_col = 0;
            while (current_row < rowCount)
            {
                while (current_col < colCount)
                {
                    if (g.MeasureString(dataGridView1[current_col, current_row].Value.ToString(), font).Width > widthC[current_col])
                    {
                        widthC[current_col] = (int)g.MeasureString(dataGridView1[current_col, current_row].Value.ToString(), font).Width;
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
            int height = dataGridView1[current_col, current_row].Size.Height;

            Rectangle cell_border;
            SolidBrush brush = new SolidBrush(Color.Black);


            while (current_col < colCount)
            {
                width = widthC[current_col];
                cell_height = dataGridView1[current_col, current_row].Size.Height;
                cell_border = new Rectangle(x, y, width, height);
                value = dataGridView1.Columns[current_col].HeaderText.ToString();
                g.DrawRectangle(new Pen(Color.Black), cell_border);
                g.DrawString(value, font, brush, x, y);
                x += widthC[current_col];
                current_col++;
            }
            current_col = 0;
            current_row = 0;
            x = 0;
            y += dataGridView1[current_col, current_row].Size.Height;
            while (current_row < rowCount)
            {
                while (current_col < colCount)
                {
                    width = widthC[current_col];
                    cell_height = dataGridView1[current_col, current_row].Size.Height;
                    cell_border = new Rectangle(x, y, width, height);
                    value = dataGridView1[current_col, current_row].Value.ToString();
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
