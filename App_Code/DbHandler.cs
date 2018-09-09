using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



/// <summary>
/// Summary description for DbHandler
/// </summary>
public class DbHandler
{
    SqlConnection conn;
    SqlCommand sqlCommand;
    SqlTransaction transaction = null;

    public DbHandler()
    {
        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
        sqlCommand = conn.CreateCommand();
    }
    public DbHandler(string con, string DBType)
    {
        string conStr = con;

        conn = new SqlConnection(conStr);
        sqlCommand = conn.CreateCommand();

    }
    public void CloseConnection()
    {
        conn.Close();
    }

    public DataTable GetDataTable(string sqlQuery)
    {
        DataTable datatable = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter();
        try
        {
            adapter.SelectCommand = new SqlCommand(sqlQuery, conn);
            adapter.Fill(datatable);
        }
        catch (Exception ex)
        {
            throw new Exception("SQL:" + sqlQuery + " Error found" + ex.ToString());
        }
        finally
        {
            conn.Close();
        }
        return datatable;
    }

    public DataTable GetDataTable(SqlCommand sqlQuery)
    {
        DataTable dataTable = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter();
        try
        {
            sqlQuery.Connection = conn;
            adapter.SelectCommand = sqlQuery;
            adapter.Fill(dataTable);
        }
        catch (Exception ex)
        {
            throw new HttpException("Npgsql:" + sqlQuery.ToString() + " Error found" + ex.ToString());
            //ex.ToString();
        }
        finally
        {
            conn.Close();
        }

        return dataTable;
    }

    public bool ExecuteQuery(string query)
    {
        bool status = false;
        try
        {
            conn.Open();// open the connection  
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery(); //execute query
            status = true;
        }
        catch (SqlException e)
        {
            status = false;
        }
        finally
        {
            conn.Close(); //closing connection
        }
        return status;

    }

    public bool ExecuteQuery(string query, SqlTransaction trans)
    {
        bool status = false;
        try
        {
            conn.Open();// open the connection  

            SqlCommand cmd = new SqlCommand(query, conn, trans);
            cmd.ExecuteNonQuery(); //execute query
            status = true;
        }
        catch (SqlException e)
        {
            status = false;
        }
        finally
        {
            conn.Close(); //closing connection
        }
        return status;

    }

    public bool ExecuteQuery(SqlCommand command)
    {
        bool status = false;
        try
        {
            conn.Open();                // open the connection  
            command.Connection = conn;
            command.ExecuteNonQuery(); //execute query
            status = true;
        }
        catch (SqlException e)
        {
            //status = false;
            throw new Exception(e.Message);
        }
        finally
        {
            conn.Close(); //closing connection
        }
        return status;
    }

    public void GetDataTable(string query, DataTable dbTable)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        try
        {
            adapter.SelectCommand = new SqlCommand(query, conn);
            adapter.Fill(dbTable);
        }
        catch (Exception ex)
        {
            throw new Exception("SQL:" + query + " Error found" + ex.ToString());
        }
        finally
        {
            conn.Close();
        }
        //return datatable;
    }

    public void blankField(params Control[] array)
    {
        //Control ctrl;
        foreach (Control ctrl in array)
        {
            if ((ctrl) is TextBox)
            {
                //((TextBox)ctrl).Attributes.Add("onblur", "validate();return false;");
                ((TextBox)ctrl).Text = "";
            }
            else
            {
                if ((ctrl) is DropDownList)
                {
                    //((CheckBox)ctrl).Attributes.Add("onfocusin", "validate();return false;");
                    ((DropDownList)ctrl).ClearSelection();

                }
                //AddAttribute(ctrl.Controls); 
            }
        }
    }
    public string datefmt(string dt)
    {
        string newdt, dd, mm, yy;
        dd = dt.Substring(0, 2);
        mm = dt.Substring(3, 2);
        yy = dt.Substring(6, 4);
        newdt = mm + "/" + dd + "/" + yy;
        return newdt;
    }

    public object GetSingleField(string query)
    {
        object obj;
        try
        {
            conn.Open();
            sqlCommand.CommandText = query;
            obj = sqlCommand.ExecuteScalar();
            return obj;
        }
        catch
        {
            return null;
        }
        finally
        {
            conn.Close();
        }
    }



    public DataTable GetParameters()
    {
        DataTable dataTable = new DataTable();
        SqlDataAdapter adapter = new SqlDataAdapter();
        string sqlQuery = "";
        try
        {
            sqlQuery = "select BankName, Branch, Address from Parameter";
            adapter.SelectCommand = new SqlCommand(sqlQuery, conn);
            adapter.Fill(dataTable);
        }
        catch (Exception ex)
        {
            throw new HttpException("Npgsql:" + sqlQuery.ToString() + " Error found" + ex.ToString());
            //ex.ToString();
        }
        finally
        {
            conn.Close();
        }

        return dataTable;
    }

    public void GetDataTable()
    {
        throw new NotImplementedException();
    }
}
