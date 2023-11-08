using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace AHCSSOP
{
    class SConnectMySQL
    {
        CultureInfo provider = CultureInfo.InvariantCulture;

        private MySqlConnection connection;
        
        //Constructor
        public SConnectMySQL()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");

            string connectionString;
            //connectionString = "Data Source=191.1.10.76;Initial Catalog=ARCUS;User ID=ahcit;Password=@hc1t";
            connectionString = "Data Source=191.1.2.113;" + "DATABASE=aikchol_ssop;" + "UID=welfrare;" + "PASSWORD=GJcTCftjjZdUQnV;port=3306;charset=utf8;Allow Zero Datetime=True";
           //connectionString = "Data Source=localhost;" + "DATABASE=test;" + "UID=root;" + "PASSWORD=root;port=3306;charset=utf8;Allow Zero Datetime=True";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        connection.Close();
                        break;

                    case 1045:
                        connection.Close();
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Exeute(string sql)
        {
            try
            {
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch { this.CloseConnection(); }
        }

        public DataSet Select_Value_Med()
        {
            DataSet ds = new DataSet();

            string query = "SELECT * FROM medsupply Order By IPD ASC";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public DataSet Select_Value_NSS()
        {
            DataSet ds = new DataSet();

            string query = "SELECT * FROM tbl_nss Order By IPD ASC";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public DataSet Select_Value_Medication()
        {
            DataSet ds = new DataSet();

            string query = "SELECT * FROM medication Order By code ASC";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public string Select_Value_FSCode(string Code)
        {
            DataSet ds = new DataSet();

            string query = "SELECT FSCODE FROM FSCatalogue WHERE HospitalCode='" + Code + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (ds.Tables["0"].Rows.Count != 0)
                {
                    return ds.Tables["0"].Rows[0]["FSCODE"].ToString();
                }
                else { return ""; }
            }
            else
            {
                return "";
            }
        }

        public string Select_Value_DrugCode(string Code)
        {
            DataSet ds = new DataSet();

            string query = "SELECT TMTID FROM Drugcatalog WHERE HOSPDRUGCODE='" + Code + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (ds.Tables["0"].Rows.Count != 0)
                {
                    return ds.Tables["0"].Rows[0]["TMTID"].ToString();
                }
                else { return ""; }
            }
            else
            {
                return "";
            }
        }

        public string Select_NameFS(string Code)
        {
            DataSet ds = new DataSet();

            string query = "SELECT Description FROM FSCatalogue WHERE HospitalCode='" + Code + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (ds.Tables["0"].Rows.Count != 0)
                {
                    return ds.Tables["0"].Rows[0]["Description"].ToString();
                }
                else { return ""; }
            }
            else
            {
                return "";
            }
        }

        public DataSet GetExits(string Code)
        {
            DataSet ds = new DataSet();

            string query = "SELECT * FROM tblpatient WHERE an='" + Code + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (ds.Tables["0"].Rows.Count != 0)
                {
                    return ds;
                }
                else { return ds; }
            }
            else
            {
                return ds;
            }
        }

        public DataSet LoadData_FSCatalog(string Fscode, string Hoscode, string name)
        {
            DataSet ds = new DataSet();

            string query = "SELECT FSCODE, HospitalCode, Description, Price, Category FROM FSCatalogue WHERE ";
            string sql = "";
            //FSCODE LIKE '%" + Fscode + "%' Or HospitalCode LIKE '%" + Hoscode + "%' Or Description LIKE '%" + name + "%'
            if (Fscode.Trim() != "")
            {
                if (sql.Trim() != "")
                {
                    sql += " AND FSCODE LIKE '" + Fscode + "%'";
                }
                else
                {
                    sql = " FSCODE LIKE '" + Fscode + "%'";
                }
            }

            if (Hoscode.Trim() != "")
            {
                if (sql.Trim() != "")
                {
                    sql += " AND HospitalCode LIKE '" + Hoscode + "%'";
                }
                else
                {
                    sql = " HospitalCode LIKE '" + Hoscode + "%'";
                }
            }

            if (name.Trim() != "")
            {
                if (sql.Trim() != "")
                {
                    sql += " AND Description LIKE '" + name + "%'";
                }
                else
                {
                    sql = " Description LIKE '" + name + "%'";
                }
            }

            query += sql;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public DataSet LoadData_Description(string Code)
        {
            DataSet ds = new DataSet();

            string query = "SELECT * FROM tbldescription WHERE vn='" + Code + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public DataSet LoadDataFSUCEP()
        {
            DataSet ds = new DataSet();

            string query = " SELECT tblpatient.patientuid, tblpatient.hn, tblpatient.fullname, tblpatient.an, tblmovement.standardprice, ";
            query += " tblmovement.uploadprice, tblmovement.fsprice, tblmovement.dateupload, tblmovement.daterecived, tblmovement.puid, tblmovement.onprice ";
            query += " FROM tblpatient LEFT JOIN tblmovement ON tblpatient.patientuid = tblmovement.puid Order By tblpatient.uid DESC";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public string GetExits_SSOP(string Code)
        {
            DataSet ds = new DataSet();

            string query = "SELECT visitnumber FROM tblssop WHERE visitnumber='" + Code + "'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                if (ds.Tables["0"].Rows.Count != 0)
                {
                    return ds.Tables["0"].Rows[0]["visitnumber"].ToString();
                }
                else { return ""; }
            }
            else
            {
                return "";
            }
        }

        public DataSet Select_Value_ICD10Lab()
        {
            DataSet ds = new DataSet();

            string query = "SELECT * FROM sheet1 Order By VisitDate ASC";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public DataSet GetExits_SSOP()
        {
            DataSet ds = new DataSet();

            string query = "SELECT visitnumber FROM tblssop ";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                data.Fill(ds, "0");

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return ds;
            }
            else
            {
                return ds;
            }
        }

        public string Select_Value_IDCard(string code, string bill)
        {
            DataSet ds = new DataSet();

            string query = "SELECT Idcard FROM tbl_tmp WHERE Idcard='" + code + "' AND inv='" + bill + "' Order By Idcard ASC";

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                    data.Fill(ds, "0");

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    if (ds.Tables["0"].Rows.Count != 0) { 
                        return ds.Tables["0"].Rows[0]["Idcard"].ToString(); 
                    } else { return ""; }
                }
                else
                {
                    return "";
                }
            }
            catch { this.CloseConnection(); }
            return "";
        }

        public int Select_SessionID(string hos, string year, string month)
        {
            DataSet ds = new DataSet();

            string query = "SELECT cnt FROM tbl_session_id WHERE hos='" + hos + "' AND year='" + year + "' AND month='" + month + "'";

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                    data.Fill(ds, "0");

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    if (ds.Tables["0"].Rows.Count != 0)
                    {
                        return Convert.ToInt32(ds.Tables["0"].Rows[0]["cnt"]);
                    }
                    else { return 0; }
                }
                else
                {
                    return 0;
                }
            }
            catch { this.CloseConnection(); }
            return 0;
        }

        public int Select_Ssopbill()
        {
            DataSet ds = new DataSet();

            string query = "SELECT session FROM tbl_ssopbill";

            try
            {
                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlDataAdapter data = new MySqlDataAdapter(query, connection);


                    data.Fill(ds, "0");

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    if (ds.Tables["0"].Rows.Count != 0)
                    {
                        return Convert.ToInt32(ds.Tables["0"].Rows[0]["session"]);
                    }
                    else { return 0; }
                }
                else
                {
                    return 0;
                }
            }
            catch { this.CloseConnection(); }
            return 0;
        }
    }
}
