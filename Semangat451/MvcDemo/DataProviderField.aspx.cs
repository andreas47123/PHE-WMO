using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Text;
using System.Globalization;
using System.Xml;
using System.Diagnostics;

using System.Configuration;

using InfoSoftGlobal;

public partial class DataProviderField : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //biar gampang sih ini
        string[] f = new string[20];
        
        string dateFrom = Request.QueryString["from"];
        //string dateFrom = "";

        string dateTo = Request.QueryString["to"];
        //string dateTo = "";

        string nome = Request.QueryString["tab"];
        //string nome = "Oil";

        string well = Request.QueryString["well"];
        //string nome = "Oil";

        Debug.WriteLine(well);
        Debug.WriteLine(nome);

        string between = "";

        if ((dateFrom != "") && (dateFrom != null) && (dateTo != "") && (dateTo != null))
        {
            between = " WHERE date BETWEEN '" + dateFrom + "' AND '" + dateTo + "'";
        }
        
        // Construct the connection string to interface with the SQL Server Database
        string connStr = ConfigurationManager.ConnectionStrings["DummyDBContext"].ConnectionString;
        string satuanGas = "MMSCFD";
        string satuanOil = "BOPD";
        string satuanWater = "BWPD";
        string satuan = "";

        if (nome == "Gas") { satuan = satuanGas; }
        else if (nome == "Oil") { satuan = satuanOil; }
        else if (nome == "Water") { satuan = satuanWater; }

        // Initialize the string which would contain the chart data in XML format
        StringBuilder xmlStr = new StringBuilder();
        if ((nome == null) || (nome == ""))
        //<chart caption="Number of visitors last week" subcaption="Bakersfield Central vs Los Angeles Topanga" captionfontsize="14" subcaptionfontsize="14" subcaptionfontbold="0" palettecolors="#0075c2,#1aaf5d" bgcolor="#ffffff" showborder="0" showshadow="0" showcanvasborder="0" useplotgradientcolor="0" legendborderalpha="0" legendshadow="0" showaxislines="0" showalternatehgridcolor="0" divlinethickness="1" divlinedashed="1" divlinedashlen="1" divlinegaplen="1" xaxisname="Day" showvalues="0">
        //{ xmlStr.Append("<chart caption='Well phe38-B1' subcaption='Gas' xaxisname='Date' yaxisname='MSCF' numberprefix='' palettecolors='#AB4743,#89A54C,#71598F,#4298AE,#E74C3C,#DA843B,#663399,#93A9CF,#674172' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1'>"); }
        { xmlStr.Append("<chart caption='Well phe38-B1' subcaption='Gas' yaxisname='" + satuan + "' captionfontsize='20' subcaptionfontsize='14' subcaptionfontbold='0' palettecolors='#AB4743' bgcolor='#ffffff' showborder='0' showshadow='0' showcanvasborder='0' useplotgradientcolor='0' legendborderalpha='0' legendshadow='0' showaxislines='0' showalternatehgridcolor='0' divlinethickness='1' divlinedashed='1' divlinedashlen='1' divlinegaplen='1' xaxisname='Date' showvalues='0'>"); }
        else
        {
            // Provide the relevant customization attributes to the chart
            xmlStr.Append("<chart caption='Well " + well + "' subcaption='" + nome + "' yaxisname='" + satuan + "' captionfontsize='20' subcaptionfontsize='14' subcaptionfontbold='0' palettecolors='#AB4743' bgcolor='#ffffff' showborder='0' showshadow='0' showcanvasborder='0' useplotgradientcolor='0' legendborderalpha='0' legendshadow='0' showaxislines='0' showalternatehgridcolor='0' divlinethickness='1' divlinedashed='1' divlinedashlen='1' divlinegaplen='1' xaxisname='Date' showvalues='0' xAxisNameFontSize='20' yAxisNameFontSize='20' valueFontSize='16' legendItemFontSize='16'>");
        }


        

        // <chart caption="Product-wise quarterly revenue Vs profit in last year" subcaption="Harry&#39;s SuperMart" xaxisname="Quarter" yaxisname="Revenue (In USD)" numberprefix="$" palettecolors="#0075c2,#1aaf5d,#f2c500" bgcolor="#ffffff" borderalpha="20" showcanvasborder="0" useplotgradientcolor="0" plotborderalpha="10" legendborderalpha="0" legendshadow="0" legendbgalpha="0" valuefontcolor="#ffffff" showxaxisline="1" xaxislinecolor="#999999" divlinecolor="#999999" divlinedashed="1" showalternatehgridcolor="0" subcaptionfontbold="0" subcaptionfontsize="14" showhovereffect="1">
        // Create a SQLConnection object 
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            
            // Establish the connection with the database
            conn.Open();
            string result;
            //Buat Kategori
            SqlCommand query = new SqlCommand("SELECT * FROM Gas_PHE38B ORDER BY date", conn);
            if ((well != null) && (well != ""))
            {
                string command = "SELECT * FROM " + nome + "_PHE38B";
                if ((between != "") && (between != null))
                {
                    command += between;
                }
                Debug.WriteLine(command);
                query = new SqlCommand(command, conn);
            }

            SqlDataReader rst = query.ExecuteReader();
            xmlStr.AppendFormat("<categories>");
            while (rst.Read())
            {
                result = rst["date"].ToString().Substring(0, 9);
                xmlStr.AppendFormat("<category label='{0}'/>", result);
            }
            xmlStr.AppendFormat("</categories>");
            int tempi = rst.FieldCount - 3;
            int i = 1;
            while (i <= tempi)
            {
                f[i] = rst.GetName(i + 1);
                i++;
            }
            rst.Close();
            rst.Close();

            Debug.WriteLine(well);
            Debug.WriteLine(nome);
            if ((well != null) && (well != "")) { }
            else
            {
                well = "phe38-B1";
            }
            i = 1;
            double yvon = 1;
            while (i <= 1)
            {
                SqlDataReader rst2 = query.ExecuteReader();
                string wellName2 = well;
                xmlStr.AppendFormat("<dataset seriesname='{0}' showValues='0' anchorRadius='7'>", nome + " " + well);
                if ((well != null) && (well != ""))
                {
                    string wellName = well.Remove(0, 3);
                    wellName2 = "phe" + wellName;
                    Debug.WriteLine(wellName2);
                }
                while (rst2.Read())
                {
                    if (nome == "Gas") { yvon = Convert.ToDouble(rst2[wellName2]) / 1000; }
                    else { yvon = Convert.ToDouble(rst2[wellName2]); }
                    xmlStr.AppendFormat("<set value='{0}'/>", yvon.ToString());
                }

                xmlStr.AppendFormat("</dataset>");
                i++;
                rst2.Close();
            }

           /* SqlDataReader rst4 = query.ExecuteReader();

            xmlStr.AppendFormat("<dataset seriesname='{0}' renderas='{1}' showvalues='{2}'>", "total", "line", "0");

            while (rst4.Read())
            {
                xmlStr.AppendFormat("<set value='{0}' link='../Home/Index'/>", rst4["total"].ToString());
            }

            xmlStr.AppendFormat("</dataset>");
            rst4.Close();*/




            // End the XML string

            xmlStr.Append("</chart>");


            // Close the result set Reader object and the Connection object

            conn.Close();

            // This page should return only XML content

            Response.ContentType = "text/xml";
            Response.Write(xmlStr.ToString());
        }
    }
}