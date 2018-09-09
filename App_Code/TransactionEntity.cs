using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TransactionEntity
/// </summary>
public class TransactionEntity
{
    public TransactionEntity()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // Common Fields for All Table
    //public int BANK_ID { get; set; }

    public string BANK_ID { get; set; }

    public string BRANCH_CODE { get; set; }

    public int INSERT_STATUS { get; set; }

    public int UPDATE_STATUS { get; set; }

    public int CATEGORY_ID { get; set; }

    public string TRAN_ID { get; set; }

    public int ACCEP_SL { get; set; }

    public string ACCEP_ID { get; set; }

    public string ENTRY_DATE { get; set; }

    public string USER_ID { get; set; }

    public string AS_ON_DATE { get; set; }

    public int ZONE_ID { get; set; }

    public int TRAN_SL { get; set; }

    public int SUBCATEGORY_ID { get; set; }

    public double AMOUNT { get; set; }

    public string ENTRY_TIME { get; set; }

    public string USER_NAME { get; set; }

    public string PASSWORD { get; set; }

    public int IS_ACTIVE { get; set; }

    public int HO_STATUS { get; set; }

    public int BR_STATUS { get; set; }

    //Specific Field in BRANCHWISE_MONITORING_MAINTENANCE

    public int CAT_MAIN_SL { get; set; }

    //Specific Field in ISS_ACCEPTANCE_TRAN

    public double ACCEPTANCE_ISSUED_AMOUNT { get; set; }

    public double ACCEPTANCE_MATURED_AMOUNT { get; set; }

    public double ACCEPTANCE_RECEIVED_AMOUNT { get; set; }

    public double ACCEPTANCE_PURCHASED_AMOUNT { get; set; }

    public double ACCEPTANCE_REC_MATURED_AMOUNT { get; set; }

    //Specific Field in ISS_BRANCH_INFO

    public int BRANCH_SL { get; set; }

    public string BRANCH_NAME { get; set; }

    public int BRANCH_HO_ID { get; set; }

    public int AD_STATUS { get; set; }

    //Specific Field in ISS_LOG_DETAILS

    public int LOG_SL { get; set; }

    public string LOG_DESCRIPTION { get; set; }

    public int ACTIVITY_ID { get; set; }

    public string ENTRY_EXIT_TIME { get; set; }

    //Specific Field in ISS_REGION_INFO

    public int REGION_SL { get; set; }

    public string REGION_NAME { get; set; }

    public string REG_SHORT_NAME { get; set; }

    public string REG_ADDRESS { get; set; }

    public string REG_CONTRACT { get; set; }

    public string REMARKS { get; set; }

    //Specific Field in ISS_USER_AUDIT

    public int USER_AUDIT_SL { get; set; }

    public string AUDIT_DESCRIPTION { get; set; }

    public string AUTHORISED_USER { get; set; }

    public string AUTHORISED_TIME { get; set; }

    //Specific Field in User Info Table
    public int USER_SL { get; set; }

    public int PERMISSION_ID { get; set; }

    public int STATUS { get; set; }

    public int PREVILEGE_STATUS { get; set; }

    public string CREATION_DATE { get; set; }

    public string LAST_MODIFICATION_DATE { get; set; }

    public string OFFICE_IND { get; set; }

    public string DIVISION { get; set; }

    public string DESIGNATION { get; set; }

    //Specific Field in ISS_BRANCH_AUTHORIZATION

    public int AUTHO_STATUS { get; set; }
    public string TYPE { get; set; }

    //Specific Field in ISS_SFT_ALIVE

    public int SL_NO { get; set; }
    public int ALIVE_DATE { get; set; }

}