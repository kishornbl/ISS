<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserModification.aspx.cs" Inherits="UserModification" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .menu
        {}
        .style1
        {
            width: 308px;
        }
        .style2
        {
            width: 688px;
        }
    </style>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" Runat="Server">
    <table width= "500px" align="center" style="margin-right: 39px">
    <tr>
    <td class="style2">
    <fieldset>
    <legend>User Modification</legend>
    <table style="width: 721px">
    <tr>
    <td ><label>Select Branch:</label></td>
    <td class="style1">
        <asp:DropDownList ID="branchDropDownList" runat="server" 
            AutoPostBack="true" Width="200px">           
         </asp:DropDownList>       
        </td>
    </tr>
    <tr>
    <td ><label>Division:</label></td>
    <td class="style1"> <asp:TextBox ID="txtDivision" runat="server"  Width="200px" ReadOnly="True"  /> </td>    
    </tr>
    <tr>
    <td ><label>User ID:</label></td>
    <td class="style1">
      <asp:TextBox ID="txtUserId" runat="server"  
            Width="200px" ReadOnly="True"  />
   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="s" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtUserId"
                runat="server" />                

        </td>      
    </tr>
       <tr>
    <td >User
        <label>Name:</label></td>
    <td class="style1">
      <asp:TextBox ID="txtUserName" runat="server"  
            Width="200px"  />
   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="s" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtUserName"
                runat="server" />
                
        </td>
    </tr>   
    <tr>
    <td >
        <label>Designation:</label></td>
    <td class="style1">
      <asp:TextBox ID="txtDesignation" runat="server"  
            Width="197px"  />
   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="s" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtDesignation"
                runat="server" />
                
        </td>
    </tr>   
    <tr>
    <td ><label>Active:</label></td>
    <td class="style1">
        <asp:CheckBox ID="chkBoxActive" Checked="true" runat="server" />
        
        </td>
    </tr> 
      
    <tr><td>
        
        </td>
        <td class="style1">
        <asp:Button ID="btnUpdate" CssClass="menu" runat="server" Text="Update" 
           Width="200px" ValidationGroup="s" onclick="btnUpdate_Click"/>
            </td>
            <td>
        <label>Branch:</label>
        <asp:TextBox ID="txtSearch" runat="server" Width="120px"></asp:TextBox>
        <asp:Button ID="btnSearch" CssClass="menu" runat="server" Text="Search" 
           Width="100px" onclick="btnSearch_Click"/>
        </td>
        </tr>
        
    </table>
     <table style="width: 110%"><tr><td >
       <ContentTemplate>
            <asp:GridView ID="gvUserTransaction" runat="server" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" AutoPostBack="false" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" DataKeyNames="USER_SL" GridLines="Horizontal" 
                PageSize="20" onpageindexchanging="gvUserTransaction_PageIndexChanging" 
             onselectedindexchanged="gvUserTransaction_SelectedIndexChanged" 
             Width="684px" >
                    <PagerSettings FirstPageText="First" 
                        LastPageText="Last" Mode="NextPreviousFirstLast" 
                        NextPageText="Next" PageButtonCount="6" 
                        PreviousPageText="Previous" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="Tran_Serial" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="USER_SL" runat="server" 
                                    Text='<%#Bind("USER_SL") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="User Id">
                            <ItemTemplate>
                                <asp:Label ID="UserId" runat="server" Text='<%#Bind("USER_ID") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="User Name" Visible = "true">
                            <ItemTemplate>
                                <asp:Label ID="UserName" runat="server" Text='<%#Bind("USER_NAME") %>' ></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField>
                          <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Designation" Visible = "true">
                            <ItemTemplate>
                                <asp:Label ID="Designation" runat="server" Text='<%#Bind("DESIGNATION") %>' ></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Branch Code" Visible = "false">
                            <ItemTemplate>
                                <asp:Label ID="BranchCode" runat="server" Text='<%#Bind("BRANCH_CODE") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Branch Name">
                            <ItemTemplate>
                                <asp:Label ID="BranchName" runat="server" Text='<%#Bind("BRANCH_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Division">
                            <ItemTemplate>
                                <asp:Label ID="Division" runat="server" Text='<%#Bind("DIVISION") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="User Activity">
                            <ItemTemplate>
                                <asp:Label ID="Activity" runat="server" Text='<%#Bind("IS_ACTIVE") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277" />
                </asp:GridView>
          </ContentTemplate>
             </td></tr>
            
             </table>
       
    </fieldset> 
    
     </td>
    </tr>
   
    </table>

</asp:Content>

