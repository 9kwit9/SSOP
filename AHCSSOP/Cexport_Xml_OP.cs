using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Data;
using System.Security.Cryptography;
using Klik.Windows.Forms.v1.EntryLib;
using System.Net.Mail;
using System.Net.Mime;

namespace AHCSSOP
{
    class Cexport_Xml_OP
    {
        SConnectMSSL mssql;
        SConnectMySQL mysql;

        ELDataGridView glOP;
        ELDataGridView glOP_1;
        ELDataGridView glBill;
        ELDataGridView glDisp;
        ELDataGridView glDisp2;

        string Write = "";
        string WriteHead = "";
        string WriteFoot = "";
        string Hash_MD5 = "";

        string SessionID = AhcSession.GetInstances().HospitalCode + "_" + AhcSession.GetInstances().SessionYear + "_" +
               AhcSession.GetInstances().SessionMonth + "_" + AhcSession.GetInstances().SessionID;

        public string _Name_file = "";
        public string _Name_ssopbill = "";

        private static MD5 md5 = MD5.Create();

        string air = @"D:\SSOP\";

        private string Name_file
        {
            get { return _Name_file; }
            set { _Name_file = value; }
        }

        private string Name_ssopbill
        {
            get { return _Name_ssopbill; }
            set { _Name_ssopbill = value; }
        }

