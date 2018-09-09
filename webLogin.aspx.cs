/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Login Page
 create date: 26.07.2015
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

public partial class webLogin : System.Web.UI.Page
{
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    public static string msg = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Cache-control", "no-store, must-revalidate, private,no-cache");
        Response.AddHeader("Pragma", "no-cache");
        Response.AddHeader("Expires", "0");
        lblInfo.Text = "";
    }

    protected void Btn_LogIn_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }
        DbHandler dbh = new DbHandler();
        DataTable dt = dbh.GetDataTable("EXEC getLogInInfo '" + txtUser.Text.Trim() + "'");

        if (dt.Rows.Count < 1)
        {
            lblInfo.Text = "Invalid User Id & Password.";
        }
        else
        {
            if (btnLogIn.Text == "Log In")
            {
                if (dt.Rows[0]["Password"].ToString().Trim() != txtPass.Text.Trim())
                {
                    lblInfo.Text = "Invalid User Id or Password.";
                }
                else if (dt.Rows[0]["Password"].ToString().Trim() == "123456")
                {
                    lblInfo.Text = "Update your Password.";
                    btnLogIn.Text = "Update";
                }
                else
                {
                    Session.Add("UserId", txtUser.Text.Trim());
                    Session.Add("brCode", dt.Rows[0]["BRANCH_CODE"].ToString());
                    Session["BRANCH_NAME"] = dt.Rows[0]["BRANCH_NAME"].ToString();
                    Session["AD_STATUS"] = dt.Rows[0]["AD_STATUS"].ToString();
                    Session["DIVISION"] = dt.Rows[0]["DIVISION"].ToString();
                    //Session["DIVISION"] = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "DIVISION");

                    cTransactionEntity.LOG_DESCRIPTION = "LogIn User :" + Session["UserId"].ToString();
                    cTransactionEntity.ACTIVITY_ID = 1;
                    cTransactionEntity.USER_ID = Session["UserId"].ToString();
                    cTransactionEntity.ENTRY_EXIT_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
                    oTransactionDAL.InsertIntoISS_LOG_DETAILS(cTransactionEntity);

                    Response.Redirect("Default2.aspx");
                    //Response.Redirect("Default2.aspx?id= 10");
                    Session["UserId"] = txtUser.Text.Trim();
                }
            }
            else
            {
                int i = 0;
                cTransactionEntity.USER_ID = txtUser.Text.Trim();
                cTransactionEntity.PASSWORD = txtPass.Text.Trim();
                cTransactionEntity.STATUS = 1;
                if (cTransactionEntity.PASSWORD != "123456")
                {
                    if (CheckPassword(cTransactionEntity.PASSWORD) == cTransactionEntity.PASSWORD)
                    {
                        i = oTransactionDAL.UpdatePasswordIntoUserInfo(cTransactionEntity);
                    }
                    if (i == 1)
                    {
                        //string division = null;
                        btnLogIn.Text = "Log In";
                        Session.Add("UserId", txtUser.Text.Trim());
                        Session.Add("brCode", dt.Rows[0]["BRANCH_CODE"].ToString());
                        Session["BRANCH_NAME"] = dt.Rows[0]["BRANCH_NAME"].ToString();
                        Session["AD_STATUS"] = dt.Rows[0]["AD_STATUS"].ToString();
                        Session["DIVISION"] = dt.Rows[0]["DIVISION"].ToString();
                        //Session["DIVISION"] = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "DIVISION");

                        cTransactionEntity.LOG_DESCRIPTION = "LogIn User :" + Session["UserId"].ToString();
                        cTransactionEntity.ACTIVITY_ID = 1;
                        cTransactionEntity.USER_ID = Session["UserId"].ToString();
                        cTransactionEntity.ENTRY_EXIT_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
                        oTransactionDAL.InsertIntoISS_LOG_DETAILS(cTransactionEntity);

                        Response.Redirect("Default2.aspx");
                        Session["UserId"] = txtUser.Text.Trim();
                    }
                    else
                    {
                        lblInfo.Text = "Password Must Contain atleast 6-10 digits with minimum of One Upper Case,One Lower Case & One special Character";
                    }
                }
                else
                {
                    lblInfo.Text = "Default Password would not be your Current Password!";
                }
            }
        }
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
