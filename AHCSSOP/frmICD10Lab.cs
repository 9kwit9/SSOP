using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel; 

namespace AHCSSOP
{
    public partial class frmICD10Lab : Form
    {
        SConnectMySQL MySQLdb;
        SConnectMSSL MSSQL;
        Ctool_Control Ctrl;

        DataDs.DataSetICD10 Dds;

        public frmICD10Lab()
        {
            InitializeComponent();
            MySQLdb = new SConnectMySQL();
            MSSQL = new SConnectMSSL();
            Ctrl = new Ctool_Control();
        }

        private void frmICD10Lab_Load(object sender, EventArgs e)
        {
            DataSet ds = MySQLdb.Select_Value_ICD10Lab();
            if (ds != null)
            {
                gl.DataSource = ds.Tables["0"];
                //label1.Text = ds.Tables["0"].Rows.Count.ToString();
                gl.Columns[0].Width = 100;
                gl.Columns[1].Width = 100;
                gl.Columns[2].Width = 200;
                gl.Columns[3].Width = 120;
                gl.Columns[11].Width = 520;
            }
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (gl.RowCount != 0)
            {
                Dds = new DataDs.DataSetICD10();

                gl1.DataSource = null;
                gl1.Rows.Clear();
                gl1.Refresh();

                try
                {
                    Dds.Tables["0"].Rows.Clear();
                    Dds = null;
                }
                catch { }

                for (int i = 0; i <= gl.Rows.Count - 1; i++)
                {
                    Application.DoEvents();
                    try
                    {
                        gl1.Rows.Add();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[0].Value = gl.Rows[i].Cells[0].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[1].Value = gl.Rows[i].Cells[1].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[2].Value = gl.Rows[i].Cells[2].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[3].Value = gl.Rows[i].Cells[3].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[4].Value = gl.Rows[i].Cells[4].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[5].Value = gl.Rows[i].Cells[5].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[6].Value = gl.Rows[i].Cells[6].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[7].Value = gl.Rows[i].Cells[7].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[8].Value = gl.Rows[i].Cells[8].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[9].Value = gl.Rows[i].Cells[9].Value.ToString();
                        gl1.Rows[gl1.Rows.Count - 1].Cells[10].Value = gl.Rows[i].Cells[10].Value.ToString();

                        //int j = 11;
                        //for (int j = 11; j <= gl1.ColumnCount - 1; j++)
                        foreach (string str in gl.Rows[i].Cells[11].Value.ToString().Split('#'))
                        {
                            //foreach (string str in gl.Rows[i].Cells[11].Value.ToString().Split(','))
                            for (int j = 11; j <= gl1.ColumnCount - 1; j++)
                            {
                                try
                                {
                                    if (gl1.Columns[j].HeaderText != "")
                                    {
                                        if (gl1.Columns[j].Tag.ToString() != "")
                                        {
                                            if ((gl1.Columns[j].Tag.ToString() == str.Split('|').GetValue(0).ToString()) && (gl1.Columns[j].HeaderText == str.Split('@').GetValue(0).ToString().Split('|').GetValue(1).ToString()))
                                            {
                                                //gl1.Rows[i].Cells[j].Value = str.Split(',').GetValue(1);
                                                gl1.Rows[gl1.Rows.Count - 1].Cells[j].Value = str.Split('@').GetValue(1).ToString();
                                                //Ctrl.AddControl(row, txt, str.Split(',').GetValue(1).ToString(), "", false);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        gl1.Columns[j].Tag = str.Split('|').GetValue(0).ToString();
                                        gl1.Columns[j].HeaderText = str.Split('@').GetValue(0).ToString().Split('|').GetValue(1).ToString();
                                        //gl1.Rows[i].Cells[j].Value = str.Split(',').GetValue(1);
                                        //Ctrl.AddControl(row, txt, str.Split(':').GetValue(1).ToString(), "", false);
                                        gl1.Rows[gl1.Rows.Count - 1].Cells[j].Value = str.Split('@').GetValue(1).ToString();
                                        break;
                                    }
                                }
                                catch { }
                            }
                        }

                        gl1.Refresh();
                    }
                    catch { }
                }
                MessageBox.Show("Complete", "Complete", MessageBoxButtons.OK);
            }
            this.Cursor = Cursors.Default;
        }

        private void ultraButton2_Click(object sender, EventArgs e)
        {
            excel();
        }

        void excel()
        {
            Application.DoEvents();
            //label3.Text = "Export Data";
            this.Cursor = Cursors.WaitCursor;
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            int i = 0;
            int j = 0;

            for (j = 0; j <= gl1.Columns.Count - 1; j++)
            {
                //DataGridViewCell cell = glview[j, i];
                try
                {
                    xlWorkSheet.Cells[i + 1, j + 1] = gl1.Columns[j].HeaderText;
                }
                catch { }
            }

            for (i = 0; i <= gl1.Rows.Count - 1; i++)
            {
                ultraLabel1.Text = i.ToString();
                for (j = 0; j <= gl1.Columns.Count - 1; j++)
                {
                    //DataGridViewCell cell = glview[j, i];
                    try
                    {
                        ultraLabel2.Text = j.ToString();
                        xlWorkSheet.Cells[i + 3, j + 1] = gl1.Rows[i].Cells[j].Value.ToString();
                    }
                    catch { }
                }
                //label1.Text = i.ToString();
            }

            xlWorkBook.SaveAs("C:\\informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            this.Cursor = Cursors.Default;

            MessageBox.Show("Excel file created , you can find the file c:\\csharp.net-informations.xls");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
