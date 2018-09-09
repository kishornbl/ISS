/* Version:01.01
 * Author: Kishor Kumar Saha
 Opearation: Inactive User List 
 create date: 22.07.2015
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

public partial class InActiveUser : System.Web.UI.Page
{
    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string condition = "";
            string UserId = Session["UserId"].ToString().Trim();
            if (UserId == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            if (UserId != "HOIT")
            {
                Response.Redirect("Default2.aspx");
            }
            DataTable dt = new DataTable();
            condition = " WHERE (ISS_USER_INFO.IS_ACTIVE = 0) ";
            dt = oTransactionDAL.GetHoReportData("Active", condition);
            ExportToExcel(dt, "InActive User");
        }
    }
    public void ExportToExcel(DataTable dt, string reportName)
    {
        if (dt.Rows.Count > 0)
        {
            string filename = reportName + ".xls";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            hw.Write("InActive User List");
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
    }
}