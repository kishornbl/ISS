<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TimeExtension.aspx.cs" Inherits="TimeExtension" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" Runat="Server">

<table width="auto" align="center">
    <tr>
    <td>
    <fieldset>
    <table>
    <tr>
    <td>  </td>
    <td>  
       <strong><p style=" color : Green; text-align : center;">Software Time Extension</p></strong>
    </td>
    </tr>
    
    
    <tr>
    <td ><label style = "">Current Month:</label></td>
    <td class="style1">
        <asp:TextBox ID="txtCurrentMonth" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
    </td>
    </tr>
        
    <tr>
    <td ><label>Extend Date Upto:</label></td>
    <td class="style1">
         <asp:DropDownList ID="drpSelectDate" runat="server" AutoPostBack="false" Width="205px">   
               <asp:ListItem> 10 </asp:ListItem>        
               <asp:ListItem> 11 </asp:ListItem>        
               <asp:ListItem> 12 </asp:ListItem>        
               <asp:ListItem> 13 </asp:ListItem>        
               <asp:ListItem> 14 </asp:ListItem>        
               <asp:ListItem> 15 </asp:ListItem>        
               <asp:ListItem> 16 </asp:ListItem>        
               <asp:ListItem> 17 </asp:ListItem>        
               <asp:ListItem> 18 </asp:ListItem>        
               <asp:ListItem> 19 </asp:ListItem>        
               <asp:ListItem> 20 </asp:ListItem>        
               <asp:ListItem> 21 </asp:ListItem>        
               <asp:ListItem> 22 </asp:ListItem>        
               <asp:ListItem> 23 </asp:ListItem>        
               <asp:ListItem> 24 </asp:ListItem>        
               <asp:ListItem> 25 </asp:ListItem>        
         </asp:DropDownList>
    </td>
    </tr>
    
          
    <tr>
    <td>  </td>
    <td colspan="2" style = " text-align : center;">
        <asp:Button ID="btnUpdate" CssClass="menu" runat="server" Text="Update" 
           Width="150px" ValidationGroup="s" onclick="btnUpdate_Click"/>
     </td>
     </tr>
       
    </table>
       
    </fieldset> 
    
     </td>
    </tr>
   
    </table>

</asp:Content>

