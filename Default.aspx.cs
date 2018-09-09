using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    TransactionDAL oTransactionDAL = new TransactionDAL();
    TransactionEntity cTransactionEntity = new TransactionEntity();
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();

    protected void Page_Load(object sender, EventArgs e)
    {  
        DbHandler dbh = new DbHandler();
        DataTable dt = new DataTable();
        try
        {
            //Session["userName"] = Request.QueryString["userName"];

            Session["UserId"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["userId"].Trim()));
            Session["UserName"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["userName"].Trim()));
            Session["brCode"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["brCode"].Trim()));
            Session["userType"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["userType"].Trim()));
            Session["BRANCH_NAME"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["brName"].Trim()));

            //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "MessageBox", "alert('useid" + Session["UserId"] + ",username:" + Session["UserName"] + ",brCode:" + Session["brCode"] + ",userType:" + Session["userType"] + ",BRANCH_NAME:" + Session["BRANCH_NAME"] + "')", true);
                  

            //useidkamrun.nessa,username:Kamrun Nessa,brCode:1999,userType:User,BRANCH_NAME:Dilkusha Branch

            //Session["userName"] = "";
            //Session["UserId"] = "kamrun.nessa";
            //Session["brCode"] = "1999";
            //Session["userType"] = "User";
            //Session["BRANCH_NAME"] = "Dilkusha Branch";

            //dbh.GetDataTable("EXEC getLogInInfo '" + Session["UserId"].ToString().Trim() + "'");
            if (!IsPostBack)
            {
                ViewState["brCode"] = Session["brCode"];
                ViewState["userId"] = Session["UserId"];
            }
            if (Session["brCode"].ToString() == "1904" || Session["brCode"].ToString() == "1905" || Session["brCode"].ToString() == "1906" || Session["brCode"].ToString() == "1907" || Session["brCode"].ToString() == "1909" || Session["brCode"].ToString() == "1910" || Session["brCode"].ToString() == "1912" || Session["brCode"].ToString() == "1914" || Session["brCode"].ToString() == "1915" || Session["brCode"].ToString() == "1918" || Session["brCode"].ToString() == "1931")
            {
                Session["DIVISION"] = "HO";
                Session["HO_STATUS"] = "1";
            }
            else
            {
                if (Session["userType"].ToString() == "User")
                {
                    Session["DIVISION"] = "Branch";
                }
                else
                {
                    Session["DIVISION"] = "Branch Admin";
                }
                Session["HO_STATUS"] = "0";
            }
            //Session["DIVISION"] = oTransactionDAL.GetoneReturnOneString("ISS_USER_INFO", "USER_ID", Session["UserId"].ToString(), "DIVISION");
            //Session["DIVISION"] = "Branch";
            Session["AD_STATUS"] = oTransactionDAL.GetoneReturnOneString("ISS_BRANCH_INFO", "BRANCH_CODE", Session["brCode"].ToString(), "AD_STATUS"); ;
        }
        catch (Exception ex)
        {
            Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
        }
        if (Session["UserId"] == null)
        {            
            Response.Redirect(ConfigurationManager.AppSettings["serverAddress"]);
        }
        //if (!IsPostBack)
        //{
        //    int l = oTransactionDAL.UpdateLoginStatus(ViewState["userId"].ToString(), ViewState["brCode"].ToString(), "description", "ISS", " and loginStatus=1 AND description is null");
        //}
    }
    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI657328B";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
}
