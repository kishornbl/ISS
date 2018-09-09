<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Header" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 41px;
        }
        .menu
        {}
    </style>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" runat="server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <table width="630px" align="center">
    <tr>
    <td>
    <fieldset>
    <table>
    <tr>
    <%--<td> 
    <cc1:CalendarExtender ID="fromDateCalendar" PopupButtonID="imgPopup" runat="server" TargetControlID="fromDateTextBox"
        Format="dd/MM/yyyy">
    </cc1:CalendarExtender></td>--%>
    <td ><label>Date</label></td>
   
    <td>
      <asp:TextBox ID="toDateTextBox" runat="server" Height="20px" 
            Width="195px" />
        </td>
         <td class="style1">
    <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImageButton1" runat="server" TargetControlID="toDateTextBox"
         Format="MMM-yyyy" DefaultView="Months">        
    </cc1:CalendarExtender>
             <asp:ImageButton ID="ImageButton1" ImageUrl="image/calendar.png" ImageAlign="Bottom"
        runat="server" Height="16px" Width="21px" />
        </td><td>
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="MM-yyyy"></asp:Label></td>
    </tr>
     <tr>
     <td >Select Branch</td>
     <td colspan="2">
         <asp:DropDownList ID="branchDropDownList" runat="server" AutoPostBack="True" 
             Width="200px" 
             onselectedindexchanged="branchDropDownList_SelectedIndexChanged">           
         </asp:DropDownList>
        </td>
     </tr>
      <tr>
      <td>   </td>
        <td colspan = "2">
            <asp:RadioButton ID="MonitoringRadioButton" GroupName="g" Text="Branch Wise Monitoring Report" runat="server" Checked="true" />
        </td>
            <td> </td>
        </tr>
        <tr><td>
        
        </td>
        <td colspan = "2">
            <asp:RadioButton ID="HOMonitoringReport" GroupName="g" Text="HO Wise Monitoring Report" runat="server" Checked="false" />
        </td>
            <td>
          
        </td>
        </tr>
        <tr><td>
        
        </td>
        <td colspan="2">
             <asp:RadioButton ID="rdbtnAcceptance" GroupName="g" Text="Acceptance Report" runat="server" />
            </td>
            <td>
          
        </td>
        </tr>
    <tr><td>
        
        </td>
        <td colspan="2">
        <asp:Button ID="btnReport" CssClass="menu" runat="server" Text="Export to Excel" 
                Width="203px" onclick="btnReport_Click" />
            </td>
            <td>
          
        </td>
        </tr>
         <tr><td>
        
        </td>
        <td colspan="2">
        <asp:Button ID="btnCSV" CssClass="menu" runat="server" Text="Export to CSV" 
                Width="203px" onclick="btnCSV_Click" />            
            <br />
            <%--<asp:Label ID="value" runat="server" Text="value"></asp:Label>--%>
            </td>
            <td>
          
        </td>
        </tr>
    </table>
    </fieldset> 
    
     </td>
    </tr>
   
    </table>

</asp:Content>