        void CreateXmlFile()
        {
            Name_ssopbill = "11847_SSOPBIL_" + AhcSession.GetInstances().SessionSsopID + "_01" + "_" + DateTime.Now.AddYears(-543).ToString("yyyyMMdd") + "-" + Convert.ToDateTime(DateTime.Now.ToShortTimeString()).ToString("HHmmss");
            Name_file = "OPServices" + DateTime.Now.AddYears(-543).ToString("yyyyMMdd") + ".txt";

            air = air + Name_ssopbill;

            if (!Directory.Exists(air))
            {
                Directory.CreateDirectory(air);
            }
           
            WriteHead = "<?xml version=\"1.0\" encoding=\"windows-874\"?>" + Environment.NewLine;
            Write= "<ClaimRec System=\"OP\" PayPlan=\"SS\" Version=\"0.93\">" + Environment.NewLine;

            Xml_Header();
            Xml_OPServices();
            Xml_OPDx();

            Write += "</ClaimRec>" + Environment.NewLine;

            Save_file_xml(WriteHead + Write);
            //Load_xml();
            Hash_MD5 = CalculateChecksum(@"D:\xml_tmp.txt");
            File.Delete(@"D:\xml_tmp.txt");

            WriteFoot = "<?EndNote Checksum=" + "\"" + Hash_MD5 + "\"" + "?>";

            StreamWriter wt = null;
            try
            {
                wt = new StreamWriter(air + @"\" + Name_file, false, Encoding.GetEncoding("windows-874"));

                wt.Write(WriteHead + Write + WriteFoot);
                wt.Flush();
            }
            catch { }
            finally
            {
                wt.Close();
                //UpdateDatabase();
            }
        }

        void CreateXmlFile_Bill()
        {
            Name_file = "BILLTRAN" + DateTime.Now.AddYears(-543).ToString("yyyyMMdd") + ".txt";

            WriteHead = "<?xml version=\"1.0\" encoding=\"windows-874\"?>" + Environment.NewLine;
            Write = "<ClaimRec System=\"OP\" PayPlan=\"SS\" Version=\"0.93\">" + Environment.NewLine;

            Xml_Header_Bill();
            Xml_Billtran();
            Xml_BilltranDetail();

            Write += "</ClaimRec>" + Environment.NewLine;

            Save_file_xml(WriteHead + Write);
            //Load_xml();
            Hash_MD5 = CalculateChecksum(@"D:\xml_tmp.txt");
            File.Delete(@"D:\xml_tmp.txt");

            WriteFoot = "<?EndNote Checksum=" + "\"" + Hash_MD5 + "\"" + "?>";

            StreamWriter wt = null;
            try
            {
                wt = new StreamWriter(air + @"\" + Name_file, false, Encoding.GetEncoding("windows-874"));
                wt.Write(WriteHead + Write + WriteFoot);
                wt.Flush();
            }
            catch { }
            finally
            {
                wt.Close();
                //UpdateDatabase();
            }
        }

        void CreateXmlFile_Disp()
        {
            Name_file = "BILLDISP" + DateTime.Now.AddYears(-543).ToString("yyyyMMdd") + ".txt";

            WriteHead = "<?xml version=\"1.0\" encoding=\"windows-874\"?>" + Environment.NewLine;
            Write = "<ClaimRec System=\"OP\" PayPlan=\"SS\" Version=\"0.93\">" + Environment.NewLine;

            Xml_Header_Disp();
            Xml_Disp();
            Xml_DispDetail();

            Write += "</ClaimRec>" + Environment.NewLine;

            Save_file_xml(WriteHead + Write);
            //Load_xml();
            Hash_MD5 = CalculateChecksum(@"D:\xml_tmp.txt");
            File.Delete(@"D:\xml_tmp.txt");

            WriteFoot = "<?EndNote Checksum=" + "\"" + Hash_MD5 + "\"" + "?>";

            StreamWriter wt = null;
            try
            {
                wt = new StreamWriter(air + @"\" + Name_file, false, Encoding.GetEncoding("windows-874"));

                wt.Write(WriteHead + Write + WriteFoot);
                wt.Flush();
            }
            catch { }
            finally
            {
                wt.Close();
                //UpdateDatabase();
            }
        }

        void UpdateDatabase()
        {
            try
            {
                mysql.Exeute("INSERT INTO tblssop (hn, visitnumber, status_export) VALUES('" + User_login.GetInstances().hn + "', '" + User_login.GetInstances().VisitID + "','0')");
            }
            catch
            {
                mysql.Exeute("UPDATE tblssop SET status_export='1' WHERE hn='" + User_login.GetInstances().hn + "' AND visitnumber='" + User_login.GetInstances().VisitID + "'");
            }
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            // step 2, convert byte array to hex string
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < hash.Length; i++)
            //{
            //    sb.Append(hash[i].ToString("X2"));
            //}
            //return sb.ToString();

            return (BitConverter.ToString(hash).Replace("-", string.Empty));
        }

        public void ToXml(ELDataGridView _glOP,
            ELDataGridView _glOP_1,
            ELDataGridView _glBill,
            ELDataGridView _glDisp,
            ELDataGridView _glDisp2)
        {
            try
            {
                mssql = new SConnectMSSL();
                mysql = new SConnectMySQL();

                glOP = _glOP;
                glOP_1 = _glOP_1;
                glBill = _glBill;
                glDisp = _glDisp;
                glDisp2 = _glDisp2;

                CreateXmlFile();
                CreateXmlFile_Bill();
                CreateXmlFile_Disp();

                zipfile();
            }
            catch { }
        }

        void zipfile()
        {
            string fileDir = air;
            string zipPath = air + ".zip";
            ZipFile.CreateFromDirectory(fileDir, zipPath);
        }

        private static string CalculateChecksum(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                byte[] checksum = md5.ComputeHash(stream);
                
                return (BitConverter.ToString(checksum).Replace("-", string.Empty));
            } // End of using fileStream
        } // End of CalculateChecksum 

        protected void Xml_Header()
        {
            try
            {
                Write += "<Header>" + Environment.NewLine;

                Write += "<HCODE>";
                Write += User_login.GetInstances().Hcode;
                Write += "</HCODE>" + Environment.NewLine;

                Write += "<HNAME>";
                Write += User_login.GetInstances().Hname;
                Write += "</HNAME>" + Environment.NewLine;

                Write += "<DATETIME>";
                Write += DateTime.Now.AddYears(-543).ToString("yyyy-MM-dd") + "T" +
                    DateTime.Now.ToString("hh:mm:ss");
                Write += "</DATETIME>" + Environment.NewLine;

                Write += "<SESSNO>";
                Write += SessionID;
                Write += "</SESSNO>" + Environment.NewLine;

                Write += "<RECCOUNT>";
                Write += glOP.RowCount.ToString();
                Write += "</RECCOUNT>" + Environment.NewLine;

                Write += "</Header>" + Environment.NewLine;
            }
            catch { }
        }

        void Xml_OPServices()
        {
            if (glOP.RowCount != 0)
            {
                Write += "<OPServices>" + Environment.NewLine;
                for (int i = 0; i<=glOP.Rows.Count - 1; i++)
                {
                    string licen = "";

                    if (glOP.Rows[i].Cells[17].Tag.ToString().Substring(0, 2).Trim() == "ท")
                    {
                        licen = glOP.Rows[i].Cells[17].Tag.ToString().Trim().Replace(".", "");
                    }
                    else
                    {
                        licen = "ว" + glOP.Rows[i].Cells[17].Tag.ToString().Trim();
                    }

                    Write += glOP.Rows[i].Cells[19].Value.ToString() + "|" +
                        glOP.Rows[i].Cells[1].Value.ToString() + "|" +
                        glOP.Rows[i].Cells[2].Tag.ToString() + "|" +
                        "11847" + "|" +
                        glOP.Rows[i].Cells[0].Tag.ToString() + "|" +
                        glOP.Rows[i].Cells[12].Tag.ToString().Replace("-", "") + "|" +
                        glOP.Rows[i].Cells[3].Tag.ToString() + "|" +
                        glOP.Rows[i].Cells[4].Tag.ToString() + "|" +
                        glOP.Rows[i].Cells[5].Tag.ToString() + "|" +
                        glOP.Rows[i].Cells[6].Tag.ToString() + "|" +
                        glOP.Rows[i].Cells[7].Value.ToString() + "|" +
                        licen + "|" +
                        glOP.Rows[i].Cells[9].Tag.ToString() + "|" +
                        Convert.ToDateTime(glOP.Rows[i].Cells[10].Value).AddYears(-543).ToString("yyyy-MM-dd") + "T" +
                            Convert.ToDateTime(glOP.Rows[i].Cells[10].Value).ToString("HH:mm:ss") + "|" +
                        Convert.ToDateTime(glOP.Rows[i].Cells[11].Value).AddYears(-543).ToString("yyyy-MM-dd") + "T" +
                            Convert.ToDateTime(glOP.Rows[i].Cells[11].Value).ToString("HH:mm:ss") +
                        "|||" +
                        glOP.Rows[i].Cells[14].Value.ToString() + "|" +
                        Convert.ToDouble(glOP.Rows[i].Cells[15].Value).ToString("00000#.#0") + "|" +
                        glOP.Rows[i].Cells[16].Tag.ToString() + "|" +
                        glOP.Rows[i].Cells[17].Value.ToString() + "|" +
                        glOP.Rows[i].Cells[18].Tag.ToString() +
                         (char)0x0D + (char)0x0A;
                }
                    
                Write += "</OPServices>" + Environment.NewLine;
            }
        }

        void Xml_OPDx()
        {
            if (glOP.RowCount != 0)
            {
                Write += "<OPDx>" + Environment.NewLine;

                for (int i = 0; i <= glOP.Rows.Count - 1; i++)
                {
                    try
                    {
                        Write += glOP.Rows[i].Cells[2].Tag.ToString() + "|" +
                   glOP.Rows[i].Cells[1].Value.ToString() + "|1|" +
                   "IT|" +
                   glOP.Rows[i].Cells[8].Tag.ToString().Split('|').GetValue(0).ToString() + "|" +
                   glOP.Rows[i].Cells[8].Tag.ToString().Split('|').GetValue(1).ToString() +
                    (char)0x0D + (char)0x0A;
                    }
                    catch
                    {
                        Write += glOP.Rows[i].Cells[2].Tag.ToString() + "|" +
                   glOP.Rows[i].Cells[1].Value.ToString() + "|1|" +
                   "IT|" +
                   "" + "|" +
                   "" +
                    (char)0x0D + (char)0x0A;
                    }
                }
               
                Write += "</OPDx>" + Environment.NewLine;
            }
        }

        #region "BILLTRAN"

        protected void Xml_Header_Bill()
        {
            try
            {
                Write += "<Header>" + Environment.NewLine;

                Write += "<HCODE>";
                Write += User_login.GetInstances().Hcode;
                Write += "</HCODE>" + Environment.NewLine;

                Write += "<HNAME>";
                Write += User_login.GetInstances().Hname;
                Write += "</HNAME>" + Environment.NewLine;

                Write += "<DATETIME>";
                Write += DateTime.Now.AddYears(-543).ToString("yyyy-MM-dd") + "T" +
                    DateTime.Now.AddYears(-543).ToString("hh:mm:ss");
                Write += "</DATETIME>" + Environment.NewLine;

                Write += "<SESSNO>";
                Write += SessionID;
                Write += "</SESSNO>" + Environment.NewLine;

                Write += "<RECCOUNT>";
                Write += glOP_1.RowCount.ToString();
                Write += "</RECCOUNT>" + Environment.NewLine;

                Write += "</Header>" + Environment.NewLine;
            }
            catch { }
        }

        void Xml_Billtran()
        {
            if (glOP.RowCount != 0)
            {
                Write += "<BILLTRAN>" + Environment.NewLine;

                for (int i = 0; i <= glOP_1.Rows.Count - 1; i++)
                {
                    try
                    {
                        Write += "01||" +
                       Convert.ToDateTime(glOP_1.Rows[i].Cells[0].Value).AddYears(-543).ToString("yyyy-MM-dd") + "T" +
                       Convert.ToDateTime(glOP_1.Rows[i].Cells[0].Value).AddYears(-543).ToString("hh:mm:ss") + "|" +
                       "11847" + "|" +
                       glOP_1.Rows[i].Cells[1].Value.ToString() + "||" +
                       glOP_1.Rows[i].Cells[1].Tag.ToString() + "||" +
                       glOP_1.Rows[i].Cells[2].Value + "|000000.00" +
                       "||" +
                       User_login.GetInstances().Tflag + "|" +
                       glOP_1.Rows[i].Cells[3].Tag.ToString().Replace("-", "") + "|" +
                       glOP_1.Rows[i].Cells[2].Tag.ToString() + "|" +
                       "23207|" +
                       User_login.GetInstances().Plan + "|" +
                       glOP_1.Rows[i].Cells[2].Value + "|ZZ|000000.00" +
                       (char)0x0D + (char)0x0A;
                    }
                    catch { }
                }

                Write += "</BILLTRAN>" + Environment.NewLine;
            }
        }

        void Xml_BilltranDetail()
        {
            if (glBill.RowCount != 0)
            {
                Write += "<BillItems>" + Environment.NewLine;

                for (int i = 0; i <= glBill.Rows.Count - 1; i++)
                {
                    try
                    {
                        if (glBill.Rows[i].Cells[3].Value.ToString() != "")
                        {
                            string Code = "";
                            if (glBill.Rows[i].Cells[2].Tag.ToString() == "3")
                            {
                                Code = glBill.Rows[i].Cells[3].Tag.ToString();
                            }
                            Write += glBill.Rows[i].Cells[0].Tag.ToString() + "|" +
                           Convert.ToDateTime(glBill.Rows[i].Cells[1].Value).AddYears(-543).ToString("yyyy-MM-dd") + "|" +
                           glBill.Rows[i].Cells[2].Tag.ToString() + "|" +
                           glBill.Rows[i].Cells[3].Value.ToString() + "|" +
                           Code + "|" +
                           glBill.Rows[i].Cells[5].Value.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") + "|" +
                           glBill.Rows[i].Cells[6].Value.ToString() + "|" +
                           glBill.Rows[i].Cells[7].Value.ToString() + "|" +
                           glBill.Rows[i].Cells[8].Value.ToString() + "|" +
                           glBill.Rows[i].Cells[9].Value.ToString() + "|" +
                           glBill.Rows[i].Cells[10].Value.ToString() + "|" +
                           glBill.Rows[i].Cells[11].Tag.ToString() + "|" +
                           glBill.Rows[i].Cells[12].Tag.ToString() +
                           (char)0x0D + (char)0x0A;
                        }
                    }
                    catch { }
                }

                Write += "</BillItems>" + Environment.NewLine;
            }
        }

        #endregion

        #region "Dispensing"

        protected void Xml_Header_Disp()
        {
            try
            {
                Write += "<Header>" + Environment.NewLine;

                Write += "<HCODE>";
                Write += User_login.GetInstances().Hcode;
                Write += "</HCODE>" + Environment.NewLine;

                Write += "<HNAME>";
                Write += User_login.GetInstances().Hname;
                Write += "</HNAME>" + Environment.NewLine;

                Write += "<DATETIME>";
                Write += DateTime.Now.AddYears(-543).ToString("yyyy-MM-dd") + "T" +
                    DateTime.Now.AddYears(-543).ToString("hh:mm:ss");
                Write += "</DATETIME>" + Environment.NewLine;

                Write += "<SESSNO>";
                Write += SessionID;
                Write += "</SESSNO>" + Environment.NewLine;

                Write += "<RECCOUNT>";
                Write += glDisp2.Rows.Count;
                Write += "</RECCOUNT>" + Environment.NewLine;

                Write += "</Header>" + Environment.NewLine;
            }
            catch { }
        }

        void Xml_Disp()
        {
            if (glDisp2.Rows.Count != 0)
            {
                    Write += "<Dispensing>" + Environment.NewLine;
                    for (int i = 0; i <= glDisp2.Rows.Count - 1; i++)
                    {
                        try
                        {
                            string licen = "";

                            if (glOP.Rows[i].Cells[17].Tag.ToString().Substring(0, 2).Trim() == "ท")
                            {
                                licen = glOP.Rows[i].Cells[17].Tag.ToString().Trim().Replace(".", "");
                            }
                            else
                            {
                                licen = "ว" + glOP.Rows[i].Cells[17].Tag.ToString().Trim();
                            }

                            Write += User_login.GetInstances().Hcode + "|" +
                            glDisp2.Rows[i].Cells[0].Value.ToString() + "|" +
                            glDisp2.Rows[i].Cells[0].Tag.ToString() + "|" +
                            glDisp2.Rows[i].Cells[1].Tag.ToString() + "|" +
                            glDisp2.Rows[i].Cells[3].Tag.ToString().Replace("-", "") + "|" +
                            Convert.ToDateTime(glDisp2.Rows[i].Cells[1].Value).ToString("yyyy-MM-dd") + "T" +
                            Convert.ToDateTime(glDisp2.Rows[i].Cells[1].Value).ToString("hh:mm:ss") + "|" +
                            Convert.ToDateTime(glDisp2.Rows[i].Cells[2].Value).ToString("yyyy-MM-dd") + "T" +
                            Convert.ToDateTime(glDisp2.Rows[i].Cells[2].Value).ToString("hh:mm:ss") + "|" +
                            licen + "|" +
                            Convert.ToDouble(glDisp2.Rows[i].Cells[2].Tag).ToString("00000#.#0") + "|" +
                            Convert.ToDouble(glDisp2.Rows[i].Cells[3].Value).ToString("00000#.#0") + "|" +
                            Convert.ToDouble(glDisp2.Rows[i].Cells[3].Value).ToString("00000#.#0") + "|000000.00|000000.00|HP|SS|1|" +
                            glDisp2.Rows[i].Cells[0].Value.ToString() + "|" +
                            (char)0x0D + (char)0x0A;
                        }
                        catch { }
                    }

                    Write += "</Dispensing>" + Environment.NewLine;
            }
            else
            {
                Write += "<Dispensing></Dispensing>" + Environment.NewLine;
            }
        }

        void Xml_DispDetail()
        {
            if (glDisp.Rows.Count != 0)
            {
                try
                {
                    Write += "<DispensedItems>" + Environment.NewLine;

                    for (int i = 0; i <= glDisp.Rows.Count - 1; i++)
                    {
                        Write += glDisp.Rows[i].Cells[1].Value.ToString() + "|" +
                       glDisp.Rows[i].Cells[2].Value.ToString() + "|" +
                       glDisp.Rows[i].Cells[3].Value.ToString() + "|" +
                       glDisp.Rows[i].Cells[4].Value.ToString() + "||" +
                       glDisp.Rows[i].Cells[6].Value.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") + "|" +
                       glDisp.Rows[i].Cells[7].Value.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") + "|" +
                       glDisp.Rows[i].Cells[8].Value.ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") + "|" +
                       Convert.ToDouble(glDisp.Rows[i].Cells[10].Value).ToString("00000#.#0") + "|" +
                       Convert.ToDouble(glDisp.Rows[i].Cells[11].Value).ToString("00000#.#0") + "|" +
                       Convert.ToDouble(glDisp.Rows[i].Cells[12].Value).ToString("00000#.#0") + "|" +
                       Convert.ToDouble(glDisp.Rows[i].Cells[11].Value).ToString("00000#.#0") + "|" +
                       Convert.ToDouble(glDisp.Rows[i].Cells[12].Value).ToString("00000#.#0") + "||OD|OP1||" +
                       (char)0x0D + (char)0x0A;
                    }

                    Write += "</DispensedItems>" + Environment.NewLine;
                }
                catch { }
            }
            else
            {
                Write += "<DispensedItems></DispensedItems>" + Environment.NewLine;
            }
        }

        #endregion

        void Save_file_xml(string _xml)
        {
            StreamWriter wt = null;
            try
            {
                wt = new StreamWriter(@"D:\xml_tmp.txt", false, Encoding.GetEncoding("windows-874"));

                wt.Write(_xml);
                wt.Flush();
            }
            catch { }
            finally
            {
                wt.Close();
                wt.Dispose();
            }
        }
    }
}
