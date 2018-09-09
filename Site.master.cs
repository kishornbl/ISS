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

public partial class SiteMaster : System.Web.UI.MasterPage
{
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] != null)
        {
            lblUser.Text = Convert.ToString(Session["userName"]);
            lblBranch.Text = Convert.ToString(Session["BRANCH_NAME"]);
        }
        else
        {
            Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
        }
    }
    protected void btnSignOut_Click(object sender, EventArgs e)
    {
        //cTransactionEntity.LOG_DESCRIPTION = "LogOut User :" + Session["UserId"].ToString();
        //cTransactionEntity.ACTIVITY_ID = 1;
        //cTransactionEntity.USER_ID = Session["UserId"].ToString();
        //cTransactionEntity.ENTRY_EXIT_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
        //oTransactionDAL.InsertIntoISS_LOG_DETAILS(cTransactionEntity);

        //Session.Clear();
        //Session.Abandon();
        //Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
        TransactionDAL oTransactionDAL = new TransactionDAL();
        ViewState["brCode"] = Session["brCode"];
        ViewState["userId"] = Session["UserId"];
        //SSO_DAO oSSO_DAO = new SSO_DAO();
        WebReference.WebService web = new WebReference.WebService();
        int l = web.UpdateLoginStatus(ViewState["userId"].ToString(), ViewState["brCode"].ToString(), "LogoutDateTime", DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss"), " and loginStatus=1");
        l = web.UpdateLoginStatus(ViewState["userId"].ToString(), ViewState["brCode"].ToString(), "LoginStatus", "0", ""); Session.Clear();
        Session.Abandon();
        Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
    }
}
