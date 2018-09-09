using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
/// <summary>
/// Summary description for ConnectionDatabase
/// </summary>
public class ConnectionDatabase
{
    public SqlConnection DatabaseConnection()
    {
        SqlConnection con;

        //string sqlConnection = "Data Source=USER-HP\\SQLEXPRESS;Initial Catalog=CollectionAccount;Integrated Security=True";
        //con = new SqlConnection(sqlConnection);
        //try { con.Open(); }
        //catch { }
        //return con;
        con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
        try { con.Open(); }
        catch { }
        return con;
    }
}