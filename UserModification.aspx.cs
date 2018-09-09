/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: User Modification 
 create date: 26.07.2015
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class UserModification : System.Web.UI.Page
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

    protected void gvUserTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32.TryParse(Convert.ToString(gvUserTransaction.SelectedDataKey[0]), out gvRowId);
        int BranchId = Convert.ToInt32((gvUserTransaction.SelectedRow.FindControl("BranchCode") as Label).Text);
        string BranchName = (gvUserTransaction.SelectedRow.FindControl("BranchName") as Label).Text;
        txtUserId.Text = (gvUserTransaction.SelectedRow.FindControl("UserId") as Label).Text;
        txtUserName.Text = (gvUserTransaction.SelectedRow.FindControl("UserName") as Label).Text;
        txtDivision.Text = (gvUserTransaction.SelectedRow.FindControl("Division") as Label).Text;
        txtDesignation.Text = (gvUserTransaction.SelectedRow.FindControl("Designation") as Label).Text;
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
        txtDivision.Text = "";
        txtDesignation.Text = "";
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string message = null,UserPassword = null;
        string userId = txtUserId.Text.ToString().Trim();
        cTransactionEntity.USER_SL = gvRowId;
        cTransactionEntity.USER_ID = userId;
        cTransactionEntity.USER_NAME = txtUserName.Text.ToString();
        UserPassword = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", userId, "PASSWORD");
        cTransactionEntity.PASSWORD = UserPassword;
        cTransactionEntity.AUTHORISED_USER = Session["UserId"].ToString().Trim();
        cTransactionEntity.AUDIT_DESCRIPTION = "User Modification";
        cTransactionEntity.AUTHORISED_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
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
        int listatus = 0,liAudit = 0;
        cTransactionEntity.DIVISION = txtDivision.Text;
        cTransactionEntity.DESIGNATION = txtDesignation.Text.ToString();
        if (btnUpdate.Text == "Update")
        {
            cTransactionEntity.STATUS = 1;
            listatus = oTransactionDAL.UpdateIntoTableUserInfo(cTransactionEntity);
            liAudit = oTransactionDAL.InsertIntoISS_USER_AUDIT(cTransactionEntity);
            if (listatus == 1 && liAudit == 1)
            {
                message = "Data Updated Successfully";
            }
            else
            {
                message = "Data Not Updated!";
            }
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        LoadGridview();
        Clear();
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