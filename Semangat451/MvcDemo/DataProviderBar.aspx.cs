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

public partial class DataProviderBar : System.Web.UI.Page
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
        string field = Request.QueryString["field"];

        /*string dateFrom = "07/29/2015";
        string dateTo = "07/31/2015";
        string nome = "Field";
        string column = "gas_to_orf";
        string satuan = "Gas";
        string field = "phe23";*/

        string tab = satuan;

        nome = "tbl_" + nome;

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
            
            // Establish the connection with the database
            conn.Open();
            string result;
            //Buat Kategori
            string command = "SELECT " + nome + "_Data.field_id, field_name, date, " + column + " FROM " + nome + "_ID," + nome + "_Data WHERE " + nome + "_Data.field_id = " + nome + "_ID.field_id AND field_name = '" + field + "'";
            if ((between != "") && (between != null))
            {
                command += " AND" + between;
            }
            command += " ORDER BY field_id, date";
            Debug.WriteLine(command);

            SqlCommand query = new SqlCommand(command, conn);
            if ((nome != null) && (nome != ""))
            {
                Debug.WriteLine(nome);
                string command2 = "SELECT " + nome + "_Data.field_id, field_name, date, " + column + " FROM " + nome + "_ID," + nome + "_Data WHERE " + nome + "_Data.field_id = " + nome + "_ID.field_id AND field_name = '" + field + "'";
                if ((between != "") && (between != null))
                {
                    command2 += " AND" + between;
                }
                command2 += " ORDER BY field_id, date";
                Debug.WriteLine(command2);
                query = new SqlCommand(command2, conn);
            }

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
            int color = 0;
            //#AB4743,#89A54C,#71598F,#4298AE,#E74C3C,#DA843B,#663399,#93A9CF,#674172
            
            while (rst2.Read())
            {
                if (color == 0)
                {
                    color = Convert.ToInt32(rst2[f[1]]);
                    color = color % 9;
                    Debug.WriteLine(color);
                    string palet = "";
                    switch (color)
                    {
                        case 1:
                            palet = "#AB4743";
                            break;
                        case 2:
                            palet = "#89A54C";
                            break;
                        case 3:
                            palet = "#71598F";
                            break;
                        case 4:
                            palet = "#4298AE";
                            break;
                        case 5:
                            palet = "#E74C3C";
                            break;
                        case 6:
                            palet = "#DA843B";
                            break;
                        case 7:
                            palet = "#663399";
                            break;
                        case 8:
                            palet = "#93A9CF";
                            break;
                        case 9:
                            palet = "#674172";
                            break;
                        default:
                            palet = "#AB4743";
                            break;
                    }
                    if ((nome == null) || (nome == ""))
                    {
                        xmlStr.Append("<chart caption='' subcaption='' xaxisname='' yaxisname='" + satuan + "' numberprefix='' palettecolors='" + palet + "' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1' xAxisNameFontSize='20' yAxisNameFontSize='20' valueFontSize='16' legendItemFontSize='16'>");
                        tab = "Oil";
                        color = 1;
                    }
                    else
                    {
                        // Provide the relevant customization attributes to the chart
                        xmlStr.Append("<chart caption='' subcaption='' xaxisname='' yaxisname='" + satuan + "' numberprefix='' palettecolors='" + palet + "' bgcolor='#ffffff' borderalpha='20' showcanvasborder='1' useplotgradientcolor='0' plotborderalpha='10' legendborderalpha='0' legendshadow='0' legendbgalpha='0' valuefontcolor='#ffffff' showxaxisline='1' showYAxisValues='1' xaxislinecolor='#999999' divlinecolor='#999999' divlinedashed='1' showalternatehgridcolor='0' subcaptionfontbold='0' subcaptionfontsize='14' showhovereffect='1' xAxisNameFontSize='20' yAxisNameFontSize='20' valueFontSize='16' legendItemFontSize='16'>");
                        color = 1;
                    }
                }
                xmlStr.AppendFormat("<set label='{0}' value='{1}' link='../Home/Index?field={2}' />", rst2[f[3]], rst2[f[4]], f[2]);
            }
            xmlStr.AppendFormat("</chart>");
            rst2.Close();

            
            // Close the result set Reader object and the Connection object

            conn.Close();

            // This page should return only XML content

            Response.ContentType = "text/xml";
            Response.Write(xmlStr.ToString());
        }
    }
}