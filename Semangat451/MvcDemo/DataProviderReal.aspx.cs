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

public partial class DataProviderReal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //biar gampang sih ini
        string[] f = new string[50];

        string dateFrom = Request.QueryString["from"];
        string dateTo = Request.QueryString["to"];
        string nome = Request.QueryString["table"];
        string column = Request.QueryString["column"];
        string satuan = Request.QueryString["satuan"];

        /*string dateFrom = "";
        string dateTo = "";
        string nome = "Field";
        string column = "gas_to_orf";
        string satuan = "Gas";*/

        string tab = satuan;

        string between = "";

        if ((dateFrom != "") && (dateFrom != null) && (dateTo != "") && (dateTo != null))
        {
            between = " date BETWEEN '" + dateFrom + "' AND '" + dateTo + "'";
        }
        Debug.WriteLine(nome);

        // Construct the connection string to interface with the SQL Server Database
        string connStr = ConfigurationManager.ConnectionStrings["RealDBContext"].ConnectionString;

        // Initialize the string which would contain the chart data in XML format
        StringBuilder xmlStr = new StringBuilder();
        
        // <chart caption="Product-wise quarterly revenue Vs profit in last year" subcaption="Harry&#39;s SuperMart" xaxisname="Quarter" yaxisname="Revenue (In USD)" numberprefix="$" palettecolors="#0075c2,#1aaf5d,#f2c500" bgcolor="#ffffff" borderalpha="20" showcanvasborder="0" useplotgradientcolor="0" plotborderalpha="10" legendborderalpha="0" legendshadow="0" legendbgalpha="0" valuefontcolor="#ffffff" showxaxisline="1" xaxislinecolor="#999999" divlinecolor="#999999" divlinedashed="1" showalternatehgridcolor="0" subcaptionfontbold="0" subcaptionfontsize="14" showhovereffect="1">
        // Create a SQLConnection object 
        string satuanGas = "MMSCFD";
        string satuanOil = "BOPD";
        string satuanWater = "BWPD";
        
        if (satuan == "Gas") { satuan = satuanGas; }
        else if (satuan == "Oil") { satuan = satuanOil; }
        else if (satuan == "Water") { satuan = satuanWater; }

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            if ((nome == null) || (nome == ""))
            {
                xmlStr.Append("<chart caption='' subcaption='' xaxisname='' yaxisname='" + satuan + "' numberprefix='' palettecolors='#AB4743,#89A54C,#71598F,#4298AE,#E74C3C,#DA843B,#663399,#93A9CF,#674172' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1' xAxisNameFontSize='20' yAxisNameFontSize='20' valueFontSize='16' legendItemFontSize='16'>");
                tab = "Oil";
            }
            else
            {
                // Provide the relevant customization attributes to the chart
                xmlStr.Append("<chart caption='' subcaption='' xaxisname='' yaxisname='" + satuan + "' numberprefix='' palettecolors='#AB4743,#89A54C,#71598F,#4298AE,#E74C3C,#DA843B,#663399,#93A9CF,#674172' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1' xAxisNameFontSize='20' yAxisNameFontSize='20' valueFontSize='16' legendItemFontSize='16'>");

            }

            // Establish the connection with the database
            conn.Open();
            string result;
            //Buat Kategori
            string command = "SELECT " + nome + ".field_id, field_name, date, " + column + " FROM Field_ID," + nome + " WHERE " + nome + ".field_id = Field_ID.field_id";
            if ((between != "") && (between != null))
            {
                command += between;
            }
            command += " ORDER BY field_id, date";
            Debug.WriteLine(command);

            SqlCommand query = new SqlCommand(command, conn);
            if ((nome != null) && (nome != ""))
            {
                Debug.WriteLine(nome);
                string command2 = "SELECT " + nome + ".field_id, field_name, date, " + column + " FROM Field_ID," + nome + " WHERE " + nome + ".field_id = Field_ID.field_id";
                if ((between != "") && (between != null))
                {
                    command2 += between;
                }
                command2 += " ORDER BY field_id, date";
                Debug.WriteLine(command2);
                query = new SqlCommand(command2, conn);
            }

            string commandDate = "SELECT date FROM " + nome + " GROUP BY date";
            Debug.WriteLine(commandDate);
            SqlCommand query2 = new SqlCommand(commandDate, conn);
            SqlDataReader rst = query2.ExecuteReader();
            xmlStr.AppendFormat("<categories>");
            while (rst.Read())
            {
                result = rst["date"].ToString().Substring(0, 9);
                xmlStr.AppendFormat("<category label='{0}'/>", result);
            }
            xmlStr.AppendFormat("</categories>");

            
            rst.Close();
            int i;
            i = 0;
            double yvon = 1;
            
            SqlDataReader rst2 = query.ExecuteReader();

            int tempi = rst2.FieldCount;
            int a = 1;
            while (a <= tempi)
            {
                f[a] = rst2.GetName(a - 1);
                a++;
            }
            while (rst2.Read())
            {
                
                if (Convert.ToInt32(rst2[f[1]]) != i)
                {
                    if (i != 0) { xmlStr.AppendFormat("</dataset>"); }
                    xmlStr.AppendFormat("<dataset seriesname='{0}' showValues='0'>", rst2[f[2]]);
                    i++;
                    
                }

                if (tab == "Gas")
                {
                    yvon = Convert.ToDouble(rst2[f[4]]) / 1000;
                } else
                {
                    yvon = Convert.ToDouble(rst2[f[4]]);
                }

                xmlStr.AppendFormat("<set value='{0}' link='../Home/Index?field={1}' />", yvon.ToString(), f[2]);

            }
            xmlStr.AppendFormat("</dataset>");
            rst2.Close();

            double tempTotal = 0;
            string commandTotal = "SELECT " + nome + ".field_id, field_name, date, " + column + " FROM Field_ID," + nome + " WHERE " + nome + ".field_id = Field_ID.field_id";
            if ((between != "") && (between != null))
            {
                commandTotal += between;
            }
            commandTotal += " ORDER BY date, field_id";
            Debug.WriteLine(commandTotal);
            SqlCommand query3 = new SqlCommand(commandTotal, conn);
            SqlDataReader rst3 = query3.ExecuteReader();


            xmlStr.AppendFormat("<dataset seriesname='{0}' renderas='{1}' showvalues='{2}' anchorRadius='7'>", "total", "line", "0");

            int yvonia = 0;
            DateTime tempdate = Convert.ToDateTime("01/01/2000");
            while (rst3.Read())
            {
                if (Convert.ToDateTime(rst3[f[3]]) != tempdate)
                {
                    if (yvonia != 0)
                    {
                        xmlStr.AppendFormat("<set value='{0}'/>", tempTotal.ToString());
                    }
                    yvonia++;
                    tempTotal = 0;
                }
                if (tab == "Gas")
                {
                    yvon = Convert.ToDouble(rst3[f[4]]) / 1000;
                }
                else
                {
                    yvon = Convert.ToDouble(rst3[f[4]]);
                }
                tempTotal += yvon;
                Debug.WriteLine(tempTotal);
                tempdate = Convert.ToDateTime(rst3[f[3]]);
            }
            rst3.Close();
            xmlStr.AppendFormat("<set value='{0}'/>", tempTotal.ToString());
            xmlStr.AppendFormat("</dataset>");
            
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