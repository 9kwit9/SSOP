using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Klik.Windows.Forms.v1.EntryLib;

namespace AHCSSOP
{
    public partial class frmdescription : Form
    {
        SConnectMSSL mssql;
        SConnectMySQL mysql;
        Ctool_Control Ctrl;
        ELDataGridView glMain;

        public frmdescription(ELDataGridView _glMain)
        {
            InitializeComponent();

            glMain = _glMain;

            mssql = new SConnectMSSL();
            mysql = new SConnectMySQL();
            Ctrl = new Ctool_Control();

            this.Text = "HN : " + User_login.GetInstances().hn +
                ",   Visit Number : " + User_login.GetInstances().VisitID +
                ",   Name : " + User_login.GetInstances().fullname;

            Combtflag.SelectedIndex = 0;
            Combhos.SelectedIndex = 0;
            Combplan.SelectedIndex = 5;
            otherplan.SelectedIndex = -1;
            txtid.Text = User_login.GetInstances().NationalID;
        }

        private void frmdescription_Load(object sender, EventArgs e)
        {
            lblname.Text = User_login.GetInstances().fullname;
            lblhn.Text = User_login.GetInstances().hn;
            lblvn.Text = User_login.GetInstances().VisitID;

            LoadData_Billtran_Patient();
            LoadData_BilltranDetail_Patient();

            LoadData_BillDisp_Patient();
            LoadData_BillDispDetail_Patient();

            LoadData_OPServices_Patient();

            User_login.GetInstances().Tflag = Combtflag.SelectedItem.DataValue.ToString();
        }
        
        private void frmdescription_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        void LoadData_Billtran_Patient()
        {
            DataSet ds = null;
            try
            {
                for (int ii = 0; ii <= glMain.Rows.Count - 1; ii++)
                {
                    if (glMain.Rows[ii].Cells[3].Tag.ToString() != "D")
                    {
                        ds = mssql.GetPatientBill(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(), glMain.Rows[ii].Cells[2].Tag.ToString());
                        if (ds.Tables["0"].Rows.Count != 0)
                        {
                            //Glbill.Rows.Clear();

                            for (int i = 0; i <= ds.Tables["0"].Rows.Count - 1; i++)
                            {
                                DataSet ds1 = null;
                                try
                                {
                                    DataGridViewRow row = new DataGridViewRow();
                                    DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                                    DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();

                                    ds1 = mssql.GetPatientBill_DisCount(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(), glMain.Rows[ii].Cells[2].Tag.ToString());

                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["BillGeneratedDttm"].ToString(), "", false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["BillNumber"].ToString(), ds.Tables["0"].Rows[i]["PatientID"].ToString(), false);

                                    if (ds1.Tables["0"].Rows.Count != 0)
                                    {
                                        Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(ds.Tables["0"].Rows[i]["TotalAmount"]) + Convert.ToDouble(ds1.Tables["0"].Rows[i]["Discount"])).ToString("00000#.#0"), ds.Tables["0"].Rows[i]["PatientName"].ToString(), false);
                                    }
                                    else
                                    {
                                        Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(ds.Tables["0"].Rows[i]["TotalAmount"])).ToString("00000#.#0"), ds.Tables["0"].Rows[i]["PatientName"].ToString(), false);
                                    }
                                    
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitID"].ToString(), ds.Tables["0"].Rows[i]["NationalID"].ToString(), false);



                                    Glbill.Rows.Add(row);
                                }
                                catch { }
                            }

                            Glbill.Refresh();

