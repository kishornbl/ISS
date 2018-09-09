/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Create User 
 create date: 12.07.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class CreateUser : System.Web.UI.Page
{
    protected static int listatus = 0, gvRowId = 0;
    protected static string division = null;
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            division = Session["DIVISION"].ToString();
            if (Session["UserId"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            string UserId = Session["UserId"].ToString().Trim();
            if (UserId == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            if (division != "HO")
            {
                Response.Redirect("Default2.aspx");
            }
            DataTable dt = oTransactionDAL.GetBranchName("1");
            branchDropDownList.DataSource = dt;
            branchDropDownList.DataTextField = "BRANCH_NAME";
            branchDropDownList.DataValueField = "BRANCH_CODE";
            branchDropDownList.DataBind();
            LoadGridview();
            drpDivision.Items.Clear();
            drpDivision.Items.Add("Branch");
            drpDivision.Items.Add("Branch Admin");
        }
    }

    private void LoadGridview()
    {
        //string condition = " WHERE ISS_USER_INFO.DIVISION <> 'HO' ";
        string condition = " WHERE ISS_USER_INFO.DIVISION <> 'HO' ORDER BY ISS_BRANCH_INFO.BRANCH_NAME ";
        oTransactionDAL = new TransactionDAL();
        DataTable dt = oTransactionDAL.LoadUserInfoGridView(condition);
        gvUserTransaction.DataSource = dt;
        gvUserTransaction.DataBind();       
    }
    
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        string message = null;
        string userId = txtUserId.Text.ToString().Trim();
        cTransactionEntity.USER_SL = gvRowId;
        cTransactionEntity.USER_ID = userId;
        cTransactionEntity.USER_NAME = txtUserName.Text.ToString();
        cTransactionEntity.PASSWORD = "123456";
        if (chkBoxActive.Checked == true)
        {
            cTransactionEntity.IS_ACTIVE = 1;
        }
        else
        {
            cTransactionEntity.IS_ACTIVE = 0;
        }
        cTransactionEntity.PERMISSION_ID = 1;
        cTransactionEntity.PREVILEGE_STATUS = 1;
        cTransactionEntity.BRANCH_CODE = branchDropDownList.SelectedValue.ToString();
        cTransactionEntity.CREATION_DATE = DateTime.Today.ToShortDateString();
        cTransactionEntity.LAST_MODIFICATION_DATE = DateTime.Today.ToShortDateString();
        cTransactionEntity.OFFICE_IND = branchDropDownList.SelectedItem.ToString();
        cTransactionEntity.DESIGNATION = txtDesignation.Text.ToString();
        //if (branchDropDownList.SelectedItem.ToString() != "Head Office")
        //{
        //    cTransactionEntity.OFFICE_IND = "BRANCH OFFICE";
        //}
        //else
        //{
        //    cTransactionEntity.OFFICE_IND = "HEAD OFFICE";
        //}
        cTransactionEntity.AUDIT_DESCRIPTION = "User Creation";
        cTransactionEntity.AUTHORISED_USER = Session["UserId"].ToString().Trim();
        cTransactionEntity.AUTHORISED_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
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
        int listatus = 0;
        cTransactionEntity.DIVISION = drpDivision.SelectedValue.ToString();
        if (btnCreate.Text == "Create")
        {
            cTransactionEntity.STATUS = 0;
            string condition = " where userId='" + userId + "'";
            listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", userId, "IS_ACTIVE");
            if (listatus > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alert('User Id Already in Database. Please Enter Another User Id!');", true);
                return;
            }
            else
            {               
                int i = oTransactionDAL.InsertIntoTableUserInfo(cTransactionEntity);
                int AuditTable = oTransactionDAL.InsertIntoISS_USER_AUDIT(cTransactionEntity);
                if (i == 1 && AuditTable ==1)
                {
                    message = "Data Inserted Successfully";
                }
                else
                {
                    message = "Data Not Inserted!";
                }
            }
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        LoadGridview();
        Clear();
    }
    protected void gvUserTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32.TryParse(Convert.ToString(gvUserTransaction.SelectedDataKey[0]), out gvRowId);
        int BranchId = Convert.ToInt32((gvUserTransaction.SelectedRow.FindControl("BranchCode") as Label).Text);
        string BranchName = (gvUserTransaction.SelectedRow.FindControl("BranchName") as Label).Text;
        txtUserId.Text = (gvUserTransaction.SelectedRow.FindControl("UserId") as Label).Text;
        txtUserName.Text = (gvUserTransaction.SelectedRow.FindControl("UserName") as Label).Text;
        string Activity = (gvUserTransaction.SelectedRow.FindControl("Activity") as Label).Text;
        if (Activity == "Active")
        {
            chkBoxActive.Checked = true;
        }
        else
        {
            chkBoxActive.Checked = false;
        }
        branchDropDownList.Items.Clear();
        branchDropDownList.Items.Add(BranchName);
        branchDropDownList.DataBind();
    }
    protected void gvUserTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUserTransaction.PageIndex = e.NewPageIndex;
        LoadGridview();
    }

    private void Clear()
    {
        gvRowId = 0;
        txtUserId.Text = "";
        txtUserName.Text = "";
        chkBoxActive.Checked = true;
    }
    protected void branchDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (branchDropDownList.SelectedValue == "1001")
        {
            drpDivision.Items.Clear();
            drpDivision.Items.Add("ID");
            drpDivision.Items.Add("LRD");
            drpDivision.Items.Add("CAD");
            drpDivision.Items.Add("FAD");
            drpDivision.Items.Add("CRM-III");
            drpDivision.Items.Add("CRM-V");
            drpDivision.Items.Add("RMD");
            drpDivision.Items.Add("IT");
            drpDivision.Items.Add("Treasury");
        }
        else
        {
            drpDivision.Items.Clear();
            drpDivision.Items.Add("Branch");
            drpDivision.Items.Add("Branch Admin");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string condition = " WHERE ISS_USER_INFO.DIVISION <> 'HO' AND ISS_BRANCH_INFO.BRANCH_NAME LIKE '" + txtSearch.Text.ToString() + "%' ORDER BY ISS_BRANCH_INFO.BRANCH_NAME ";
        oTransactionDAL = new TransactionDAL();
        DataTable dt = oTransactionDAL.LoadUserInfoGridView(condition);
        gvUserTransaction.DataSource = dt;
        gvUserTransaction.DataBind();
    }
}