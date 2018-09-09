/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Acceptance Data Entry 
 create date: 23.07.2015
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ACCEP_TRANSACTION : System.Web.UI.Page
{
    protected static string ISSAcceptanceId = null, Condition = null,message = null;
    protected static int listatus = 0,AdStatus = 0;
    protected static int gvRowId = 0;
    protected static string division = null;
    protected static int hostatus = 0;
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Load(object sender, EventArgs e)
    {
        int alivedate = oTransactionDAL.GetOneReturnOne("ISS_SFT_ALIVE", "SL_NO", "1", "ALIVE_DATE");
        DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alivedate);
        DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
        if (Convert.ToDateTime(DateTime.Now) > fromDate && Convert.ToDateTime(DateTime.Now) < toDate)
        {
            Response.Redirect("Default2.aspx");
        }
        if (!IsPostBack)
        {
            DateTime date = DateTime.Now;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            ISSAcceptanceId = date.ToString("yyyyMMdd");
            ViewState["date"] = date;
            division = Session["DIVISION"].ToString();
            //hostatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
            hostatus = Convert.ToInt32(Session["HO_STATUS"]);
            txtAsonDate.Text = date.ToString("dd-MMM-yyyy");
            AdStatus = Convert.ToInt32(Session["AD_STATUS"]);
            if (Session["UserId"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            if (AdStatus != 1)
            {
                Response.Redirect("Default2.aspx");
            }
            else
            {
                if (hostatus == 1)
                {
                    if (division != "ID")
                    {
                        Response.Redirect("Default2.aspx");
                    }
                    else
                    {
                        LoadBankName();
                        LoadGridview();
                    }
                }
                else
                {
                    int checkAuthoStatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", ISSAcceptanceId, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "ACCEPTANCE", "AUTHO_STATUS");
                    if (checkAuthoStatus == 0)
                    {
                        if (division != "Branch")
                        {
                            Response.Redirect("Default2.aspx");
                        }
                        else
                        {
                            LoadBankName();
                            LoadGridview();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('No Entry is Possible,Already being Authorized.')", true);
                        return;
                    }
                }
            }
        }
    }
    
    //Load GridView
    private void LoadGridview()
    {
        string condition = " ";
        condition = "WHERE ISS_ACCEPTANCE_TRAN.BANK_ID = " + drpBankName.SelectedValue + " AND BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_ACCEPTANCE_TRAN.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "'";
        oTransactionDAL = new TransactionDAL();
        DataTable dt = oTransactionDAL.LoadAcceptanceGridView(condition);
        gvAcceptanceTransaction.DataSource = dt;
        gvAcceptanceTransaction.DataBind();
    }

    //Load BankName
    private void LoadBankName()
    {
        oTransactionDAL = new TransactionDAL();
        Condition = "";
        DataTable dt = oTransactionDAL.Bank_Name(Condition);
        drpBankName.DataSource = dt;
        drpBankName.DataTextField = "BANK_NAME";
        drpBankName.DataValueField = "BANK_ID";
        drpBankName.DataBind();
    }

    // Clear All Field
    private void clear()
    {
        txtAcceptanceIssued.Text = "";
        txtAcceptanceMatured.Text = "";
        txtMatofRecAccep.Text = "";
        txtPurchaseRecAccep.Text = "";
        txtReceivedAcceptance.Text = "";
        btnSave.Text = "Save";
    }

    //Save the Data
    protected void btnSave_Click(object sender, EventArgs e)
    {
        double accissuedamnt = 0, accmatamnt = 0, accrecamnt = 0, puramnt = 0, rercmatamnt = 0;
        cTransactionEntity = new TransactionEntity();
        cTransactionEntity.ACCEP_SL = gvRowId;
        cTransactionEntity.ACCEP_ID = ISSAcceptanceId;
        cTransactionEntity.AS_ON_DATE = Convert.ToDateTime(ViewState["date"]).ToShortDateString();
        cTransactionEntity.BANK_ID = drpBankName.SelectedValue;
        if (cTransactionEntity.BANK_ID != "0")
        {
            cTransactionEntity.ENTRY_DATE = DateTime.Today.ToShortDateString();
            cTransactionEntity.USER_ID = Session["UserId"].ToString();
            cTransactionEntity.BRANCH_CODE = Session["brCode"].ToString();
            Double.TryParse(Convert.ToString(txtAcceptanceIssued.Text), out accissuedamnt);
            Double.TryParse(Convert.ToString(txtAcceptanceMatured.Text), out accmatamnt);
            Double.TryParse(Convert.ToString(txtReceivedAcceptance.Text), out accrecamnt);
            Double.TryParse(Convert.ToString(txtPurchaseRecAccep.Text), out puramnt);
            Double.TryParse(Convert.ToString(txtMatofRecAccep.Text), out rercmatamnt);
            cTransactionEntity.ACCEPTANCE_ISSUED_AMOUNT = accissuedamnt;
            cTransactionEntity.ACCEPTANCE_MATURED_AMOUNT = accmatamnt;
            cTransactionEntity.ACCEPTANCE_RECEIVED_AMOUNT = accrecamnt;
            cTransactionEntity.ACCEPTANCE_PURCHASED_AMOUNT = puramnt;
            cTransactionEntity.ACCEPTANCE_REC_MATURED_AMOUNT = rercmatamnt;
            cTransactionEntity.ENTRY_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
            cTransactionEntity.INSERT_STATUS = 1;
            cTransactionEntity.UPDATE_STATUS = 0;
            int listatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_ACCEPTANCE_MAINTENANCE", "ACCEP_ID", ISSAcceptanceId, "BANK_ID", drpBankName.SelectedValue, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (listatus == 0)
            {
                if (btnSave.Text == "Save")
                {
                    int i = oTransactionDAL.InsertIntoTableISS_ACCEPTANCE_TRAN(cTransactionEntity);
                    int AuditStatus = oTransactionDAL.InsertIntoISS_TRAN_ACCEPTANCE_AUDIT(cTransactionEntity);
                    int BrWiseAccMain = oTransactionDAL.InsertIntoBRANCHWISE_ACCEPTANCE_MAINTENANCE(cTransactionEntity);
                    if (i == 1 && AuditStatus == 1 && BrWiseAccMain == 1)
                    {
                        message = "Data Inserted Successfully";
                        clear();
                    }
                    else if (i != 1)
                    {
                        message = "Data Not Inserted into Transaction Table";
                    }
                    else
                    {
                        message = "Data Not Inserted!";
                    }
                }
            }
            else
            {
                message = "Data ALready Inserted!";
            }
        }
        else
        {
            message = "Please Select the Bank Name First!";
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        LoadBankName();
        LoadGridview();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }

    // GridView Selection
    protected void gvAcceptanceTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
        double accissuedamnt = 0, accmatamnt = 0, accrecamnt = 0, puramnt = 0, rercmatamnt = 0;
        Int32.TryParse(Convert.ToString(gvAcceptanceTransaction.SelectedDataKey[0]), out gvRowId);
        int BankId = Convert.ToInt32((gvAcceptanceTransaction.SelectedRow.FindControl("BankId") as Label).Text);
        string BankName = (gvAcceptanceTransaction.SelectedRow.FindControl("BankName") as Label).Text;
        accissuedamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccIssueAmnt") as Label).Text);
        accmatamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccMatAmnt") as Label).Text);
        accrecamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccRecAmnt") as Label).Text);
        puramnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccPurAmnt") as Label).Text);
        rercmatamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccRecMatAmnt") as Label).Text);
        txtAcceptanceIssued.Text = Convert.ToString(accissuedamnt);
        txtAcceptanceMatured.Text = Convert.ToString(accmatamnt);
        txtReceivedAcceptance.Text = Convert.ToString(accrecamnt);
        txtMatofRecAccep.Text = Convert.ToString(rercmatamnt );
        txtPurchaseRecAccep.Text = Convert.ToString(puramnt);
        drpBankName.Items.Clear();
        drpBankName.Items.Add(BankName);
        drpBankName.DataBind();
        btnSave.Text = "Update";
    }
    protected void drpBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridview();
    }

    //GridView PageIndex Changing
    protected void gvAcceptanceTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAcceptanceTransaction.PageIndex = e.NewPageIndex;
        LoadGridview();
    }
}