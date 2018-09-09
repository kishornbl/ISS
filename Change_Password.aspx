<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Change_Password.aspx.cs" Inherits="Change_Password" %>

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
    <legend>Change Password</legend>
    <table>
    <tr>
    <td ><label>Current Password:</label></td>
    <td class="style1">
      <asp:TextBox ID="txtCurrentPassword" runat="server" Width="127px"  TextMode="password"/>
   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="s" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtCurrentPassword" runat="server" />                
    </td>      
    </tr>

    <tr>
    <td ><label>New Password:</label></td>
    <td class="style1">
      <asp:TextBox ID="txtNewPassword" runat="server" Width="127px"  TextMode="password"/>
   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="s" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtNewPassword" runat="server" />                
    </td>      
    </tr>

    <tr>
    <td ><label>Confirm Password:</label></td>
    <td class="style1">
      <asp:TextBox ID="txtConfirmPassword" runat="server" Width="127px"  TextMode="password"/>
   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="s" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtConfirmPassword" runat="server" />                
    </td>      
    </tr>
       
       <tr>
       <td></td>
        <td colspan="2">
        <asp:Button ID="btnChangePassword" CssClass="menu" runat="server" Text="Change Password" 
           Width="130px" ValidationGroup="s" onclick="btnChangePassword_Click" />
        </td>
        </tr>
  </table>
    </fieldset> 
    
     </td>
    </tr>
    </table>
    </asp:Content>

