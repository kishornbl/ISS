<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ACCEP_TRANSACTION_MODI.aspx.cs" Inherits="ACCEP_TRANSACTION_MODI" %>

<asp:Content ID="Header" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript" language="javascript">
    function intOnly(i) {
        if (i.value.length > 0) {
            i.value = i.value.replace(/[^\d.]+/g, '');
        }
    }
    function Confirm() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Do you want to Authorize data?")) {
            confirm_value.value = "Yes";
        } else {
            confirm_value.value = "No";
        }
        document.forms[0].appendChild(confirm_value);
    }   
 </script>
    <style type="text/css">
        .style1
        {
            height: 26px;
        }
        input[type=text]:focus, textarea:focus
        {
            background-color: #B3DDEB;            
        }
    </style>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="MainContent" Runat="Server">
    <center>
 <table style="height: 75px">
 <tr>
 <th class="style1">
 <fieldset  style="width: 600px" >
  <legend>Acceptance Data Modification
      
     </legend>

  <table  border="0" >
  <tr>
    <th class="style2" align = "left">As On Date </th>
    <th class="style1">
     <asp:TextBox ID="txtAsonDate" runat="server" 
           width="200px" MaxLength="15" ReadOnly="True" ></asp:TextBox> </th>
      </th>      
    </tr>
    <tr>
    <th class="style2" align = "left">Bank Name
    
    <th class="style1">
    <asp:DropDownList ID ="drpBankName" runat="Server" style="margin-left: 0px" 
            width="205px" Height="22px" AutopostBack ="true" 
            onselectedindexchanged="drpBankName_SelectedIndexChanged"></asp:DropDownList>
      </th>      
      <th class="style4">
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCategory" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="drpBankName"
                runat="server" />
    </th>
    </tr>             
    
     <tr> 
    <th class="style1" align = "left"><asp:Label ID="lblAmount" runat="server" Text="Acceptance Issued Amount"></asp:Label></th>
   <th class="style1"> 
       <asp:TextBox ID="txtAcceptanceIssued" runat="server" 
           width="200px" MaxLength="15"  onKeyUp="intOnly(this);"></asp:TextBox> </th>
    
    <%--<th class="style1">

            <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmount" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtAcceptanceIssued"
                runat="server" />
    </th>--%>

    </tr>
    <tr> 
    <th class="style1" align = "left"><asp:Label ID="lblAccepMatAmnt" runat="server" Text="Acceptance Matured Amount"></asp:Label></th>
   <th class="style1"> 
       <asp:TextBox ID="txtAcceptanceMatured" runat="server" 
           width="200px" MaxLength="15"  onKeyUp="intOnly(this);"></asp:TextBox> </th>
    
   <%-- <th class="style1">

            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtAcceptanceMatured"
                runat="server" />
    </th>--%>

    </tr>
    <tr> 
    <th class="style1" align = "left">
    <asp:Label ID="lblValueRecAccep" runat="server" Text="Value of Received Acceptance"></asp:Label></th>
   <th class="style1"> 
       <asp:TextBox ID="txtReceivedAcceptance" runat="server" 
           width="200px" MaxLength="15"  onKeyUp="intOnly(this);"></asp:TextBox> </th>
    
    <%--<th class="style1">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtReceivedAcceptance"
                runat="server" />
    </th>--%>

    </tr>
    <tr> 
    <th class="style1" align = "left"><asp:Label ID="lblPurAmntOfRecAcc" runat="server" Text="Purchased Amount Of Received Acceptance"></asp:Label></th>
   <th class="style1"> 
       <asp:TextBox ID="txtPurchaseRecAccep" runat="server" 
           width="200px" MaxLength="15"  onKeyUp="intOnly(this);"></asp:TextBox> </th>    
   <%-- <th class="style1">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtPurchaseRecAccep"
                runat="server" />
    </th>--%>

    </tr>
    <tr> 
    <th class="style1" align = "left"><asp:Label ID="lblMatofRecAcc" runat="server" Text="Matured of Received Acceptance"></asp:Label></th>
   <th class="style1"> 
       <asp:TextBox ID="txtMatofRecAccep" runat="server" 
           width="200px" MaxLength="15"  onKeyUp="intOnly(this);"></asp:TextBox> </th>
    
   <%-- <th class="style1">

            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtMatofRecAccep"
                runat="server" />
    </th>--%>

    </tr>
 <tr>
 
 <th>
 </th>


 <th>
 <asp:Button  ID ="btnSave" ValidationGroup="v" Text="Update" runat="Server" 
         Width="100px" onclick="btnSave_Click"
          > </asp:Button>
     
    <asp:Button  ID ="btnClear" ValidationGroup="v" Text="Clear" runat="Server" 
         Width="100px" onclick="btnClear_Click"
          > </asp:Button>
   
 </th>
 
 </tr>

    </table> 

 </fieldset>
</th>

</tr>
</table>
<table ><tr><td colspan="3">
       
            <ContentTemplate>
                <asp:GridView ID="gvAcceptanceTransaction" runat="server" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" AutoPostBack="false" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" DataKeyNames="ACCEP_SL" GridLines="Horizontal" 
                PageSize="10" 
                onselectedindexchanged="gvAcceptanceTransaction_SelectedIndexChanged" 
                onpageindexchanging="gvAcceptanceTransaction_PageIndexChanging" >
                    <PagerSettings FirstPageText="First" 
                        LastPageText="Last" Mode="NextPreviousFirstLast" 
                        NextPageText="Next" PageButtonCount="6" 
                        PreviousPageText="Previous" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="ACCEP_SL" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="ACCEP_SL" runat="server" 
                                    Text='<%#Bind("ACCEP_SL") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Bank Id" Visible = "false">
                            <ItemTemplate>
                                <asp:Label ID="BankId" runat="server" Text='<%#Bind("BANK_ID") %>'></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField>
                         <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Bank Name" Visible = "true">
                            <ItemTemplate>
                                <asp:Label ID="BankName" runat="server" Text='<%#Bind("BANK_NAME") %>'></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Acceptance Iss Amnt">
                            <ItemTemplate>
                                <asp:Label ID="AccIssueAmnt" runat="server" Text='<%#Bind("ACCEPTANCE_ISSUED_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Acceptance Mat Amnt">
                            <ItemTemplate>
                                <asp:Label ID="AccMatAmnt" runat="server" Text='<%#Bind("ACCEPTANCE_MATURED_AMOUNT") %>' ></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Acceptance Rec Amnt" >
                            <ItemTemplate>
                                <asp:Label ID="AccRecAmnt" runat="server" Text='<%#Bind("ACCEPTANCE_RECEIVED_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Acc Pur Amnt">
                            <ItemTemplate>
                                <asp:Label ID="AccPurAmnt" runat="server" Text='<%#Bind("ACCEPTANCE_PURCHASED_AMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Acc Rec Mat Amnt">
                            <ItemTemplate>
                                <asp:Label ID="AccRecMatAmnt" runat="server" Text='<%#Bind("ACCEPTANCE_REC_MATURED_AMOUNT") %>'></asp:Label>
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
        
        </td></tr></table>
      <table style="height:auto">
       <th>
        <asp:Button  ID ="btn_Authorize" Text="Authorize" runat="Server" 
         Width="130px" onclick="btn_Authorize_Click" OnClientClick = "Confirm()"> </asp:Button>
     
        <asp:Button  ID ="btn_Clear_Data" Text="Clear" runat="Server" 
         Width="130px" onclick="btn_Clear_Data_Click"> </asp:Button>
       </th>
     </table>  
  </center>
  <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
</asp:Content>

