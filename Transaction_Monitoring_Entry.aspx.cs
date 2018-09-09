/* Version:01.01
   Author: Kishor Kumar Saha
   Opearation: Monitoring Data Entry 
   create date: 22.07.2015
   Modification Date : 02.02.2016
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Transaction_Monitoring_Entry : System.Web.UI.Page
{
    protected static string pssTransactionId = null, Condition = null;

    protected static int pssCategoryId = 0, pssSubcategoryId = 0, listatus = 0,liMonitoringStatus = 0;
    protected static double ldTotalStandardLoan = 0, ldSMALoan = 0, ldTotalSubstandardLoan = 0, ldTotalDoubtfulLoan = 0, ldTotalBadLossLoan = 0, ldTotalNplOutstanding = 0, ldTotalLoanOutstanding = 0;
    protected static double ldTotalTradeLoanOutstanding = 0, ldTotalServiceLoanOutstanding = 0, ldTotalIndLoanOutstanding = 0, ldTotalTradeSerIndLoanOutStanding = 0;
    protected static double ReLoanOutstanding = 0, ReLoanOutstandingUC = 0, ReLoanOutstandingNP = 0,DeclassifiedLoanOutStanding = 0;
    protected static double NetIncome = 0, TotalIncome = 0,TotalInterestIncome = 0,TotalNonInterestIncome = 0,TotalExp = 0,AdminCost = 0,interestexp = 0;
    protected static double BrRenovationCost = 0,TotalOtherExp = 0,TotalBusinessPromExp = 0;
    protected static double BrMaintenanceExp = 0, PreExpOtherThnTax = 0, ASFForFCBS = 0, SoftMaintenanceExp = 0,TotalCSRExp = 0;
    protected static string division = null;

    protected static int gvRowId = 0;
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("webLogin.aspx");
        }
        int alivedate = oTransactionDAL.GetOneReturnOne("ISS_SFT_ALIVE", "SL_NO", "1","ALIVE_DATE");
        DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alivedate);
        DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
        if (Convert.ToDateTime(DateTime.Now) > fromDate && Convert.ToDateTime(DateTime.Now) < toDate)
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("webLogin.aspx");
            }
            DateTime date = DateTime.Now;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            pssTransactionId = date.ToString("yyyyMMdd");
            ViewState["date"] = date;
            txtAsonDate.Text = date.ToString("dd-MMM-yyyy");
            int checkAuthoStatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");
            if (checkAuthoStatus == 0)
            {
                listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
                if (listatus == 0)
                {
                    LoadCategorydrpdownList();
                }
                division = Session["DIVISION"].ToString();
                if (division != "Branch")
                {
                    lblCategory.Visible = false;
                    drpCategory.Visible = false;
                    LoadGridview();
                    AllSubCategorySearchforHO();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('No Entry is Possible,Already being Authorized.')", true);
                return;
            }
        }
    }

    private void LoadCategorydrpdownList()
    {
        oTransactionDAL = new TransactionDAL();
        listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
        if (listatus == 0)
        {
            //Condition = " WHERE HO_STATUS = 1";
            Condition = " WHERE BRANCH_STATUS = 1";
            //}
            //else
            //{
            //    Condition = " WHERE BRANCH_STATUS = 1";
            DataTable dt = oTransactionDAL.GetCategory(Condition);
            drpCategory.DataSource = dt;
            drpCategory.DataTextField = "CATEGORY_NAME";
            drpCategory.DataValueField = "CATEGORY_ID";
            drpCategory.DataBind();
        }
    }

    private void LoadGridview()
    {
        gvTransaction.DataSource = null;
        gvTransaction.DataBind();
        listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
        string condition = " ";
        if (listatus == 0)
        {
            liMonitoringStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", drpCategory.SelectedValue.ToString(), "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
        }
        else
        {
            liMonitoringStatus = oTransactionDAL.GetTwoReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "TRAN_ID", pssTransactionId, "DIVISION", Session["DIVISION"].ToString(), "INSERT_STATUS");
        }
        if (liMonitoringStatus != 1)
        {
            if (division == "Branch")
            {
                if (listatus == 1)
                {
                    condition = "WHERE ISS_CATEGORY.CATEGORY_ID = " + drpCategory.SelectedValue + " AND ISS_SUBCATEGORY.HO_STATUS = " + listatus + " ORDER BY ISS_SUBCATEGORY.HO_SEQ ";
                }
                else
                {
                    int liBrStatus = 1;
                    condition = "WHERE ISS_CATEGORY.CATEGORY_ID = " + drpCategory.SelectedValue + " AND ISS_SUBCATEGORY.BRANCH_STATUS = " + liBrStatus + " ORDER BY ISS_SUBCATEGORY.BR_SEQ ";
                }
            }
            else
            {
                condition = "WHERE ISS_SUBCATEGORY.DIVISION = '" + division + "' ORDER BY ISS_SUBCATEGORY.HO_SEQ ";
            }
            oTransactionDAL = new TransactionDAL();
            DataTable dt = oTransactionDAL.LoadMonitoringEntryGridView(condition,listatus);
            gvTransaction.DataSource = dt;
            gvTransaction.DataBind();
        }
    }
    protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        pssCategoryId = Convert.ToInt32(drpCategory.SelectedValue);
        if (pssCategoryId == 18 || pssCategoryId == 20 || pssCategoryId == 21 || pssCategoryId == 22)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "17", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                LoadGridview();
                AllSubCategorySearch();
            }
            else
            {
                LoadCategorydrpdownList();
                LoadGridview();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first complete the General Credit Information Head data')", true);
            }
        }
        else if (pssCategoryId == 36)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "37", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                LoadGridview();
                AllSubCategorySearch();
            }
            else
            {
                LoadCategorydrpdownList();
                LoadGridview();
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first complete the Expenditure Information Head data')", true);
            }
        }
        else
        {
            LoadGridview();
            AllSubCategorySearch();
        }
        
    }

    private void AllSubCategorySearch()
    {
        for (int i = 0; i < gvTransaction.Rows.Count; i++)
        {
            string SubcategoryId = (gvTransaction.Rows[i].FindControl("SubCategoryId") as Label).Text;

            #region Unwanted Code
            //if (SubcategoryId == "14102")//Total Loan Outstanding
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "14106")//Total NPL Outstanding
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "14148")//Total Rescheduled Loan Outstanding
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "16101")//Net Income(+/-)
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "16102")//Total Income
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "16105")//Total Expenditure
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = false;
            //}
            #endregion
        }
    }

    private void AllSubCategorySearchforHO()
    {
        for (int i = 0; i < gvTransaction.Rows.Count; i++)
        {
            string SubcategoryId = (gvTransaction.Rows[i].FindControl("SubCategoryId") as Label).Text;

            #region Unwanted Code
            //if (SubcategoryId == "12104")//Total Reserve
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "14167")//Loan Outstanding Under Off-shore Banking Unit
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "16101")//Net Income(+/-)
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "16102")//Total Income
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else if (SubcategoryId == "16106")//Administrative Cost
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = true;
            //    itm.BackColor = Color.CadetBlue;
            //    itm.Text = "ReadOnly Field";
            //}
            //else
            //{
            //    TextBox itm = (gvTransaction.Rows[i].FindControl("txtAmount") as TextBox);
            //    itm.ReadOnly = false;
            //}
            #endregion
        }
    }

    #region Function
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
    //        LoadCategorydrpdownList();
    //        LoadGridview();
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
    //        LoadCategorydrpdownList();
    //        LoadGridview();
    //    }
    //}

    //private void TotalAsset()
    //{
    //    string condition = "";
    //    string msg = "";
    //    condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14102')";
    //    //condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('13103','14102')";
    //    DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
    //    if (SubData.Rows.Count < 1)
    //    {
    //        //string msg = "";
    //        int msgTwo = 0;
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {

    //            if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14102")
    //            {
    //                msgTwo = 1;
    //            }
    //        }
    //        if (msgTwo == 0)
    //        {
    //            msg = msg + "Total Loan Outstanding " + " ";
    //        }
    //        msg = "Please Insert " + msg;
    //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + msg + "')", true);
    //        LoadCategorydrpdownList();
    //        LoadGridview();
    //    }
    //}

    //private void TotalExpenditure(int rowId)
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
    //            LoadCategorydrpdownList();
    //            LoadGridview();
    //        }
    //        else
    //        {
    //            for (int i = 0; i < SubData.Rows.Count; i++)
    //            {
    //                LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //            }
    //            TextBox itm = (gvTransaction.Rows[rowId].FindControl("txtAmount") as TextBox);
    //            itm.ReadOnly = true;
    //            gvTransaction.Rows[rowId].Cells[4].Text = Convert.ToString(LoanOutstandingTotal);
    //            //txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //            //txtAmount.ReadOnly = true;
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
    //            LoadCategorydrpdownList();
    //            LoadGridview();
    //        }
    //        else
    //        {
    //            for (int i = 0; i < SubData.Rows.Count; i++)
    //            {
    //                LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //            }
    //            TextBox itm = (gvTransaction.Rows[rowId].FindControl("txtAmount") as TextBox);
    //            itm.ReadOnly = true;
    //            gvTransaction.Rows[rowId].Cells[4].Text = Convert.ToString(LoanOutstandingTotal);
    //            //txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //            //txtAmount.ReadOnly = true;
    //        }
    //    }
    //}

    //private void TotalReLoanOutStanding(int rowId)
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
    //        LoadCategorydrpdownList();
    //        LoadGridview();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        TextBox itm = (gvTransaction.Rows[rowId].FindControl("txtAmount") as TextBox);
    //        itm.ReadOnly = true;
    //        gvTransaction.Rows[rowId].Cells[4].Text = Convert.ToString(LoanOutstandingTotal);
    //        //txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        //txtAmount.ReadOnly = true;
    //    }
    //}

    //private void TotalNplOutstanding(int rowId)
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    //pssSubcategoryId = Convert.ToInt32(drpSubCategory.SelectedValue);
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
    //        LoadCategorydrpdownList();
    //        LoadGridview();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        TextBox itm = (gvTransaction.Rows[rowId].FindControl("txtAmount") as TextBox);
    //        itm.ReadOnly = true;
    //        gvTransaction.Rows[rowId].Cells[4].Text = Convert.ToString(LoanOutstandingTotal);
    //        //txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        //txtAmount.ReadOnly = true;
    //    }
    //}

    //private void TotalLoanOutstanding(int rowId)
    //{
    //    string condition = "";
    //    string msg = "";
    //    double LoanOutstandingTotal = 0;
    //    //pssSubcategoryId = Convert.ToInt32(drpSubCategory.SelectedValue);
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
    //        LoadCategorydrpdownList();
    //        LoadGridview();
    //    }
    //    else
    //    {
    //        for (int i = 0; i < SubData.Rows.Count; i++)
    //        {
    //            LoanOutstandingTotal = LoanOutstandingTotal + Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
    //        }
    //        TextBox itm = (gvTransaction.Rows[rowId].FindControl("txtAmount") as TextBox);
    //        itm.ReadOnly = true;
    //        gvTransaction.Rows[rowId].Cells[4].Text = Convert.ToString(LoanOutstandingTotal);
    //        //txtAmount.Text = Convert.ToString(LoanOutstandingTotal);
    //        //txtAmount.ReadOnly = true;
    //    }
    //}
    #endregion

    private int InsertCheckCondition()
    {
        int returnStatus = 1;
        if (pssCategoryId == 14)
        {
            double amount = 0, ldTotalAsset = 0;
            for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
            {
                string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                if (SubcategoryId == "14101")//Total Asset
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalAsset);
                    ldTotalAsset = Math.Round(ldTotalAsset,2);
                    //string condition = " where BRANCH_CODE='" + Session["brCode"].ToString() + "' and AS_ON_DATE='" + Convert.ToDateTime(ViewState["date"]).ToString("yyyy-MM-dd") + "' and SUBCATEGORY_ID in('14102')";
                    //DataTable SubData = oTransactionDAL.CheckTransactionData(condition);
                    //double LoanOutstandingTotal = 0;
                    //for (int i = 0; i < SubData.Rows.Count; i++)
                    //{
                    //    if (SubData.Rows[i]["SUBCATEGORY_ID"].ToString().Trim() == "14102")
                    //    {
                    //        LoanOutstandingTotal = Convert.ToDouble(SubData.Rows[i]["AMOUNT"]);
                    //    }
                    //}
                }
                else if (SubcategoryId == "14102")//Total Fixed Asset
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out amount);
                    amount = Math.Round(amount, 2);
                    if (amount >= ldTotalAsset)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Fixed Asset Must less than Total Asset')", true);
                        returnStatus = 0;
                    }
                }
                else if (SubcategoryId == "14116")//Total Other Asset
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out amount);
                    amount = Math.Round(amount, 2);
                    if (amount >= ldTotalAsset)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Other Asset Must less than Total Asset')", true);
                        returnStatus = 0;
                    }
                }
                //else if (SubcategoryId == "13103")//Total Liabilites
                //{
                //    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out totalLibilities);
                //    if (ldTotalAsset != totalLibilities)
                //    {
                //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Asset is Not equal to Total Liabilities')", true);
                //        returnStatus = 0;
                //    }
                //}
            }
        }
        if (pssCategoryId == 17)
        {
            double ldOverdueLoan = 0, ldNPLOutstand = 0;
            for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
            {
                string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                if (SubcategoryId == "17104")//Overdue Loan Amount
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldOverdueLoan);
                    ldOverdueLoan = Math.Round(ldOverdueLoan, 2);
                }
                else if (SubcategoryId == "17106")//Total NPL Outstanding
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldNPLOutstand);
                    ldNPLOutstand = Math.Round(ldNPLOutstand, 2);
                    if (ldOverdueLoan < ldNPLOutstand)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Overdue Loan Amount Must Greater than or Equal to Total NPL Outstanding')", true);
                        returnStatus = 0;
                    }
                }
            }
        }
        if (pssCategoryId == 18)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "17", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                double ldLoanOutstanding = 0, ldNPLOutstanding = 0 , ldValue = 0 , ldIntSusBalance = 0 , ldIntSusAgnstLoan = 0 ;
                for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
                {
                    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                    if (SubcategoryId == "18101") //Total Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldValue);
                        ldLoanOutstanding = ldLoanOutstanding + ldValue;
                    }
                    else if (SubcategoryId == "18102") //Total Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldValue);
                        ldLoanOutstanding = ldLoanOutstanding + ldValue;
                    }
                    else if (SubcategoryId == "18103") //Total Loan Outstanding & Total NPL Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldValue);
                        ldLoanOutstanding = ldLoanOutstanding + ldValue;
                        ldNPLOutstanding = ldNPLOutstanding + ldValue;
                    }
                    else if (SubcategoryId == "18104") //Total Loan Outstanding & Total NPL Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldValue);
                        ldLoanOutstanding = ldLoanOutstanding + ldValue;
                        ldNPLOutstanding = ldNPLOutstanding + ldValue;
                    }
                    else if (SubcategoryId == "18105") //Total Loan Outstanding & Total NPL Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldValue);
                        ldLoanOutstanding = ldLoanOutstanding + ldValue;
                        ldNPLOutstanding = ldNPLOutstanding + ldValue;
                    }
                    else if (SubcategoryId == "18108") //Total Interest Suspense Against Loan
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldIntSusAgnstLoan);
                        ldIntSusAgnstLoan = Math.Round(ldIntSusAgnstLoan, 2);
                    }
                    else if (SubcategoryId == "18109") //Total Interest Suspense Balance
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldIntSusBalance);
                        ldIntSusBalance = Math.Round(ldIntSusBalance, 2);
                    }
                }
                if (ldIntSusBalance < ldIntSusAgnstLoan)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Interest Suspense Balance Must Greater than or Equal to Total Interest Suspense Against Loan')", true);
                    returnStatus = 0;
                }
                Condition = "WHERE TRAN_ID = " + pssTransactionId + " AND SUBCATEGORY_ID = 17102 AND BRANCH_CODE =  '" + Session["brCode"].ToString() +"' ";
                double LoanOutstanding = oTransactionDAL.GetSingleSubcatData(Condition);
                ldLoanOutstanding = Math.Round(ldLoanOutstanding, 2);
                if (LoanOutstanding != ldLoanOutstanding)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Standard Loan + Total SMA Loan + Total Substandard Loan + Total Doubtful Loan + Total Bad Loan) must equal Total Loan Outstanding')", true);
                    returnStatus = 0;
                }
                else
                {
                    Condition = "WHERE TRAN_ID = " + pssTransactionId + " AND SUBCATEGORY_ID = 17106 AND BRANCH_CODE =  '" + Session["brCode"].ToString() + "' ";
                    double NPLOutstanding = oTransactionDAL.GetSingleSubcatData(Condition);
                    ldNPLOutstanding = Math.Round(ldNPLOutstanding, 2);
                    if (NPLOutstanding != ldNPLOutstanding)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Substandard Loan + Total Doubtful Loan + Total Bad Loan) must equal Total NPL Outstanding')", true);
                        returnStatus = 0;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first input the General Credit Information data')", true);
                returnStatus = 0;
            }
        }
        if (pssCategoryId == 20)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "17", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                double ldmafndindlnout = 0, ldservicelnout = 0, ldnonmafndtrdlnout = 0, ldTotalAmount = 0;
                for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
                {
                    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                    if (SubcategoryId == "20101")//Total Manufacturing and Industrial Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldmafndindlnout);
                        ldmafndindlnout = Math.Round(ldmafndindlnout, 2);
                    }
                    else if (SubcategoryId == "20102")//Total Service Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldservicelnout);
                        ldservicelnout = Math.Round(ldservicelnout, 2);
                    }
                    else if (SubcategoryId == "20103")//Total Non-Manufacturing and Trade Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldnonmafndtrdlnout);
                        ldnonmafndtrdlnout = Math.Round(ldnonmafndtrdlnout, 2);
                    }
                }
                Condition = "WHERE TRAN_ID = " + pssTransactionId + " AND SUBCATEGORY_ID = 17102 AND BRANCH_CODE =  '" + Session["brCode"].ToString() + "' ";
                double LoanOutstanding = oTransactionDAL.GetSingleSubcatData(Condition);
                ldTotalAmount = Math.Round((ldmafndindlnout + ldservicelnout + ldnonmafndtrdlnout), 2);
                if (LoanOutstanding != ldTotalAmount)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Manufacturing and Industrial Loan Outstanding + Total Service Loan Outstanding + Total Non-Manufacturing and Trade Loan Outstanding) must equal Total Loan Outstanding')", true);
                    returnStatus = 0;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first input the General Credit Information data')", true);
                returnStatus = 0;
            }
        }
        if (pssCategoryId == 21)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "17", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                double ldtotalassetbaclnout = 0, ldtotalguaranteebaclnout = 0, ldTotalAmount = 0;
                for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
                {
                    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                    if (SubcategoryId == "21101")//Total Asset backed Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalassetbaclnout);
                        ldtotalassetbaclnout = Math.Round(ldtotalassetbaclnout, 2);
                    }
                    else if (SubcategoryId == "21102")//Total Guarantee Backed(and Unsecured) Loan Outstanding 
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalguaranteebaclnout);
                        ldtotalguaranteebaclnout = Math.Round(ldtotalguaranteebaclnout, 2);
                    }
                }
                Condition = "WHERE TRAN_ID = " + pssTransactionId + " AND SUBCATEGORY_ID = 17102 AND BRANCH_CODE =  '" + Session["brCode"].ToString() + "' ";
                double LoanOutstanding = oTransactionDAL.GetSingleSubcatData(Condition);
                ldTotalAmount = Math.Round((ldtotalassetbaclnout + ldtotalguaranteebaclnout), 2);
                if (LoanOutstanding != ldTotalAmount)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Asset backed Loan Outstanding + Total Guarantee Backed(and Unsecured) Loan Outstanding) must equal Total Loan Outstanding')", true);
                    returnStatus = 0;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first input the General Credit Information data')", true);
                returnStatus = 0;
            }
        }
        if (pssCategoryId == 22)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "17", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                double ldshorttermlnout = 0, ldmediumtermlnout = 0, ldlongtermlnout = 0, ldTotalAmount = 0;
                for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
                {
                    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                    if (SubcategoryId == "22101")//Short Term Loan Outstanding 
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldshorttermlnout);
                        ldshorttermlnout = Math.Round(ldshorttermlnout, 2);
                    }
                    else if (SubcategoryId == "22102")//Medium Term Loan Outstanding 
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldmediumtermlnout);
                        ldmediumtermlnout = Math.Round(ldmediumtermlnout, 2);
                    }
                    else if (SubcategoryId == "22103")//Long Term Loan Outstanding
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldlongtermlnout);
                        ldlongtermlnout = Math.Round(ldlongtermlnout, 2);
                    }
                }
                Condition = "WHERE TRAN_ID = " + pssTransactionId + " AND SUBCATEGORY_ID = 17102 AND BRANCH_CODE =  '" + Session["brCode"].ToString() + "' ";
                double LoanOutstanding = oTransactionDAL.GetSingleSubcatData(Condition);
                ldTotalAmount = Math.Round((ldshorttermlnout + ldmediumtermlnout + ldlongtermlnout), 2);
                if (LoanOutstanding != ldTotalAmount)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Short Term Loan Outstanding + Medium Term Loan Outstanding + Long Term Loan Outstanding) must equal Total Loan Outstanding')", true);
                    returnStatus = 0;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first input the General Credit Information data')", true);
                returnStatus = 0;
            }
        }
        if (pssCategoryId == 28)
        {
            double ldresclnout = 0, ldresclnoutUC = 0, ldresclnoutNP = 0, lddeclasslnout = 0 , ldamount = 0;
            for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
            {
                string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                if (SubcategoryId == "28101")//Total Rescheduled Loan Outstanding
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldresclnout);
                    ldresclnout = Math.Round(ldresclnout, 2);
                }
                else if (SubcategoryId == "28102")//Total Rescheduled Loan Outstanding Presently UC 
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldresclnoutUC);
                    ldresclnoutUC = Math.Round(ldresclnoutUC, 2);
                }
                else if (SubcategoryId == "28103")//Total Rescheduled Loan Outstanding Presently NP
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldresclnoutNP);
                    ldresclnoutNP = Math.Round(ldresclnoutNP, 2);
                }
                else if (SubcategoryId == "28104")//Total Declassified Loan Outstanding
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out lddeclasslnout);
                    lddeclasslnout = Math.Round(lddeclasslnout, 2);
                }
            }
            ldamount = Math.Round((ldresclnoutUC + ldresclnoutNP), 2);
            if (ldresclnout != ldamount)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Rescheduled Loan Outstanding Presently UC + Total Rescheduled Loan Outstanding Presently NP) must equal Total Rescheduled Loan Outstanding')", true);
                returnStatus = 0;
            }
            else
            {
                if (lddeclasslnout < ldresclnout)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Declassified Loan Outstanding must equal  or greater than Total Rescheduled Loan Outstanding')", true);
                    returnStatus = 0;
                }
            }
        }
        if (pssCategoryId == 31)
        {
            double ldissuedbnkgur = 0, ldtotalpergurlocal = 0, ldtotalothergurlocal = 0, ldtotalpergurforeign = 0,ldtotalothergurforeign = 0, ldamount = 0;
            for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
            {
                string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                if (SubcategoryId == "31110")//Total Outstanding Balance of Issued Bank Guarantee
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldissuedbnkgur);
                    ldissuedbnkgur = Math.Round(ldissuedbnkgur, 2);
                }
                else if (SubcategoryId == "31111")//Total Performance Guarantee Local 
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalpergurlocal);
                    ldtotalpergurlocal = Math.Round(ldtotalpergurlocal, 2);
                }
                else if (SubcategoryId == "31112")//Total Other Guarantee Local
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalothergurlocal);
                    ldtotalothergurlocal = Math.Round(ldtotalothergurlocal, 2);
                }
                else if (SubcategoryId == "31113")//Total Performance Guarantee Foreign
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalpergurforeign);
                    ldtotalpergurforeign = Math.Round(ldtotalpergurforeign, 2);
                }
                else if (SubcategoryId == "31114")//Total Other Guarantee Foreign
                {
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalothergurforeign);
                    ldtotalothergurforeign = Math.Round(ldtotalothergurforeign, 2);
                }
            }
            ldamount = Math.Round((ldtotalpergurlocal + ldtotalothergurlocal + ldtotalpergurforeign + ldtotalothergurforeign), 2);
            if (ldissuedbnkgur != ldamount)
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Performance Guarantee Local + Total Other Guarantee Local + Total Performance Guarantee Foreign + Total Other Guarantee Foreign) must equal Total Outstanding Balance of Issued Bank Guarantee')", true);
                returnStatus = 0;
            }
        }
        if (pssCategoryId == 36)
        {
            int generalcreditinfoStatus = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "CATEGORY_ID", "37", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "INSERT_STATUS");
            if (generalcreditinfoStatus == 1)
            {
                double ldTotalIncome = 0, ldtotalInterestIncome = 0, ldtotalnoninterestincome = 0, ldnetinterestincome = 0, ldamount = 0;
                for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
                {
                    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
                    if (SubcategoryId == "36101")//Total Income
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalIncome);
                        ldTotalIncome = Math.Round(ldTotalIncome, 2);
                    }
                    else if (SubcategoryId == "36102")//Total Interest Income
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalInterestIncome);
                        ldtotalInterestIncome = Math.Round(ldtotalInterestIncome, 2);
                    }
                    else if (SubcategoryId == "36103")//Total Non-interest Income
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldtotalnoninterestincome);
                        ldtotalnoninterestincome = Math.Round(ldtotalnoninterestincome, 2);
                    }
                    else if (SubcategoryId == "36105")//Net Interest Income
                    {
                        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldnetinterestincome);
                        ldnetinterestincome = Math.Round(ldnetinterestincome, 2);
                    }
                }
                ldamount = Math.Round((ldtotalInterestIncome + ldtotalnoninterestincome), 2);
                if (ldTotalIncome != ldamount)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Interest Income + Total Non-interest Income) must equal Total Income')", true);
                    returnStatus = 0;
                }
                else
                {
                    Condition = "WHERE TRAN_ID = " + pssTransactionId + " AND SUBCATEGORY_ID = 37101 AND BRANCH_CODE =  '" + Session["brCode"].ToString() + "' ";
                    double InterestExpense = oTransactionDAL.GetSingleSubcatData(Condition);
                    ldamount = Math.Round((ldtotalInterestIncome - InterestExpense), 2);
                    if (ldnetinterestincome != ldamount)
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your (Total Interest Income - Total Interest Expenses) must equal Net Interest Income')", true);
                        returnStatus = 0;
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your need to first input the Expenditure Information data')", true);
                returnStatus = 0;
            }
        }

        #region Currently not needed
        //if (pssCategoryId == 14)
        //{
        ////TakeGridViewValueForCredit();

        //    ldTotalNplOutstanding = ldTotalSubstandardLoan + ldTotalDoubtfulLoan + ldTotalBadLossLoan;
        //    ldTotalLoanOutstanding = ldTotalStandardLoan + ldSMALoan + ldTotalSubstandardLoan + ldTotalDoubtfulLoan + ldTotalBadLossLoan;
        //    ldTotalTradeSerIndLoanOutStanding = ldTotalTradeLoanOutstanding + ldTotalServiceLoanOutstanding + ldTotalIndLoanOutstanding;
        //    ReLoanOutstanding = ReLoanOutstandingUC + ReLoanOutstandingNP;

        //    for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
        //    {
        //        string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out amount);
        //        if (SubcategoryId == "14102")// Total Loan Outstanding
        //        {
        //            (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(ldTotalLoanOutstanding);
        //        }
        //        //else if (SubcategoryId == "14104")// Overdue Loan Amount
        //        //{
        //        //    if (amount < (ldTotalNplOutstanding + ldSMALoan))
        //        //    {
        //        //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Overdue Loan Amount must equal or greater than (NPL+SMA) Total')", true);
        //        //        returnStatus = 0;
        //        //    }
        //        //}
        //        else if (SubcategoryId == "14104")// Overdue Loan Amount
        //        {
        //            if (amount < (ldTotalNplOutstanding))
        //            {
        //                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Overdue Loan Amount must equal or greater than NPL')", true);
        //                returnStatus = 0;
        //            }
        //        }
        //        else if (SubcategoryId == "14106")// Total NPL Outstanding
        //        {
        //            (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(ldTotalNplOutstanding);
        //        }
        //        else if (SubcategoryId == "14148")// Total Rescheduled Loan Outstanding
        //        {
        //            (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(ReLoanOutstanding);
        //        }
        //        else if (SubcategoryId == "14151")// Total Declassified Loan Outstanding
        //        {
        //            if (DeclassifiedLoanOutStanding < ReLoanOutstanding)
        //            {
        //                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Declassified Loan Outstanding must greater than Total Rescheduled Loan Outstanding ')", true);
        //                returnStatus = 0;
        //            }
        //        }
        //    }
        //    if (ldTotalTradeSerIndLoanOutStanding < ldTotalLoanOutstanding)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of total amount is must greater or equal to Total Loan Outstanding ')", true);
        //        returnStatus = 0;
        //    }
        //}
        //if (pssCategoryId == 16)
        //{
        //    //TakeGridViewValueForIncomeExpenses();
        //    TotalIncome = TotalInterestIncome + TotalNonInterestIncome;
        //    if (Session["brCode"].ToString() == "1001")
        //    {
        //        TotalExp = AdminCost + interestexp + BrMaintenanceExp + BrRenovationCost + TotalOtherExp + PreExpOtherThnTax + ASFForFCBS + SoftMaintenanceExp + TotalCSRExp;
        //        NetIncome = TotalIncome - TotalExp;
        //    }
        //    else
        //    {
        //        TotalExp = AdminCost + interestexp + BrRenovationCost + TotalOtherExp + TotalBusinessPromExp;
        //        NetIncome = TotalIncome - TotalExp;
        //    }
        //    for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
        //    {
        //        string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
        //        if (SubcategoryId == "16101")
        //        {
        //            (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(NetIncome);
        //        }
        //        else if (SubcategoryId == "16102")
        //        {
        //            (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(TotalIncome);
        //        }
        //        else if (SubcategoryId == "16105")
        //        {
        //            (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(TotalExp);
        //        }
        //    }
        //}
        #endregion

        return returnStatus;
    }
  
    //private void TakeGridViewValueForIncomeExpenses()
    //{        
    //    for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
    //    {
    //        string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
    //        if (SubcategoryId == "16103")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out TotalInterestIncome);
    //        }
    //        if (SubcategoryId == "16104")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out TotalNonInterestIncome);
    //        }
    //        if (SubcategoryId == "16106")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out AdminCost);
    //        }
    //        if (SubcategoryId == "16107")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out interestexp);
    //        }
    //        if (SubcategoryId == "16109")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out BrRenovationCost);
    //        }
    //        if (SubcategoryId == "16110")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out TotalOtherExp);
    //        }

    //        if (Session["brCode"].ToString() == "1001")
    //        {                              
    //            if (SubcategoryId == "16108")
    //            {
    //                Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out BrMaintenanceExp);
    //            }                               
    //            if (SubcategoryId == "16113")
    //            {
    //                Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out PreExpOtherThnTax);
    //            }
    //            if (SubcategoryId == "16114")
    //            {
    //                Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ASFForFCBS);
    //            }
    //            if (SubcategoryId == "16115")
    //            {
    //                Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out SoftMaintenanceExp);
    //            }
    //            if (SubcategoryId == "16116")
    //            {
    //                Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out TotalCSRExp);
    //            }

    //        }
    //        else
    //        {
    //            if (SubcategoryId == "16111")
    //            {
    //                Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out TotalBusinessPromExp);
    //            }
    //        }
    //    }
    //}

    //private void TakeGridViewValueForCredit()
    //{
    //    for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
    //    {
    //        string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
    //        if (SubcategoryId == "14107")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalStandardLoan);
    //        }
    //        if (SubcategoryId == "14108")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldSMALoan);
    //        }
    //        if (SubcategoryId == "14109")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalSubstandardLoan);
    //        }
    //        if (SubcategoryId == "14110")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalDoubtfulLoan);
    //        }
    //        if (SubcategoryId == "14111")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalBadLossLoan);
    //        }

    //        if (SubcategoryId == "14139")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalTradeLoanOutstanding);
    //        }
    //        if (SubcategoryId == "14140")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalServiceLoanOutstanding);
    //        }
    //        if (SubcategoryId == "14141")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ldTotalIndLoanOutstanding);
    //        }

    //        if (SubcategoryId == "14149")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ReLoanOutstandingUC);
    //        }
    //        if (SubcategoryId == "14150")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out ReLoanOutstandingNP);
    //        }
    //        if (SubcategoryId == "14151")
    //        {
    //            Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out DeclassifiedLoanOutStanding);
    //        }
    //    }
    //}
    

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        double tranamount = 0;
        int SubcategoryId = 0, InsertStatus = 0, AuditStatus = 0, MonitoringStatus = 0,liInsertCheck = 0;
        Int32.TryParse(Convert.ToString(drpCategory.SelectedValue), out pssCategoryId);
        string message = "";
        //AllSubCategorySearch();
        //for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
        //{
        //    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out fieldValue);
        //    if (fieldValue > 0)
        //    {
        //        fieldStatus = 1;
        //        break;
        //    }
        //}
        //if (fieldStatus == 1)
        //{
            int status = 0;
            listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
            if (listatus == 0)
            {
                status = InsertCheckCondition();
            }
            else
            {
                if (division == "FAD")
                {
                    status = InsertCheckConditionforFAD();
                }
                else
                {
                    status = 1;
                }
            }
            if (status == 1)
            {
                cTransactionEntity = new TransactionEntity();

                for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
                {
                    cTransactionEntity.AS_ON_DATE = Convert.ToDateTime(ViewState["date"]).ToShortDateString();
                    cTransactionEntity.TRAN_ID = pssTransactionId;
                    cTransactionEntity.ENTRY_DATE = DateTime.Today.ToShortDateString();
                    cTransactionEntity.DIVISION = division;
                    SubcategoryId = Convert.ToInt32((gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text);
                    Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out tranamount);
                    cTransactionEntity.SUBCATEGORY_ID = SubcategoryId;
                    if (listatus == 1)
                    {
                        string subcategory = Convert.ToString(SubcategoryId);
                        Int32.TryParse(Convert.ToString(subcategory.Substring(0, 2)), out pssCategoryId);
                        cTransactionEntity.CATEGORY_ID = pssCategoryId;
                    }
                    else
                    {
                        cTransactionEntity.CATEGORY_ID = pssCategoryId;
                    }
                    cTransactionEntity.AMOUNT = tranamount;
                    cTransactionEntity.ENTRY_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
                    cTransactionEntity.USER_ID = Session["UserId"].ToString();
                    string catid = Convert.ToString(pssCategoryId);
                    liInsertCheck = oTransactionDAL.GetThreeReturnOne("BRANCHWISE_MONITORING_MAINTENANCE", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "CATEGORY_ID", catid, "INSERT_STATUS");
                    if (liInsertCheck == 0)
                    {
                        InsertStatus = oTransactionDAL.InsertintoTable(cTransactionEntity);
                        AuditStatus = oTransactionDAL.InsertintoISS_TRAN_MONITORING_AUDIT(cTransactionEntity);
                    }
                }
                cTransactionEntity.INSERT_STATUS = 1;
                cTransactionEntity.UPDATE_STATUS = 0;
                cTransactionEntity.BRANCH_CODE = Session["brCode"].ToString();
                if (InsertStatus == 1 && AuditStatus == 1)
                {
                    MonitoringStatus = oTransactionDAL.InsertIntoBRANCHWISE_MONITORING_MAINTENANCE(cTransactionEntity);
                    message = "Data Inserted Successfully";
                    Clear();
                }
                else
                {
                    message = "Data Not Inserted / Already Inserted..";
                }
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
            }
        //}
        else
        {
            message = "Please Insert the Field Value First.";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        }
    }

    private int InsertCheckConditionforFAD()
    {
        int returnvalue = 1;
        #region Unwanted Code
        //double statutory_reserve = 0, Asset_Revaluation_Reserve = 0, Other_Reserve = 0, Interest_Income = 0, Non_Interest_Income = 0,Total_Income =0;
        //double Total_Expenditure = 0, Interest_Expense = 0, Branch_Maintenance_Expense = 0, Branch_Renovation_Cost = 0, Other_Expenditure = 0;
        //double Business_Promotion_Expense = 0,CSR_Expenses = 0,Loan_Outstanding_Under_Offsore_Banking = 0,Total_Reserve = 0;
        //double Net_Income = 0,Administrative_Cost = 0;
        //for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
        //{
        //    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
        //    if (SubcategoryId == "12105")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out statutory_reserve);
        //    }
        //    else if (SubcategoryId == "12106")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Asset_Revaluation_Reserve);
        //    }
        //    else if (SubcategoryId == "12107")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Other_Reserve);
        //    }
        //    else if (SubcategoryId == "14142")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Loan_Outstanding_Under_Offsore_Banking);
        //    }
        //    else if (SubcategoryId == "16103")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Interest_Income);
        //    }
        //    else if (SubcategoryId == "16104")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Non_Interest_Income);
        //    }
        //    else if (SubcategoryId == "16105")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Total_Expenditure);
        //    }
        //    else if (SubcategoryId == "16107")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Interest_Expense);
        //    }
        //    else if (SubcategoryId == "16108")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Branch_Maintenance_Expense);
        //    }
        //    else if (SubcategoryId == "16109")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Branch_Renovation_Cost);
        //    }
        //    else if (SubcategoryId == "16110")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Other_Expenditure);
        //    }
        //    else if (SubcategoryId == "16111")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out Business_Promotion_Expense);
        //    }
        //    else if (SubcategoryId == "16116")
        //    {
        //        Double.TryParse(Convert.ToString((gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text), out CSR_Expenses);
        //    }
        //}
        //Total_Reserve = statutory_reserve + Asset_Revaluation_Reserve + Other_Reserve;
        //Total_Income = Interest_Income + Non_Interest_Income;
        //Net_Income = Total_Income - Total_Expenditure;
        //Administrative_Cost = Total_Expenditure - Interest_Expense - Branch_Maintenance_Expense - Branch_Renovation_Cost - Other_Expenditure - Business_Promotion_Expense - CSR_Expenses;
        //for (int gridRow = 0; gridRow < gvTransaction.Rows.Count; gridRow++)
        //{
        //    string SubcategoryId = (gvTransaction.Rows[gridRow].FindControl("SubCategoryId") as Label).Text;
        //    if (SubcategoryId == "12104")
        //    {
        //        (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(Total_Reserve);
        //    }
        //    else if (SubcategoryId == "14167")
        //    {
        //        (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(Loan_Outstanding_Under_Offsore_Banking);
        //    }
        //    else if (SubcategoryId == "16101")
        //    {
        //        (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(Net_Income);
        //    }
        //    else if (SubcategoryId == "16102")
        //    {
        //        (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(Total_Income);
        //    }
        //    else if (SubcategoryId == "16106")
        //    {
        //        (gvTransaction.Rows[gridRow].FindControl("txtAmount") as TextBox).Text = Convert.ToString(Administrative_Cost);
        //        returnvalue = 1;
        //    }
        //}
        #endregion
        return returnvalue;
    }

    private void Clear()
    {
        LoadCategorydrpdownList();
        LoadGridview();
    }
    protected void gvTransaction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransaction.PageIndex = e.NewPageIndex;
        LoadGridview();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }

    public System.Drawing.Color color { get; set; }
    //protected void gvTransaction_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}
}