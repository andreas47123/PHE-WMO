using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

// Use the `FusionCharts.Charts` namespace to be able to use classes and methods required to create charts.
using FusionCharts.Charts;

public partial class BasicExample_BasicChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // This page demonstrates the ease of generating charts using FusionCharts.
        // For this chart, we've used a pre-defined Data.json (contained in /Data/ folder)
        // Ideally, you would NOT use a physical data file. Instead you'll have
        // your own ASP.NET scripts virtually relay the JSON / XML data document.
        // For a head-start, we've kept this example very simple.
        //Initialize the chart.
        Chart sales = new Chart("column3d", "myChart", "600", "350", "jsonurl", "~/Data/Data.json");
        // Render the chart
        Debug.WriteLine(sales.GetType());
        Literal Literal1 = (Literal)FindControl("Literal1");
        Literal1.Text = sales.Render();
    }
}