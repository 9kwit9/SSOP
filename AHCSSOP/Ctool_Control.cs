using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klik.Windows.Forms.v1.EntryLib;
using System.Windows.Forms;
using Infragistics.Win.UltraDataGridView;

namespace AHCSSOP
{
    public class Ctool_Control
    {
        public DataGridViewTextBoxCell AddControl(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...") { txt = ""; }

            tbox.Value = txt;
            tbox.Tag = txt;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControl_Emoving(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, string em, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...") { txt = ""; }

            tbox.Value = txt;
            tbox.Tag = em;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControlTime(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...") { txt = ""; }

            try
            {
                tbox.Value = Convert.ToDateTime(txt).ToLongTimeString();
            }
            catch { tbox.Value = ""; }

            tbox.Tag = txt;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControlDateTime(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...") { txt = ""; }

            tbox.Value = txt;
            tbox.Tag = txt;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControlStatusTime(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, bool bl, string timerec, string timecomplete)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...") { txt = ""; }

            try
            {
                tbox.Value = Convert.ToDateTime(timecomplete).Subtract(Convert.ToDateTime(timerec)).ToString(@"hh\:mm\:ss");
            }
            catch
            {
                tbox.Value = "";
            }

            tbox.Tag = txt;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControlStatus(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...") { txt = ""; }

            tbox.Value = "";
            tbox.Tag = txt;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControlstatus(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();

            if (txt.Trim() == "...")
            {
                txt = "";
            }

            tbox.Value = "N";
            tbox.Tag = txt;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewTextBoxCell AddControl(DataGridViewRow row, DataGridViewTextBoxCell tbox, string txt, string txt2, bool bl)
        {
            tbox = new DataGridViewTextBoxCell();
            tbox.Value = txt;
            tbox.Tag = txt2;
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewButtonCell AddControl(DataGridViewRow row, DataGridViewButtonCell tbox, string status, string txt2, bool bl)
        {
            tbox = new DataGridViewButtonCell();
            tbox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            tbox.UseColumnTextForButtonValue = true;

            if (status == "0")
            {
                tbox.Value = "รับงาน";
                tbox.Tag = status + "," + txt2;
            }
                        
            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewButtonCell AddControl2(DataGridViewRow row, DataGridViewButtonCell tbox, string status, string txt2, bool bl)
        {
            tbox = new DataGridViewButtonCell();
            tbox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            tbox.UseColumnTextForButtonValue = true;

            if (status == "1")
            {
                tbox.Value = "เสร็จงาน";
                tbox.Tag = status + "," + txt2;
            }

            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewImageCell AddControl(DataGridViewRow row, DataGridViewImageCell tbox, string txt, bool bl)
        {
            tbox = new DataGridViewImageCell();
            tbox.ToolTipText = "Click";

            //tbox.Value = Img;

            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public DataGridViewCheckBoxCell AddControl(DataGridViewRow row, DataGridViewCheckBoxCell tbox, string status, bool bl)
        {
            tbox = new DataGridViewCheckBoxCell();

            if (status == "1")
            {
                tbox.Value = true;
            }
            else
            {
                tbox.Value = null;
                tbox.ReadOnly = bl;
            }
            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControl(DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt)
        {
            tbox = new DataGridViewComboBoxCell();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;

            if (txt.Trim() != "")
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }

            row.Cells.Add(tbox);

            return tbox;
        }

        /*public DataGridViewComboBoxCell AddControl(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.LoadCoInsurance().Tables["0"].Rows)
            {
                if (r["coinsurance_name"].ToString() == txt) { tmp = true; }
                tbox.Items.Add(r["coinsurance_name"]);
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControlItems(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.LoadDataItems().Tables["0"].Rows)
            {
                if (r["item_items"].ToString() + " | " + r["item_code"].ToString() == txt) { tmp = true; }
                tbox.Items.Add(r["item_items"].ToString() + " | " + r["item_code"].ToString());
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControlCode(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.LoadGroupClass().Tables["0"].Rows)
            {
                if (r["group_name"].ToString() + "-" + r["group_comment"].ToString() == txt) { tmp = true; }
                tbox.Items.Add(r["group_name"].ToString() + "-" + r["group_comment"].ToString());
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControlInvoice(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.Load_product().Tables["0"].Rows)
            {
                if (r["product_definition"].ToString() == txt) { tmp = true; }
                tbox.Items.Add(r["product_definition"].ToString());
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControl_csmbsiref(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.Load_csmbsiref().Tables["0"].Rows)
            {
                if (r["cscode"].ToString() == txt) { tmp = true; }
                //tbox.Items.Add(r["csdesc"].ToString());
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControl_SubCode(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.LoadSubClass("").Tables["0"].Rows)
            {
                if (r["class_id"].ToString() + "-" + r["class_name"].ToString() == txt) { tmp = true; }
                tbox.Items.Add(r["class_id"].ToString() + "-" + r["class_name"].ToString());
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControl_Doctor(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            foreach (DataRow r in db.LoadDoctor().Tables["0"].Rows)
            {
                if (r["doctor_code"].ToString() + ", \t" + r["doctor_name"].ToString() == txt) { tmp = true; }
                tbox.Items.Add(r["doctor_code"].ToString() + ", \t" + r["doctor_name"].ToString());
            }

            if ((txt.Trim() != "") && (tmp == false))
            {
                tbox.Items.Add(txt);
                tbox.Value = txt;
            }
            else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }

        public DataGridViewComboBoxCell AddControlStaff(DBConnect db, DataGridViewRow row, DataGridViewComboBoxCell tbox, string txt, string list)
        {
            //bool tmp = false;
            tbox = new DataGridViewComboBoxCell();
            // tbox.DetachEditingControl();
            tbox.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            tbox.AutoComplete = true;
            tbox.Tag = list;
            tbox.Items.Clear();

            //foreach (DataRow r in db.Loaddatastaff().Tables["0"].Rows)
            //{
            //    if (r["staff_name"].ToString() + "  " + r["staff_surname"].ToString() == txt) { tmp = true; }
            //    tbox.Items.Add(r["staff_name"].ToString() + "  " + r["staff_surname"].ToString());
            //}

            //if ((txt.Trim() != "") && (tmp == false))
            //{
            //    tbox.Items.Add(txt);
            //    tbox.Value = txt;
            //}
            //else { tbox.Value = txt; }

            row.Cells.Add(tbox);

            return tbox;
        }*/

        public DataGridViewImageCell AddControl(DataGridViewRow row, DataGridViewImageCell tbox, string txt, Image Img, bool bl)
        {
            tbox = new DataGridViewImageCell();

            tbox.Value = Img;
            tbox.Tag = txt;

            row.Cells.Add(tbox);
            tbox.ReadOnly = bl;

            return tbox;
        }

        public string Check_typecar(string typecar)
        {
            string tmp = "";

            try
            {
                if (typecar.Trim() != "")
                {
                    string[] str = typecar.Split('|');
                    if (str[0].Trim() == "1")
                    {
                        tmp += "รถนอนปรับนั่ง, ";
                    }
                    if (str[1].Trim() == "1")
                    {
                        tmp += "รถนอน OR, ";
                    }
                    if (str[2].Trim() == "1")
                    {
                        tmp += "รถนั่ง, ";
                    }
                    if (str[3].Trim() == "1")
                    {
                        tmp += "รถนั่งแบบ Ortho, ";
                    }
                    if (str[4].Trim() == "1")
                    {
                        tmp += "เปลตัก, ";
                    }
                    if (str[5].Trim() == "1")
                    {
                        tmp += "ถังอ๊อกซิเจน, ";
                    }
                    if (str[6].Trim() == "1")
                    {
                        tmp += "สายรัด, ";
                    }
                }
                tmp = tmp.Substring(0, tmp.Length - 2);
            }
            catch { }

            return tmp;
        }

        public List<string> CalculateAge(string bd)
        {
            try
            {
                DateTime d1, d2;
                long days = 0, months = 0, years = 0;

                d1 = Convert.ToDateTime(bd);
                d2 = DateTime.Today;

                if (d1 >= d2) { }
                else
                {
                    years = d1.Year;
                    months = d1.Month;
                    days = d1.Day;

                    years = d2.Year - years;
                    months = d2.Month - months;
                    days = d2.Day - days;

                    if (Math.Sign(days) == -1)
                    {
                        days = 30 - Math.Abs(days);
                        months = months - 1;
                    }

                    if (Math.Sign(months) == -1)
                    {
                        months = 12 - Math.Abs(months);
                        years = years - 1;
                    }
                }

                List<string> list = new List<string>();

                list.Add(years.ToString());
                list.Add(months.ToString());
                list.Add(days.ToString());

                return list;
            }
            catch { }

            return null;
        }
    }
}
