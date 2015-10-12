using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using System.EnterpriseServices;

using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

namespace MvcDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult CekASPXtoHTML()
        { return View(); }
        [HttpPost]
        public ActionResult CekASPXtoHTML(HttpPostedFileBase file)
        {

            var date = new List<DateTime> { };
            var phe23 = new List<float> { };
            var phe30 = new List<float> { };
            var phe32 = new List<float> { };
            var phe38 = new List<float> { };
            var phe38b = new List<float> { };
            var phe40 = new List<float> { };
            var phe54 = new List<float> { };
            var phe556 = new List<float> { };
            var phe39 = new List<float> { };
            var total = new List<float> { };

            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {

                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                    
                        t++;
                        
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    
                    xmlreader.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                     string conn = ConfigurationManager.ConnectionStrings["DummyDBContext"].ConnectionString;
                     SqlConnection con = new SqlConnection(conn);
                    string query = "Insert into Status_PHE32 (date,[KE32-A1-1],[KE32-A1-2],[KE32-A3]) Values('" + ds.Tables[0].Rows[i][0] + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() + "','" + ds.Tables[0].Rows[i][3].ToString() + "')";
                    con.Open();
                         SqlCommand cmd = new SqlCommand(query, con);
                       cmd.ExecuteNonQuery();
                    
                         con.Close();

                  

                }

            }

           

            return View();
        }


public ActionResult About()
        { return View(); }

        public ActionResult JSONURL()
        { return View("JSONURL"); }

        public ActionResult Index()
        { return View(); }

        public ActionResult Login()
        { return View(); }

        public ActionResult MorningNews()
        {
            return View();
        }

        public ActionResult ManagementDashboard()
        {
            return View();
        }


        public ActionResult Contact()
        { return View(); }
        [HttpPost]
        public ActionResult Contact(HttpPostedFileBase file)
        {

            var date = new List<DateTime> { };
            var phe23 = new List<float> { };
            var phe30 = new List<float> { };
            var phe32 = new List<float> { };
            var phe38 = new List<float> { };
            var phe38b = new List<float> { };
            var phe40 = new List<float> { };
            var phe54 = new List<float> { };
            var phe556 = new List<float> { };
            var phe39 = new List<float> { };
            var total = new List<float> { };

            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {

                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();

                        t++;

                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string conn = ConfigurationManager.ConnectionStrings["DummyDBContext"].ConnectionString;
                    SqlConnection con = new SqlConnection(conn);
                    string query = "Insert into Status_PHE54 (date,[phe54-1],[phe54-2],[phe54-3],[phe54-5],[phe54-6],[phe54-7],total) Values('" + ds.Tables[0].Rows[i][0] + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() + "','" + ds.Tables[0].Rows[i][3].ToString() + "','" + ds.Tables[0].Rows[i][4].ToString() + "','" + ds.Tables[0].Rows[i][5].ToString() + "','" + ds.Tables[0].Rows[i][6].ToString() + "','" + ds.Tables[0].Rows[i][7].ToString() + "')";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();



                }

            }



            return View();
        }
    }
}