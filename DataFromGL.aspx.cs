using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class DataFromGL : System.Web.UI.Page
{
    protected static string pssTransactionId = null;
    TransactionDAL oTransactionDAL = new TransactionDAL();
    TransactionEntity cTransactionEntity = new TransactionEntity();
    protected void Page_Load(object sender, EventArgs e)
    {
        int alivedate = oTransactionDAL.GetOneReturnOne("ISS_SFT_ALIVE", "SL_NO", "1", "ALIVE_DATE");
        DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, alivedate);
        DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
        if (Convert.ToDateTime(DateTime.Now) > fromDate && Convert.ToDateTime(DateTime.Now) < toDate)
        {
            Response.Redirect("Default.aspx");
        }
        if (!IsPostBack)
        {
            //Session["UserId"] = "kishor.it";
            string UserId = Session["UserId"].ToString().Trim();
            if (Session["UserId"] == null)
            {
                Response.Redirect("webLogin.aspx");
            }
            if (UserId != "asaduzzaman.it" && UserId != "kishor.it" && UserId != "najim.it")
            //if (UserId != "asaduzzaman.it")
            {
                Response.Redirect("Default2.aspx");
            }
            else
            {
                DateTime date = DateTime.Now;
                if (DateTime.Now.Day >= 1 && DateTime.Now.Day <= 28)
                {
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                }
                pssTransactionId = date.ToString("yyyyMMdd");
                ViewState["date"] = date;
                toDateTextBox.Text = date.ToString("dd-MMM-yyyy");
            }
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        int exist = oTransactionDAL.GetOneReturnOne("ISS_BRANCH_AUTHORIZATION", "TRAN_ID", pssTransactionId, "BR_AUTHO_SL");
        if (exist > 0)
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Data Already Uploaded')", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('Data Uploaded Successfully')", true);
            int InsertStatus = 0, Last_Category = 0, MonitoringStatus = 0, InsertBrAutho = 0;
            double output = 0;
            string date = txtUpload.Text.ToString();
            string CATEGORY_ID = null, SUBCATEGORY_ID = null;
            DataTable dt = null;
            DataTable GetBranchFromGL_DT = oTransactionDAL.GetBranchFromGL();
            DataTable GL_NUMBER_DT = oTransactionDAL.GL_NUMBER_DT("");
            for (int m = 0; m < GetBranchFromGL_DT.Rows.Count; m++)
            {
                DataTable item = new DataTable();
                item.Clear();
                item.Columns.Add("String");
                item.Columns.Add("value");

                for (int i = 0; i < GL_NUMBER_DT.Rows.Count; i++)
                {
                    CATEGORY_ID = GL_NUMBER_DT.Rows[i]["CATEGORY_ID"].ToString();
                    SUBCATEGORY_ID = GL_NUMBER_DT.Rows[i]["SUBCATEGORY_ID"].ToString();
                    if (GL_NUMBER_DT.Rows[i]["GLPL_NO"].ToString() != "")
                    {
                        string Status = GL_NUMBER_DT.Rows[i]["STATUS"].ToString();
                        int length = GL_NUMBER_DT.Rows[i]["GLPL_NO"].ToString().Length;
                        char[] characters = GL_NUMBER_DT.Rows[i]["GLPL_NO"].ToString().ToCharArray();

                        string[] split = GL_NUMBER_DT.Rows[i]["GLPL_NO"].ToString().Split(new Char[] { '+', '-', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                        decimal[] value = new decimal[split.Length];

                        for (int k = 0; k < split.Length; k++)
                        {
                            if (Status == "GL")
                            {
                                dt = oTransactionDAL.GetGLAmount(split[k], date, GetBranchFromGL_DT.Rows[m][0].ToString());
                            }
                            else if (Status == "PL")
                            {
                                dt = oTransactionDAL.GetPLAmount(split[k], date, GetBranchFromGL_DT.Rows[m][0].ToString());
                            }
                            try
                            {
                                if (SUBCATEGORY_ID == "36108")
                                {
                                    value[k] = Convert.ToDecimal(dt.Rows[0]["ClosingBal"]);
                                }
                                else
                                {
                                    value[k] = Math.Abs(Convert.ToDecimal(dt.Rows[0]["ClosingBal"]));
                                }
                            }
                            catch
                            {
                                value[k] = 0;
                            }
                        }
                        string buildString = value[0].ToString();
                        if (length > 4)
                        {
                            int counter = 0, n = 0, j = 0;
                            string val = "", result = "";
                            string last = "", data = "", lastSymbol = "";
                            double sndOutput = 0, CurValue = 0;
                            for (j = n; j < characters.Length; j++)
                            {
                                data = characters[j].ToString();
                                if (data == "(")
                                {
                                    lastSymbol = result.Substring(result.Length - 1, 1);
                                    for (n = j + 1; n < characters.Length; n++)
                                    {
                                        data = characters[n].ToString();
                                        if (data == ")")
                                        {
                                            j = n + 1;
                                            break;
                                        }
                                        else
                                        {
                                            if (data != "+" && data != "-" && data != "(" && data != ")")
                                            {
                                                val = val + characters[n].ToString();
                                            }
                                            else
                                            {
                                                result = result + data;
                                            }
                                            if (val.Length == 4)
                                            {
                                                if (result != "")
                                                {
                                                    last = result.Substring(result.Length - 1, 1);
                                                }
                                                if (Convert.ToDouble(value[counter].ToString()) < 0 && last == "+")
                                                {
                                                    result = result.Remove(result.Length - 1);
                                                }
                                                Double.TryParse(value[counter].ToString(), out CurValue);
                                                sndOutput = sndOutput + CurValue;
                                                result = result + value[counter].ToString();
                                                val = "";
                                                counter++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (data != "+" && data != "-" && data != "(" && data != ")")
                                    {
                                        val = val + characters[j].ToString();
                                    }
                                    else
                                    {
                                        result = result + data;
                                    }
                                    if (val.Length == 4)
                                    {
                                        if (result != "")
                                        {
                                            last = result.Substring(result.Length - 1, 1);
                                        }
                                        if (Convert.ToDouble(value[counter].ToString()) < 0 && last == "+")
                                        {
                                            result = result.Remove(result.Length - 1);
                                        }
                                        Double.TryParse(value[counter].ToString(), out CurValue);
                                        if (lastSymbol != "" && sndOutput != 0)
                                        {
                                            if (lastSymbol == "-")
                                            {
                                                output = output - sndOutput;
                                            }
                                            else
                                            {
                                                output = output + sndOutput;
                                            }
                                        }
                                        else
                                        {
                                            if (last == "-")
                                            {
                                                output = output - CurValue;
                                            }
                                            else
                                            {
                                                output = output + CurValue;
                                            }
                                        }
                                        result = result + value[counter].ToString();
                                        val = "";
                                        counter++;
                                    }
                                }
                            }

                            if (lastSymbol != "" && sndOutput != 0)
                            {
                                if (lastSymbol == "-")
                                {
                                    output = output - sndOutput;
                                }
                                else
                                {
                                    output = output + sndOutput;
                                }
                            }
                            else
                            {
                                output = output + sndOutput;
                            }
                        }
                        else
                        {
                            output = Convert.ToDouble(buildString);
                        }
                    }
                    cTransactionEntity.TRAN_ID = pssTransactionId;
                    cTransactionEntity.CATEGORY_ID = Convert.ToInt32(CATEGORY_ID);
                    cTransactionEntity.SUBCATEGORY_ID = Convert.ToInt32(SUBCATEGORY_ID);
                    if (cTransactionEntity.SUBCATEGORY_ID == 14128)
                    {
                        if (output >= 0)
                        {
                            cTransactionEntity.AMOUNT = Math.Abs(output);
                        }
                        else
                        {
                            cTransactionEntity.AMOUNT = 0;
                        }
                    }
                    else if (cTransactionEntity.SUBCATEGORY_ID == 15131)
                    {
                        if (output < 0)
                        {
                            cTransactionEntity.AMOUNT = Math.Abs(output);
                        }
                        else
                        {
                            cTransactionEntity.AMOUNT = 0;
                        }
                    }
                    else if (cTransactionEntity.SUBCATEGORY_ID == 36105 || cTransactionEntity.SUBCATEGORY_ID == 36108)
                    {
                        cTransactionEntity.AMOUNT = output;
                    }
                    else
                    {
                        cTransactionEntity.AMOUNT = Math.Abs(output);
                    }
                    cTransactionEntity.ENTRY_DATE = DateTime.Today.ToShortDateString();
                    cTransactionEntity.AS_ON_DATE = Convert.ToDateTime(ViewState["date"]).ToShortDateString();
                    cTransactionEntity.ENTRY_TIME = System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToLongTimeString();
                    cTransactionEntity.USER_ID = "System";
                    cTransactionEntity.BRANCH_CODE = "1" + GetBranchFromGL_DT.Rows[m]["BrCode"].ToString();
                    //if (GetBranchFromGL_DT.Rows[m]["BrCode"].ToString() == "999")
                    //{
                    //    cTransactionEntity.BRANCH_CODE = "1999";
                    //}
                    //else if (GetBranchFromGL_DT.Rows[m]["BrCode"].ToString() == "001")
                    //{
                    //    cTransactionEntity.BRANCH_CODE = "1001";
                    //}
                    //else
                    //{
                    //    cTransactionEntity.BRANCH_CODE = "1" + GetBranchFromGL_DT.Rows[m]["BrCode"].ToString();
                    //}
                    if (Last_Category != cTransactionEntity.CATEGORY_ID && Last_Category != 0)
                    {
                        cTransactionEntity.INSERT_STATUS = 1;
                        cTransactionEntity.UPDATE_STATUS = 0;
                        cTransactionEntity.DIVISION = "Branch";
                        cTransactionEntity.CATEGORY_ID = Last_Category;
                        MonitoringStatus = oTransactionDAL.InsertIntoBRANCHWISE_MONITORING_MAINTENANCE(cTransactionEntity);
                        cTransactionEntity.CATEGORY_ID = Convert.ToInt32(CATEGORY_ID);
                        InsertStatus = oTransactionDAL.InsertintoTable(cTransactionEntity);
                        output = 0;
                    }
                    else
                    {
                        InsertStatus = oTransactionDAL.InsertintoTable(cTransactionEntity);
                        output = 0;
                    }
                    Last_Category = cTransactionEntity.CATEGORY_ID;
                }
                cTransactionEntity.AUTHO_STATUS = 1;
                cTransactionEntity.TYPE = "MONITORING";
                InsertBrAutho = oTransactionDAL.InsertIntoISS_BRANCH_AUTHORIZATION(cTransactionEntity);
            }
        }
    }
}