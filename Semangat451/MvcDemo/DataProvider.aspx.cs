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

public partial class DataProvider : System.Web.UI.Page
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

        string field = Request.QueryString["field"];
        //string field = "phe38";

        string tab = nome;

        string between = "";

        if ((dateFrom != "") && (dateFrom != null) && (dateTo != "") && (dateTo != null))
        {
            between = " WHERE date BETWEEN '" + dateFrom + "' AND '" + dateTo + "'";
        }
        Debug.WriteLine(nome);

        // Construct the connection string to interface with the SQL Server Database
        string connStr = ConfigurationManager.ConnectionStrings["DummyDBContext"].ConnectionString;

        // Initialize the string which would contain the chart data in XML format
        StringBuilder xmlStr = new StringBuilder();


        // <chart caption="Product-wise quarterly revenue Vs profit in last year" subcaption="Harry&#39;s SuperMart" xaxisname="Quarter" yaxisname="Revenue (In USD)" numberprefix="$" palettecolors="#0075c2,#1aaf5d,#f2c500" bgcolor="#ffffff" borderalpha="20" showcanvasborder="0" useplotgradientcolor="0" plotborderalpha="10" legendborderalpha="0" legendshadow="0" legendbgalpha="0" valuefontcolor="#ffffff" showxaxisline="1" xaxislinecolor="#999999" divlinecolor="#999999" divlinedashed="1" showalternatehgridcolor="0" subcaptionfontbold="0" subcaptionfontsize="14" showhovereffect="1">
        // Create a SQLConnection object 
        string satuanGas = "MMSCFD";
        string satuanOil = "BOPD";
        string satuanWater = "BWPD";
        string satuan = "";

        if (nome == "Gas") { satuan = satuanGas; }
        else if (nome == "Oil") { satuan = satuanOil; }
        else if (nome == "Water") { satuan = satuanWater; }

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            if ((nome == null) || (nome == ""))
            {
                xmlStr.Append("<chart caption='Oil Production Field' subcaption='' xaxisname='Date' yaxisname='" + satuan + "' numberprefix='' palettecolors='#AB4743,#89A54C,#71598F,#4298AE,#E74C3C,#DA843B,#663399,#93A9CF,#674172' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1'>");
                tab = "Oil";
            }
            else
            {
                // Provide the relevant customization attributes to the chart
                xmlStr.Append("<chart caption='" + nome + " Production Field' subcaption='' xaxisname='Date' yaxisname='" + satuan + "' numberprefix='' palettecolors='#AB4743,#89A54C,#71598F,#4298AE,#E74C3C,#DA843B,#663399,#93A9CF,#674172' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1' xAxisNameFontSize='20' yAxisNameFontSize='20' valueFontSize='16' legendItemFontSize='16'>");

            }

            // Establish the connection with the database
            conn.Open();
            string result;
            //Buat Kategori
            string command = "SELECT * FROM Gas_Total_Production_Field";
            if ((between != "") && (between != null))
            {
                command += between;
            }
            command += " ORDER BY date";
            Debug.WriteLine(between);

            SqlCommand query = new SqlCommand(command, conn);
            if ((nome != null) && (nome != ""))
            {
                if ((field != null) && (field != ""))
                {
                    nome += "_" + field;
                }
                else
                {
                    if (nome == "Gas")
                    {
                        nome += "_Total";
                    }
                    nome += "_Production_Field";
                }
                Debug.WriteLine(nome);
                string command2 = "SELECT * FROM " + nome;
                if ((between != "") && (between != null))
                {
                    command2 += between;
                }
                command2 += " ORDER BY date";
                Debug.WriteLine(command2);
                query = new SqlCommand(command2, conn);
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

            i = 1;
            double yvon = 1;
            while (i <= tempi)
            {
                SqlDataReader rst2 = query.ExecuteReader();

                xmlStr.AppendFormat("<dataset seriesname='{0}' showValues='0'>", f[i]);

                while (rst2.Read())
                {

                    if (tab == "Gas") { yvon = Convert.ToDouble(rst2[f[i]]) / 1000; }
                    else { yvon = Convert.ToDouble(rst2[f[i]]); }
                    xmlStr.AppendFormat("<set value='{0}' link='../Home/Index?field={1}&tab={2}' />", yvon.ToString(), f[i], tab);
                }

                xmlStr.AppendFormat("</dataset>");
                i++;
                rst2.Close();
            }

            SqlDataReader rst4 = query.ExecuteReader();

            xmlStr.AppendFormat("<dataset seriesname='{0}' renderas='{1}' showvalues='{2}' anchorRadius='7'>", "total", "line", "0");

            double yvonia = 1;
            while (rst4.Read())
            {
                if (tab == "Gas") { yvonia = Convert.ToDouble(rst4["total"]) / 1000; }
                else { yvonia = Convert.ToDouble(rst4["total"]); }
                xmlStr.AppendFormat("<set value='{0}'/>", yvonia.ToString());
            }

            xmlStr.AppendFormat("</dataset>");
            rst4.Close();




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