/* Version:01.01
 * Author: Kishor Kumar Saha
 Opearation: Report Generation 
 create date: 14.07.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

public partial class Report : System.Web.UI.Page
{
    SqlConnection con;
    SqlCommand cmd;
    string sqlQuery = null;
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected static string Condition = "";
    protected static int hostatus = 0;
    protected static string division = null;
    protected static string pssTransactionId = null;
    protected static string ReportPrefix = null;
    protected static int ExcelStatus = 0, CSVStatus = 0;
    protected static double ldtotlnOut = 0, ldtotnplOut = 0, ldovrduelnAmount = 0, ldtotresculedlnOut = 0, ldtotdeclassifiedlnOut = 0, ldtotOutofAccepIssued = 0, ldtotOutBlnceofIssedBnkGrnt = 0;
    protected static double ldConditionOne = 0, ldConditionTwo = 0, ldConditionThree = 0, ldConditionFour = 0, ldConditionFive = 0;    
    protected static double ldConditionSix = 0, ldConditionSeven = 0, ldConditionEight = 0,ldConditionNine = 0, ldConditionTen = 0;   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("webLogin.aspx");
        }
        if (!IsPostBack)
        {
            DateTime date = DateTime.Now;
            if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            }
            pssTransactionId = date.ToString("yyyyMMdd");
            division = Session["DIVISION"].ToString();
            
            hostatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
            String getValue = Request.QueryString["value"];
            if (hostatus == 1 && getValue == "HOMonitoring")
            {
                if (division != "HO")
                {
                    MonitoringRadioButton.Visible = false;
                    HOMonitoringReport.Checked = true;
                    MonitoringRadioButton.Checked = false;
                }
                rdbtnAcceptance.Visible = false;
                DateTime reportDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                toDateTextBox.Text = reportDate.ToString("MMM-yyyy");
                //toDateTextBox.Text = date.ToString("MMM-yyyy");
                LoadBranchName();
            }
            else if (hostatus == 1 && getValue == "HOAcceptance")
            {
                MonitoringRadioButton.Visible = false;
                HOMonitoringReport.Visible = false;
                HOMonitoringReport.Checked = false;
                MonitoringRadioButton.Checked = false;
                rdbtnAcceptance.Checked = true;
                DateTime reportDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                toDateTextBox.Text = reportDate.ToString("MMM-yyyy");
                //toDateTextBox.Text = DateTime.Now.ToString("MMM-yyyy");
                LoadBranchName();
            }
            else if (hostatus == 0 && getValue == "BranchMonitoring")
            {
                rdbtnAcceptance.Visible = false;
                HOMonitoringReport.Visible = false;
                DateTime reportDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                toDateTextBox.Text = reportDate.ToString("MMM-yyyy");
                //toDateTextBox.Text = DateTime.Now.ToString("MMM-yyyy");
                LoadBranchName();
            }
            else if (hostatus == 0 && getValue == "BranchAcceptance")
            {
                MonitoringRadioButton.Visible = false;
                HOMonitoringReport.Visible = false;
                HOMonitoringReport.Checked = false;
                MonitoringRadioButton.Checked = false;
                rdbtnAcceptance.Checked = true;
                DateTime reportDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                toDateTextBox.Text = reportDate.ToString("MMM-yyyy");
                //toDateTextBox.Text = DateTime.Now.ToString("MMM-yyyy");
                LoadBranchName();
            }
            else
            {
                Response.Redirect("Default2.aspx");
            }
        }
    }

    private void LoadBranchName()
    {
        //if (division == "Branch" || division == "HO")
        //{
            int listatus = 0;
            oTransactionDAL = new TransactionDAL();
            listatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
            if (listatus == 1)
            {
                Condition = "1";
            }
            else
            {
                Condition = " WHERE (ISS_USER_INFO.BR_STATUS = 1 AND ISS_USER_INFO.USER_ID = '" + Session["UserId"] + "')";
            }
            DataTable dt = oTransactionDAL.GetBranchName(Condition);
            branchDropDownList.DataSource = dt;
            branchDropDownList.DataTextField = "BRANCH_NAME";
            branchDropDownList.DataValueField = "BRANCH_CODE";
            branchDropDownList.DataBind();
            if (listatus == 1)
            {
                branchDropDownList.SelectedValue = "1001";
            }
        //}
        //else
        //{
        //    branchDropDownList.Items.Add(division);
        //}
    }
    protected void branchDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (branchDropDownList.SelectedValue == "1001")
        {
            HOMonitoringReport.Visible = true;
        }
        else
        {
            HOMonitoringReport.Visible = false;
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        ExcelStatus = 1;
        CSVStatus = 0;
        if (Request.QueryString["value"] == "HOMonitoring" || Request.QueryString["value"] == "BranchMonitoring")
        {
            pickReqData();
            int status = chkCondition();
            if (status > 0)
            {
                ReportGeneration();
            }
        }
        else
        {
            ReportGeneration();
        }
    }
    public void ExportToExcel(DataTable dt, string reportName)
    {
        #region Export to Excel
        string BranchId = oTransactionDAL.GetoneReturnOneString("ISS_BRANCH_INFO", "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "BRANCH_HO_ID");
        if (dt.Rows.Count > 0)
        {
            string filename = reportName + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43." || ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
            {
                hw.RenderBeginTag(HtmlTextWriterTag.Table);
                hw.RenderBeginTag(HtmlTextWriterTag.Tr);
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");

                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("BEG");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("1106");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43.")
                {
                    hw.Write("T_PS_M_FI_MONITOR_BR");
                }
                else if (ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
                {
                    hw.Write("T_PS_M_FI_MONITOR_HO");
                }
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("2");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("M");
                hw.RenderEndTag();
                //hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                //hw.RenderBeginTag(HtmlTextWriterTag.Td);
                //hw.Write(" ");
                //hw.RenderEndTag();                
                //hw.RenderBeginTag(HtmlTextWriterTag.Td);
                //hw.Write(" ");
                //hw.RenderEndTag();
                //hw.RenderBeginTag(HtmlTextWriterTag.Td);
                //hw.Write(" ");
                //hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("43");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(BranchId);
                hw.RenderEndTag();

                hw.RenderEndTag();
                hw.RenderEndTag();
            }
            else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
            {
                hw.RenderBeginTag(HtmlTextWriterTag.Table);
                hw.RenderBeginTag(HtmlTextWriterTag.Tr);

                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("BEG");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("1");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(" ");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("M");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43.")
                {
                    hw.Write("T_PS_M_FI_ACCEPTANCE_BR");
                }
                else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
                {
                    hw.Write("T_PS_M_FI_ACCEPTANCE_HO");
                }
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(" ");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(" ");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(DateTime.Now.ToString("dd-MMM-yyyy"));
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(" ");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(" ");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write(" ");
                hw.RenderEndTag();
                hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
                hw.RenderBeginTag(HtmlTextWriterTag.Td);
                hw.Write("List of Banks with ID");
                hw.RenderEndTag();

                hw.RenderEndTag();
                hw.RenderEndTag();
            }
            GridView dgGrid = new GridView();
            dgGrid.DataSource = dt;
            dgGrid.DataBind();
            if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43." || ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
            {
                if (dgGrid.HeaderRow.Cells[8].Text == "Blank_One")
                {
                    dgGrid.HeaderRow.Cells[8].Text = "";
                }
                if (dgGrid.HeaderRow.Cells[9].Text == "Blank_Two")
                {
                    dgGrid.HeaderRow.Cells[9].Text = "";
                }
            }
            else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
            {
                if (dgGrid.HeaderRow.Cells[9].Text == "Blank_One")
                {
                    dgGrid.HeaderRow.Cells[9].Text = "";
                }
            }
            for (int i = 0; i < dgGrid.Rows.Count; i++)
            {
                if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43." || ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
                {
                    dgGrid.Rows[i].Cells[5].Attributes.Add("style", "mso-number-format:\\@");
                }
                else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
                {
                    dgGrid.Rows[i].Cells[4].Attributes.Add("style", "mso-number-format:\\@");
                    dgGrid.Rows[i].Cells[5].Attributes.Add("style", "mso-number-format:\\@");
                    dgGrid.Rows[i].Cells[6].Attributes.Add("style", "mso-number-format:\\@");
                    dgGrid.Rows[i].Cells[7].Attributes.Add("style", "mso-number-format:\\@");
                    dgGrid.Rows[i].Cells[8].Attributes.Add("style", "mso-number-format:\\@");
                }
            }
            //Get the HTML for the control.
            dgGrid.RenderControl(hw);
            //Write the HTML back to the browser.
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();
        }

        #endregion
        #region Export to ExcelOLD

        //if (dt.Rows.Count > 0)
        //{
        //    string filename = reportName + ".xls";
        //    System.IO.StringWriter tw = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //    if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43." || ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        //    {
        //        hw.RenderBeginTag(HtmlTextWriterTag.Table);
        //        hw.RenderBeginTag(HtmlTextWriterTag.Tr);
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");

        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("BEG");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("1");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("M");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43.")
        //        {
        //            hw.Write("T_PS_M_FI_MONITOR_BR");
        //        }
        //        else if (ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        //        {
        //            hw.Write("T_PS_M_FI_MONITOR_HO");
        //        }
        //        hw.RenderEndTag();
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(DateTime.Now.ToString("dd-MMM-yyyy"));
        //        hw.RenderEndTag();

        //        hw.RenderEndTag();
        //        hw.RenderEndTag();
        //    }
        //    else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //    {
        //        hw.RenderBeginTag(HtmlTextWriterTag.Table);
        //        hw.RenderBeginTag(HtmlTextWriterTag.Tr);

        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("BEG");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("1");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("M");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43.")
        //        {
        //            hw.Write("T_PS_M_FI_ACCEPTANCE_BR");
        //        }
        //        else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //        {
        //            hw.Write("T_PS_M_FI_ACCEPTANCE_HO");
        //        }
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(DateTime.Now.ToString("dd-MMM-yyyy"));
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write(" ");
        //        hw.RenderEndTag();
        //        hw.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //        hw.AddAttribute(HtmlTextWriterAttribute.Border, "2");
        //        hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //        hw.Write("List of Banks with ID");
        //        hw.RenderEndTag();

        //        hw.RenderEndTag();
        //        hw.RenderEndTag();
        //    }
        //    GridView dgGrid = new GridView();
        //    dgGrid.DataSource = dt;
        //    dgGrid.DataBind();
        //    if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43." || ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        //    {
        //        if (dgGrid.HeaderRow.Cells[8].Text == "Blank_One")
        //        {
        //            dgGrid.HeaderRow.Cells[8].Text = "";
        //        }
        //        if (dgGrid.HeaderRow.Cells[9].Text == "Blank_Two")
        //        {
        //            dgGrid.HeaderRow.Cells[9].Text = "";
        //        }
        //    }
        //    else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //    {
        //        if (dgGrid.HeaderRow.Cells[9].Text == "Blank_One")
        //        {
        //            dgGrid.HeaderRow.Cells[9].Text = "";
        //        }
        //    }
        //    for (int i = 0; i < dgGrid.Rows.Count; i++)
        //    {
        //        if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43." || ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        //        {
        //            dgGrid.Rows[i].Cells[5].Attributes.Add("style", "mso-number-format:\\@");
        //        }
        //        else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //        {
        //            dgGrid.Rows[i].Cells[4].Attributes.Add("style", "mso-number-format:\\@");
        //            dgGrid.Rows[i].Cells[5].Attributes.Add("style", "mso-number-format:\\@");
        //            dgGrid.Rows[i].Cells[6].Attributes.Add("style", "mso-number-format:\\@");
        //            dgGrid.Rows[i].Cells[7].Attributes.Add("style", "mso-number-format:\\@");
        //            dgGrid.Rows[i].Cells[8].Attributes.Add("style", "mso-number-format:\\@");
        //        }
        //    }
        //    //Get the HTML for the control.
        //    dgGrid.RenderControl(hw);
        //    //Write the HTML back to the browser.
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
        //    this.EnableViewState = false;
        //    Response.Write(tw.ToString());
        //    Response.End();
        //}

        #endregion
    }

    public void ExportToCSV(DataTable dt, string reportName)
    {
        #region Export to CSV
        string BranchId = oTransactionDAL.GetoneReturnOneString("ISS_BRANCH_INFO", "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "BRANCH_HO_ID");
        Response.ContentType = "application/csv";
        Response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".csv");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        GridView grdDisplay = new GridView();
        string strValue = string.Empty;
        grdDisplay.AllowPaging = false;
        grdDisplay.DataSource = dt;
        grdDisplay.DataBind();
        string csvData = "";
        int ColumnNo = dt.Columns.Count;
        if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43.")
        {
            csvData = "BEG,1106,T_PS_M_FI_MONITOR_BR,2,M," + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + ",43," + BranchId + ",,,,,,,,,,,,CONVENTIONAL";
        }
        else if (ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        {
            csvData = "BEG,1,M,,T_PS_M_FI_MONITOR_HO,,," + DateTime.Now.ToString("dd-MMM-yy") + ",,,,,,,,,,,CONVENTIONAL";
        }
        else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43.")
        {
            csvData = "BEG,1110,T_PS_M_FI_ACCEPTANCE_BR,2,M," + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + ",43," + BranchId + ",,,List of Banks with ID";
        }
        else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        {
            csvData = "BEG,1,,M,T_PS_M_FI_ACCEPTANCE_HO,,," + DateTime.Now.ToString("dd-MMM-yy") + ",,,List of Banks with ID";
        }
        csvData += "\n";
        int row_no = 0;
        for (int j = 0; j < ColumnNo; j++)
        {
            if (dt.Columns[j].ColumnName == "Blank_One")
            {
                csvData += "" + ",";
            }
            else if (dt.Columns[j].ColumnName == "Blank_Two")
            {
                csvData += "" + ",";
            }
            else
            {
                csvData += dt.Columns[j].ColumnName + ",";
            }
        }
        if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43.")
        {
            csvData += ",,,,,,ISLAMIC" + "\n";
        }
        else if (ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        {
            csvData += ",,,,,ISLAMIC" + "\n";
        }
        else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        {
            csvData += ",Date Format" + "\n";
        }
        while (row_no < dt.Rows.Count)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName == "Field Description")
                {
                    csvData += dt.Rows[row_no][i].ToString().Replace(",", "");
                }

                else if (dt.Columns[i].ColumnName == "OFFICE_IND")
                {
                    if (dt.Rows[row_no][i].ToString() == "Head Office")
                    {
                        csvData += "HEAD OFFICE" + ",";
                    }
                    else
                    {
                        csvData += "BRANCH OFFICE" + ",";
                    }
                }
                else
                {
                    csvData += dt.Rows[row_no][i].ToString().Trim() + ",";
                }
            }
            if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
            {
                if (row_no == 0)
                {
                    csvData += ",DD-Mon-YYYY";
                }
            }
            row_no++;
            csvData += "\n";
        }
        Response.Write(csvData.ToString());
        Response.End();

        #endregion
        #region Export to CSVOLD

        //Response.ContentType = "application/csv";
        //Response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".csv");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //GridView grdDisplay = new GridView();
        //string strValue = string.Empty;
        //grdDisplay.AllowPaging = false;
        //grdDisplay.DataSource = dt;
        //grdDisplay.DataBind();
        //string csvData = "";
        //int ColumnNo = dt.Columns.Count;
        //if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43.")
        //{
        //    csvData = "BEG,1,M,,T_PS_M_FI_MONITOR_BR,,," + DateTime.Now.ToString("dd-MMM-yy") + ",,,,,,,,,,,,CONVENTIONAL";
        //}
        //else if (ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        //{
        //    csvData = "BEG,1,M,,T_PS_M_FI_MONITOR_HO,,," + DateTime.Now.ToString("dd-MMM-yy") + ",,,,,,,,,,,CONVENTIONAL";
        //}
        //else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43.")
        //{
        //    csvData = "BEG,1,,M,T_PS_M_FI_ACCEPTANCE_BR,,," + DateTime.Now.ToString("dd-MMM-yy") + ",,,List of Banks with ID";
        //}
        //else if(ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //{
        //    csvData = "BEG,1,,M,T_PS_M_FI_ACCEPTANCE_HO,,," + DateTime.Now.ToString("dd-MMM-yy") + ",,,List of Banks with ID";
        //}
        //csvData += "\n";
        //int row_no = 0;
        //for (int j = 0; j < ColumnNo; j++)
        //{
        //    if (dt.Columns[j].ColumnName == "Blank_One")
        //    {
        //        csvData += "" + ",";
        //    }
        //    else if (dt.Columns[j].ColumnName == "Blank_Two")
        //    {
        //        csvData += "" + ",";
        //    }
        //    else
        //    {
        //        csvData += dt.Columns[j].ColumnName + ",";
        //    }
        //}
        //if (ReportPrefix == "T_PS_M_FI_MONITOR_BR.43.")
        //{
        //    csvData += ",,,,,,ISLAMIC" + "\n";
        //}
        //else if (ReportPrefix == "T_PS_M_FI_MONITOR_HO.43.")
        //{
        //    csvData += ",,,,,ISLAMIC" + "\n";
        //}
        //else if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." ||ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //{
        //    csvData += ",Date Format" + "\n";
        //}
        //while (row_no < dt.Rows.Count)
        //{
        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        if (dt.Columns[i].ColumnName == "Field Description")
        //        {
        //            csvData += dt.Rows[row_no][i].ToString().Replace(",", "");
        //        }

        //        else if (dt.Columns[i].ColumnName == "OFFICE_IND")
        //        {
        //            if (dt.Rows[row_no][i].ToString() == "Head Office")
        //            {
        //                csvData += "HEAD OFFICE" + ",";
        //            }
        //            else
        //            {
        //                csvData += "BRANCH OFFICE" + ",";
        //            }
        //        }
        //        else
        //        {
        //            csvData += dt.Rows[row_no][i].ToString().Trim() + ",";
        //        }
        //    }
        //    if (ReportPrefix == "T_PS_M_FI_ACCEPTANCE_BR.43." || ReportPrefix == "T_PS_M_FI_ACCEPTANCE_HO.43.")
        //    {
        //        if (row_no == 0)
        //        {
        //            csvData += ",DD-Mon-YYYY";
        //        }
        //    }
        //    row_no++;
        //    csvData += "\n";
        //}
        //Response.Write(csvData.ToString());
        //Response.End();

        #endregion
    }

    protected void btnCSV_Click(object sender, EventArgs e)
    {
        ExcelStatus = 0;
        CSVStatus = 1;
        if (Request.QueryString["value"] == "HOMonitoring" || Request.QueryString["value"] == "BranchMonitoring")
        {
            //ReportGeneration();
            pickReqData();
            int status = chkCondition();
            if (status > 0)
            {
                ReportGeneration();
            }
        }
        else
        {
            ReportGeneration();
        }
    }

    public void ReportGeneration()
    {
        string dateformat = "";
        string reportName = "";
        string value = branchDropDownList.SelectedValue.ToString();
        DateTime date = Convert.ToDateTime(DateTime.Now.Day + "-" + toDateTextBox.Text);
        date = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        dateformat = date.ToString("yyyy-MM-dd");
        string tranid = date.ToString("yyyyMMdd");
        DataTable dt = new DataTable();
        int listatus = 0;
        string BranchId = oTransactionDAL.GetoneReturnOneString("ISS_BRANCH_INFO", "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "BRANCH_HO_ID");
        if (MonitoringRadioButton.Checked || HOMonitoringReport.Checked)
        {
            //if (branchDropDownList.SelectedValue == "1001")
            if (division == "HO")
            {
                //listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");
                listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", tranid, "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");
                if (MonitoringRadioButton.Checked)
                {
                    if (branchDropDownList.SelectedValue == "1001")
                    {
                        dt = oTransactionDAL.GetHoReportData(dateformat, "");
                        ReportPrefix = "HO_Monitoring_Branchwise";
                        reportName = ReportPrefix + date.ToString("ddMMyyyy");
                    }
                    else
                    {
                        if (listatus == 1)
                        {
                            dt = oTransactionDAL.GetBranchReportData(dateformat, branchDropDownList.SelectedValue.ToString(),1);
                            ReportPrefix = "T_PS_M_FI_MONITOR_BR.43.";
                            reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
                        }
                        else
                        {
                            //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Till Now Authorization is not Completed')", true);
                            //return;
                            dt = oTransactionDAL.GetBranchReportData(dateformat, branchDropDownList.SelectedValue.ToString(), 0);
                            ReportPrefix = "T_PS_M_FI_MONITOR_BR.43.";
                            reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
                        }
                    }
                }
                else if (HOMonitoringReport.Checked)
                {
                    dt = oTransactionDAL.GetBranchReportData(dateformat, "1001",1);
                    ReportPrefix = "T_PS_M_FI_MONITOR_HO.43.";
                    reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
                }
            }
            else if (division == "Branch" || division == "Branch Admin")
            {
                //listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");
                listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", tranid, "BRANCH_CODE", Session["brCode"].ToString(), "TYPE", "MONITORING", "AUTHO_STATUS");
                if (listatus == 1)
                {
                    dt = oTransactionDAL.GetBranchReportData(dateformat, branchDropDownList.SelectedValue.ToString(), listatus);
                    ReportPrefix = "T_PS_M_FI_MONITOR_BR.43.";
                    reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
                }
                else
                {
                    dt = oTransactionDAL.GetBranchReportData(dateformat, branchDropDownList.SelectedValue.ToString(), listatus);
                    ReportPrefix = "T_PS_M_FI_MONITOR_BR.43.";
                    reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
                    //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Till Now Authorization is not Completed')", true);
                    //return;
                }
            }
            else
            {
                dt = oTransactionDAL.GetHoDivisionReportData(dateformat, "1001", division);
                ReportPrefix = "Division_Monitoring";
                reportName = ReportPrefix + date.ToString("ddMMyyyy");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = i + 1; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[i]["Data category"].ToString().Trim() == dt.Rows[j]["Data category"].ToString().Trim())
                    {
                        dt.Rows[j]["Data category"] = "";
                    }
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["DATE"].ToString() == "")
                {
                    dt.Rows[i]["DATE"] = date.ToString("dd-MMM-yyyy");
                    dt.Rows[i]["BANK_ID"] = "43";
                    if (dt.Columns[2].ColumnName.ToString() == "HO_ID")
                    {
                        dt.Rows[i]["HO_ID"] = BranchId;
                    }
                    else if (dt.Columns[2].ColumnName.ToString() == "BRANCH_ID")
                    {
                        dt.Rows[i]["BRANCH_ID"] = BranchId;
                    }
                    dt.Rows[i]["AMOUNT_BDT"] = "0";
                    dt.Rows[i]["OFFICE_IND"] = branchDropDownList.SelectedItem.ToString();
                }
            }
        }
        else
        {
            //listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "TYPE", "ACCEPTANCE", "AUTHO_STATUS");
            listatus = oTransactionDAL.GetThreeReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", tranid, "BRANCH_CODE", branchDropDownList.SelectedValue.ToString(), "TYPE", "ACCEPTANCE", "AUTHO_STATUS");
            if (branchDropDownList.SelectedValue == "1001")
            {
                //dt = oTransactionDAL.GetHoAcceptanceReportData(dateformat);
                dt = oTransactionDAL.GetBranchAcceptancdeReportData(dateformat, branchDropDownList.SelectedValue.ToString(),1);
                ReportPrefix = "T_PS_M_FI_ACCEPTANCE_HO.43.";
                reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
            }
            else
            {
                //if (listatus == 1)
                //{
                    dt = oTransactionDAL.GetBranchAcceptancdeReportData(dateformat, branchDropDownList.SelectedValue.ToString(), listatus);
                    ReportPrefix = "T_PS_M_FI_ACCEPTANCE_BR.43.";
                    reportName = ReportPrefix + BranchId + "." + date.ToString("yyyyMMdd");
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Till Now Authorization is not Completed')", true);
                //    return;
                //}
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["DATE"].ToString() == "")
                {
                    dt.Rows[i]["DATE"] = date.ToString("dd-MMM-yyyy");
                    dt.Rows[i]["BANK_ID"] = "43";
                    if (dt.Columns[2].ColumnName.ToString() == "HO_ID")
                    {
                        dt.Rows[i]["HO_ID"] = BranchId;
                    }
                    else if (dt.Columns[2].ColumnName.ToString() == "BRANCH_ID")
                    {
                        dt.Rows[i]["BRANCH_ID"] = BranchId;
                    }
                    dt.Rows[i]["VALUE OF ACCEPTANCE ISSUED AMOUNT"] = "0";
                    dt.Rows[i]["VALUE OF ISSUED ACCEPTANCE MATURED"] = "0";
                    dt.Rows[i]["VALUE OF RECEIVED ACCEPTANCE"] = "0";
                    dt.Rows[i]["PURCHASED AMOUNT OF RECEIVED ACCEPTANCE"] = "0";
                    dt.Rows[i]["MATURED OF RECEIVED ACCEPTANCE"] = "0"; 
                }
            }
        }

        if (ExcelStatus == 1)
        {
            ExportToExcel(dt, reportName);
        }
        if (CSVStatus == 1)
        {
            ExportToCSV(dt, reportName);
        }
    }

    public void pickReqData()
    {
        ldConditionOne = 0.0;
        ldConditionTwo = 0.0;
        ldConditionThree = 0.0;
        ldConditionFour = 0.0;
        ldConditionFive = 0.0;
        ldConditionSix = 0.0;
        ldConditionSeven = 0.0;
        ldConditionEight = 0.0;
        ldConditionNine = 0.0;
        ldConditionTen = 0.0;
        string Subcategory = "";
        double balance = 0;
        string BranchCode = branchDropDownList.SelectedValue.ToString();
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT * FROM [ISS].[dbo].[ISS_TRANSACTION] WHERE TRAN_ID = '" + pssTransactionId + "' AND BRANCH_CODE = '" + BranchCode + "' AND SUBCATEGORY_ID IN ('17102','17104','17106','18101','18102','18103','18104','18105','20101','20102','20103','21101','21102','22101','22102','22103','28101','28102','28103','28104','31101','31102','31103','31104','31110','31111','31112','31113','31114') ";
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                try
                {
                    Subcategory = Convert.ToString(dr["SUBCATEGORY_ID"]);
                    balance = Convert.ToDouble(dr["AMOUNT"]);
                    if (Subcategory == "17102")
                    {
                        ldtotlnOut = balance;
                    }
                    else if (Subcategory == "17104")
                    {
                        ldovrduelnAmount = balance;
                    }
                    else if (Subcategory == "17106")
                    {
                        ldtotnplOut = balance;
                    }
                    else if (Subcategory == "28101")
                    {
                        ldtotresculedlnOut = balance;
                    }
                    else if (Subcategory == "28104")
                    {
                        ldtotdeclassifiedlnOut = balance;
                    }
                    else if (Subcategory == "31104")
                    {
                        ldtotOutofAccepIssued = balance;
                    }
                    else if (Subcategory == "31110")
                    {
                        ldtotOutBlnceofIssedBnkGrnt = balance;
                    }
                    else if (Subcategory == "18101" || Subcategory == "18102" || Subcategory == "18103" || Subcategory == "18104" || Subcategory == "18105")
                    {
                        if (Subcategory == "18101" || Subcategory == "18102")
                        {
                            ldConditionOne = ldConditionOne + balance;
                        }
                        else
                        {
                            ldConditionOne = ldConditionOne + balance;
                            ldConditionTwo = ldConditionTwo + balance;
                        }
                    }
                    else if (Subcategory == "20101" || Subcategory == "20102" || Subcategory == "20103")
                    {
                        ldConditionFour = ldConditionFour + balance;
                    }
                    else if (Subcategory == "21101" || Subcategory == "21102")
                    {
                        ldConditionFive = ldConditionFive + balance;
                    }
                    else if (Subcategory == "22101" || Subcategory == "22102" || Subcategory == "22103")
                    {
                        ldConditionSix = ldConditionSix + balance;
                    }
                    else if (Subcategory == "28102" || Subcategory == "28103")
                    {
                        ldConditionSeven = ldConditionSeven + balance;
                    }
                    else if (Subcategory == "31101" || Subcategory == "31102" || Subcategory == "31103")
                    {
                        ldConditionNine = ldConditionNine + balance;
                    }
                    else if (Subcategory == "31111" || Subcategory == "31112" || Subcategory == "31113"||Subcategory == "31114")
                    {
                        ldConditionTen = ldConditionTen + balance;
                    }
                }
                catch { }
            }
            dr.Close();
        }
        con.Close();
    }
    private int chkCondition()
    {
        int returnStatus = 1;

        //Condition One
        ldtotlnOut = Math.Round((ldtotlnOut), 2);
        ldConditionOne = Math.Round((ldConditionOne), 2);
        if (ldtotlnOut != ldConditionOne)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Standard Loan + Total SMA Loan + Total Substandard Loan + Total Doubtful Loan + Total Bad Loan) must equal to Total Loan Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Two
        ldtotnplOut = Math.Round((ldtotnplOut), 2);
        ldConditionTwo = Math.Round((ldConditionTwo), 2);       
        if (ldtotnplOut != ldConditionTwo)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Substandard Loan + Total Doubtful Loan + Total Bad Loan) must equal to Total NPL Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Three
        ldovrduelnAmount = Math.Round((ldovrduelnAmount), 2);
        if (ldovrduelnAmount < ldtotnplOut)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Overdue Loan Amount must Greater than/Equal to Total NPL Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Four
        ldConditionFour = Math.Round((ldConditionFour), 2);        
        if (ldtotlnOut != ldConditionFour)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Manufacturing and Industrial Loan Outstanding + Total Service Loan Outstanding + Total Non-Manufacturing and Trade Loan Outstanding) must equal to Total Loan Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Five
        ldConditionFive = Math.Round((ldConditionFive), 2);
        if (ldtotlnOut != ldConditionFive)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Asset backed Loan Outstanding + Total Guarantee Backed(and Unsecured) Loan Outstanding) must equal to Total Loan Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Six
        ldConditionSix = Math.Round((ldConditionSix), 2);
        //value.Text = Convert.ToString(Math.Round(ldtotlnOut, 2) - Math.Round(ldConditionSix,2));
        if (ldtotlnOut != ldConditionSix)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Short Term Loan Outstanding + Medium Term Loan Outstanding + Long Term Loan Outstanding) must equal to Total Loan Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Seven
        ldConditionSeven = Math.Round((ldConditionSeven), 2);
        ldtotresculedlnOut = Math.Round((ldtotresculedlnOut), 2);
        if (ldtotresculedlnOut != ldConditionSeven)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Rescheduled Loan Outstanding Presently UC + Total Rescheduled Loan Outstanding Presently NP) must equal to Total Rescheduled Loan Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Eight
        ldtotdeclassifiedlnOut = Math.Round((ldtotdeclassifiedlnOut), 2);
        ldtotresculedlnOut = Math.Round((ldtotresculedlnOut), 2);
        if (ldtotdeclassifiedlnOut < ldtotresculedlnOut)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Total Declassified Loan Outstanding must Greater than/Equal to Total Rescheduled Loan Outstanding')", true);
            returnStatus = 0;
        }
        //Condition Nine
        ldtotOutofAccepIssued = Math.Round((ldtotOutofAccepIssued), 2);
        ldConditionNine = Math.Round((ldConditionNine), 2);
        if (ldtotOutofAccepIssued != ldConditionNine)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Acceptance provided Against Inland Bill Related to Export LC + Total Acceptance Provided Against Inland Bill not Related to Export LC + Total Acceptance Provided Against Foreign Bill) must equal to Total Outstanding of Acceptance Issued Against  FB/IB/AB')", true);
            returnStatus = 0;
        }
        //Condition Ten
        ldtotOutBlnceofIssedBnkGrnt = Math.Round((ldtotOutBlnceofIssedBnkGrnt), 2);
        ldConditionTen = Math.Round((ldConditionTen), 2);
        if (ldtotOutBlnceofIssedBnkGrnt != ldConditionTen)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Your Sum of (Total Performance Guarantee Local + Total Other Guarantee Local + Total Performance Guarantee Foreign + Total Other Guarantee Foreign) must equal to Total Outstanding Balance of Issued Bank Guarantee')", true);
            returnStatus = 0;
        }

        return returnStatus;
    }
}