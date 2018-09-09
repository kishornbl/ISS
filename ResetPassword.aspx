<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="ResetPassword" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .menu
        {}
    </style>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" Runat="Server">
    <table width="auto" align="center">
    <tr>
    <td>
    <fieldset>
    <strong><p style=" color : Green; text-align :center;">Withdraw Authorization</p></strong>
    <table>
    <tr>
    <td ><label>Select Branch:</label></td>
    <td class="style1">
        <asp:DropDownList ID="branchDropDownList" runat="server" 
            AutoPostBack="true" Width="200px" 
            onselectedindexchanged="branchDropDownList_SelectedIndexChanged">           
         </asp:DropDownList>       
        </td>
    </tr>
    <tr>
    <td ><label>Select User:</label></td>
    <td class="style1">
        <asp:DropDownList ID="UserDropDownList" runat="server" 
            AutoPostBack="true" Width="200px">           
         </asp:DropDownList>       
        </td>
    </tr>
    <tr>
    <td> Type : </td>
    <td class="style1">
        <asp:DropDownList ID="trType" runat="server" 
            AutoPostBack="false" Width="200px">   
            <asp:ListItem> MONITORING </asp:ListItem>        
            <asp:ListItem> ACCEPTANCE </asp:ListItem>        
         </asp:DropDownList>       
    </td>
     </tr>
          
    <tr>
    <td>
        
    </td>
    <td colspan="2" style = " text-align : right;">
    <asp:Button ID="btnDeleteRecord" CssClass="menu" runat="server" Text="Withdraw Autho" 
           Width="150px" ValidationGroup="s" onclick="btnDeleteRecord_Click"/>

    <asp:Button ID="btnReset" CssClass="menu" runat="server" Text="Reset Password" 
           Width="150px" ValidationGroup="s" onclick="btnReset_Click" Visible="False"/>
     </td>
     </tr>
       
    </table>
       
    </fieldset> 
    
     </td>
    </tr>
   
    </table>

</asp:Content>

