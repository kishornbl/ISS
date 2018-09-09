/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Report Generation 
 create date: 10.03.2016
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

public partial class AuthoBranchList : System.Web.UI.Page
{
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected static string Condition = "";
    protected static int hostatus = 0;
    protected static string division = null;
    protected static string pssTransactionId = null;
    protected static string txtboxDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
        }
        if (!IsPostBack)
        {
            division = Session["DIVISION"].ToString();

            hostatus = oTransactionDAL.GetOneReturnOne("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "HO_STATUS");
            String getValue = Request.QueryString["value"];
            if (hostatus == 1 && getValue == "AUTHO")
            {
                if (division == "HO")
                {
                    DateTime reportDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                    toDateTextBox.Text = reportDate.ToString("MMM-yyyy");
                    txtboxDate = toDateTextBox.Text.ToString();
                }
                else
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

    public void ExportToExcel(DataTable dt, string reportName)
    {
        if (dt.Rows.Count > 0)
        {
            string filename = reportName + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            dgGrid.DataSource = dt;
            dgGrid.DataBind();
            //Get the HTML for the control.
            dgGrid.RenderControl(hw);
            //Write the HTML back to the browser.
            //Response.ContentType = application/vnd.ms-excel;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            this.EnableViewState = false;
            Response.Write(tw.ToString());
            Response.End();
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('No data is Available.')", true);
        }
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        //string dateformat = "";
        string reportName = "";
        DateTime date = Convert.ToDateTime(DateTime.Now.Day + "-" + toDateTextBox.Text);
        date = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        //dateformat = date.ToString("yyyy-MM-dd");
        pssTransactionId = date.ToString("yyyyMMdd");
        DataTable dt = new DataTable();
        string type = "MONITORING";
        dt = oTransactionDAL.GetAuthorizedBranch(pssTransactionId, type);
        reportName = "Authorized_Branch_List-" + txtboxDate.ToString();
        ExportToExcel(dt, reportName);
    }
}