<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Transaction_Monitoring_Entry.aspx.cs" Inherits="Transaction_Monitoring_Entry" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript" language="javascript">

    function intOnly(i) {
        if (i.value.length > 0) {
            i.value = i.value.replace(-/[^\d.]+/g, '');
        }
    }

    function isNumber(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode == 45) {
            var inputValue = $("#inputfield").val()
            if (inputValue.indexOf('-') < 1) {
                return true;
            }
            return false;
        }

        if (charCode == 46) {
            var inputValue = $("#inputfield").val()
            if (inputValue.indexOf('.') < 1) {
                return true;
            }
            return false;
        }
        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
 </script>
 <style type="text/css">
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
 <fieldset  style="width: 400px" >
  <legend>Monitoring Entry 
      
     </legend>

  <table  border="0" >
  <tr>
    <th class="style2">As On Date</th>
    
    <th class="style1">
     <asp:TextBox ID="txtAsonDate" runat="server" 
           width="200px" MaxLength="15" ReadOnly="True" ></asp:TextBox> </th>
      </th>      
    </tr>
    <tr>
    <th class="style2">
        <asp:Label ID="lblCategory" runat="server" Text="Data Category"></asp:Label>
    </th>
    
    <th class="style1">
    <asp:DropDownList ID ="drpCategory" runat="Server" style="margin-left: 0px" 
            width="205px" Height="22px" AutopostBack ="true" 
            onselectedindexchanged="drpCategory_SelectedIndexChanged"></asp:DropDownList>
      </th>      
      <%--<th class="style4">
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCategory" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="drpCategory"
                runat="server" />
    </th>--%>
    </tr>             
    <%--<tr> 
    <th class="style1">COA Description</th>
   <th class="style1"> 
        <asp:DropDownList ID ="drpSubCategory" runat="Server" style="margin-left: 0px" 
            width="205px" Height="22px" 
            onselectedindexchanged="drpSubCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>          
           </th>
    
    <th class="style1">

            <asp:RequiredFieldValidator ID="RequiredFieldValidatorSubCategory" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="drpSubCategory"
                runat="server" />
    </th>
    </tr>--%>
    
     <%--<tr> 
    <th class="style2"><asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label></th>
   <th class="style1"> 
       <asp:TextBox ID="txtAmount" runat="server" 
           width="200px" MaxLength="15"  onKeyUp="intOnly(this);"></asp:TextBox> </th>
    
    <th class="style4">

            <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmount" ValidationGroup="v" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtAmount"
                runat="server" />
    </th>
    </tr>--%>
 <tr>
 
 <%--<th>
 </th>


 <th>
 <asp:Button  ID ="btnInsert" ValidationGroup="v" Text="Save" runat="Server" 
         Width="100px" 
          > </asp:Button>
     
    <asp:Button  ID ="btnClear" ValidationGroup="v" Text="Clear" runat="Server" 
         Width="100px" 
          > </asp:Button>
   
 </th>--%>
 
 </tr>

    </table> 

 </fieldset>
</th>

</tr>
</table>
<table ><tr><td colspan="3">
       
            <ContentTemplate>
                <asp:GridView ID="gvTransaction" runat="server" AllowPaging="false" 
                    AllowSorting="True" AutoGenerateColumns="False" AutoPostBack="false" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" 
                PageSize="100" onpageindexchanging="gvTransaction_PageIndexChanging">
                    <PagerSettings FirstPageText="First" 
                        LastPageText="Last" Mode="NextPreviousFirstLast" 
                        NextPageText="Next" PageButtonCount="6" 
                        PreviousPageText="Previous" />
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <Columns>
                       <%-- <asp:CommandField ShowSelectButton="True" Visible="false"/>--%>
                        <%--<asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                            HeaderText="Tran_Serial" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="TRAN_SL" runat="server" 
                                    Text='<%#Bind("TRAN_SL") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Category Id" Visible = "false">
                            <ItemTemplate>
                                <asp:Label ID="CategoryId" runat="server" Text='<%#Bind("CATEGORY_ID") %>'></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Data Category">
                            <ItemTemplate>
                                <asp:Label ID="CategoryName" runat="server" Text='<%#Bind("CATEGORY_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Subcategory Id" Visible = "false">
                            <ItemTemplate>
                                <asp:Label ID="SubCategoryId" runat="server" Text='<%#Bind("SUBCATEGORY_ID") %>' ></asp:Label>
                            </ItemTemplate>
                         </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="COA Description" >
                            <ItemTemplate>
                                <asp:Label ID="SubCategoryName" runat="server" Text='<%#Bind("SUBCATEGORY_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Amount">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAmount" runat="server" onkeydown = "return (event.keyCode!=13);" onkeypress="return isNumber(event)"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Figure Indocation" >
                            <ItemTemplate>
                                <asp:Label ID="FigureIndication" runat="server" Text='<%#Bind("FIGURE_IND") %>'></asp:Label>
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
        
<table style="height:auto">
 <th>
 <asp:Button  ID ="btnInsert" ValidationGroup="v" Text="Save" runat="Server" 
         Width="170px" onclick="btnInsert_Click" > </asp:Button>
     
    <asp:Button  ID ="btnClear" ValidationGroup="v" Text="Clear" runat="Server" 
         Width="170px" onclick="btnClear_Click" > </asp:Button>
   
 </th>
 </table>
  </center>
  <asp:ScriptManager ID="ScriptManager1" runat="server">
      </asp:ScriptManager>
</asp:Content>

