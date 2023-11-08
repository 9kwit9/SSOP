using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHCSSOP
{
    public class User_login
    {
        private static User_login instances = null;

        public string fullname = "";
        public string hn = "";
        public string NationalID = "";
        public string PatientVisitUID = "";
        public string PatientUID = "";
        public string Startdttm = "";
        public string Enddttm = "";
        public string EncounterType = "";
        public string VisitID = "";
        public string FileName = "";
        public string HMain = "";
        public string Hcode = "";
        public string Hname = "";
        public string Location = "";
        public string BillNumber = "";
        public string BillDate = "";
        public string BillTotal = "";
        public string Tflag = "";
        public string Plan = "";
        public string DispDate = "";
        public string Dispid = "";
        public string DispenDate = "";
        public string DispCount = "";
        public string DispSum = "";
        public string Doctor_licens = "";
        public string Doctor_charge = "0.00";
        public string StatusClass = "";
        public string DiagCode = "";
        public string DiagName = "";
        public bool Enable = false;

        private User_login() { }

        public static User_login GetInstances()
        {
            if (instances == null)
            {
                instances = new User_login();
            }
            return instances;
        }
    }

    public class OPServices
    {
        private static OPServices instances = null;

        public string Reccount = "";

        private OPServices() { }

        public static OPServices GetInstances()
        {
            if (instances == null)
            {
                instances = new OPServices();
            }
            return instances;
        }
    }

    public class AhcSession
    {
        private static AhcSession instances = null;

        public string HospitalCode = "";
        public string SessionYear = "";
        public string SessionMonth = "";
        public string SessionID = "";
        public string SessionSsopID = "";

        public static AhcSession GetInstances()
        {
            if (instances == null)
            {
                instances = new AhcSession();
            }
            return instances;
        }
    }
}
