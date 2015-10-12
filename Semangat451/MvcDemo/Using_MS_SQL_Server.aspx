<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Using_MS_SQL_Server.aspx.cs" Inherits="Using_MS_SQL_Server" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<script type="text/javascript" src="../fusioncharts/fusioncharts.js"></script>
    <title>Using MS SQL Server data to create JavaScript charts with FusionCharts XT</title>

    <script runat="server">
    protected void Button1_Click(object sender, System.EventArgs e)
    {
        label1.Text = dor.SelectedItem.ToString();
        label2.Text = dor2.SelectedItem.ToString();
        
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        
            <asp:Label ID="label1" runat="server" Font-Size="Large" ForeColor="Crimson" Text="Gas"></asp:Label>
             <asp:Label ID="label2" runat="server" Font-Size="Large" ForeColor="Crimson" Text="phe38-B1"></asp:Label>
        <asp:RadioButtonList ID="dor" runat="server">
            <asp:ListItem>Gas</asp:ListItem>
            <asp:ListItem>Oil</asp:ListItem>
            <asp:ListItem>Water</asp:ListItem>
        </asp:RadioButtonList>

             <asp:RadioButtonList ID="dor2" runat="server">
            <asp:ListItem>phe38-B1</asp:ListItem>
            <asp:ListItem>phe38-B2</asp:ListItem>
        </asp:RadioButtonList>

            <asp:Calendar ID="cal1" runat="server"></asp:Calendar>

            <br/>
             <asp:Button ID="Button1" runat="server" Text="Click Me Senpai!" OnClick="Button1_Click" />
            <br/><br/>
            <asp:Literal ID="chart_from_db" runat="server">        
        </asp:Literal>
            </div>
    </form>
    
       
</body>
</html>
