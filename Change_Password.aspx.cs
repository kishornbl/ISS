/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Change Password
 create date: 21.07.2015
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;

public partial class Change_Password : System.Web.UI.Page
{
    protected static string UserId = null,UserPassword = null,UserName = null;
    public static string msg = null;
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            UserId = Session["UserId"].ToString().Trim();
            if (UserId == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
        }
    }

    //Change Password
    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        string message = null, branchCode = null, division = null, Designation = null;
        UserPassword = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", UserId, "PASSWORD");
        branchCode = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", UserId, "BRANCH_CODE");
        Designation = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", UserId, "DESIGNATION");
        division = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", UserId, "DIVISION");
        if (txtCurrentPassword.Text.ToString().Trim() == UserPassword)
        {
            if (txtNewPassword.Text.ToString().Trim() == txtConfirmPassword.Text.ToString().Trim())
            {
                if (txtNewPassword.Text.ToString().Trim() != "123456")
                {
                    UserName = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", UserId, "USER_NAME");
                    cTransactionEntity.USER_ID = UserId;
                    cTransactionEntity.USER_NAME = UserName;
                    cTransactionEntity.PASSWORD = txtConfirmPassword.Text.ToString().Trim();
                    cTransactionEntity.IS_ACTIVE = 1;
                    cTransactionEntity.AUDIT_DESCRIPTION = "Change Password";
                    cTransactionEntity.AUTHORISED_USER = UserId;
                    cTransactionEntity.DIVISION = division;
                    cTransactionEntity.STATUS = 1;
                    cTransactionEntity.AUTHORISED_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
                    if (branchCode == "1001")
                    {
                        cTransactionEntity.HO_STATUS = 1;
                        cTransactionEntity.BR_STATUS = 0;
                    }
                    else
                    {
                        cTransactionEntity.HO_STATUS = 0;
                        cTransactionEntity.BR_STATUS = 1;
                    }
                    cTransactionEntity.DESIGNATION = Designation;
                    int listatus = 0, liAudit = 0;
                    if (CheckPassword(cTransactionEntity.PASSWORD) == cTransactionEntity.PASSWORD)
                    {
                        listatus = oTransactionDAL.UpdateIntoTableUserInfoforResetPass(cTransactionEntity);
                        liAudit = oTransactionDAL.InsertIntoISS_USER_AUDIT(cTransactionEntity);
                    }
                    if (listatus == 1 && liAudit == 1)
                    {
                        message = "Data Updated Successfully";
                    }
                    else
                    {
                        message = "Password Must Contain atleast 6-10 digits with minimum of One Upper Case,One Lower Case & One special Character";
                    }
                }
                else
                {
                    message = "Your New Password will not be default password";
                }
            }
            else
            {
                message = "Your New Password & Confirm Password is Not Same";
            }
        }
        else
        {
            message = "Your Current Password is Not Valid";
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
    }

    public string CheckPassword(string password)
    {
        string MatchPasswordPattern = "^.*(?=.{6,10})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$";
        if (password != null)
        {
            if (Regex.IsMatch(password, MatchPasswordPattern))
                msg = password;
            return msg;
        }
        else
            return msg;
    }
}