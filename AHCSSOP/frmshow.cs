using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHCSSOP
{
    public partial class frmshow : Form
    {
        SConnectMSSL mssql;
        SConnectMySQL mysql;
        Ctool_Control Ctrl;

        public frmshow()
        {
            InitializeComponent();
            mssql = new SConnectMSSL();
            mysql = new SConnectMySQL();
            Ctrl = new Ctool_Control();

            AhcSession.GetInstances().HospitalCode = "11847";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            this.Cursor = Cursors.WaitCursor;

            txtvn.Text = "";
            lblCount.Text = "0";
            Glitem.Rows.Clear();
            Glitem.Refresh();

            Dfrom.Value = DateTime.Now.ToShortDateString();
            Dto.Value = DateTime.Now.ToShortDateString();

            txtvn.Focus();
            this.Cursor = Cursors.Default;
        }

        private void frmshow_Load(object sender, EventArgs e)
        {
            txtvn.Focus();
        }

        void LoadData_Data_SSOP()
        {
            this.Cursor = Cursors.WaitCursor;
            DataSet ds = null;

            if (txtvn.Text.Trim() == "")
            {
                ds = mssql.GetPatient((DateTime)Dfrom.Value, (DateTime)Dto.Value);
            }
            else
            {
                ds = mssql.GetPatient(txtvn.Text.Trim());
            }

            if (ds.Tables["0"].Rows.Count != 0)
            {
                int j = 0;
                Glitem.Rows.Clear();

                for (int i = 0; i <= ds.Tables["0"].Rows.Count - 1; i++)
                {
                    try
                    {
                        if ((mssql.GetDF(ds.Tables["0"].Rows[i]["UID"].ToString(), ds.Tables["0"].Rows[i]["VisitUID"].ToString()) == "DOCTOR FEES")
                            && (mysql.Select_Value_IDCard(ds.Tables["0"].Rows[i]["NationalID"].ToString(), ds.Tables["0"].Rows[i]["BillNumber"].ToString()) == ""))
                        {
                            lblCount.Text = Convert.ToInt32(j + 1).ToString();

                            DataGridViewRow row = new DataGridViewRow();
                            DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                            DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();
                            DataGridViewImageCell img = new DataGridViewImageCell();

                            //if (mysql.GetExits_SSOP(ds.Tables["0"].Rows[i]["VisitNumber"].ToString()) != "")
                            //{
                            //    Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["UID"].ToString(), Img2.Images[7], true);
                            //}
                            //else
                            //{
                            //    Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["UID"].ToString(), Img2.Images[9], true);
                            //}
                            Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["UID"].ToString(), Img2.Images[7], true);

                            Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["VisitUID"].ToString(), Img2.Images[24], true);

                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitNumber"].ToString(), ds.Tables["0"].Rows[i]["BillNumber"].ToString(), false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["PatientID"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["PatientName"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["AgeString"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Sex"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Location"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["AdmissionDttm"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["DischargeDate"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Encounter"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitType"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["NationalID"].ToString(), "", false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["AdmissionDiagnosis"].ToString(),
                                ds.Tables["0"].Rows[i]["DiagnosisCode"].ToString(), false);
                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["CareproviderName"].ToString(),
                                ds.Tables["0"].Rows[i]["LicenseNo"].ToString(), false);

                            Glitem.Rows.Add(row);
                            j += 1;
                        }
                    }
                    catch { }
                }

                lblCount.Text = ds.Tables["0"].Rows.Count.ToString();
            }

            this.Cursor = Cursors.Default;
            Glitem.Refresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dForm = (DateTime)Dfrom.Value;
            DateTime dTo = (DateTime)Dto.Value;

            if (dForm <= dTo)
            {
                LoadData_Data_SSOP();
            }
            else
            {
                MessageBox.Show("   Date is not valid.   ", "Warning", MessageBoxButtons.OK);
            }
        }

        private void txtvn_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtvn.Text.Trim() != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    LoadData_Data_SSOP();
                }
            }
        }

        private void Glitem_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Application.DoEvents();
                this.Cursor = Cursors.WaitCursor;
                if (Glitem.SelectedRows[0].Cells[2].Value.ToString() != "")
                {
                    User_login.GetInstances().hn = Glitem.SelectedRows[0].Cells[3].Value.ToString();
                    User_login.GetInstances().VisitID = Glitem.SelectedRows[0].Cells[2].Value.ToString();
                    User_login.GetInstances().fullname = Glitem.SelectedRows[0].Cells[4].Value.ToString();
                    User_login.GetInstances().PatientVisitUID = Glitem.SelectedRows[0].Cells[1].Tag.ToString();
                    User_login.GetInstances().PatientUID = Glitem.SelectedRows[0].Cells[0].Tag.ToString();
                    User_login.GetInstances().Location = Glitem.SelectedRows[0].Cells[7].Value.ToString();
                    User_login.GetInstances().Doctor_licens = Glitem.SelectedRows[0].Cells[14].Tag.ToString();
                    User_login.GetInstances().DiagCode = Glitem.SelectedRows[0].Cells[13].Tag.ToString();
                    User_login.GetInstances().DiagName = Glitem.SelectedRows[0].Cells[13].Value.ToString();
                    User_login.GetInstances().Startdttm = Glitem.SelectedRows[0].Cells[8].Value.ToString();
                    User_login.GetInstances().Enddttm = Glitem.SelectedRows[0].Cells[9].Value.ToString();

                    try
                    {
                        User_login.GetInstances().NationalID = Glitem.SelectedRows[0].Cells[12].Value.ToString().Insert(1, "-").Insert(6, "-").Insert(12, "-").Insert(15, "-");
                    }
                    catch { User_login.GetInstances().NationalID = Glitem.SelectedRows[0].Cells[12].Value.ToString(); }

                    frmdescription fdes = new frmdescription(Glitem);
                    fdes.ShowDialog();
                    fdes.Dispose();
                }
                this.Cursor = Cursors.Default;

                //LoadData_Data_SSOP();
            }
            catch { txtvn.Focus(); }
        }

        private void Glitem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MenudontSend_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (Glitem.SelectedRows.Count != 0)
                {
                    Glitem.SelectedRows[0].Cells[3].Tag = "D";
                    Glitem.Refresh();

                    UpdateDatabase();
                }
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        void UpdateDatabase()
        {
            try
            {
                mysql.Exeute("INSERT INTO tbl_dont_send (VisitID, DSDate, Sessionid, Statusflag) VALUES('" + Glitem.SelectedRows[0].Cells[2].Value.ToString() +
                    "', '" + Glitem.SelectedRows[0].Cells[8].Value.ToString() + 
                    "','0','A')");
            }
            catch
            {
                mysql.Exeute("UPDATE tbl_dont_send SET Statusflag='A' WHERE VisitID='" + Glitem.SelectedRows[0].Cells[2].Value.ToString() + "'");
            }
        }

        private void Dto_ValueChanged(object sender, EventArgs e)
        {
            AhcSession.GetInstances().SessionYear = Convert.ToDateTime(Dto.Value).Year.ToString("000#");
            AhcSession.GetInstances().SessionMonth = Convert.ToDateTime(Dto.Value).Month.ToString("0#");
            AhcSession.GetInstances().SessionID = "01";

            int tmp = mysql.Select_SessionID(AhcSession.GetInstances().HospitalCode, AhcSession.GetInstances().SessionYear,
                AhcSession.GetInstances().SessionMonth);
            if (tmp != 0)
            {
                AhcSession.GetInstances().SessionID = Convert.ToInt32(tmp + 1).ToString("0#");
            }

            int tmp1 = mysql.Select_Ssopbill();
            if (tmp1 != 0)
            {
                AhcSession.GetInstances().SessionSsopID = Convert.ToInt32(tmp1 + 1).ToString("000#");
            }

            lblSession.Text = AhcSession.GetInstances().HospitalCode + "_" + AhcSession.GetInstances().SessionYear + "_" +
               AhcSession.GetInstances().SessionMonth + "_" + AhcSession.GetInstances().SessionID;
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataSet ds1 = mysql.GetExits_SSOP();
            DataSet ds = null;

            if (ds1 != null)
            {
                if (ds1.Tables["0"].Rows.Count != 0)
                {
                    int j = 0;
                    Glitem.Rows.Clear();

                    for (int l = 0; l <= ds1.Tables["0"].Rows.Count - 1; l++)
                    {
                        ds = mssql.GetPatient(ds1.Tables["0"].Rows[l]["visitnumber"].ToString());
                        if (ds.Tables["0"].Rows.Count != 0)
                        {
                            for (int i = 0; i <= ds.Tables["0"].Rows.Count - 1; i++)
                            {
                                try
                                {
                                    if ((mssql.GetDF(ds.Tables["0"].Rows[i]["UID"].ToString(), ds.Tables["0"].Rows[i]["VisitUID"].ToString()) == "DOCTOR FEES")
                                        && (mysql.Select_Value_IDCard(ds.Tables["0"].Rows[i]["NationalID"].ToString(), ds.Tables["0"].Rows[i]["BillNumber"].ToString()) == ""))
                                    {
                                        lblCount.Text = Convert.ToInt32(j + 1).ToString();

                                        DataGridViewRow row = new DataGridViewRow();
                                        DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                                        DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();
                                        DataGridViewImageCell img = new DataGridViewImageCell();

                                        //if (mysql.GetExits_SSOP(ds.Tables["0"].Rows[i]["VisitNumber"].ToString()) != "")
                                        //{
                                        //    Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["UID"].ToString(), Img2.Images[7], true);
                                        //}
                                        //else
                                        //{
                                        //    Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["UID"].ToString(), Img2.Images[9], true);
                                        //}
                                        Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["UID"].ToString(), Img2.Images[7], true);

                                        Ctrl.AddControl(row, img, ds.Tables["0"].Rows[i]["VisitUID"].ToString(), Img2.Images[24], true);

                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitNumber"].ToString(), ds.Tables["0"].Rows[i]["BillNumber"].ToString(), false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["PatientID"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["PatientName"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["AgeString"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Sex"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Location"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["AdmissionDttm"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["DischargeDate"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Encounter"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitType"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["NationalID"].ToString(), "", false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["AdmissionDiagnosis"].ToString(),
                                            ds.Tables["0"].Rows[i]["DiagnosisCode"].ToString(), false);
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["CareproviderName"].ToString(),
                                            ds.Tables["0"].Rows[i]["LicenseNo"].ToString(), false);

                                        Glitem.Rows.Add(row);
                                        Glitem.Refresh();
                                        j += 1;
                                    }
                                }
                                catch { }
                            }

                            lblCount.Text = ds.Tables["0"].Rows.Count.ToString();
                        }
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}