<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DataFromGL.aspx.cs" Inherits="DataFromGL" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
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
    <tr>
    <td ><label>Data Upload Date</label></td>   
    <td>
      <asp:TextBox ID="txtUpload" runat="server" Height="20px" 
            Width="195px" ReadOnly="False" />
        </td>
       <td>
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="yyyyMMdd"></asp:Label></td>
        </tr>
        <tr>
        <td ><label>Last Date of Month</label></td>
   
    <td>
      <asp:TextBox ID="toDateTextBox" runat="server" Height="20px" 
            Width="195px" ReadOnly="True" />
        </td>
         <td class="style1">
    <cc1:CalendarExtender ID="CalendarExtender1" PopupButtonID="ImageButton1" runat="server" TargetControlID="toDateTextBox"
         Format="yyyyMMdd" DefaultView="Months">        
    </cc1:CalendarExtender>
             <asp:ImageButton ID="ImageButton1" ImageUrl="image/calendar.png" ImageAlign="Bottom"
        runat="server" Height="16px" Width="21px" Visible="False" />
        </td>        
      </tr>
    </tr>
    
    <tr><td>
        
        </td>
        <td colspan="2">
        <asp:Button ID="btnImport" CssClass="menu" runat="server" Text="Import Data" 
                Width="203px" onclick="btnImport_Click"  />
            </td>
            <td>
          
        </td>
        </tr>
       <tr>
       <td colspan="4">
           &nbsp;</td>
       </tr>
    </table>
    </fieldset> 
    
     </td>
    </tr>
   
    </table>

</asp:Content>

