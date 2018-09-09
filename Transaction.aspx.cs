/* Version:01.01
   Author: Kishor Kumar Saha
   Opearation: Monitoring Data Modification 
   create date: 22.07.2015
   Modification Date : 07.02.2016
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Transaction : System.Web.UI.Page
{
    protected static string pssTransactionId = null,Condition = null;
    protected static int pssCategoryId = 0, pssSubcategoryId = 0, listatus = 0;
    protected static int gvRowId = 0;
    protected static string division = null, getValue = null;
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Init(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("webLogin.aspx");
        }
        int alivedate = oTransactionDAL.GetOneReturnOne("ISS_SFT_ALIVE", "SL_NO", "1", "ALIVE_DATE");
        DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alivedate);
        DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
        if (Convert.ToDateTime(DateTime.Now) > fromDate && Convert.ToDateTime(DateTime.Now) < toDate)
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            DateTime date = DateTime.Now;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            getValue = Request.QueryString["value"];
            pssTransactionId = date.ToString("yyyyMMdd");
            ViewState["date"] = date;
            ViewState["Value"] = getValue;
            txtAsonDate.Text = date.ToString("dd-MMM-yyyy");
            division = Session["DIVISION"].ToString();
            int checkAuthoStatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");

            //listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");

            if (getValue == "Branch" && division == "Branch")
            {
                btn_Authorize.Visible = false;
                //btn_Clear_Data.Visible = false;
                btnInsert.Visible = true;
                //btnClear.Visible = true;
            }
            //if (listatus == 0)
            //{
            //if (checkAuthoStatus == 0)
            //{
            if (getValue == "Branch" && division == "Branch")
            {
                LoadCategorydrpdownList();
            }
            if (getValue == "Branch" && division == "Branch Admin")
            {
                Response.Redirect("Default.aspx");
            }
            else if (getValue == "Branch Admin" && division == "Branch Admin")
            {
                LoadCategorydrpdownList();
                btn_Authorize.Visible = true;
                //btn_Clear_Data.Visible = true;
                btnInsert.Visible = false;
                //btnClear.Visible = false;
            }
            else if (getValue == "Branch Admin" && division == "Branch")
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                btn_Authorize.Visible = false;
                // btn_Clear_Data.Visible = false;
            }

            LoadGridview();
            //}
            //else
            //{
            //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('No Modification is Possible,Already being Authorized.')", true);
            //    return;
            //    //Response.Redirect("Default.aspx");
            //}

            if (Session["UserId"] == null)
            {
                Response.Redirect("webLogin.aspx");
            }
        }
    }

    //Load GridView
    private void LoadGridview()
    {
        string condition = " ";
        listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
        if (getValue == "Branch Admin" && division == "Branch Admin")
        {
            condition = "WHERE ISS_USER_INFO.HO_STATUS = " + listatus + " AND ISS_BRANCH_INFO.BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_TRANSACTION.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' ORDER BY ISS_TRANSACTION.CATEGORY_ID";
        }
        else
        {
            //condition = "WHERE ISS_TRANSACTION.CATEGORY_ID = " + pssCategoryId + " AND ISS_BRANCH_INFO.BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_TRANSACTION.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' AND (ISS_SUBCATEGORY.BRANCH_STATUS = '1') ORDER BY ISS_TRANSACTION.CATEGORY_ID";
            condition = "WHERE ISS_TRANSACTION.CATEGORY_ID = " + drpCategory.SelectedValue + " AND ISS_BRANCH_INFO.BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_TRANSACTION.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' AND (ISS_SUBCATEGORY.BRANCH_STATUS = '1') ORDER BY ISS_TRANSACTION.CATEGORY_ID";
            //if (listatus == 0)
            //{
            //    condition = "WHERE ISS_TRANSACTION.CATEGORY_ID = " + drpCategory.SelectedValue + " AND ISS_USER_INFO.HO_STATUS = " + listatus + " AND ISS_BRANCH_INFO.BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_TRANSACTION.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "'";
            //}
            //else
            //{
            //    condition = "WHERE ISS_SUBCATEGORY.DIVISION = '" + division + "' AND ISS_USER_INFO.HO_STATUS = " + listatus + " AND ISS_BRANCH_INFO.BRANCH_CODE = '" + Session["brCode"] + "' AND ISS_TRANSACTION.AS_ON_DATE = '" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "'";
            //}
        }
        oTransactionDAL = new TransactionDAL();
        DataTable dt = oTransactionDAL.GetTransactionData(condition, listatus);
        gvTransaction.DataSource = dt;
        gvTransaction.DataBind();
        if (ViewState["Value"].ToString() == "Branch Admin")
        {
            for (int i = 0; i < gvTransaction.Rows.Count; i++)
            {
                gvTransaction.Rows[i].Cells[0].Enabled = false;
            }
        }
        else
        {
            string subcat = "", status = "";
            for (int i = 0; i < gvTransaction.Rows.Count; i++)
            {
                subcat = (gvTransaction.Rows[i].FindControl("SubCategoryId") as Label).Text;
                status = oTransactionDAL.GetoneReturnOneString("ISS_GL_NUMBER", "SUBCATEGORY_ID", subcat, "GLPL_NO");
                if (status.Length != 0)
                {
                    gvTransaction.Rows[i].Cells[6].BackColor = System.Drawing.Color.Bisque;
                }
            }
        }
    }

    //Load Category Data
    private void LoadCategorydrpdownList()
    {
        SqlConnection con;
        SqlCommand cmd;
        string sqlQuery = null;
        oTransactionDAL = new TransactionDAL();
        if (getValue == "Branch Admin" && division == "Branch Admin")
        {
            sqlQuery = "SELECT TOP 1 0 CATEGORY_ID, 'All Item' CATEGORY_NAME FROM ISS_CATEGORY " ;
            con = oConnectionDatabase.DatabaseConnection();
            cmd = new SqlCommand(sqlQuery, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch { }
            con.Close();
            drpCategory.DataSource = dt;
            drpCategory.DataTextField = "CATEGORY_NAME";
            drpCategory.DataValueField = "CATEGORY_ID";
            drpCategory.DataBind();
        }
        else
        {
            listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
            if (listatus == 1)
            {
                Condition = " WHERE HO_STATUS = 1";
            }
            else
            {
                Condition = " WHERE BRANCH_STATUS = 1";
            }
            DataTable dt = oTransactionDAL.GetCategory(Condition);
            drpCategory.DataSource = dt;
            drpCategory.DataTextField = "CATEGORY_NAME";
            drpCategory.DataValueField = "CATEGORY_ID";
            drpCategory.DataBind();            
        }
    }

    // Insert Button
    protected void btnInsert_Click(object sender, EventArgs e)
    {        
        double amount = 0,tranamount = 0 ;
        int localpssCategoryId =0;
        Double.TryParse(Convert.ToString(txtAmount.Text), out amount);
        Double.TryParse(Convert.ToString(txtAmount.Text), out tranamount);
        string message = "";
        if (division == "Branch")
        {
            AllSubCategorySearch();

            # region Unwanted Condition
            //if (drpSubCategory.SelectedValue == "13101")//Total Asset
            //{
            //    string condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('13103','14102')";
            //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
            //    double totalLibilities = 0, LoanOutstandingTotal = 0;
            //    for (int i = 0; i < SubData.Rows.Count; i++)
            //    {
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "13103")
            //        {
            //            totalLibilities = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14102")
            //        {
            //            LoanOutstandingTotal = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //    }
            //    if (amount != totalLibilities)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Asset is Not equal to Total Liabilities')", true);
            //        return;
            //    }
            //    if (amount < LoanOutstandingTotal)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Asset is less than Total Loan Outstanding')", true);
            //        return;
            //    }
            //}

            //else if (drpSubCategory.SelectedValue == "14104")// Overdue Loan Amount
            //{
            //    string condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14106','14108')";
            //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
            //    double NplOutstanding = 0, TotalSMALoan = 0;
            //    if (SubData.Rows.Count == 2)
            //    {
            //        for (int i = 0; i < SubData.Rows.Count; i++)
            //        {
            //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14106")
            //            {
            //                NplOutstanding = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //            }
            //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14108")
            //            {
            //                TotalSMALoan = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //            }
            //        }
            //        if (amount < (NplOutstanding + TotalSMALoan))
            //        {
            //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Overdue Loan Amount must equal or greater than (NPL+SMA) Total')", true);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Please Insert the Corresponding field's before')", true);
            //        return;
            //    }
            //}

            //else if (drpSubCategory.SelectedValue == "14151")// Total Declassified Loan Outstanding
            //{
            //    string condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14148')";
            //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
            //    double ReLoanOutstanding = 0;
            //    for (int i = 0; i < SubData.Rows.Count; i++)
            //    {
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14148")
            //        {
            //            ReLoanOutstanding = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //    }
            //    if (amount < ReLoanOutstanding)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Declassified Loan Outstanding must greater than Total Rescheduled Loan Outstanding ')", true);
            //        return;
            //    }
            //}
            //else if ((drpSubCategory.SelectedValue == "14139") || (drpSubCategory.SelectedValue == "14140") || (drpSubCategory.SelectedValue == "14141"))//Total Trade (non manufacturing) Loan Outstanding,Total Service Loan Outstanding,Total Industrial (manufactured) Loan Outstanding
            //{
            //    string condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14139','14140','14141','14102')";
            //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
            //    double TradeLoanOutstanding = 0, ServiceLoanOutstanding = 0, IndLoanOutstanding = 0, TotalLoanOutstanding = 0;
            //    for (int i = 0; i < SubData.Rows.Count; i++)
            //    {
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14139")
            //        {
            //            TradeLoanOutstanding = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14140")
            //        {
            //            ServiceLoanOutstanding = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14141")
            //        {
            //            IndLoanOutstanding = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //        if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14102")
            //        {
            //            TotalLoanOutstanding = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
            //        }
            //    }
            //    if (TotalLoanOutstanding > (TradeLoanOutstanding + ServiceLoanOutstanding + IndLoanOutstanding))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of total amount is must greater or equal to Total Loan Outstanding ')", true);
            //        return;
            //    }
            //}
            #endregion
        }
        cTransactionEntity = new TransactionEntity();
        //cTransactionEntity.TRAN_SL = gvRowId;
        cTransactionEntity.TRAN_SL = Convert.ToInt32(ViewState["gvRowId"]);
        cTransactionEntity.AS_ON_DATE =Convert.ToDateTime(ViewState["date"]).ToShortDateString();
        cTransactionEntity.TRAN_ID = pssTransactionId;
        cTransactionEntity.CATEGORY_ID = Convert.ToInt32(ViewState["CatId"]);
        //cTransactionEntity.CATEGORY_ID = pssCategoryId;
        //localpssCategoryId = cTransactionEntity.CATEGORY_ID;
        //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + pssCategoryId + "," + localpssCategoryId + "')", true);
                  
                    

        cTransactionEntity.SUBCATEGORY_ID = pssSubcategoryId;
        cTransactionEntity.ENTRY_DATE = DateTime.Today.ToShortDateString();
        Double.TryParse(Convert.ToString(txtAmount.Text), out tranamount);
        cTransactionEntity.AMOUNT = tranamount;
        //if (tranamount != 0)
        //{
        //    cTransactionEntity.AMOUNT = tranamount;
        //}
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Amount Zero is not allowed')", true);
        //    return;
        //}
        cTransactionEntity.USER_ID =  Session["UserId"].ToString();
        if (btnInsert.Text == "Save")
        {
            //int i = oTransactionDAL.InsertintoTable(cTransactionEntity);
            //if (i == 1)
            //{
            //    message = "Data Inserted Successfully";
            //    Clear();
            //    Int32.TryParse(Convert.ToString(drpCategory.SelectedValue), out pssCategoryId);
            //    Int32.TryParse(Convert.ToString(drpSubCategory.SelectedValue), out pssSubcategoryId);
            //    AllSubCategorySearch();
            //}
            //else
            //    message = "Data Not Inserted!";
        }
        else
        {
            int i = oTransactionDAL.UpdateIntoTable(cTransactionEntity);
            cTransactionEntity.ENTRY_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
            cTransactionEntity.BRANCH_CODE = Session["brCode"].ToString();
            cTransactionEntity.DIVISION = division;
            int MonitoringStatus = oTransactionDAL.InsertintoISS_TRAN_MONITORING_AUDIT(cTransactionEntity);
            if (i == 1)
            {               
                message = "Data Updated Successfully";
                if (division == "Branch")
                {
                    //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + pssCategoryId + "," + drpCategory.SelectedValue + "')", true);
                  
                   // LoadGridview();
                   LoadCategorydrpdownList();
                   drpCategory.SelectedValue = Convert.ToString(Convert.ToInt32(ViewState["CatId"]));
                   LoadGridview();
                   
                }
               // Clear();
            }
            else
                message = "Data Not Updated!";
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        LoadGridview();
    }

    // Clear All Fields
    public void Clear()
    {
        txtAmount.Text = "";
        txtAmount.Text = " ";
        Condition = null;
        LoadGridview();
        btnInsert.Text = "Update";
        gvRowId = 0;
        listatus = 0;
        txtAmount.ReadOnly = false;
    }
    
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
        if (division == "Branch")
        {
            LoadCategorydrpdownList();
        }
        LoadGridview();
    }
    protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        pssCategoryId = Convert.ToInt32(drpCategory.SelectedValue);
        //LoadSubCategorydrpdownList();
        AllSubCategorySearch();
        LoadGridview();
    }

    //Load SubCategory Data
    private void LoadSubCategorydrpdownList()
    {
        listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
        string lstrCategoryId = drpCategory.SelectedValue.ToString();
        string lstrCategoryName = drpCategory.SelectedItem.ToString();
        if (listatus == 1)
        {
            Condition = " WHERE HO_STATUS = 1 AND CATEGORY_ID = " + lstrCategoryId + " ";
        }
        else
        {
            Condition = " WHERE BRANCH_STATUS = 1 AND CATEGORY_ID = " + lstrCategoryId + "";
        }
        string brcode = Session["brCode"].ToString();
        string date = Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd");
        DataTable dt = oTransactionDAL.GetSubCategory(Condition, brcode, date);
        drpSubCategory.DataSource = dt;
        drpSubCategory.DataTextField = "SUBCATEGORY_NAME";
        drpSubCategory.DataValueField = "SUBCATEGORY_ID";
        drpSubCategory.DataBind();
    }
    protected void drpSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        AllSubCategorySearch();
    }

    //Search Condition for Subcategory
    private void AllSubCategorySearch()
    {
        if (drpSubCategory.SelectedValue == "11105" || drpSubCategory.SelectedValue == "11110")
        {
            lblAmount.Text = "Date";
        }
        else
        {
            lblAmount.Text = "Amount";
        }
        #region Condition
        //if (drpSubCategory.SelectedValue == "14102")//Total Loan Outstanding
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalLoanOutstanding();
        //}
        //else if (drpSubCategory.SelectedValue == "14106")//Total NPL Outstanding
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalNplOutstanding();
        //}
        //else if (drpSubCategory.SelectedValue == "14148")//Total Rescheduled Loan Outstanding
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalReLoanOutStanding();
        //}
        //else if (drpSubCategory.SelectedValue == "16101")//Net Income(+/-)
        //{
        //    txtAmount.ReadOnly = true;
        //    NetIncome();
        //}
        //else if (drpSubCategory.SelectedValue == "16102")//Total Income
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalIncome();
        //}
        //else if (drpSubCategory.SelectedValue == "16105")//Total Expenditure
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalExpenditure();
        //}
        //else if (drpSubCategory.SelectedValue == "13101")//Total Asset
        //{
        //    //txtAmount.ReadOnly = true;
        //    TotalAsset();
        //}
        //else if (drpSubCategory.SelectedValue == "14151")//Total Declassified Loan Outstanding
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalDeclassifiedLoanOutstanding();
        //}
        //else if ((drpSubCategory.SelectedValue == "14139") || (drpSubCategory.SelectedValue == "14140") || (drpSubCategory.SelectedValue == "14141"))//Total Trade (non manufacturing) Loan Outstanding,Total Service Loan Outstanding,Total Industrial (manufactured) Loan Outstanding
        //{
        //    txtAmount.ReadOnly = true;
        //    TotalTradeServiceIndLoanOutstanding();
        //}
        //else
        //{
        //    txtAmount.ReadOnly = false;
        //}
        #endregion
    }

    #region Unwanted Function......
    //private void TotalTradeServiceIndLoanOutstanding()
    //{
    //    string condition = "";
    //    string msg = "";
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14102')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 1)
    //    {
    //        int msgOne = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14102")
    //            {
    //                msgOne = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Loan Outstanding ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //}

    //private void TotalDeclassifiedLoanOutstanding()
    //{
    //    string condition = "";
    //    string msg = "";
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14148')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 1)
    //    {
    //        //string msg = "";
    //        int msgOne = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14148")
    //            {
    //                msgOne = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Rescheduled Loan Outstanding ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //}

    //private void TotalAsset()
    //{
    //    string condition = "";
    //    string msg = "";
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('13103','14102')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 2)
    //    {
    //        //string msg = "";
    //        int msgOne = 0, msgTwo = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "13103")
    //            {
    //                msgOne = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14102")
    //            {
    //                msgTwo = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Liability ";
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total Loan Outstanding " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }  
    //}

    
    //private void TotalExpenditure()
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    if (Session["brCode"].ToString() == "1001")
    //    {
    //        condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('16106','16107','16108','16109','16110','16113','16114','16115','16116')";
    //        DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //        if (SubData.Rows.Count < 9)
    //        {
    //            //string msg = "";
    //            int msgOne = 0, msgTwo = 0, msgThree = 0, msgFour = 0, msgFive = 0, msgSix = 0, msgSeven = 0, msgEight = 0, msgNine = 0;
    //            for (int i = 0; i < SubData.Rows.Count; i++)
    //            {
    //                if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16106")
    //                {
    //                    msgOne = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16107")
    //                {
    //                    msgTwo = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16108")
    //                {
    //                    msgThree = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16109")
    //                {
    //                    msgFour = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16110")
    //                {
    //                    msgFive = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16113")
    //                {
    //                    msgSix = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16114")
    //                {
    //                    msgSeven = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16115")
    //                {
    //                    msgEight = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16116")
    //                {
    //                    msgNine = 1;
    //                }
    //            }
    //            if (msgOne == 0)
    //            {
    //                msg = "Administrative Cost ";
    //            }
    //            if (msgTwo == 0)
    //            {
    //                msg = msg + "Interest Expenses " + " ";
    //            }
    //            if (msgThree == 0)
    //            {
    //                msg = msg + "Branch Renovation Cost " + " ";
    //            }
    //            if (msgFour == 0)
    //            {
    //                msg = msg + "Total Other Expenditure " + " ";
    //            }
    //            if (msgFive == 0)
    //            {
    //                msg = msg + "Total Business Promotion Expense " + " ";
    //            }
    //            if (msgSix == 0)
    //            {
    //                msg = msg + "Prepaid Expenses Other Than Tax " + " ";
    //            }
    //            if (msgSeven == 0)
    //            {
    //                msg = msg + "Annual Service Fee for Foreign Core Banking Solution " + " ";
    //            }
    //            if (msgEight == 0)
    //            {
    //                msg = msg + "Software Maintenance Expenses for Self Developed/ Indigenous Core Banking Solution " + " ";
    //            }
    //            if (msgNine == 0)
    //            {
    //                msg = msg + "Total CSR Expenses " + " ";
    //            }
    //            msg = "Please Insert " + msg;
    //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //        }
    //        else
    //        {
    //            for (int i = 0; i < SubData.Rows.Count; i++)
    //            {
    //                LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //            }
    //            txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //            txtAmount.ReadOnly = true;
    //        }
    //    }
    //    else
    //    {
    //        condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('16106','16107','16109','16110','16111')";
    //        DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //        if (SubData.Rows.Count < 5)
    //        {
    //            //string msg = "";
    //            int msgOne = 0, msgTwo = 0, msgThree = 0, msgFour = 0, msgFive = 0;
    //            for (int i = 0; i < SubData.Rows.Count; i++)
    //            {
    //                if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16106")
    //                {
    //                    msgOne = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16107")
    //                {
    //                    msgTwo = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16109")
    //                {
    //                    msgThree = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16110")
    //                {
    //                    msgFour = 1;
    //                }
    //                else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16111")
    //                {
    //                    msgFive = 1;
    //                }
    //            }
    //            if (msgOne == 0)
    //            {
    //                msg = "Administrative Cost ";
    //            }
    //            if (msgTwo == 0)
    //            {
    //                msg = msg + "Interest Expenses " + " ";
    //            }
    //            if (msgThree == 0)
    //            {
    //                msg = msg + "Branch Renovation Cost " + " ";
    //            }
    //            if (msgFour == 0)
    //            {
    //                msg = msg + "Total Other Expenditure " + " ";
    //            }
    //            if (msgFive == 0)
    //            {
    //                msg = msg + "Total Business Promotion Expense " + " ";
    //            }
    //            msg = "Please Insert " + msg;
    //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //        }
    //        else
    //        {
    //            for (int i = 0; i < SubData.Rows.Count; i++)
    //            {
    //                LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //            }
    //            txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //            txtAmount.ReadOnly = true;
    //        }
    //    }
    //}

    //private void TotalIncome()
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('16103','16104')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 2)
    //    {
    //        //string msg = "";
    //        int msgOne = 0, msgTwo = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16103")
    //            {
    //                msgOne = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16104")
    //            {
    //                msgTwo = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Interest Income ";
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total Non-interest Income " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        txtAmount.ReadOnly = true;
    //    }
    //}

    //private void NetIncome()
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    double TotalIncome = 0, TotalExp = 0;
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('16102','16105')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 2)
    //    {
    //        //string msg = "";
    //        int msgOne = 0, msgTwo = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16102")
    //            {
    //                msgOne = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16105")
    //            {
    //                msgTwo = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Income ";
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total Expenditure " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16102")
    //            {
    //                TotalIncome = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //            }
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "16105")
    //            {
    //                TotalExp = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //            }
    //            LoanOutstandingTotal = TotalIncome - TotalExp;
    //        }
    //        txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        txtAmount.ReadOnly = true;
    //    }
    //}

    //private void TotalReLoanOutStanding()
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14149','14150')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 2)
    //    {
    //        //string msg = "";
    //        int msgOne = 0, msgTwo = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14149")
    //            {
    //                msgOne = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14150")
    //            {
    //                msgTwo = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Rescheduled Loan Outstanding Presently UC ";
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total Rescheduled Loan Outstanding Presently NP " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        txtAmount.ReadOnly = true;
    //    }
    //}

    //private void TotalNplOutstanding()
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    pssSubcategoryId = Convert.ToInt32(drpSubCategory.SelectedValue);
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14109','14110','14111')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 3)
    //    {
    //        //string msg = "";
    //        int msgOne = 0, msgTwo = 0, msgThree = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14109")
    //            {
    //                msgOne = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14110")
    //            {
    //                msgTwo = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14111")
    //            {
    //                msgThree = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Substandard Loan ";
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total Doubtful Loan " + " ";
    //        }
    //        if (msgThree == 0)
    //        {
    //            msg = msg + "Total Bad/Loss Loan " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        txtAmount.ReadOnly = true;
    //    }
    //}

    //private void TotalLoanOutstanding()
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    pssSubcategoryId = Convert.ToInt32(drpSubCategory.SelectedValue);
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14107','14108','14109','14110','14111')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 5)
    //    {
    //        int msgOne = 0, msgTwo = 0, msgThree = 0, msgFour = 0, msgFive = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14107")
    //            {
    //                msgOne = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14108")
    //            {
    //                msgTwo = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14109")
    //            {
    //                msgThree = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14110")
    //            {
    //                msgFour = 1;
    //            }
    //            else if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14111")
    //            {
    //                msgFive = 1;
    //            }
    //        }
    //        if (msgOne == 0)
    //        {
    //            msg = "Total Standard Loan ";
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total SMA Loan " + " ";
    //        }
    //        if (msgThree == 0)
    //        {
    //            msg = msg + "Total Substandard Loan " + " ";
    //        }
    //        if (msgFour == 0)
    //        {
    //            msg = msg + "Total Doubtful Loan " + " ";
    //        }
    //        if (msgFive == 0)
    //        {
    //            msg = msg + "Total Bad/Loss Loan " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        txtAmount.ReadOnly = true;
    //    }
    //}
    #endregion

    protected void gvTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransaction.PageIndex = e.NewPageIndex;
        LoadGridview();
    }

    //GridView Selection
    protected void gvTransaction_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["gvRowId"] = gvTransaction.SelectedDataKey[0];
        Int32.TryParse(Convert.ToString(gvTransaction.SelectedDataKey[0]), out gvRowId);
        int liCatId = Convert.ToInt32 ((gvTransaction.SelectedRow.FindControl("CategoryId") as Label).Text);
        pssCategoryId = liCatId;
        ViewState["CatId"] = liCatId;
        string lstrCategory = (gvTransaction.SelectedRow.FindControl("CategoryName") as Label).Text;
        int liSubCatId = Convert.ToInt32((gvTransaction.SelectedRow.FindControl("SubCategoryId") as Label).Text);
        string SubcatId = Convert.ToString(liSubCatId);
        pssSubcategoryId = liSubCatId;
        string lstrSubCategory = (gvTransaction.SelectedRow.FindControl("SubCategoryName") as Label).Text;
        double ldAmount = Convert.ToDouble((gvTransaction.SelectedRow.FindControl("Amount") as Label).Text);
        drpCategory.Items.Clear();
        drpSubCategory.Items.Clear();
        drpCategory.Items.Add(lstrCategory);
        drpCategory.DataBind();
        drpSubCategory.Items.Add(lstrSubCategory);
        drpSubCategory.DataBind();
        btnInsert.Text = "Update";
        txtAmount.Text = Convert.ToString(ldAmount);
        string status = oTransactionDAL.GetoneReturnOneString("ISS_GL_NUMBER", "SUBCATEGORY_ID", SubcatId ,"GLPL_NO");
        if (status.Length == 0)
        {
            txtAmount.ReadOnly = false;
        }
        else
        {
            txtAmount.ReadOnly = true;
        }
        //if (division != "Branch")
        //{
        //    if (SubcatId == "12104" || SubcatId == "14167" || SubcatId == "16101" || SubcatId == "16102" || SubcatId == "16106")
        //    {
        //        txtAmount.ReadOnly = true;
        //    }
        //    else 
        //    {
        //        txtAmount.ReadOnly = false;
        //    }
        //}
    }
    protected void btn_Authorize_Click(object sender, EventArgs e)
    {
        string confirmValue = Request.Form["confirm_value"];
        string message = null,condition = null;
        if (confirmValue == "Yes")
        {
            listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");
            if (listatus == 0)
            {
                condition = " WHERE TRAN_ID = '" + pssTransactionId + "'  AND BRANCH_CODE = '" + Session["brCode"].ToString() + "'";
                int counter = oTransactionDAL.CountDataBRANCHWISE_MONITORING_MAINTENANCE(condition);
                //if (counter == 11)
                //{
                int Authorizestatus = 0;
                cTransactionEntity.TRAN_ID = pssTransactionId;
                cTransactionEntity.ENTRY_DATE = DateTime.Today.ToShortDateString();
                cTransactionEntity.BRANCH_CODE = Session["brCode"].ToString();
                cTransactionEntity.USER_ID = Session["UserId"].ToString();
                cTransactionEntity.AUTHO_STATUS = 1;
                cTransactionEntity.TYPE = "MONITORING";
                Authorizestatus = oTransactionDAL.InsertIntoISS_BRANCH_AUTHORIZATION(cTransactionEntity);
                if (Authorizestatus == 1)
                {
                    message = "Data Authorized Successfully";
                }
                else
                {
                    message = "Data Authorization is UnSuccessfully";
                }

                //}
                //else
                //{
                //    message = "Data Input is not Completed Yet.";
                //}
            }
            else
            {
                message = "Already Authorized";
            }
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
            Clear();
        }
        else
        {
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('You are not ready to Authorize!')", true);
        }
    }
    //protected void btn_Clear_Data_Click(object sender, EventArgs e)
    //{
    //    Clear();
    //    if (division == "Branch" || division == "Branch Admin")
    //    {
    //        LoadCategorydrpdownList();
    //    }
    //    LoadGridview();
    //}
}