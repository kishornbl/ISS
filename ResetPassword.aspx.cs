/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Reset Password 
 create date: 22.07.2015
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;


public partial class ResetPassword : System.Web.UI.Page
{
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    public static string strTranId = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime date = DateTime.Now;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            strTranId = date.ToString("yyyyMMdd");
            string UserId = Session["UserId"].ToString().Trim();
            if (UserId == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            if (Session["DIVISION"].ToString() != "HO")
            {
                Response.Redirect("Default2.aspx");
            }
            //if (UserId != "HOIT" && UserId != "ITSUPPORT")
            //{
            //    Response.Redirect("Default2.aspx");
            //}
            DataTable dt = oTransactionDAL.GetBranchName("1");
            branchDropDownList.DataSource = dt;
            branchDropDownList.DataTextField = "BRANCH_NAME";
            branchDropDownList.DataValueField = "BRANCH_CODE";
            branchDropDownList.DataBind();
            LoadUser();
        }
    }

    private void LoadUser()
    {
        string Condition = "WHERE (ISS_BRANCH_INFO.BRANCH_CODE = '" + branchDropDownList.SelectedValue.ToString() + "') ";
        DataTable dt = oTransactionDAL.GetUser(Condition);
        UserDropDownList.DataSource = dt;
        UserDropDownList.DataTextField = "USER_ID";
        UserDropDownList.DataValueField = "USER_NAME";
        UserDropDownList.DataBind();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        string message = "";
        cTransactionEntity.USER_ID = UserDropDownList.SelectedItem.ToString().Trim();
        string division = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", cTransactionEntity.USER_ID, "DIVISION");
        string Designation = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", cTransactionEntity.USER_ID, "DESIGNATION");
        cTransactionEntity.USER_NAME = UserDropDownList.SelectedValue.ToString().Trim();
        cTransactionEntity.PASSWORD = "123456";
        cTransactionEntity.IS_ACTIVE = 1;
        cTransactionEntity.PREVILEGE_STATUS = 0;
        if (branchDropDownList.SelectedValue.ToString() == "1001")
        {
            cTransactionEntity.HO_STATUS = 1;
            cTransactionEntity.BR_STATUS = 0;
        }
        else
        {
            cTransactionEntity.HO_STATUS = 0;
            cTransactionEntity.BR_STATUS = 1;
        }
        cTransactionEntity.AUDIT_DESCRIPTION = "Reset Password";
        cTransactionEntity.AUTHORISED_USER = Session["UserId"].ToString().Trim();
        cTransactionEntity.DIVISION = division;
        cTransactionEntity.DESIGNATION = Designation;
        cTransactionEntity.AUTHORISED_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
        int ResetStatus = 0, AuditStatus = 0;
        ResetStatus  = oTransactionDAL.UpdateIntoTableUserInfoforResetPass(cTransactionEntity);
        AuditStatus = oTransactionDAL.InsertIntoISS_USER_AUDIT(cTransactionEntity);
        if (ResetStatus == 1 && AuditStatus == 1)
        {
            message = "Password Reset Successfully";
        }
        else
        {
            message = "Password Reset Unsuccessful!";
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        LoadUser();
    }
    protected void branchDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUser();
    }
    protected void btnDeleteRecord_Click(object sender, EventArgs e)
    {
        string message = "";
        int liExist = 0;
        cTransactionEntity.TRAN_ID = strTranId;
        cTransactionEntity.USER_ID = UserDropDownList.SelectedItem.ToString().Trim();
        cTransactionEntity.BRANCH_CODE = branchDropDownList.SelectedValue.ToString().Trim();
        cTransactionEntity.TYPE = trType.SelectedItem.ToString().Trim() ;
        liExist = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", strTranId, "BRANCH_CODE", cTransactionEntity.BRANCH_CODE, "TYPE", cTransactionEntity.TYPE, "BR_AUTHO_SL");
        if (liExist > 0)
        {
            string lstrexist = Convert.ToString(liExist);
            string userid = oTransactionDAL.GetoneReturnOneString("ISS_BRANCH_AUTHORIZATION","BR_AUTHO_SL",lstrexist,"USER_ID");
            int DeleteBranchAuthoData = 0;
            if (userid == cTransactionEntity.USER_ID)
            {
                DeleteBranchAuthoData = oTransactionDAL.DeleteDatafromBranchAuthorization(cTransactionEntity);
                if (DeleteBranchAuthoData == 1)
                {
                    message = "Data Deletion Successful";
                }
                else
                {
                    message = "Data Deletion UnSuccessful";
                }
            }
            else
            {
                message = "Please Select the Authorized User.";
            }
        }
        else
        {
            message = "Data is ALready Deleated or Not Authorized Till Now.";
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        LoadUser();
    }
}