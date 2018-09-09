/* Version:01.01
 Author: Kishor Kumar Saha
 Opearation: Time Extension 
 create date: 27.12.2015
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class TimeExtension : System.Web.UI.Page
{

    TransactionEntity cTransactionEntity = new TransactionEntity();
    TransactionDAL oTransactionDAL = new TransactionDAL();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string UserId = Session["UserId"].ToString().Trim();
            txtCurrentMonth.Text = DateTime.Now.ToString("MMMM");
            if (UserId == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
            }
            if (Session["DIVISION"].ToString() != "HO")
            {
                Response.Redirect("Default2.aspx");
            }
            //if (UserId != "HOIT" && UserId != "ITSUPPORT")
            //{
            //    Response.Redirect("Default2.aspx");
            //}
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int UpdateStatus = 0;
        string message = null;

        cTransactionEntity.SL_NO = 1;
        cTransactionEntity.ALIVE_DATE = Convert.ToInt32(drpSelectDate.SelectedItem.ToString()) + 1;
        UpdateStatus = oTransactionDAL.UpdateIntoISS_SFT_ALIVE(cTransactionEntity);

        if (UpdateStatus == 1)
        {
            message = "Time Extended Successfully";
        }
        else
        {
            message = "Time Extension is Unsuccessful!";
        }
        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('" + message + "')", true);
        drpSelectDate.SelectedIndex = 0;
    }
}