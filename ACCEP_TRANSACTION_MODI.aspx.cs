/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Acceptance Data Modification 
 create date: 20.07.2015
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

public partial class ACCEP_TRANSACTION_MODI : System.Web.UI.Page
{
    protected static string ISSAcceptanceId = null, Condition = null, message = null;
    protected static int listatus = 0, AdStatus = 0,BankIdforAudit = 0;
    protected static int gvRowId = 0;
    protected static string division = null, getValue = null;
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
            getValue = Request.QueryString["value"];
            DateTime date = DateTime.Now;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            ISSAcceptanceId = date.ToString("yyyyMMdd");
            ViewState["date"] = date;
            ViewState["Value"] = getValue;
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
                        btn_Authorize.Visible = false;
                        btn_Clear_Data.Visible = false;
                    }
                }
                else
                {
                    if (division == "Branch" || division == "Branch Admin")
                    {
                        if (getValue == "Branch" && division == "Branch")
                        {
                            btnSave.Visible = true;
                            btnClear.Visible = true;
                            btn_Authorize.Visible = false;
                            btn_Clear_Data.Visible = false;
                            lblAccepMatAmnt.Visible = true;
                            lblAmount.Visible = true;
                            lblMatofRecAcc.Visible = true;
                            lblPurAmntOfRecAcc.Visible = true;
                            lblValueRecAccep.Visible = true;
                            txtAcceptanceIssued.Visible = true;
                            txtAcceptanceMatured.Visible = true;
                            txtMatofRecAccep.Visible = true;
                            txtPurchaseRecAccep.Visible = true;
                            txtReceivedAcceptance.Visible = true;
                        }
                        else if (getValue == "Branch" && division == "Branch Admin")
                        {
                            Response.Redirect("Default2.aspx");
                        }
                        else if (getValue == "Branch Admin" && division == "Branch Admin")
                        {
                            btn_Authorize.Visible = true;
                            btn_Clear_Data.Visible = true;
                            btnSave.Visible = false;
                            btnClear.Visible = false;
                            lblAccepMatAmnt.Visible = false;
                            lblAmount.Visible = false;
                            lblMatofRecAcc.Visible = false;
                            lblPurAmntOfRecAcc.Visible = false;
                            lblValueRecAccep.Visible = false;
                            txtAcceptanceIssued.Visible = false;
                            txtAcceptanceMatured.Visible = false;
                            txtMatofRecAccep.Visible = false;
                            txtPurchaseRecAccep.Visible = false;
                            txtReceivedAcceptance.Visible = false;
                        }
                        else if (getValue == "Branch Admin" && division == "Branch")
                        {
                            Response.Redirect("Default2.aspx");
                        }
                    }
                    else
                    {
                         Response.Redirect("Default2.aspx");
                    }
                }
            }
            LoadBankName();
            LoadGridview();
        }
    }

    //Save the Data
    protected void btnSave_Click(object sender, EventArgs e)
    {
        double accissuedamnt = 0, accmatamnt = 0, accrecamnt = 0, puramnt = 0, rercmatamnt = 0;
        cTransactionEntity = new TransactionEntity();
        cTransactionEntity.ACCEP_SL = gvRowId;
        cTransactionEntity.ACCEP_ID = ISSAcceptanceId;
        cTransactionEntity.AS_ON_DATE = Convert.ToDateTime(ViewState["date"]).ToShortDateString();
        cTransactionEntity.BANK_ID = Convert.ToString(BankIdforAudit);
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
            if (btnSave.Text == "Save")
            {
                int i = oTransactionDAL.InsertIntoTableISS_ACCEPTANCE_TRAN(cTransactionEntity);
                if (i == 1)
                {
                    message = "Data Inserted Successfully";
                    clear();
                }
                else
                    message = "Data Not Inserted!";
            }
            else
            {
                int i = oTransactionDAL.UpdateIntoTableISS_ACCEPTANCE_TRAN(cTransactionEntity);
                int AuditStatus = oTransactionDAL.InsertIntoISS_TRAN_ACCEPTANCE_AUDIT(cTransactionEntity);
                if (i == 1 && AuditStatus == 1)
                {
                    message = "Data Updated Successfully";
                    clear();
                }
                else
                    message = "Data Not Updated!";
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
    protected void drpBankName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridview();
    }

    //Load GridView
    private void LoadGridview()
    {
        string condition = " ";
        if (getValue == "Branch Admin" && division == "Branch Admin")
        {
            condition = "WHERE BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_ACCEPTANCE_TRAN.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' ORDER BY ISS_ACCEPTANCE_TRAN.BANK_ID";
        }
        else
        {
            condition = "WHERE ISS_ACCEPTANCE_TRAN.BANK_ID = " + drpBankName.SelectedValue + " AND BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_ACCEPTANCE_TRAN.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "'";
        }
        oTransactionDAL = new TransactionDAL();
        DataTable dt = oTransactionDAL.LoadAcceptanceGridView(condition);
        gvAcceptanceTransaction.DataSource = dt;
        gvAcceptanceTransaction.DataBind();
        if (ViewState["Value"].ToString() == "Branch Admin")
        {
            for (int i = 0; i < gvAcceptanceTransaction.Rows.Count; i++)
            {
                gvAcceptanceTransaction.Rows[i].Cells[0].Enabled = false;
            }
        }
    }

    //Load BankName
    private void LoadBankName()
    {
        SqlConnection con;
        SqlCommand cmd;
        string sqlQuery = null;
        oTransactionDAL = new TransactionDAL();
        DataTable dt = new DataTable();
        Condition = "";
        if (getValue == "Branch Admin" && division == "Branch Admin")
        {
            sqlQuery = "SELECT TOP 1 0 BANK_ID, 'All Item' BANK_NAME FROM ISS_BANK_INFO ORDER BY BANK_NAME";
            con = oConnectionDatabase.DatabaseConnection();
            cmd = new SqlCommand(sqlQuery, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                da.Fill(dt);
            }
            catch { }
            con.Close();
        }
        else
        {
            dt = oTransactionDAL.Bank_Name(Condition);
        }
        drpBankName.DataSource = dt;
        drpBankName.DataTextField = "BANK_NAME";
        drpBankName.DataValueField = "BANK_ID";
        drpBankName.DataBind();
    }

    //Clear Text Field
    private void clear()
    {
        gvRowId = 0;
        txtAcceptanceIssued.Text = "";
        txtAcceptanceMatured.Text = "";
        txtMatofRecAccep.Text = "";
        txtPurchaseRecAccep.Text = "";
        txtReceivedAcceptance.Text = "";
    }

    //GridView Selection
    protected void gvAcceptanceTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
        double accissuedamnt = 0, accmatamnt = 0, accrecamnt = 0, puramnt = 0, rercmatamnt = 0;
        Int32.TryParse(Convert.ToString(gvAcceptanceTransaction.SelectedDataKey[0]), out gvRowId);
        int BankId = Convert.ToInt32((gvAcceptanceTransaction.SelectedRow.FindControl("BankId") as Label).Text);
        BankIdforAudit = BankId;
        string BankName = (gvAcceptanceTransaction.SelectedRow.FindControl("BankName") as Label).Text;
        accissuedamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccIssueAmnt") as Label).Text);
        accmatamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccMatAmnt") as Label).Text);
        accrecamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccRecAmnt") as Label).Text);
        puramnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccPurAmnt") as Label).Text);
        rercmatamnt = Convert.ToDouble((gvAcceptanceTransaction.SelectedRow.FindControl("AccRecMatAmnt") as Label).Text);
        txtAcceptanceIssued.Text = Convert.ToString(accissuedamnt);
        txtAcceptanceMatured.Text = Convert.ToString(accmatamnt);
        txtReceivedAcceptance.Text = Convert.ToString(accrecamnt);
        txtMatofRecAccep.Text = Convert.ToString(rercmatamnt);
        txtPurchaseRecAccep.Text = Convert.ToString(puramnt);
        drpBankName.Items.Clear();
        drpBankName.Items.Add(BankName);
        drpBankName.DataBind();
    }

    //GridView PageIndex Changing
    protected void gvAcceptanceTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAcceptanceTransaction.PageIndex = e.NewPageIndex;
        LoadGridview();
    }
    protected void btn_Authorize_Click(object sender, EventArgs e)
    {
        string confirmValue = Request.Form["confirm_value"];
        string message = null;
        if (confirmValue == "Yes")
        {
            listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", ISSAcceptanceId, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "ACCEPTANCE", "AUTHO_STATUS");
            if (listatus == 0)
            {
                int Authorizestatus = 0;
                cTransactionEntity.TRAN_ID = ISSAcceptanceId;
                cTransactionEntity.ENTRY_DATE = DateTime.Today.ToShortDateString();
                cTransactionEntity.BRANCH_CODE = Session["brCode"].ToString();
                cTransactionEntity.USER_ID = Session["UserId"].ToString();
                cTransactionEntity.AUTHO_STATUS = 1;
                cTransactionEntity.TYPE = "ACCEPTANCE";
                Authorizestatus = oTransactionDAL.InsertIntoISS_BRANCH_AUTHORIZATION(cTransactionEntity);
                if (Authorizestatus == 1)
                {
                    message = "Data Authorized Successfully";
                }
                else
                {
                    message = "Data Authorization is UnSuccessfully";
                }

            }
            else
            {
                message = "Already Authorized";
            }
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
            clear();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('You are not ready to Authorize!')", true);
        }
    }
    protected void btn_Clear_Data_Click(object sender, EventArgs e)
    {
        clear();
    }
}