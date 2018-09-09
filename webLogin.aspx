<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webLogin.aspx.cs" Inherits="webLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="toolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ISS User Login</title>
    <style type="text/css">
        .watermerk
        {
            width: 41px;
            color: Silver;
        }
        .error
        {
            color:Red;
        }
        .warning
        {
            color: #9F6000;
            background-color: #FEEFB3;
            /*background-image: url('warning.png');*/
        }
        .forlogin:hover
        {
            background-color: #31C4E1;
            border-color:#9900CC;
            text-align: center;
            font-family:Verdana;
            font-size:20px;
            line-height:19px;
        }
       .forSave
        {
            background: url(Image/save.png) no-repeat 0 0;
            border: 0;
            height: 20px;
            width: 30px
        }
            
    .forUpdate
       {
            background: url(Image/edit.png) no-repeat 0 0;
            border: 0;
            height: 20px;
            width: 30px
       }
            
    .forDelete
      {
            background: url(Image/delete.png) no-repeat 0 0;
            border: 0;
            height: 20px;
            width: 30px
      }
            
    .forClear
      {
            background: url(Image/edit_clear.png) no-repeat 0 0;
            border: 0;
            height: 20px;
            width: 30px
      }
        
    </style>
    <script type="text/javascript" language="javascript">
        function capLock(e) {
            kc = e.keyCode ? e.keyCode : e.which;
            sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
                document.getElementById('divMayus').style.visibility = 'visible';
            else
                document.getElementById('divMayus').style.visibility = 'hidden';
        }
    </script>
</head>
<body onload="document.form1.txtUser.focus()">

    <form id="form1" runat="server" style = "background-image: url('page-background.png');">
        <toolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
        </toolkit:ToolkitScriptManager>
        <div style="width: 800px; height: 400px; margin-left: auto; margin-right: auto; padding-top: auto; padding-bottom: auto;">
            <div style="width: 800px; text-align: center; font-family: 'Courier New', Courier, monospace; font-size: 25px; font-weight: bold; font-style: italic; color: #000000;">
                ISS Report Management<hr />
            </div>

            <div style="width: 800px">
                <div style="float: left; width: 398px; padding-top: 100px; ">
                   <center style = "border-right: 1px dashed #0B243B;">
                   <div style = "background-image: url('homepage.png'); height: 252px; width: 256px;">
                   </div>
                   </center>
                </div>
                <div style="float: right; width: 400px; padding-top: 100px">
                    <center>
                        <table style="width: 300px;">
                            <tr>
                                <td style="text-align: center; font-family: Calibri;" colspan="2"> Log In
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 100px; text-align: left; vertical-align: top; font-family: Calibri;">User Id :
                                </td>
                                <td style="text-align: left; width: 200px">

                                    <asp:TextBox ID="txtUser" runat="server" Style="width: 150px" title="Insert User Name Here"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_UserId" runat="server"
                                        ErrorMessage="Enter User Id" ControlToValidate="txtUser"
                                        ValidationGroup="LogIn" CssClass = "error" >*</asp:RequiredFieldValidator>
                                        <toolkit:TextBoxWatermarkExtender ID="TBWM1" runat="server"
                                            TargetControlID="txtUser"
                                            WatermarkText="User Id"
                                            WatermarkCssClass="watermerk" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 100px; font-family: Calibri; ">Password : 
                                </td>
                                <td style="text-align: left; width: 200px">
                                    <asp:TextBox ID="txtPass" runat="server" Style="width: 150px" TextMode="Password" title = "Insert Password Atleast 6 Characters" onkeypress="capLock(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Pass" runat="server" 
                                     ErrorMessage="Enter Password" ControlToValidate="txtPass" 
                                     ValidationGroup="LogIn" CssClass = "error">*</asp:RequiredFieldValidator>
                                      
                                     <toolkit:TextBoxWatermarkExtender ID="TBWM2" runat="server"
                                            TargetControlID="txtPass"
                                            WatermarkText="Password"
                                            WatermarkCssClass="watermerk" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 100px">&nbsp;
                                </td>
                                <td style="text-align: left; width: 200px">
                                    <asp:Label ID="lblInfo" runat="server" Font-Names="Times New Roman"
                                        Font-Size="12px" ForeColor="Red"></asp:Label>
                                        <div id="divMayus" style="visibility:hidden">
                                        <asp:Label ID="lblWarning" runat="server" Font-Names="Times New Roman"
                                        Font-Size="18px" CssClass = "warning">Your Caps Lock is on</asp:Label>
                                        </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left; width: 100px">&nbsp;
                                </td>
                                <td style="text-align: left; width: 200px">
                                    <asp:Button ID="btnLogIn" runat="server" Font-Names = "Calibri" Text="Log In" Style="width: 75px;"
                                        OnClick="Btn_LogIn_Click" ValidationGroup="LogIn" Width="75px" Height="30px" CssClass = "forlogin" />
                                </td>
                            </tr>

                            <tr>
                            <td>
                            
                            </td>
                            </tr>
                            <tr>
                                <td style="text-align: center" colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary_LogIn" runat="server"
                                        ValidationGroup="LogIn" Font-Names="Calibri" Font-Size="16px" CssClass = "error"  />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </div>
        </div>

        <div style="width: 800px; height: 400px; text-align: center; margin-left: auto; margin-right: auto; padding-top: 100px; padding-bottom: auto; font-family: Calibri; font-size: 11px;">
            <hr />
            National Bank Limited &nbsp; |&nbsp; Information Technology Division
        <hr />
        </div>
    </form>
</body>
</html>