                            User_login.GetInstances().BillDate = Glbill.Rows[0].Cells[0].Value.ToString();
                            User_login.GetInstances().BillNumber = Glbill.Rows[0].Cells[1].Value.ToString();
                            User_login.GetInstances().BillTotal = Glbill.Rows[0].Cells[2].Value.ToString();
                        }
                    }
                }
            }
            catch { }
        }

        void LoadData_BillDisp_Patient()
        {
            DataSet ds = null;
            try
            {
                for (int ii = 0; ii <= glMain.Rows.Count - 1; ii++)
                {
                    if (glMain.Rows[ii].Cells[3].Tag.ToString() != "D")
                    {
                        ds = mssql.GetPatientBillDisp(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(), glMain.Rows[ii].Cells[2].Tag.ToString());
                        //User_login.GetInstances().Dispid = ds.Tables["0"].Rows[0]["PrescriptionNumber"].ToString();
                        //User_login.GetInstances().DispDate = ds.Tables["0"].Rows[0]["PrescribedDttm"].ToString();
                        //User_login.GetInstances().DispenDate = ds.Tables["0"].Rows[0]["DispensedDttm"].ToString();

                        if (ds.Tables["0"].Rows.Count != 0)
                        {
                            //Gldisp.Rows.Clear();
                            string FDatetime = "";
                            string EDatetime = "";
                            for (int i = 0; i <= ds.Tables["0"].Rows.Count - 1; i++)
                            {
                                try
                                {
                                    DataGridViewRow row = new DataGridViewRow();
                                    DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                                    DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();

                                    //NetAmount

                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitID"].ToString(), ds.Tables["0"].Rows[i]["BillNumber"].ToString(), false);
                                    //Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["BillNumber"].ToString(), ds.Tables["0"].Rows[i]["BillNumber"].ToString(), false);

                                    FDatetime = ds.Tables["0"].Rows[i]["CWhen"].ToString();
                                    EDatetime = ds.Tables["0"].Rows[i]["CWhen"].ToString();

                                    if (ds.Tables["0"].Rows[i]["PrescribedDttm"].ToString().Trim() != "")
                                    {
                                        FDatetime = ds.Tables["0"].Rows[i]["PrescribedDttm"].ToString();
                                    }
                                    if (ds.Tables["0"].Rows[i]["DispensedDttm"].ToString().Trim() != "")
                                    {
                                        EDatetime = ds.Tables["0"].Rows[i]["DispensedDttm"].ToString();
                                    }

                                    try
                                    {
                                        if ((ds.Tables["0"].Rows[i]["PrescribedDttm"].ToString().Trim() == "") || (ds.Tables["0"].Rows[i]["PrescribedDttm"].ToString().Trim() == "NULL"))
                                        {
                                            Ctrl.AddControl(row, txt, FDatetime, ds.Tables["0"].Rows[i]["PatientID"].ToString(), false);
                                        }
                                        else
                                        {
                                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["PrescribedDttm"].ToString(), ds.Tables["0"].Rows[i]["PatientID"].ToString(), false);
                                        }
                                    }
                                    catch
                                    {
                                        Ctrl.AddControl(row, txt, FDatetime, ds.Tables["0"].Rows[i]["PatientID"].ToString(), false);
                                    }

                                    try
                                    {
                                        if ((ds.Tables["0"].Rows[i]["DispensedDttm"].ToString().Trim() == "") || (ds.Tables["0"].Rows[i]["DispensedDttm"].ToString().Trim() == "NULL"))
                                        {
                                            Ctrl.AddControl(row, txt, EDatetime, Convert.ToInt32(ds.Tables["0"].Rows[i]["Cnt"]).ToString(), false);
                                        }
                                        else
                                        {
                                            Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["DispensedDttm"].ToString(), Convert.ToInt32(ds.Tables["0"].Rows[i]["Cnt"]).ToString(), false);
                                        }
                                    }
                                    catch
                                    {
                                        Ctrl.AddControl(row, txt, EDatetime, Convert.ToInt32(ds.Tables["0"].Rows[i]["Cnt"]).ToString(), false);
                                    }

                                    //Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["DispensedDttm"].ToString(), Convert.ToInt32(ds.Tables["0"].Rows[i]["Cnt"]).ToString(), false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["NetAmount"].ToString(), ds.Tables["0"].Rows[i]["NationalID"].ToString(), false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["LicenseNo"].ToString(), "", false);

                                    Gldisp.Rows.Add(row);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        void LoadData_BilltranDetail_Patient()
        {
            DataSet ds = null;
            try
            {
                for (int ii = 0; ii <= glMain.Rows.Count - 1; ii++)
                {
                    if (glMain.Rows[ii].Cells[3].Tag.ToString() != "D")
                    {
                        DataSet ds1 = null;
                        ds1 = mssql.GetPatientBill_Package(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(), glMain.Rows[ii].Cells[2].Tag.ToString());

                        if (ds1.Tables["0"].Rows.Count != 0)
                        {
                            for (int i = 0; i <= ds1.Tables["0"].Rows.Count - 1; i++)
                            {
                                try
                                {
                                    DataSet ds2 = null;
                                    ds2 = mssql.GetPatientBill_Package_Price(ds1.Tables["0"].Rows[i]["BillPackageUID"].ToString());

                                    DataGridViewRow row = new DataGridViewRow();
                                    DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                                    DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();

                                    //string tmp = mssql.GetBill(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString());

                                    double bl = 0;

                                    Ctrl.AddControl(row, txt, Convert.ToInt32(i + 1).ToString(), glMain.Rows[ii].Cells[2].Tag.ToString(), false);
                                    Ctrl.AddControl(row, txt, ds1.Tables["0"].Rows[i]["CWhen"].ToString(), "", false);

                                    if (ds2.Tables["0"].Rows[0]["BillGroup"].ToString().Trim() == "")
                                    {
                                        Ctrl.AddControl(row, txt, "ค่ายา และสารอาหาร", "3", false);
                                    }
                                    else
                                    {
                                        Ctrl.AddControl(row, txt, CheckBillGroup(ds2.Tables["0"].Rows[0]["BillGroup"].ToString()),
                                        CheckBillGroupTag(ds2.Tables["0"].Rows[0]["BillGroup"].ToString()), false);
                                    }
                                    Ctrl.AddControl(row, txt, ds2.Tables["0"].Rows[0]["Code"].ToString(), "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, ds2.Tables["0"].Rows[0]["PackageName"].ToString(), "", false);

                                    int Qty = 0;
                                    double Amount = 0;
                                    double NetAmount = Convert.ToDouble(ds2.Tables["0"].Rows[0]["TotalAmount"]);
                                    if (Convert.ToInt32(ds2.Tables["0"].Rows[0]["Qty"]) == 0)
                                    {
                                        Qty = 1;
                                    }
                                    else
                                    {
                                        Qty = Convert.ToInt32(ds2.Tables["0"].Rows[0]["Qty"]);
                                    }

                                    try
                                    {
                                        bl = 0;
                                    }
                                    catch { }

                                    if (Convert.ToDouble(ds2.Tables["0"].Rows[0]["Qty"]) < 1)
                                    {
                                        Qty = 1;
                                        Amount = Convert.ToDouble(ds2.Tables["0"].Rows[0]["TotalAmount"]);
                                    }
                                    else
                                    {
                                        Amount = Convert.ToDouble(ds2.Tables["0"].Rows[0]["TotalAmount"]);
                                    }

                                    //if ((Convert.ToDouble(Qty * Amount) != NetAmount))
                                    //{
                                    //    Amount = NetAmount;
                                    //}

                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Qty).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Amount) - bl).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Qty *
                                        Convert.ToDouble(Convert.ToDouble(Amount))) - bl).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Amount) - bl).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Qty *
                                        Convert.ToDouble(Convert.ToDouble(Amount))) - bl).ToString("00000#.#0"), Convert.ToDouble(Convert.ToDouble(NetAmount)).ToString("00000#.#0"), false);
                                    Ctrl.AddControl(row, txt, ds1.Tables["0"].Rows[i]["VisitID"].ToString(), ds1.Tables["0"].Rows[i]["VisitID"].ToString(), false);
                                    Ctrl.AddControl(row, txt, "OP1", "OP1", false);
                                    Ctrl.AddControl(row, txt, glMain.Rows[ii].Cells[2].Tag.ToString(), "", false);

                                    GlbillItems.Rows.Add(row);
                                }
                                catch { }
                            }
                        }

                        ds = mssql.GetPatientDetail_Bill(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(), glMain.Rows[ii].Cells[2].Tag.ToString());

                        if (ds.Tables["0"].Rows.Count != 0)
                        {
                            //GlbillItems.Rows.Clear();

                            for (int i = 0; i <= ds.Tables["0"].Rows.Count - 1; i++)
                            {
                                try
                                {
                                    DataGridViewRow row = new DataGridViewRow();
                                    DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                                    DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();

                                    //string tmp = mssql.GetBill(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString());

                                    double bl = 0;

                                    Ctrl.AddControl(row, txt, Convert.ToInt32(i + 1).ToString(), glMain.Rows[ii].Cells[2].Tag.ToString(), false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["CWhen"].ToString(), "", false);

                                    if (ds.Tables["0"].Rows[i]["BillGroup"].ToString().Trim() == "")
                                    {
                                        Ctrl.AddControl(row, txt, "ค่ายา และสารอาหาร", "3", false);
                                    }
                                    else
                                    {
                                        Ctrl.AddControl(row, txt, CheckBillGroup(ds.Tables["0"].Rows[i]["BillGroup"].ToString()),
                                        CheckBillGroupTag(ds.Tables["0"].Rows[i]["BillGroup"].ToString()), false);
                                    }
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Code"].ToString(), mysql.Select_Value_DrugCode(ds.Tables["0"].Rows[i]["Code"].ToString()), false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["ItemName"].ToString(), "", false);

                                    int Qty = 0;
                                    double Amount = 0;
                                    double NetAmount = Convert.ToDouble(ds.Tables["0"].Rows[i]["NetAmount"]);
                                    if (Convert.ToInt32(ds.Tables["0"].Rows[i]["Qty"]) == 0)
                                    {
                                        Qty = 1;
                                    }
                                    else
                                    {
                                        Qty = Convert.ToInt32(ds.Tables["0"].Rows[i]["Qty"]);
                                    }

                                    try
                                    {
                                        bl = Convert.ToDouble(ds.Tables["0"].Rows[i]["Discount"]);
                                    }
                                    catch { }

                                    if (Convert.ToDouble(ds.Tables["0"].Rows[i]["Qty"]) < 1)
                                    {
                                        Qty = 1;
                                        Amount = Convert.ToDouble(ds.Tables["0"].Rows[i]["NetAmount"]);
                                    }
                                    else
                                    {
                                        Amount = Convert.ToDouble(ds.Tables["0"].Rows[i]["Amount"]);
                                    }

                                    //if ((Convert.ToDouble(Qty * Amount) != NetAmount) && (Convert.ToDouble(ds.Tables["0"].Rows[i]["Discount"]) == 0))
                                    //{
                                    //    Amount = NetAmount;
                                    //}

                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Qty).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Amount)).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Qty *
                                        Convert.ToDouble(Convert.ToDouble(Amount)))).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Amount)).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Convert.ToDouble(Qty *
                                        Convert.ToDouble(Convert.ToDouble(Amount)))).ToString("00000#.#0"), Convert.ToDouble(Convert.ToDouble(NetAmount)).ToString("00000#.#0"), false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitID"].ToString(), ds.Tables["0"].Rows[i]["VisitID"].ToString(), false);
                                    Ctrl.AddControl(row, txt, "OP1", "OP1", false);
                                    Ctrl.AddControl(row, txt, glMain.Rows[ii].Cells[2].Tag.ToString(), "", false);

                                    GlbillItems.Rows.Add(row);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        void LoadData_BillDispDetail_Patient()
        {
            DataSet ds = null;
            try
            {
                for (int ii = 0; ii <= glMain.Rows.Count - 1; ii++)
                {
                    if (glMain.Rows[ii].Cells[3].Tag.ToString() != "D")
                    {
                        int total = 0;
                        double sum = 0;
                        ds = mssql.GetPatientDetail_Disp(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(), glMain.Rows[ii].Cells[2].Tag.ToString());
                        if (ds.Tables["0"].Rows.Count != 0)
                        {
                            //GlbillDisp.Rows.Clear();
                            total = 0;
                            sum = 0;
                            for (int i = 0; i <= ds.Tables["0"].Rows.Count - 1; i++)
                            {
                                try
                                {
                                    DataGridViewRow row = new DataGridViewRow();
                                    DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                                    DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();

                                    Ctrl.AddControl(row, txt, Convert.ToInt32(i + 1).ToString(), "", false);
                                    //Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["PrescriptionNumber"].ToString(), "", false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["VisitID"].ToString(), "", false);

                                    if (ds.Tables["0"].Rows[i]["GroupUID"].ToString() == "25")
                                    {
                                        Ctrl.AddControl(row, txt, "1", "1", false);
                                    }
                                    else { Ctrl.AddControl(row, txt, "6", "6", false); }

                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Code"].ToString(), "", false);
                                    Ctrl.AddControl(row, txt, mysql.Select_Value_DrugCode(ds.Tables["0"].Rows[i]["Code"].ToString()),
                                        mysql.Select_Value_DrugCode(ds.Tables["0"].Rows[i]["Code"].ToString()), false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["ItemName"].ToString(), "", false);

                                    if (ds.Tables["0"].Rows[i]["Unit"].ToString().Trim() == "")
                                    {
                                        string[] tmp = ds.Tables["0"].Rows[i]["ItemName"].ToString().Split(' ');
                                        if (tmp[tmp.Length - 1].Trim() == "")
                                        {
                                            Ctrl.AddControl(row, txt, tmp[tmp.Length - 2].Replace(".", "").ToString(), "", false);
                                        }
                                        else
                                        {
                                            Ctrl.AddControl(row, txt, tmp[tmp.Length - 1].Replace(".", "").ToString(), "", false);
                                        }
                                    }
                                    else
                                    {
                                        Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Unit"].ToString(), "", false);
                                    }

                                    if (ds.Tables["0"].Rows[i]["GroupUID"].ToString() == "25")
                                    {
                                        if (ds.Tables["0"].Rows[i]["Frequency"].ToString().Trim() == "")
                                        {
                                            Ctrl.AddControl(row, txt, "USE|ใช้ตามแพทย์สั่ง", "", false);
                                        }
                                        else { Ctrl.AddControl(row, txt, ds.Tables["0"].Rows[i]["Frequency"].ToString(), "", false); }
                                    }
                                    else
                                    {
                                        Ctrl.AddControl(row, txt, "USE|USE", "", false);
                                    }

                                    Ctrl.AddControl(row, txt, "", "", false);

                                    int Qty = 0;
                                    double Amount = 0;
                                    double NetAmount = Convert.ToDouble(ds.Tables["0"].Rows[i]["NetAmount"]);
                                    double Discount = Convert.ToDouble(ds.Tables["0"].Rows[i]["Discount"]);

                                    if (Convert.ToInt32(ds.Tables["0"].Rows[i]["Qty"]) == 0)
                                    {
                                        Qty = 1;
                                    }
                                    else
                                    {
                                        Qty = Convert.ToInt32(ds.Tables["0"].Rows[i]["Qty"]);
                                    }

                                    if (Convert.ToDouble(ds.Tables["0"].Rows[i]["Qty"]) < 1)
                                    {
                                        Qty = 1;
                                        Amount = Convert.ToDouble(ds.Tables["0"].Rows[i]["NetAmount"]);
                                    }
                                    else
                                    {
                                        Amount = Convert.ToDouble(ds.Tables["0"].Rows[i]["Amount"]);
                                    }

                                    if ((Convert.ToDouble(Qty * Amount) != NetAmount) && (Convert.ToDouble(ds.Tables["0"].Rows[i]["Discount"]) == 0))
                                    {
                                        Amount = NetAmount;
                                    }

                                    Ctrl.AddControl(row, txt, Qty.ToString(), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Amount).ToString("00000#.#0"), "", false);
                                    Ctrl.AddControl(row, txt, Convert.ToDouble(Qty * Amount).ToString("00000#.#0"), Convert.ToDouble(Convert.ToDouble(NetAmount)).ToString("00000#.#0"), false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);
                                    Ctrl.AddControl(row, txt, "", "", false);

                                    //total += Convert.ToInt32(ds.Tables["0"].Rows[i]["Qty"].ToString());
                                    //sum += Convert.ToDouble(ds.Tables["0"].Rows[i]["Qty"]) *
                                    //    Convert.ToDouble(ds.Tables["0"].Rows[i]["Amount"]);

                                    GlbillDisp.Rows.Add(row);
                                }
                                catch { }
                            }
                            User_login.GetInstances().DispCount = total.ToString();
                            User_login.GetInstances().DispSum = sum.ToString();
                        }
                    }
                }
            }
            catch { }
        }

        void LoadData_OPServices_Patient()
        {
            try
            {
                GLOPServices.Rows.Clear();
                for (int ii = 0; ii <= glMain.Rows.Count - 1; ii++)
                {
                    if (glMain.Rows[ii].Cells[3].Tag.ToString() != "D")
                    {
                        try
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            DataGridViewComboBoxCell comb = new DataGridViewComboBoxCell();
                            DataGridViewTextBoxCell txt = new DataGridViewTextBoxCell();

                            Ctrl.AddControl(row, txt, Convert.ToInt32(ii + 1).ToString(), glMain.Rows[ii].Cells[3].Value.ToString(), false);
                            Ctrl.AddControl(row, txt, glMain.Rows[ii].Cells[2].Value.ToString(), Glbill.Rows[ii].Cells[1].Value.ToString(), false);
                            Ctrl.AddControl(row, txt, "การตรวจรักษา", "EC", false);
                            Ctrl.AddControl(row, txt, "สถานพยาบาล Supra", "2", false);
                            Ctrl.AddControl(row, txt, "วินิจฉัยโรค", "", false);
                            Ctrl.AddControl(row, txt, "รับส่งต่อมาจากสถาบันอื่น", "3", false);
                            Ctrl.AddControl(row, txt, "จำหน่ายกลับบ้าน", "1", false);
                            Ctrl.AddControl(row, txt, "", glMain.Rows[ii].Cells[4].Value.ToString(), false);
                            Ctrl.AddControl(row, txt, "", glMain.Rows[ii].Cells[13].Tag.ToString() + "|" + glMain.Rows[ii].Cells[13].Value.ToString(), false);

                            string loc_name = "";
                            string loc_code = "";

                            switch (glMain.Rows[ii].Cells[7].Value.ToString())
                            {
                                case "อายุรกรรม":
                                    loc_name = "อายุรกรรม";
                                    loc_code = "01";
                                    break;

                                case "ศัลยกรรม":
                                    loc_code = "02";
                                    loc_name = "ศัลยกรรม";
                                    break;

                                case "สูติกรรมแรกเกิด":
                                    loc_code = "03";
                                    loc_name = "สูติกรรม";
                                    break;

                                case "สูติ-นรีเวชกรรม":
                                    loc_code = "04";
                                    loc_name = "นรีเวชกรรม";
                                    break;

                                case "กุมารเวชกรรม":
                                    loc_code = "05";
                                    loc_name = "กุมารเวช";
                                    break;

                                case "หู คอ จมูก":
                                    loc_code = "06";
                                    loc_name = "โสด ศอ นาสิก";
                                    break;

                                case "ตา":
                                    loc_code = "07";
                                    loc_name = "จักษุ";
                                    break;

                                case "ศัลยกรรมกระดูกและข้อ":
                                    loc_code = "08";
                                    loc_name = "ศัลยกรรมกระดูก";
                                    break;

                                case "จิตเวช":
                                    loc_code = "09";
                                    loc_name = "จิตเวช";
                                    break;

                                case "รังสีเทคนิค":
                                    loc_code = "10";
                                    loc_name = "รังสีวิทยา";
                                    break;

                                case "สาขาทันตกรรม":
                                    loc_code = "11";
                                    loc_name = "ทันตกรรม";
                                    break;

                                case "ฉุกเฉิน":
                                    loc_code = "12";
                                    loc_name = "ฉุกเฉิน";
                                    break;

                                default:
                                    loc_name = "อื่นๆ";
                                    loc_code = "99";
                                    break;

                            }

                            Ctrl.AddControl(row, txt, loc_name, loc_code, false);
                            Ctrl.AddControl(row, txt, glMain.Rows[ii].Cells[8].Value.ToString(), glMain.Rows[ii].Cells[8].Value.ToString(), false);
                            Ctrl.AddControl(row, txt, glMain.Rows[ii].Cells[9].Value.ToString(), glMain.Rows[ii].Cells[9].Value.ToString(), false);
                            Ctrl.AddControl(row, txt, "", glMain.Rows[ii].Cells[12].Value.ToString(), false);
                            Ctrl.AddControl(row, txt, "ICD-9-CM", "IN", false);
                            Ctrl.AddControl(row, txt, "", "", false);
                            Ctrl.AddControl(row, txt, mssql.GetDF_Price(glMain.Rows[ii].Cells[0].Tag.ToString(), glMain.Rows[ii].Cells[1].Tag.ToString(),
                                glMain.Rows[ii].Cells[2].Tag.ToString()), User_login.GetInstances().Doctor_charge, false);
                            Ctrl.AddControl(row, txt, "ผู้ป่วยได้รับบริการครบแล้ว", "Y", false);
                            Ctrl.AddControl(row, txt, "", glMain.Rows[ii].Cells[14].Tag.ToString(), false);
                            Ctrl.AddControl(row, txt, "OPD ปกติ", "OP1", false);
                            Ctrl.AddControl(row, txt, glMain.Rows[ii].Cells[2].Tag.ToString(), "", false);

                            GLOPServices.Rows.Add(row);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        private void otherplan_SelectionChanged(object sender, EventArgs e)
        {
            if (otherplan.Text.Trim() != "")
            {
                otherpay.ReadOnly = false;
                otherpay.Text = "0";
            }
            else
            {
                otherpay.ReadOnly = true;
                otherpay.Text = "0";
            }
        }

        private void otherplan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                otherplan.SelectedIndex = -1;
            }
        }

        string CheckBillGroup(string tmp)
        {
            switch (tmp)
            {
                case "1.1.12 ค่าพยาบาลของผู้ประกอบวิชาชีพพยาบาลหรือผดุงค" :
                    tmp = "ค่าบริการทางการพยาบาล";
                    break;

                case "1.1.5 ค่าตรวจวินิจฉัยและการรักษาทางรังสีวิทยา":
                    tmp = "ค่าตรวจวินิจฉัย และรักษาทางรังสีวิทยา";
                    break;

                case "1.1.1 ค่ายาและสารอาหารทางเส้นเลือด":
                    tmp = "ค่ายา และสารอาหาร";
                    break;

                case "1.1.2 ค่าเวชภัณฑ์" :
                    tmp = "ค่าเวชภัณฑ์ที่มิใช่ยา";
                    break;

                case "1.1.14 ค่าบริการเหมาจ่ายค่ารักษาพยาบาล" :
                    tmp = "ค่าบริการอื่นๆ";
                    break;

                case "1.2.1 ค่าตรวจรักษาทั่วไปของผู้ประกอบวิชาชีพ":
                    tmp = "ค่าธรรมเนียมบุคลากรทางการแพทย์";
                    break;

                case "1.2.3 ค่าปฏิบัติการอื่นๆของผู้ประกอบวิชาชีพ":
                    tmp = "ค่าธรรมเนียมบุคลากรทางการแพทย์";
                    break;

                case "1.2.2 ค่าทำศัลยกรรมและหัตถการต่าง ๆ ของผู้ประกอบวิ":
                    tmp = "ค่าธรรมเนียมบุคลากรทางการแพทย์";
                    break;

                case "2.5 ค่าบริการรถพยาบาล":
                    tmp = "ค่าบริการอื่นๆ";
                    break;

                case "1.1.8 ค่าห้องผ่าตัดหรือห้องคลอด":
                    tmp = "ค่าห้องผ่าตัด/ห้องคลอด";
                    break;

                case "1.1.7 ค่าอุปกรณ์ของใช้และเครื่องมือทางการแพทย์":
                    tmp = "ค่าอุปกรณ์ของใช้ และเครื่องมือฯ";
                    break;

                case "1.1.4 ค่าตรวจวินิจฉัยทางเทคนิคการแพทย์และพยาธิวิทย":
                    tmp = "ค่าตรวจวินิจฉัยทางเทคนิคการแพทย์";
                    break;

                case "1.1.3 ค่าบริการโลหิตและส่วนประกอบของโลหิต":
                    tmp = "ค่าบริการโลหิต และส่วนประกอบของโลหิต";
                    break;

                case "1.1.10 ค่าบริการทางกายภาพบำบัด":
                    tmp = "ค่าบริการทางกายภาพบำบัด";
                    break;

                case "1.1.9 ค่าบริการทางทันตกรรม":
                    tmp = "ค่าบริการทางทันตกรรม";
                    break;

                case "1.1.11 ค่าบริการฝังเข็ม":
                    tmp = "ค่าบริการฝังเข็ม";
                    break;

                case "1.1.6 ค่าตรวจวินิจฉัยโดยวิธีพิเศษอื่นๆ":
                    tmp = "ค่าตรวจวินิจฉัยโดยวิธีพิเศษอื่นๆ";
                    break;

                 default:
                    break;
            }
            
            return tmp;
        }

        string CheckBillGroupTag(string tmp)
        {
            switch (tmp)
            {
                case "1.1.12 ค่าพยาบาลของผู้ประกอบวิชาชีพพยาบาลหรือผดุงค":
                    tmp = "C";
                    break;

                case "1.1.5 ค่าตรวจวินิจฉัยและการรักษาทางรังสีวิทยา":
                    tmp = "8";
                    break;

                case "1.1.1 ค่ายาและสารอาหารทางเส้นเลือด":
                    tmp = "3";
                    break;

                case "1.1.2 ค่าเวชภัณฑ์":
                    tmp = "5";
                    break;

                case "1.1.14 ค่าบริการเหมาจ่ายค่ารักษาพยาบาล":
                    tmp = "G";
                    break;

                case "1.2.1 ค่าตรวจรักษาทั่วไปของผู้ประกอบวิชาชีพ":
                    tmp = "I";
                    break;

                case "1.2.3 ค่าปฏิบัติการอื่นๆของผู้ประกอบวิชาชีพ":
                    tmp = "I";
                    break;

                case "1.2.2 ค่าทำศัลยกรรมและหัตถการต่าง ๆ ของผู้ประกอบวิ":
                    tmp = "I";
                    break;

                case "2.5 ค่าบริการรถพยาบาล":
                    tmp = "G";
                    break;

                case "1.1.8 ค่าห้องผ่าตัดหรือห้องคลอด":
                    tmp = "H";
                    break;

                case "1.1.7 ค่าอุปกรณ์ของใช้และเครื่องมือทางการแพทย์":
                    tmp = "A";
                    break;

                case "1.1.4 ค่าตรวจวินิจฉัยทางเทคนิคการแพทย์และพยาธิวิทย":
                    tmp = "7";
                    break;

                case "1.1.3 ค่าบริการโลหิตและส่วนประกอบของโลหิต":
                    tmp = "6";
                    break;

                case "1.1.10 ค่าบริการทางกายภาพบำบัด":
                    tmp = "E";
                    break;
                case "1.1.9 ค่าบริการทางทันตกรรม":
                    tmp = "D";
                    break;

                case "1.1.11 ค่าบริการฝังเข็ม":
                    tmp = "F";
                    break;

                case "1.1.6 ค่าตรวจวินิจฉัยโดยวิธีพิเศษอื่นๆ":
                    tmp = "9";
                    break;

                default:
                    break;
            }

            return tmp;
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            switch (e.Tool.Key)
            {
                case "Export":
                    export();
                    break;

                default :
                    break;
            }
            this.Cursor = Cursors.Default;
        }

        void export()
        {
            try
            {
                User_login.GetInstances().Hcode = txthos.Text;
                User_login.GetInstances().Hname = Combhos.Text;
                User_login.GetInstances().BillNumber = Glbill.Rows[0].Cells[1].Value.ToString();
                User_login.GetInstances().NationalID = txtid.Text;

                ExportToXml();
                Check_Session();

                MessageBox.Show("Export Data Complete", "...",MessageBoxButtons.OK);
            }
            catch { }
        }

        private void Check_Session()
        {
            if (mysql.Select_SessionID(AhcSession.GetInstances().HospitalCode, AhcSession.GetInstances().SessionYear,
                AhcSession.GetInstances().SessionMonth) == 0)
            {
                Insert_Session();
            }
            else
            {
                Update_Session();
            }
            Update_Ssopbill();
        }

        private void Update_Session()
        {
            string sql = "";
            sql = "UPDATE tbl_session_id SET cnt='" + AhcSession.GetInstances().SessionID + "'";
            sql += " WHERE hos='" + AhcSession.GetInstances().HospitalCode + "'";
            sql += " AND year='" + AhcSession.GetInstances().SessionYear + "'";
            sql += " AND month='" + AhcSession.GetInstances().SessionMonth + "'";

            mysql.Exeute(sql);
        }

        private void Insert_Session()
        {
            string sql = "";
            sql = "INSERT INTO tbl_session_id(hos, year, month, cnt) VALUES(";
            sql += "'" + AhcSession.GetInstances().HospitalCode + "',";
            sql += "'" + AhcSession.GetInstances().SessionYear + "',";
            sql += "'" + AhcSession.GetInstances().SessionMonth + "',";
            sql += "'" + AhcSession.GetInstances().SessionID + "')";

            mysql.Exeute(sql);
        }

        void Update_Ssopbill()
        {
            string sql = "";
            sql = "UPDATE tbl_ssopbill SET session='" + AhcSession.GetInstances().SessionSsopID + "'";
            mysql.Exeute(sql);
        }

        void ExportToXml()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (lblhn.Text.Trim() != "")
                {
                    User_login.GetInstances().Plan = Combplan.SelectedItem.DataValue.ToString();

                    Cexport_Xml_OP xml = new Cexport_Xml_OP();
                    xml.ToXml(GLOPServices, Glbill, GlbillItems, GlbillDisp, Gldisp);
                }
                else
                {
                    MessageBox.Show("HN is Empty ... ", "Warning", MessageBoxButtons.OK);
                }

            }
            catch { }
            this.Cursor = Cursors.Default;
        }
    }
}