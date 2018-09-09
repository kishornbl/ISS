/* Version:01.01
 * Author: Kishor Kumar Saha
 Opearation: Report Generation 
 create date: 10.08.2015
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

public partial class ItemWiseReport : System.Web.UI.Page
{
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected static string Condition = "";
    protected static int hostatus = 0;
    protected static string division = null;
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
            if (hostatus == 1 && getValue == "COA")
            {
                if (division == "HO")
                {
                    DateTime reportDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                    toDateTextBox.Text = reportDate.ToString("MMM-yyyy");
                    //toDateTextBox.Text = DateTime.Now.ToString("MMM-yyyy");
                    LoadIndividualItem();                    
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

    private void LoadIndividualItem()
    {
        oTransactionDAL = new TransactionDAL();
        DataTable dt = oTransactionDAL.LoadSubcategory(Condition);
        drpItem.DataSource = dt;
        drpItem.DataTextField = "SUBCATEGORY_NAME";
        drpItem.DataValueField = "SUBCATEGORY_ID";
        drpItem.DataBind();
    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        string dateformat = "";
        string reportName = "";
        DateTime date = Convert.ToDateTime(DateTime.Now.Day + "-" + toDateTextBox.Text);
        date = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        dateformat = date.ToString("yyyy-MM-dd");
        DataTable dt = new DataTable();

        dt = oTransactionDAL.GetIndividualFieldData(dateformat, drpItem.SelectedValue.ToString());
        reportName = "Monitoring_Item" + date.ToString("ddMMyyyy");
        ExportToExcel(dt, reportName);
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
}