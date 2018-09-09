/* Version:1.0
   Author: Kishor Kumar Saha
   Opearation: Monitoring Data Entry 
   create date: 25.07.2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
/// <summary>
/// Summary description for TransactionDAL
/// </summary>
public class TransactionDAL
{
    ConnectionDatabase oConnectionDatabase = new ConnectionDatabase();
    SqlConnection con;
    SqlCommand cmd;
    string sqlQuery = null;
	public TransactionDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    #region Insert Query for All Table(10)

    #region InsertInto ISS_TRANSACTION
    public int InsertintoTable(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_TRANSACTION (TRAN_ID,CATEGORY_ID,SUBCATEGORY_ID,AMOUNT,ENTRY_DATE,AS_ON_DATE,USER_ID,BRANCH_CODE) VALUES(@TRAN_ID,@CATEGORY_ID,@SUBCATEGORY_ID,@AMOUNT,@ENTRY_DATE,@AS_ON_DATE,@USER_ID,@BRANCH_CODE)", con);
        cmd.Parameters.AddWithValue("@TRAN_ID", SqlDbType.VarChar).Value = oTransactionEntity.TRAN_ID;
        cmd.Parameters.AddWithValue("@CATEGORY_ID", SqlDbType.VarChar).Value = oTransactionEntity.CATEGORY_ID;
        cmd.Parameters.AddWithValue("@SUBCATEGORY_ID", SqlDbType.VarChar).Value = oTransactionEntity.SUBCATEGORY_ID;
        cmd.Parameters.AddWithValue("@AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.AMOUNT;
        cmd.Parameters.AddWithValue("@ENTRY_DATE", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_DATE;
        cmd.Parameters.AddWithValue("@AS_ON_DATE", SqlDbType.VarChar).Value = oTransactionEntity.AS_ON_DATE;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_TRAN_MONITORING_AUDIT
    public int InsertintoISS_TRAN_MONITORING_AUDIT(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_TRAN_MONITORING_AUDIT (TRAN_ID,CATEGORY_ID,SUBCATEGORY_ID,AMOUNT,ENTRY_DATE,AS_ON_DATE,USER_ID,ENTRY_TIME) VALUES(@TRAN_ID,@CATEGORY_ID,@SUBCATEGORY_ID,@AMOUNT,@ENTRY_DATE,@AS_ON_DATE,@USER_ID,@ENTRY_TIME)", con);
        cmd.Parameters.AddWithValue("@TRAN_ID", SqlDbType.VarChar).Value = oTransactionEntity.TRAN_ID;
        cmd.Parameters.AddWithValue("@CATEGORY_ID", SqlDbType.VarChar).Value = oTransactionEntity.CATEGORY_ID;
        cmd.Parameters.AddWithValue("@SUBCATEGORY_ID", SqlDbType.VarChar).Value = oTransactionEntity.SUBCATEGORY_ID;
        cmd.Parameters.AddWithValue("@AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.AMOUNT;
        cmd.Parameters.AddWithValue("@ENTRY_DATE", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_DATE;
        cmd.Parameters.AddWithValue("@AS_ON_DATE", SqlDbType.VarChar).Value = oTransactionEntity.AS_ON_DATE;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@ENTRY_TIME", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_TIME;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_ACCEPTANCE_TRAN
    public int InsertIntoTableISS_ACCEPTANCE_TRAN(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_ACCEPTANCE_TRAN (ACCEP_ID,BANK_ID,ACCEPTANCE_ISSUED_AMOUNT,ACCEPTANCE_MATURED_AMOUNT,ACCEPTANCE_RECEIVED_AMOUNT,ACCEPTANCE_PURCHASED_AMOUNT,ACCEPTANCE_REC_MATURED_AMOUNT,ENTRY_DATE,USER_ID,BRANCH_CODE,AS_ON_DATE) VALUES(@ACCEP_ID,@BANK_ID,@ACCEPTANCE_ISSUED_AMOUNT,@ACCEPTANCE_MATURED_AMOUNT,@ACCEPTANCE_RECEIVED_AMOUNT,@ACCEPTANCE_PURCHASED_AMOUNT,@ACCEPTANCE_REC_MATURED_AMOUNT,@ENTRY_DATE,@USER_ID,@BRANCH_CODE,@AS_ON_DATE)", con);
        cmd.Parameters.AddWithValue("@ACCEP_ID", SqlDbType.VarChar).Value = oTransactionEntity.ACCEP_ID;
        cmd.Parameters.AddWithValue("@BANK_ID", SqlDbType.VarChar).Value = oTransactionEntity.BANK_ID;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_ISSUED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_ISSUED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_MATURED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_MATURED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_RECEIVED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_RECEIVED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_PURCHASED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_PURCHASED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_REC_MATURED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_REC_MATURED_AMOUNT;
        cmd.Parameters.AddWithValue("@ENTRY_DATE", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_DATE;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@AS_ON_DATE", SqlDbType.VarChar).Value = oTransactionEntity.AS_ON_DATE;

        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_TRAN_ACCEPTANCE_AUDIT
    public int InsertIntoISS_TRAN_ACCEPTANCE_AUDIT(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_TRAN_ACCEPTANCE_AUDIT (ACCEP_ID,BANK_ID,ACCEPTANCE_ISSUED_AMOUNT,ACCEPTANCE_MATURED_AMOUNT,ACCEPTANCE_RECEIVED_AMOUNT,ACCEPTANCE_PURCHASED_AMOUNT,ACCEPTANCE_REC_MATURED_AMOUNT,ENTRY_DATE,USER_ID,BRANCH_CODE,AS_ON_DATE,ENTRY_TIME) VALUES(@ACCEP_ID,@BANK_ID,@ACCEPTANCE_ISSUED_AMOUNT,@ACCEPTANCE_MATURED_AMOUNT,@ACCEPTANCE_RECEIVED_AMOUNT,@ACCEPTANCE_PURCHASED_AMOUNT,@ACCEPTANCE_REC_MATURED_AMOUNT,@ENTRY_DATE,@USER_ID,@BRANCH_CODE,@AS_ON_DATE,@ENTRY_TIME)", con);
        cmd.Parameters.AddWithValue("@ACCEP_ID", SqlDbType.VarChar).Value = oTransactionEntity.ACCEP_ID;
        cmd.Parameters.AddWithValue("@BANK_ID", SqlDbType.VarChar).Value = oTransactionEntity.BANK_ID;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_ISSUED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_ISSUED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_MATURED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_MATURED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_RECEIVED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_RECEIVED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_PURCHASED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_PURCHASED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_REC_MATURED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_REC_MATURED_AMOUNT;
        cmd.Parameters.AddWithValue("@ENTRY_DATE", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_DATE;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@AS_ON_DATE", SqlDbType.VarChar).Value = oTransactionEntity.AS_ON_DATE;
        cmd.Parameters.AddWithValue("@ENTRY_TIME", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_TIME;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_USER_INFO
    public int InsertIntoTableUserInfo(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_USER_INFO (USER_ID,USER_NAME,PASSWORD,IS_ACTIVE,PERMISSION_ID,STATUS,PREVILEGE_STATUS,BRANCH_CODE,CREATION_DATE,LAST_MODIFICATION_DATE,OFFICE_IND,HO_STATUS,BR_STATUS,DIVISION,DESIGNATION) VALUES(@USER_ID,@USER_NAME,@PASSWORD,@IS_ACTIVE,@PERMISSION_ID,@STATUS,@PREVILEGE_STATUS,@BRANCH_CODE,@CREATION_DATE,@LAST_MODIFICATION_DATE,@OFFICE_IND,@HO_STATUS,@BR_STATUS,@DIVISION,@DESIGNATION)", con);
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@USER_NAME", SqlDbType.VarChar).Value = oTransactionEntity.USER_NAME;
        cmd.Parameters.AddWithValue("@PASSWORD", SqlDbType.VarChar).Value = oTransactionEntity.PASSWORD;
        cmd.Parameters.AddWithValue("@IS_ACTIVE", SqlDbType.VarChar).Value = oTransactionEntity.IS_ACTIVE;
        cmd.Parameters.AddWithValue("@PERMISSION_ID", SqlDbType.VarChar).Value = oTransactionEntity.PERMISSION_ID;
        cmd.Parameters.AddWithValue("@STATUS", SqlDbType.VarChar).Value = oTransactionEntity.STATUS;
        cmd.Parameters.AddWithValue("@PREVILEGE_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.PREVILEGE_STATUS;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@CREATION_DATE", SqlDbType.VarChar).Value = oTransactionEntity.CREATION_DATE;
        cmd.Parameters.AddWithValue("@LAST_MODIFICATION_DATE", SqlDbType.VarChar).Value = oTransactionEntity.LAST_MODIFICATION_DATE;
        cmd.Parameters.AddWithValue("@OFFICE_IND", SqlDbType.VarChar).Value = oTransactionEntity.OFFICE_IND;
        cmd.Parameters.AddWithValue("@HO_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.HO_STATUS;
        cmd.Parameters.AddWithValue("@BR_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.BR_STATUS;
        cmd.Parameters.AddWithValue("@DIVISION", SqlDbType.VarChar).Value = oTransactionEntity.DIVISION;
        cmd.Parameters.AddWithValue("@DESIGNATION", SqlDbType.VarChar).Value = oTransactionEntity.DESIGNATION;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_USER_AUDIT
    public int InsertIntoISS_USER_AUDIT(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_USER_AUDIT (USER_ID,USER_NAME,PASSWORD,IS_ACTIVE,HO_STATUS,BR_STATUS,AUDIT_DESCRIPTION,AUTHORISED_USER,AUTHORISED_TIME,DIVISION,DESIGNATION) VALUES(@USER_ID,@USER_NAME,@PASSWORD,@IS_ACTIVE,@HO_STATUS,@BR_STATUS,@AUDIT_DESCRIPTION,@AUTHORISED_USER,@AUTHORISED_TIME,@DIVISION,@DESIGNATION)", con);
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@USER_NAME", SqlDbType.VarChar).Value = oTransactionEntity.USER_NAME;
        cmd.Parameters.AddWithValue("@PASSWORD", SqlDbType.VarChar).Value = oTransactionEntity.PASSWORD;
        cmd.Parameters.AddWithValue("@IS_ACTIVE", SqlDbType.VarChar).Value = oTransactionEntity.IS_ACTIVE;
        cmd.Parameters.AddWithValue("@HO_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.HO_STATUS;
        cmd.Parameters.AddWithValue("@BR_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.BR_STATUS;
        cmd.Parameters.AddWithValue("@AUDIT_DESCRIPTION", SqlDbType.VarChar).Value = oTransactionEntity.AUDIT_DESCRIPTION;
        cmd.Parameters.AddWithValue("@AUTHORISED_USER", SqlDbType.VarChar).Value = oTransactionEntity.AUTHORISED_USER;
        cmd.Parameters.AddWithValue("@AUTHORISED_TIME", SqlDbType.VarChar).Value = oTransactionEntity.AUTHORISED_TIME;
        cmd.Parameters.AddWithValue("@DIVISION", SqlDbType.VarChar).Value = oTransactionEntity.DIVISION;
        cmd.Parameters.AddWithValue("@DESIGNATION", SqlDbType.VarChar).Value = oTransactionEntity.DESIGNATION;      
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_BRANCH_INFO
    public int InsertIntoISS_BRANCH_INFO(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_BRANCH_INFO (BRANCH_CODE,BRANCH_NAME,BANK_ID,BRANCH_HO_ID,AD_STATUS,ZONE_ID) VALUES(@BRANCH_CODE,@BRANCH_NAME,@BANK_ID,@BRANCH_HO_ID,@AD_STATUS,@ZONE_ID)", con);
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@BRANCH_NAME", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_NAME;
        cmd.Parameters.AddWithValue("@BANK_ID", SqlDbType.VarChar).Value = oTransactionEntity.BANK_ID;
        cmd.Parameters.AddWithValue("@BRANCH_HO_ID", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_HO_ID;
        cmd.Parameters.AddWithValue("@AD_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.AD_STATUS;
        cmd.Parameters.AddWithValue("@ZONE_ID", SqlDbType.VarChar).Value = oTransactionEntity.ZONE_ID;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_LOG_DETAILS
    public int InsertIntoISS_LOG_DETAILS(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_LOG_DETAILS (LOG_DESCRIPTION,ACTIVITY_ID,USER_ID,ENTRY_EXIT_TIME) VALUES(@LOG_DESCRIPTION,@ACTIVITY_ID,@USER_ID,@ENTRY_EXIT_TIME)", con);
        cmd.Parameters.AddWithValue("@LOG_DESCRIPTION", SqlDbType.VarChar).Value = oTransactionEntity.LOG_DESCRIPTION;
        cmd.Parameters.AddWithValue("@ACTIVITY_ID", SqlDbType.VarChar).Value = oTransactionEntity.ACTIVITY_ID;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@ENTRY_EXIT_TIME", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_EXIT_TIME;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto BRANCHWISE_MONITORING_MAINTENANCE
    public int InsertIntoBRANCHWISE_MONITORING_MAINTENANCE(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO BRANCHWISE_MONITORING_MAINTENANCE (CATEGORY_ID,TRAN_ID,BRANCH_CODE,INSERT_STATUS,UPDATE_STATUS,DIVISION) VALUES(@CATEGORY_ID,@TRAN_ID,@BRANCH_CODE,@INSERT_STATUS,@UPDATE_STATUS,@DIVISION)", con);
        cmd.Parameters.AddWithValue("@CATEGORY_ID", SqlDbType.VarChar).Value = oTransactionEntity.CATEGORY_ID;
        cmd.Parameters.AddWithValue("@TRAN_ID", SqlDbType.VarChar).Value = oTransactionEntity.TRAN_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@INSERT_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.INSERT_STATUS;
        cmd.Parameters.AddWithValue("@UPDATE_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.UPDATE_STATUS;
        cmd.Parameters.AddWithValue("@DIVISION", SqlDbType.VarChar).Value = oTransactionEntity.DIVISION;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto BRANCHWISE_ACCEPTANCE_MAINTENANCE
    public int InsertIntoBRANCHWISE_ACCEPTANCE_MAINTENANCE(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO BRANCHWISE_ACCEPTANCE_MAINTENANCE (ACCEP_ID,BANK_ID,BRANCH_CODE,INSERT_STATUS) VALUES(@ACCEP_ID,@BANK_ID,@BRANCH_CODE,@INSERT_STATUS)", con);
        cmd.Parameters.AddWithValue("@ACCEP_ID", SqlDbType.VarChar).Value = oTransactionEntity.ACCEP_ID;
        cmd.Parameters.AddWithValue("@BANK_ID", SqlDbType.VarChar).Value = oTransactionEntity.BANK_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@INSERT_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.INSERT_STATUS;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region InsertInto ISS_BRANCH_AUTHORIZATION
    public int InsertIntoISS_BRANCH_AUTHORIZATION(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("INSERT INTO ISS_BRANCH_AUTHORIZATION (TRAN_ID,BRANCH_CODE,AUTHO_STATUS,ENTRY_DATE,USER_ID,TYPE) VALUES(@TRAN_ID,@BRANCH_CODE,@AUTHO_STATUS,@ENTRY_DATE,@USER_ID,@TYPE)", con);
        cmd.Parameters.AddWithValue("@TRAN_ID", SqlDbType.VarChar).Value = oTransactionEntity.TRAN_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@AUTHO_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.AUTHO_STATUS;
        cmd.Parameters.AddWithValue("@ENTRY_DATE", SqlDbType.VarChar).Value = oTransactionEntity.ENTRY_DATE;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@TYPE", SqlDbType.VarChar).Value = oTransactionEntity.TYPE;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion
    
    #endregion

    #region Update Query for All Tables

    #region UpdateInto ISS_TRANSACTION
    public int UpdateIntoTable(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_TRANSACTION  SET AMOUNT = @AMOUNT WHERE TRAN_SL = @TRAN_SERIAL", con);
        cmd.Parameters.AddWithValue("@AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.AMOUNT;
        cmd.Parameters.AddWithValue("@TRAN_SERIAL", SqlDbType.VarChar).Value = oTransactionEntity.TRAN_SL;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region UpdateInto ISS_ACCEPTANCE_TRAN
    public int UpdateIntoTableISS_ACCEPTANCE_TRAN(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_ACCEPTANCE_TRAN  SET ACCEPTANCE_ISSUED_AMOUNT = @ACCEPTANCE_ISSUED_AMOUNT , ACCEPTANCE_MATURED_AMOUNT = @ACCEPTANCE_MATURED_AMOUNT,ACCEPTANCE_RECEIVED_AMOUNT = @ACCEPTANCE_RECEIVED_AMOUNT,ACCEPTANCE_PURCHASED_AMOUNT = @ACCEPTANCE_PURCHASED_AMOUNT,ACCEPTANCE_REC_MATURED_AMOUNT = @ACCEPTANCE_REC_MATURED_AMOUNT WHERE ACCEP_SL = @ACCEP_SL", con);
        cmd.Parameters.AddWithValue("@ACCEPTANCE_ISSUED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_ISSUED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_MATURED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_MATURED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_RECEIVED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_RECEIVED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_PURCHASED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_PURCHASED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEPTANCE_REC_MATURED_AMOUNT", SqlDbType.VarChar).Value = oTransactionEntity.ACCEPTANCE_REC_MATURED_AMOUNT;
        cmd.Parameters.AddWithValue("@ACCEP_SL", SqlDbType.VarChar).Value = oTransactionEntity.ACCEP_SL;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region UpdateInto ISS_USER_INFO
    public int UpdateIntoTableUserInfo(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_USER_INFO  SET USER_NAME = @USER_NAME , IS_ACTIVE = @IS_ACTIVE,LAST_MODIFICATION_DATE = @LAST_MODIFICATION_DATE, DESIGNATION = @DESIGNATION WHERE USER_SL = @USER_SL", con);
        cmd.Parameters.AddWithValue("@USER_NAME", SqlDbType.VarChar).Value = oTransactionEntity.USER_NAME;
        cmd.Parameters.AddWithValue("@IS_ACTIVE", SqlDbType.VarChar).Value = oTransactionEntity.IS_ACTIVE;
        cmd.Parameters.AddWithValue("@DESIGNATION", SqlDbType.VarChar).Value = oTransactionEntity.DESIGNATION;
        cmd.Parameters.AddWithValue("@USER_SL", SqlDbType.VarChar).Value = oTransactionEntity.USER_SL;
        cmd.Parameters.AddWithValue("@LAST_MODIFICATION_DATE", SqlDbType.VarChar).Value = oTransactionEntity.LAST_MODIFICATION_DATE;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region UpdateIntoTableUserInfoforResetPass
    public int UpdateIntoTableUserInfoforResetPass(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_USER_INFO  SET PASSWORD = @PASSWORD,STATUS = @STATUS WHERE USER_ID = @USER_ID", con);
        cmd.Parameters.AddWithValue("@PASSWORD", SqlDbType.VarChar).Value = oTransactionEntity.PASSWORD;
        cmd.Parameters.AddWithValue("@STATUS", SqlDbType.VarChar).Value = oTransactionEntity.STATUS;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region DeleteDatafromBranchAuthorization
    public int DeleteDatafromBranchAuthorization(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("DELETE FROM ISS_BRANCH_AUTHORIZATION WHERE TRAN_ID = @TRAN_ID AND BRANCH_CODE = @BRANCH_CODE AND @USER_ID = USER_ID AND TYPE = @TYPE", con);
        cmd.Parameters.AddWithValue("@TRAN_ID", SqlDbType.VarChar).Value = oTransactionEntity.TRAN_ID;
        cmd.Parameters.AddWithValue("@BRANCH_CODE", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_CODE;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        cmd.Parameters.AddWithValue("@TYPE", SqlDbType.VarChar).Value = oTransactionEntity.TYPE;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region UpdatePasswordIntoUserInfo
    public int UpdatePasswordIntoUserInfo(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_USER_INFO  SET PASSWORD = @PASSWORD,STATUS = @STATUS WHERE USER_ID = @USER_ID", con);
        cmd.Parameters.AddWithValue("@PASSWORD", SqlDbType.VarChar).Value = oTransactionEntity.PASSWORD;
        cmd.Parameters.AddWithValue("@STATUS", SqlDbType.VarChar).Value = oTransactionEntity.STATUS;
        cmd.Parameters.AddWithValue("@USER_ID", SqlDbType.VarChar).Value = oTransactionEntity.USER_ID;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region UpdateInto ISS_BRANCH_INFO
    public int UpdateIntoISS_BRANCH_INFO(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_BRANCH_INFO  SET BRANCH_NAME = @BRANCH_NAME ,AD_STATUS = @AD_STATUS, ZONE_ID = @ZONE_ID WHERE BRANCH_SL = @BRANCH_SL", con);
        cmd.Parameters.AddWithValue("@BRANCH_NAME", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_NAME;
        cmd.Parameters.AddWithValue("@AD_STATUS", SqlDbType.VarChar).Value = oTransactionEntity.AD_STATUS;
        cmd.Parameters.AddWithValue("@ZONE_ID", SqlDbType.VarChar).Value = oTransactionEntity.ZONE_ID;
        cmd.Parameters.AddWithValue("@BRANCH_SL", SqlDbType.VarChar).Value = oTransactionEntity.BRANCH_SL;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #region UpdateInto ISS_SFT_ALIVE
    public int UpdateIntoISS_SFT_ALIVE(TransactionEntity oTransactionEntity)
    {
        con = oConnectionDatabase.DatabaseConnection();
        int i = 0;
        cmd = new SqlCommand("UPDATE ISS_SFT_ALIVE SET ALIVE_DATE = @ALIVE_DATE WHERE SL_NO = @SL_NO", con);
        cmd.Parameters.AddWithValue("@ALIVE_DATE", SqlDbType.VarChar).Value = oTransactionEntity.ALIVE_DATE;
        cmd.Parameters.AddWithValue("@SL_NO", SqlDbType.VarChar).Value = oTransactionEntity.SL_NO;
        try
        {
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            i = 1;
        }
        catch
        {
        }
        con.Close();
        return i;
    }
    #endregion

    #endregion

    #region Other

    #region GetoneReturnOne
    public int GetOneReturnOne (string TableName,string FieldName,string FieldValue,string ReqField)
    {
        int returnvalue = 0;
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT " + ReqField + " FROM " + TableName + " WHERE " + FieldName + " = '" + FieldValue + "' ";
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                DataRow datarow = dt.NewRow();
                returnvalue = Convert.ToInt32(dr[ReqField]);
                dt.Rows.Add(datarow);
            }
            dr.Close();
        }
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return returnvalue;
    }
    #endregion

    #region GetoneReturnOneString
    public string GetoneReturnOneString(string TableName, string FieldName, string FieldValue, string ReqField)
    {
        string returnvalue = null;
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT " + ReqField + " FROM " + TableName + " WHERE " + FieldName + " = '" + FieldValue + "' ";
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                DataRow datarow = dt.NewRow();
                returnvalue = Convert.ToString(dr[ReqField]);
                dt.Rows.Add(datarow);
            }
            dr.Close();
        }
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return returnvalue;
    }
    #endregion


    #region GetTwoReturnOne
    public int GetTwoReturnOne(string TableName, string FieldOneName, string FieldOneValue, string FieldTwoName, string FieldTwoValue, string ReqField)
    {
        int returnvalue = 0;
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT " + ReqField + " FROM " + TableName + " WHERE " + FieldOneName + " = '" + FieldOneValue + "' AND " + FieldTwoName + " = '" + FieldTwoValue + "' ";
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                DataRow datarow = dt.NewRow();
                returnvalue = Convert.ToInt32(dr[ReqField]);
                dt.Rows.Add(datarow);
            }
            dr.Close();
        }
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return returnvalue;
    }
    #endregion

    #region GetThreeReturnOne
    public int GetThreeReturnOne(string TableName, string FieldOneName, string FieldOneValue, string FieldTwoName, string FieldTwoValue, string FieldThreeName, string FieldThreeValue, string ReqField)
    {
        int returnvalue = 0;
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT " + ReqField + " FROM " + TableName + " WHERE " + FieldOneName + " = '" + FieldOneValue + "' AND " + FieldTwoName + " = '" + FieldTwoValue + "' AND " + FieldThreeName + " = '" + FieldThreeValue + "' ";
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                DataRow datarow = dt.NewRow();
                returnvalue = Convert.ToInt32(dr[ReqField]);
                dt.Rows.Add(datarow);
            }
            dr.Close();
        }
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return returnvalue;
    }
    #endregion

    #region CountDataBRANCHWISE_MONITORING_MAINTENANCE
    public int CountDataBRANCHWISE_MONITORING_MAINTENANCE(string condition)
    {
        int returnvalue = 0;
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT COUNT(CATEGORY_ID) AS COUNT_DATA FROM BRANCHWISE_MONITORING_MAINTENANCE " + condition;
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                DataRow datarow = dt.NewRow();
                returnvalue = Convert.ToInt32(dr["COUNT_DATA"]);
                dt.Rows.Add(datarow);
            }
            dr.Close();
        }
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return returnvalue;
    }
    #endregion

    #region CheckTransactionData
    public DataTable CheckTransactionData(string condition)
    {
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT ISS_TRANSACTION.*, ISS_USER_INFO.BRANCH_CODE, ISS_USER_INFO.BR_STATUS, ISS_USER_INFO.HO_STATUS " +
                 "FROM ISS_TRANSACTION INNER JOIN  ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID " + condition;
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #endregion

    #region Load GridView Or Combobox
    #region Load Category
    public DataTable GetCategory(string condition)
    {
        sqlQuery = "SELECT 0 CATEGORY_ID, '-Select-' CATEGORY_NAME Union all SELECT x.CATEGORY_ID, x.CATEGORY_NAME FROM(SELECT * FROM ISS_CATEGORY)x " + condition;
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region Load SubCategory
    public DataTable GetSubCategory(string condition, string brCode, string date)
    {
        sqlQuery = "SELECT SUBCATEGORY_ID,SUBCATEGORY_NAME FROM ISS_SUBCATEGORY " + condition + " AND SUBCATEGORY_ID NOT IN  (SELECT  ISS_TRANSACTION.SUBCATEGORY_ID " +
               "FROM   ISS_TRANSACTION INNER JOIN  ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID where BRANCH_CODE= '" + brCode + "' and AS_ON_DATE='" + date + "')";
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region LoadMonitoringEntryGridView
    public DataTable LoadMonitoringEntryGridView(string condition,int liHoStatus)
    {
        //sqlQuery = "SELECT ISS_CATEGORY.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID " + condition;
        if (liHoStatus == 0)
        {
            sqlQuery = "SELECT ISS_CATEGORY.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_NARRATION.BRANCH_NARRATION AS NARRATION,ISS_SUBCATEGORY.FIGURE_IND FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID " + condition;
        }
        else
        {
            sqlQuery = "SELECT ISS_CATEGORY.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_NARRATION.HO_NARRATION AS NARRATION,ISS_SUBCATEGORY.FIGURE_IND FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID " + condition;
        }
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region LoadTransactionGridView
    public DataTable GetTransactionData(string condition,int liHoStatus)
    {        
        if (liHoStatus == 0)
        {
            //sqlQuery = "SELECT ISS_TRANSACTION.TRAN_SL, ISS_TRANSACTION.TRAN_ID, ISS_TRANSACTION.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_TRANSACTION.AMOUNT, ISS_TRANSACTION.ENTRY_DATE, ISS_TRANSACTION.AS_ON_DATE, ISS_TRANSACTION.USER_ID, ISS_BRANCH_INFO.BRANCH_CODE, ISS_BRANCH_INFO.BRANCH_NAME, ISS_NARRATION.BRANCH_NARRATION AS NARRATION,ISS_SUBCATEGORY.FIGURE_IND FROM ISS_TRANSACTION INNER JOIN ISS_CATEGORY ON ISS_TRANSACTION.CATEGORY_ID = ISS_CATEGORY.CATEGORY_ID INNER JOIN ISS_SUBCATEGORY ON ISS_TRANSACTION.SUBCATEGORY_ID = ISS_SUBCATEGORY.SUBCATEGORY_ID INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID INNER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID " + condition;
            sqlQuery = "SELECT ISS_TRANSACTION.TRAN_SL, ISS_TRANSACTION.TRAN_ID, ISS_TRANSACTION.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_TRANSACTION.AMOUNT, ISS_TRANSACTION.ENTRY_DATE, ISS_TRANSACTION.AS_ON_DATE, ISS_TRANSACTION.USER_ID, ISS_BRANCH_INFO.BRANCH_CODE, ISS_BRANCH_INFO.BRANCH_NAME, ISS_NARRATION.BRANCH_NARRATION AS NARRATION, ISS_SUBCATEGORY.FIGURE_IND FROM ISS_TRANSACTION INNER JOIN ISS_CATEGORY ON ISS_TRANSACTION.CATEGORY_ID = ISS_CATEGORY.CATEGORY_ID INNER JOIN ISS_SUBCATEGORY ON ISS_TRANSACTION.SUBCATEGORY_ID = ISS_SUBCATEGORY.SUBCATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID INNER JOIN ISS_BRANCH_INFO ON ISS_TRANSACTION.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE " + condition;
        }
        else
        {
            //sqlQuery = "SELECT ISS_TRANSACTION.TRAN_SL, ISS_TRANSACTION.TRAN_ID, ISS_TRANSACTION.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_TRANSACTION.AMOUNT, ISS_TRANSACTION.ENTRY_DATE, ISS_TRANSACTION.AS_ON_DATE, ISS_TRANSACTION.USER_ID, ISS_BRANCH_INFO.BRANCH_CODE, ISS_BRANCH_INFO.BRANCH_NAME, ISS_NARRATION.HO_NARRATION AS NARRATION,ISS_SUBCATEGORY.FIGURE_IND FROM ISS_TRANSACTION INNER JOIN ISS_CATEGORY ON ISS_TRANSACTION.CATEGORY_ID = ISS_CATEGORY.CATEGORY_ID INNER JOIN ISS_SUBCATEGORY ON ISS_TRANSACTION.SUBCATEGORY_ID = ISS_SUBCATEGORY.SUBCATEGORY_ID INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID INNER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID " + condition;
            sqlQuery = "SELECT ISS_TRANSACTION.TRAN_SL, ISS_TRANSACTION.TRAN_ID, ISS_TRANSACTION.CATEGORY_ID, ISS_CATEGORY.CATEGORY_NAME, ISS_SUBCATEGORY.SUBCATEGORY_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_TRANSACTION.AMOUNT, ISS_TRANSACTION.ENTRY_DATE, ISS_TRANSACTION.AS_ON_DATE, ISS_TRANSACTION.USER_ID, ISS_BRANCH_INFO.BRANCH_CODE, ISS_BRANCH_INFO.BRANCH_NAME, ISS_NARRATION.HO_NARRATION AS NARRATION, ISS_SUBCATEGORY.FIGURE_IND FROM ISS_TRANSACTION INNER JOIN ISS_CATEGORY ON ISS_TRANSACTION.CATEGORY_ID = ISS_CATEGORY.CATEGORY_ID INNER JOIN ISS_SUBCATEGORY ON ISS_TRANSACTION.SUBCATEGORY_ID = ISS_SUBCATEGORY.SUBCATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID INNER JOIN ISS_BRANCH_INFO ON ISS_TRANSACTION.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE " + condition;
        }
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region LoadUserInfoGridView
    public DataTable LoadUserInfoGridView(string condition)
    {
        sqlQuery = "SELECT ISS_USER_INFO.USER_SL,ISS_USER_INFO.USER_ID, ISS_USER_INFO.USER_NAME, (Case When ISS_USER_INFO.IS_ACTIVE=1 then 'Active' else 'InActive' end) as IS_ACTIVE, ISS_BRANCH_INFO.BRANCH_CODE, ISS_BRANCH_INFO.BRANCH_NAME,ISS_USER_INFO.DIVISION,ISS_USER_INFO.DESIGNATION FROM ISS_USER_INFO INNER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE " + condition;
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region LoadAcceptanceGridView
    public DataTable LoadAcceptanceGridView(string condition)
    {
        sqlQuery = "SELECT ISS_ACCEPTANCE_TRAN.ACCEP_SL, ISS_ACCEPTANCE_TRAN.BANK_ID, ISS_BANK_INFO.BANK_NAME, ISS_ACCEPTANCE_TRAN.ACCEPTANCE_ISSUED_AMOUNT, ISS_ACCEPTANCE_TRAN.ACCEPTANCE_MATURED_AMOUNT, ISS_ACCEPTANCE_TRAN.ACCEPTANCE_RECEIVED_AMOUNT, ISS_ACCEPTANCE_TRAN.ACCEPTANCE_PURCHASED_AMOUNT, ISS_ACCEPTANCE_TRAN.ACCEPTANCE_REC_MATURED_AMOUNT FROM ISS_ACCEPTANCE_TRAN INNER JOIN ISS_BANK_INFO ON ISS_ACCEPTANCE_TRAN.BANK_ID = ISS_BANK_INFO.BANK_ID " + condition;
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region Load Bank_Name
    public DataTable Bank_Name(string condition)
    {
        sqlQuery = "SELECT 0 BANK_ID, '-Select-' BANK_NAME UNION ALL SELECT X.BANK_ID,X.BANK_NAME FROM(SELECT BANK_ID,BANK_NAME FROM ISS_BANK_INFO)X " + condition + "ORDER BY BANK_NAME";
        //sqlQuery = "SELECT BANK_ID,BANK_NAME FROM ISS_BANK_INFO " + condition;
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region Load Branch
    public DataTable GetBranchName(string condition)
    {
        if (condition == "1")
        {
            sqlQuery = "SELECT BRANCH_CODE, BRANCH_NAME FROM ISS_BRANCH_INFO ORDER BY BRANCH_NAME ";
        }
        else
        {
            sqlQuery = "SELECT ISS_BRANCH_INFO.BRANCH_CODE, ISS_BRANCH_INFO.BRANCH_NAME, ISS_USER_INFO.HO_STATUS, ISS_USER_INFO.BR_STATUS FROM ISS_BRANCH_INFO INNER JOIN ISS_USER_INFO ON ISS_BRANCH_INFO.BRANCH_CODE = ISS_USER_INFO.BRANCH_CODE " + condition;
        }
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region LoadSubcategory
    public DataTable LoadSubcategory(string condition)
    {
        sqlQuery = " SELECT SUBCATEGORY_ID,SUBCATEGORY_NAME FROM ISS_SUBCATEGORY WHERE BRANCH_STATUS = 1 ORDER BY BR_SEQ " + condition;
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region Load User
    public DataTable GetUser(string condition)
    {
        sqlQuery = "SELECT ISS_USER_INFO.USER_ID,ISS_USER_INFO.USER_NAME FROM ISS_BRANCH_INFO INNER JOIN ISS_USER_INFO ON ISS_BRANCH_INFO.BRANCH_CODE = ISS_USER_INFO.BRANCH_CODE " + condition;
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #endregion

    #region Report Data

    #region GetBranchReportData
    public DataTable GetBranchReportData(string date, string brcode,int status)
    {
        if (brcode != "1001")
        {
            if (status == 1)
            {
                sqlQuery = " SELECT REPLACE(CONVERT(VARCHAR, ISS_TRANSACTION.AS_ON_DATE, 106), ' ', '-') AS DATE, ISS_BRANCH_INFO.BANK_ID, ISS_BRANCH_INFO.BRANCH_HO_ID AS BRANCH_ID, ISS_SUBCATEGORY.SUPERVISION_COA_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME AS [COA DESCRIPTION], ISS_TRANSACTION.AMOUNT AS AMOUNT_BDT, ISS_SUBCATEGORY.ISLAMIC_CON_IND AS [ISLAMIC CONVENTIONAL INDICATOR], ISS_BRANCH_INFO.BRANCH_NAME AS OFFICE_IND, '' AS Blank_One, '' AS Blank_Two, ISS_SUBCATEGORY.FIGURE_IND AS [Figure indication], ISS_CATEGORY.CATEGORY_NAME AS [Data category], ISS_NARRATION.BRANCH_NARRATION AS [Field Description] FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID LEFT OUTER JOIN ISS_BRANCH_INFO INNER JOIN ISS_TRANSACTION ON ISS_BRANCH_INFO.BRANCH_CODE = ISS_TRANSACTION.BRANCH_CODE ON ISS_SUBCATEGORY.SUBCATEGORY_ID = ISS_TRANSACTION.SUBCATEGORY_ID WHERE (ISS_SUBCATEGORY.BRANCH_STATUS = 1) AND (ISS_TRANSACTION.AS_ON_DATE = '" + date + "') AND (ISS_BRANCH_INFO.BRANCH_CODE = '" + brcode + "') ORDER BY ISS_SUBCATEGORY.BR_SEQ ";
                //sqlQuery = "SELECT   REPLACE(CONVERT(VARCHAR, AS_ON_DATE, 106), ' ', '-') AS DATE, BANK_ID, BRANCH_HO_ID AS BRANCH_ID, " +
                //            " ISS_SUBCATEGORY.SUPERVISION_COA_ID,SUBCATEGORY_NAME AS [COA DESCRIPTION],AMOUNT AS AMOUNT_BDT, ISLAMIC_CON_IND AS [ISLAMIC CONVENTIONAL INDICATOR], OFFICE_IND,'' AS Blank_One, '' AS Blank_Two, FIGURE_IND AS [Figure indication], " +
                //            " CATEGORY_NAME AS [Data category], BRANCH_NARRATION AS [Field Description] FROM ISS_CATEGORY INNER JOIN " +
                //            "ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN " +
                //            "ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID LEFT OUTER JOIN " +
                //            "ISS_TRANSACTION INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID AND ISS_TRANSACTION.AS_ON_DATE = '" + date + "' INNER JOIN " +
                //            "ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE AND " +
                //            "ISS_BRANCH_INFO.BRANCH_CODE = '" + brcode + "' ON ISS_SUBCATEGORY.SUBCATEGORY_ID = ISS_TRANSACTION.SUBCATEGORY_ID WHERE (ISS_SUBCATEGORY.BRANCH_STATUS = 1) ORDER BY ISS_SUBCATEGORY.BR_SEQ";
            }
            else
            {
                sqlQuery = "SELECT   REPLACE(CONVERT(VARCHAR, AS_ON_DATE, 106), ' ', '-') AS DATE, BANK_ID, BRANCH_HO_ID AS BRANCH_ID, " +
                            " ISS_SUBCATEGORY.SUPERVISION_COA_ID,SUBCATEGORY_NAME AS [COA DESCRIPTION],AMOUNT AS AMOUNT_BDT, OFFICE_IND, '' AS Blank_One, '' AS Blank_Two, FIGURE_IND AS [Figure indication], " +
                            " CATEGORY_NAME AS [Data category] FROM ISS_CATEGORY INNER JOIN " +
                            "ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN " +
                            "ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID LEFT OUTER JOIN " +
                            "ISS_TRANSACTION INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID AND ISS_TRANSACTION.AS_ON_DATE = '" + date + "' INNER JOIN " +
                            "ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE AND " +
                            "ISS_BRANCH_INFO.BRANCH_CODE = '" + brcode + "' ON ISS_SUBCATEGORY.SUBCATEGORY_ID = ISS_TRANSACTION.SUBCATEGORY_ID WHERE (ISS_SUBCATEGORY.BRANCH_STATUS = 1) ORDER BY ISS_SUBCATEGORY.BR_SEQ";
            }
        }
        else
        {
            sqlQuery = "SELECT REPLACE(CONVERT(VARCHAR, ISS_TRANSACTION.AS_ON_DATE, 106), ' ', '-') AS DATE, ISS_BRANCH_INFO.BANK_ID, ISS_BRANCH_INFO.BRANCH_HO_ID AS BRANCH_ID, ISS_SUBCATEGORY.SUPERVISION_COA_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME AS [COA DESCRIPTION], ISS_TRANSACTION.AMOUNT AS AMOUNT_BDT, ISS_SUBCATEGORY.ISLAMIC_CON_IND AS [ISLAMIC CONVENTIONAL INDICATOR], ISS_BRANCH_INFO.BRANCH_NAME AS OFFICE_IND, '' AS Blank_One, '' AS Blank_Two, ISS_SUBCATEGORY.FIGURE_IND AS [Figure indication], ISS_CATEGORY.CATEGORY_NAME AS [Data category], ISS_NARRATION.HO_NARRATION AS [Field Description] FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID LEFT OUTER JOIN ISS_BRANCH_INFO INNER JOIN ISS_TRANSACTION ON ISS_BRANCH_INFO.BRANCH_CODE = ISS_TRANSACTION.BRANCH_CODE ON ISS_SUBCATEGORY.SUBCATEGORY_ID = ISS_TRANSACTION.SUBCATEGORY_ID WHERE (ISS_SUBCATEGORY.DIVISION <> ' ') AND (ISS_TRANSACTION.AS_ON_DATE = '" + date + "') AND (ISS_BRANCH_INFO.BRANCH_CODE = '1106') ORDER BY dbo.ISS_SUBCATEGORY.HO_SEQ ";
            //sqlQuery = "SELECT TOP (100) PERCENT REPLACE(CONVERT(VARCHAR, ISS_TRANSACTION.AS_ON_DATE, 106), ' ', '-') AS DATE, ISS_BRANCH_INFO.BANK_ID, ISS_BRANCH_INFO.BRANCH_HO_ID AS BRANCH_ID,ISS_SUBCATEGORY.SUPERVISION_COA_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME AS [COA DESCRIPTION], ISS_TRANSACTION.AMOUNT AS AMOUNT_BDT, ISS_SUBCATEGORY.ISLAMIC_CON_IND AS [ISLAMIC CONVENTIONAL INDICATOR], " +
            //            " ISS_USER_INFO.OFFICE_IND,'' AS Blank_One, '' AS Blank_Two, ISS_SUBCATEGORY.FIGURE_IND AS [Figure indication], ISS_CATEGORY.CATEGORY_NAME AS [Data category], ISS_NARRATION.HO_NARRATION AS [Field Description] FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN ISS_NARRATION ON ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID LEFT OUTER JOIN " +
            //            " ISS_TRANSACTION INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID AND ISS_TRANSACTION.AS_ON_DATE = '" + date + "' INNER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE AND ISS_BRANCH_INFO.BRANCH_CODE = '" + brcode + "' ON ISS_SUBCATEGORY.SUBCATEGORY_ID = ISS_TRANSACTION.SUBCATEGORY_ID " +
            //            " WHERE (ISS_SUBCATEGORY.DIVISION <> ' ') ORDER BY ISS_SUBCATEGORY.HO_SEQ";
        }
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        //dt.Rows.Add("data");
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region GetHoReportData
    public DataTable GetHoReportData(string date,string condition)
    {
        if (date != "Active")
        {
            sqlQuery = " SELECT REPLACE(CONVERT(VARCHAR, dbo.View_HO1.AS_ON_DATE, 106), ' ', '-') AS DATE, dbo.View_HO2.BANK_ID, dbo.View_HO2.BRANCH_HO_ID AS HO_ID, " +
                            " dbo.View_HO2.SUBCATEGORY_NAME AS 'COA DESCRIPTION', dbo.View_HO1.AMOUNT AS AMOUNT_BDT, " +
                             "dbo.View_HO2.ISLAMIC_CON_IND AS 'ISLAMIC CONVENTIONAL INDICATOR', dbo.View_HO2.OFFICE_IND, dbo.View_HO2.FIGURE_IND AS 'Figure indication', " +
                            " dbo.View_HO2.CATEGORY_NAME AS 'Data category', dbo.View_HO2.HO_NARRATION AS 'Field Description' " +
                          " FROM dbo.View_HO2 LEFT OUTER JOIN   dbo.View_HO1 ON dbo.View_HO2.SUBCATEGORY_ID = dbo.View_HO1.SUBCATEGORY_ID AND dbo.View_HO1.AS_ON_DATE ='" + date + "' ORDER BY dbo.View_HO2.HO_SEQ";
        }
        else
        {
            sqlQuery = "SELECT ISS_BRANCH_INFO.BRANCH_NAME, ISS_USER_INFO.USER_ID, ISS_USER_INFO.USER_NAME, ISS_USER_INFO.DIVISION,REPLACE(CONVERT(VARCHAR, ISS_USER_INFO.CREATION_DATE, 106), ' ', '-') AS CREATION_DATE,REPLACE(CONVERT(VARCHAR, ISS_USER_INFO.LAST_MODIFICATION_DATE, 106), ' ', '-') AS LAST_MODIFICATION_DATE FROM ISS_USER_INFO INNER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE " + condition;
        }
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region GetHoDivisionReportData
    public DataTable GetHoDivisionReportData(string date, string brcode, string division)
    {
        sqlQuery = " SELECT REPLACE(CONVERT(VARCHAR, ISS_TRANSACTION.AS_ON_DATE, 106), ' ', '-') AS DATE, ISS_BRANCH_INFO.BANK_ID, ISS_BRANCH_INFO.BRANCH_HO_ID AS BRANCH_ID, ISS_SUBCATEGORY.SUBCATEGORY_NAME AS [COA DESCRIPTION], dbo.ISS_TRANSACTION.AMOUNT AS AMOUNT_BDT, dbo.ISS_SUBCATEGORY.ISLAMIC_CON_IND AS [ISLAMIC CONVENTIONAL INDICATOR], " +
                   " ISS_USER_INFO.OFFICE_IND, dbo.ISS_SUBCATEGORY.FIGURE_IND AS [Figure indication], dbo.ISS_CATEGORY.CATEGORY_NAME AS [Data category], ISS_NARRATION.HO_NARRATION AS [Field Description] FROM ISS_CATEGORY INNER JOIN ISS_SUBCATEGORY ON ISS_CATEGORY.CATEGORY_ID = ISS_SUBCATEGORY.CATEGORY_ID INNER JOIN ISS_NARRATION ON " +
                   " ISS_SUBCATEGORY.NARRATION_ID = ISS_NARRATION.NARRATION_ID LEFT OUTER JOIN ISS_TRANSACTION INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID AND ISS_TRANSACTION.AS_ON_DATE = '" + date + "' INNER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE AND " +
                   " ISS_BRANCH_INFO.BRANCH_CODE = '" + brcode + "' ON dbo.ISS_SUBCATEGORY.SUBCATEGORY_ID = dbo.ISS_TRANSACTION.SUBCATEGORY_ID WHERE (ISS_SUBCATEGORY.DIVISION = '" + division + "') ORDER BY ISS_SUBCATEGORY.HO_SEQ ";
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region GetIndividualFieldData
    public DataTable GetIndividualFieldData(string date, string SubCatId)
    {
        sqlQuery = " SELECT DISTINCT TOP (100) PERCENT ISS_SUBCATEGORY.SUBCATEGORY_NAME, ISS_BRANCH_INFO.BRANCH_NAME, ISS_TRANSACTION.AMOUNT FROM ISS_TRANSACTION INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID INNER JOIN " +
                   " ISS_SUBCATEGORY ON ISS_TRANSACTION.SUBCATEGORY_ID = ISS_SUBCATEGORY.SUBCATEGORY_ID RIGHT OUTER JOIN ISS_BRANCH_INFO ON ISS_USER_INFO.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE WHERE (ISS_BRANCH_INFO.BRANCH_CODE <> '1001') AND " +
                   " (ISS_BRANCH_INFO.BRANCH_CODE <> '1000') AND (ISS_TRANSACTION.AS_ON_DATE = '" + date + "') AND (ISS_TRANSACTION.SUBCATEGORY_ID = '" + SubCatId + "') ORDER BY ISS_BRANCH_INFO.BRANCH_NAME ";
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region GetAuthorizedBranch
    public DataTable GetAuthorizedBranch(string Tran_Id , string Type)
    {
        sqlQuery = " SELECT ISS_BRANCH_INFO.BRANCH_NAME FROM ISS_BRANCH_AUTHORIZATION INNER JOIN ISS_BRANCH_INFO ON ISS_BRANCH_AUTHORIZATION.BRANCH_CODE = ISS_BRANCH_INFO.BRANCH_CODE WHERE [ISS_BRANCH_AUTHORIZATION].TRAN_ID = '" + Tran_Id + "' AND TYPE = '" + Type + "' ";
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region GetHoAcceptanceReportData
    public DataTable GetHoAcceptanceReportData(string date)
    {
        sqlQuery = " SELECT     ISNULL(REPLACE(CONVERT(VARCHAR, A.AS_ON_DATE, 106), ' ', '-'),'') AS DATE,'43' AS BANK_ID,'430026' AS 'HO_ID',ISS_BANK_INFO.BANK_ID AS 'WITH BANK_ID',ISNULL(A.ACCEPTANCE_ISSUED_AMOUNT,0) as 'VALUE OF ACCEPTANCE ISSUED AMOUNT', "+
                   " ISNULL(A.ACCEPTANCE_MATURED_AMOUNT,0) AS 'VALUE OF ISSUED ACCEPTANCE MATURED', ISNULL(A.ACCEPTANCE_RECEIVED_AMOUNT,0) AS 'VALUE OF RECEIVED ACCEPTANCE',ISNULL(A.ACCEPTANCE_PURCHASED_AMOUNT,0) AS 'PURCHASED AMOUNT OF RECEIVED ACCEPTANCE', "+
                   " ISNULL(A.ACCEPTANCE_REC_MATURED_AMOUNT,0) AS 'MATURED OF RECEIVED ACCEPTANCE',  ISS_BANK_INFO.BANK_ID AS 'Bank ID',ISS_BANK_INFO.BANK_NAME FROM (SELECT  AS_ON_DATE, BANK_ID, SUM(ACCEPTANCE_ISSUED_AMOUNT) AS ACCEPTANCE_ISSUED_AMOUNT, "+
                   " SUM(ACCEPTANCE_MATURED_AMOUNT) AS ACCEPTANCE_MATURED_AMOUNT, SUM(ACCEPTANCE_RECEIVED_AMOUNT) AS ACCEPTANCE_RECEIVED_AMOUNT, SUM(ACCEPTANCE_PURCHASED_AMOUNT) AS ACCEPTANCE_PURCHASED_AMOUNT, SUM(ACCEPTANCE_REC_MATURED_AMOUNT) "+ 
                   " AS ACCEPTANCE_REC_MATURED_AMOUNT FROM ISS_ACCEPTANCE_TRAN WHERE AS_ON_DATE='"+date+"' GROUP BY AS_ON_DATE, BANK_ID) AS A RIGHT OUTER JOIN  ISS_BANK_INFO ON A.BANK_ID = ISS_BANK_INFO.BANK_ID";
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return dt;
    }
    #endregion

    #region GetBranchAcceptancdeReportData
    public DataTable GetBranchAcceptancdeReportData(string date, string brcode, int status)
    {
        if (brcode != "1001")
        {
            //if (status == 1)
            //{
            sqlQuery = "SELECT  ISNULL(REPLACE(CONVERT(VARCHAR, dbo.ISS_ACCEPTANCE_TRAN.AS_ON_DATE, 106), ' ', '-'),'') AS DATE, dbo.ISS_BRANCH_INFO.BANK_ID AS BANK_ID, " +
                             "dbo.ISS_BRANCH_INFO.BRANCH_HO_ID AS BRANCH_ID, dbo.ISS_BANK_INFO.BANK_ID AS WITH_BANK_ID, " +
                             "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_ISSUED_AMOUNT as 'VALUE OF ACCEPTANCE ISSUED AMOUNT', dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_MATURED_AMOUNT AS 'VALUE OF ISSUED ACCEPTANCE MATURED', " +
                             "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_RECEIVED_AMOUNT AS 'VALUE OF RECEIVED ACCEPTANCE', dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_PURCHASED_AMOUNT AS 'PURCHASED AMOUNT OF RECEIVED ACCEPTANCE', " +
                             "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_REC_MATURED_AMOUNT AS 'MATURED OF RECEIVED ACCEPTANCE','' AS Blank_One, dbo.ISS_BANK_INFO.BANK_ID AS 'Bank ID', dbo.ISS_BANK_INFO.BANK_NAME AS 'FI_NAME' " +
                             "FROM dbo.ISS_ACCEPTANCE_TRAN INNER JOIN " +
                             "dbo.ISS_BRANCH_INFO ON dbo.ISS_ACCEPTANCE_TRAN.BRANCH_CODE = dbo.ISS_BRANCH_INFO.BRANCH_CODE AND " +
                             "dbo.ISS_ACCEPTANCE_TRAN.BRANCH_CODE = '" + brcode + "' AND dbo.ISS_ACCEPTANCE_TRAN.AS_ON_DATE = '" + date + "' RIGHT OUTER JOIN " +
                             "dbo.ISS_BANK_INFO ON dbo.ISS_ACCEPTANCE_TRAN.BANK_ID = dbo.ISS_BANK_INFO.BANK_ID ORDER BY ISS_BANK_INFO.BANK_SL";
            //}
            //else
            //{
            //    sqlQuery = "SELECT  ISNULL(REPLACE(CONVERT(VARCHAR, dbo.ISS_ACCEPTANCE_TRAN.AS_ON_DATE, 106), ' ', '-'),'') AS DATE, dbo.ISS_BRANCH_INFO.BANK_ID AS BANK_ID, " +
            //                     "dbo.ISS_BRANCH_INFO.BRANCH_HO_ID AS BRANCH_ID, " +
            //                     "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_ISSUED_AMOUNT as 'VALUE OF ACCEPTANCE ISSUED AMOUNT', dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_MATURED_AMOUNT AS 'VALUE OF ISSUED ACCEPTANCE MATURED', " +
            //                     "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_RECEIVED_AMOUNT AS 'VALUE OF RECEIVED ACCEPTANCE', dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_PURCHASED_AMOUNT AS 'PURCHASED AMOUNT OF RECEIVED ACCEPTANCE', " +
            //                     "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_REC_MATURED_AMOUNT AS 'MATURED OF RECEIVED ACCEPTANCE','' AS Blank_One, dbo.ISS_BANK_INFO.BANK_NAME AS 'FI_NAME' " +
            //                     "FROM dbo.ISS_ACCEPTANCE_TRAN INNER JOIN " +
            //                     "dbo.ISS_BRANCH_INFO ON dbo.ISS_ACCEPTANCE_TRAN.BRANCH_CODE = dbo.ISS_BRANCH_INFO.BRANCH_CODE AND " +
            //                     "dbo.ISS_ACCEPTANCE_TRAN.BRANCH_CODE = '" + brcode + "' AND dbo.ISS_ACCEPTANCE_TRAN.AS_ON_DATE = '" + date + "' RIGHT OUTER JOIN " +
            //                     "dbo.ISS_BANK_INFO ON dbo.ISS_ACCEPTANCE_TRAN.BANK_ID = dbo.ISS_BANK_INFO.BANK_ID";
            //}
        }
        else
        {
            sqlQuery = "SELECT  ISNULL(REPLACE(CONVERT(VARCHAR, dbo.ISS_ACCEPTANCE_TRAN.AS_ON_DATE, 106), ' ', '-'),'') AS DATE, dbo.ISS_BRANCH_INFO.BANK_ID AS BANK_ID, " +
                             "dbo.ISS_BRANCH_INFO.BRANCH_HO_ID AS HO_ID, dbo.ISS_BANK_INFO.BANK_ID AS WITH_BANK_ID, " +
                             "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_ISSUED_AMOUNT as 'VALUE OF ACCEPTANCE ISSUED AMOUNT', dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_MATURED_AMOUNT AS 'VALUE OF ISSUED ACCEPTANCE MATURED', " +
                             "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_RECEIVED_AMOUNT AS 'VALUE OF RECEIVED ACCEPTANCE', dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_PURCHASED_AMOUNT AS 'PURCHASED AMOUNT OF RECEIVED ACCEPTANCE', " +
                             "dbo.ISS_ACCEPTANCE_TRAN.ACCEPTANCE_REC_MATURED_AMOUNT AS 'MATURED OF RECEIVED ACCEPTANCE','' AS Blank_One, dbo.ISS_BANK_INFO.BANK_ID AS 'Bank ID', dbo.ISS_BANK_INFO.BANK_NAME AS 'FI_NAME' " +
                             "FROM dbo.ISS_ACCEPTANCE_TRAN INNER JOIN " +
                             "dbo.ISS_BRANCH_INFO ON dbo.ISS_ACCEPTANCE_TRAN.BRANCH_CODE = dbo.ISS_BRANCH_INFO.BRANCH_CODE AND " +
                             "dbo.ISS_ACCEPTANCE_TRAN.BRANCH_CODE = '" + brcode + "' AND dbo.ISS_ACCEPTANCE_TRAN.AS_ON_DATE = '" + date + "' RIGHT OUTER JOIN " +
                             "dbo.ISS_BANK_INFO ON dbo.ISS_ACCEPTANCE_TRAN.BANK_ID = dbo.ISS_BANK_INFO.BANK_ID";
        }
        con = oConnectionDatabase.DatabaseConnection();
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if(dt.Rows[i]["DATE"].ToString()=="")
        //           dt.Rows[i]["DATE"]
        //    }
        //}
        return dt;
    }
    #endregion

    #endregion

    public double GetSingleSubcatData(string condition)
    {
        double returnvalue = 0;
        con = oConnectionDatabase.DatabaseConnection();
        sqlQuery = "SELECT ISS_TRANSACTION.AMOUNT AS AMOUNT FROM ISS_TRANSACTION INNER JOIN ISS_USER_INFO ON ISS_TRANSACTION.USER_ID = ISS_USER_INFO.USER_ID " + condition;
        cmd = new SqlCommand(sqlQuery, con);
        cmd.CommandType = CommandType.Text;
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            while (dr.Read())
            {
                DataRow datarow = dt.NewRow();
                returnvalue = Convert.ToDouble(dr["AMOUNT"]);
                dt.Rows.Add(datarow);
            }
            dr.Close();
        }
        try
        {
            da.Fill(dt);
        }
        catch { }
        con.Close();
        return returnvalue;
    }
    public DataTable GetGLAmount(string glLine, string date, string brCode)
    {
        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection("Data Source=WIN-MD2UGJCRCFV\\NBLREP;Initial Catalog=T24ReportDBNBL;Integrated Security=false;User ID=abc;Password=sys123;"))
        {
            try
            {
                connection.Open();
                sqlQuery = "SELECT [Line],[Legacy],[AcDesc],[ClosingBal],[BrCode],[ReportingMonth],[AsOneDate]  FROM GLPLBalance where Line='" + glLine + "' and AsOneDate='" + date + "' and BrCode='" + brCode + "'";
                cmd = new SqlCommand(sqlQuery, connection);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    sqlQuery = "SELECT [Line],[Legacy],[AcDesc],[ClosingBal],[BrCode],[ReportingMonth],[AsOneDate]  FROM GLPLBalanceHistory where Line='" + glLine + "' and AsOneDate='" + date + "' and BrCode='" + brCode + "'";
                    cmd = new SqlCommand(sqlQuery, connection);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch
            {

            }

            connection.Close();
        }
        return dt;
    }

    public DataTable GetPLAmount(string glLine, string date, string brCode)
    {
        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection("Data Source=WIN-MD2UGJCRCFV\\NBLREP;Initial Catalog=T24ReportDBNBL;Integrated Security=false;User ID=abc;Password=sys123;"))
        {
            try
            {
                connection.Open();
                sqlQuery = "SELECT [Line],[Legacy],[AcDesc],[ClosingBal],[BrCode],[ReportingMonth],[AsOneDate]  FROM PLBalance where Line='" + glLine + "' and AsOneDate='" + date + "' and BrCode='" + brCode + "'";
                cmd = new SqlCommand(sqlQuery, connection);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    sqlQuery = "SELECT [Line],[Legacy],[AcDesc],[ClosingBal],[BrCode],[ReportingMonth],[AsOneDate]  FROM PLBalanceHistory where Line='" + glLine + "' and AsOneDate='" + date + "' and BrCode='" + brCode + "'";
                    cmd = new SqlCommand(sqlQuery, connection);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch
            {

            }

            connection.Close();
        }
        return dt;
    }

     public DataTable GL_NUMBER_DT(string condition)
     {
         con = oConnectionDatabase.DatabaseConnection();
         sqlQuery = "SELECT * FROM ISS_GL_NUMBER " + condition;
         cmd = new SqlCommand(sqlQuery, con);
         cmd.CommandType = CommandType.Text;
         SqlDataAdapter da = new SqlDataAdapter(cmd);
         DataTable dt = new DataTable();
         da = new SqlDataAdapter(cmd);
         try
         {
             da.Fill(dt);
         }
         catch { }
         con.Close();
         return dt;
     }
     public DataTable GetBranchFromGL()
     {
         DataTable dt = new DataTable();
         using (SqlConnection connection = new SqlConnection("Data Source=WIN-MD2UGJCRCFV\\NBLREP;Initial Catalog=T24ReportDBNBL;Integrated Security=false;User ID=abc;Password=sys123;"))
         {
             try
             {
                 connection.Open();
                 sqlQuery = "SELECT distinct [BrCode] FROM [T24ReportDBNBL].[dbo].[GLPLBalance]";
                 //sqlQuery = "SELECT distinct [BrCode] FROM [T24ReportDBNBL].[dbo].[GLPLBalance] where brcode='106'";
                 cmd = new SqlCommand(sqlQuery, connection);
                 cmd.CommandType = CommandType.Text;
                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 da.Fill(dt);                
             }
             catch
             {

             }
             connection.Close();
         }
         return dt;
     }
}