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

namespace AHCSSOP
{
    class SConnectMSSL
    {
        CultureInfo provider = CultureInfo.InvariantCulture;

        private SqlConnection connection;
        
        //Constructor
        public SConnectMSSL()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");

            string connectionString;
            connectionString = "Data Source=191.1.10.76;Initial Catalog=ARCUS;User ID=ahcit;Password=@hc1t";

            connection = new SqlConnection(connectionString);
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

        public DataSet LoadBillable(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT * FROM BillableItem WHERE Code = '" + Code + "' AND StatusFlag='A' Order By Code ASC";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds; }
                }

            }
            catch { CloseConnection(); }

            return ds;
        }

        public DataSet LoadBillableItem(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT * FROM BillableItemDetail WHERE BillableItemUID = '" + Code + "' AND ENTYPUID='1379' AND PBLCTUID='1118' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds; }
                }

            }
            catch { CloseConnection(); }

            return ds;
        }

        public string LoadUID(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT UID FROM BillableItem WHERE Code = '" + Code + "' AND StatusFlag='A' Order By Code ASC";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["UID"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public string LoadVisitUID(string VisitID)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT PatientVisitUID FROM PatientVisitID WHERE Identifier='" + VisitID + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["PatientVisitUID"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public string LoadPatientUID(string VisitID)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT PatientUID FROM PatientVisit WHERE UID='" + VisitID + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["PatientUID"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public DataSet LoadPatient_BillableItem(DateTime DateFrom, string FormTime, DateTime DateTo, string ToTime, string VisitID)
        {
            DataSet dsCustomers = new DataSet();
            string P_Visit = LoadVisitUID(VisitID);
            string P_Uid = LoadPatientUID(P_Visit);
            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_UCEP", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_FromDttm", DateFrom));
                    cmd.Parameters.Add(new SqlParameter("@P_FromTime", FormTime));
                    cmd.Parameters.Add(new SqlParameter("@P_ToDttm", DateTo));
                    cmd.Parameters.Add(new SqlParameter("@P_ToTime", ToTime));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", P_Visit));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", P_Uid));
                    cmd.Parameters.Add(new SqlParameter("@P_VisitID", VisitID));

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0)
                    {
                        return dsCustomers;
                    }
                }
            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet LoadPatient_BillableItem(string VisitID)
        {
            DataSet dsCustomers = new DataSet();
            
            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_UCEP", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", User_login.GetInstances().PatientUID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", User_login.GetInstances().PatientVisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_VisitID", VisitID));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0)
                    {
                        return dsCustomers;
                    }
                }
            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet LoadBillGroup_SubGroup(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT CollectionCenterUID, ServiceUID FROM BillableItem WHERE Code='" + Code + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds; }
                }

            }
            catch { CloseConnection(); }

            return ds;
        }

        public string GetServicesName(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT Name FROM Service WHERE UID='" + Code + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["Name"].ToString(); }
                    else
                    {

                    }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public string GetBillGroup(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT ISNULL(dbo.fGetParentServiceName(BI.ServiceUID),'') AS BillGroup";
                sql += " FROM BillableItem BI WHERE BI.Code='" + Code + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["BillGroup"].ToString(); }
                    else
                    {

                    }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public string GetSubGroup(string Code)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT ISNULL(dbo.fGetServiceName(BI.ServiceUID),'') AS BillSubGroup";
                sql += " FROM BillableItem BI WHERE BI.Code='" + Code + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["BillSubGroup"].ToString(); }
                    else
                    {

                    }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public DataSet GetPatient(DateTime DateFrom, DateTime DateTo)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP", connection);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_FromDttm", DateFrom));
                    cmd.Parameters.Add(new SqlParameter("@P_ToDttm", DateTo));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatient(string VisitID)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_VN", connection);
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_VisitID", VisitID));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatientBill(string UID, string VisitUID, string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_Billtran", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatientBill_DisCount(string UID, string VisitUID, string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_Billtran_DisCount", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatientBill_Package(string UID, string VisitUID, string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_Billtran_Package", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatientBill_Package_Price(string UID)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_Billtran_Package_Price", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PackageUID", UID));
                    

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public string GetBill(string UID, string VisitUID)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_Billtran", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers.Tables["0"].Rows[0]["BillNumber"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public DataSet GetPatientBillDisp(string UID, string VisitUID, string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_BillDisp2", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public string GetDF(string UID, string VisitUID)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_CheckDF", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers.Tables["0"].Rows[0]["DF"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public string GetDF_Price(string UID, string VisitUID,string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_BilltranDetail_DF", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers.Tables["0"].Rows[0]["NetAmount"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }

        public DataSet GetPatientDetail_Bill(string UID, string VisitUID,string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_BilltranDetail", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatientDetail_Disp(string UID, string VisitUID, string BillNumber)
        {
            DataSet dsCustomers = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_BillDispItems", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_PatientUID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_PatientVisitUID", VisitUID));
                    cmd.Parameters.Add(new SqlParameter("@P_BillNumber", BillNumber));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        dsCustomers.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        dsCustomers.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (dsCustomers.Tables["0"].Rows.Count != 0) { return dsCustomers; }
                }

            }
            catch { CloseConnection(); }

            return dsCustomers;
        }

        public DataSet GetPatientVisit(string PatientVisitUID)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT ISNULL(dbo.fGetEncounterType(" + PatientVisitUID + "),'') as EnCounterType, ";
                sql += " ISNULL(dbo.fGetPatientBedName(" + PatientVisitUID + "),'') as BedName, ISNULL(dbo.fGetPatientWardName(" + PatientVisitUID + "),'') as WardName, ";
                sql += " StartDTTM, EndDttm FROM PatientVisit WHERE UID='" + PatientVisitUID + "'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds; }
                }

            }
            catch { CloseConnection(); }

            return ds;
        }

        public DataSet PatientDiag(string UID, string VisitID)
        {
            DataSet ds = new DataSet();

            try
            {
                if (OpenConnection())
                {
                    SqlCommand cmd;

                    cmd = new SqlCommand("pRepAHC_SSOP_PatientDiag", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@P_UID", UID));
                    cmd.Parameters.Add(new SqlParameter("@P_VisitID", VisitID));

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Create a new DataSet.

                        ds.Tables.Add("0");

                        //Load DataReader into the DataTable.
                        ds.Tables[0].Load(sdr);
                    }

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds; }
                }

            }
            catch { CloseConnection(); }

            return ds;
        }

        public string PatientDiagCode(string UID, string VisitID)
        {
            DataSet ds = new DataSet();

            try
            {
                string sql = "SELECT ProblemCode FROM ProblemCodingList WHERE PatientUID='" + UID + "' AND PatientVisitUID='" + VisitID + "' AND StatusFlag='A'";

                if (OpenConnection())
                {
                    SqlDataAdapter adpt = new SqlDataAdapter(sql, connection);
                    adpt.Fill(ds, "0");
                    adpt.Dispose();

                    CloseConnection();

                    if (ds.Tables["0"].Rows.Count != 0) { return ds.Tables["0"].Rows[0]["ProblemCode"].ToString(); }
                }

            }
            catch { CloseConnection(); }

            return "";
        }
    }
}
