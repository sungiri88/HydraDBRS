using Hydra.DBRS.Infrastructure;
using Hydra.DBRS.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hydra.DBRS.DataAccess
{
    public class RulesConfig
    {
        public string ConnectionString { get; set; }
        private Microsoft.Practices.EnterpriseLibrary.Data.Database database;
        public RulesConfig()
        {
            this.ConnectionString = Helper.GetAppSetting("ConnectionString");
            this.database = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(this.ConnectionString);
        }

        public DataSet LoadCompanies()
        {
            using (SqlConnection con = new SqlConnection(this.ConnectionString))
            {
                using (DbCommand cmd = this.database.GetSqlStringCommand("select distinct CompanyName from CompanyInfo"))
                {
                    return this.database.ExecuteDataSet(cmd);
                }
            }
        }

        public DataSet LoadCompanyHeaders(string companyName)
        {
            using (DbCommand cmd = this.database.GetSqlStringCommand("select Header from CompanyInfo where CompanyName = '" + companyName + "'"))
            {
                return this.database.ExecuteDataSet(cmd);
            }
        }

        public DataSet LoadStandardFieldNames(string companyHeader)
        {
            using (DbCommand cmd = this.database.GetSqlStringCommand("select distinct StandardFieldName from DataElement DE inner join IncomingFiles I on I.CompanyHeader = '" + companyHeader + "' where DE.StandardFieldName is not null and I.Id = DE.FileId"))
            {
                return this.database.ExecuteDataSet(cmd);
            }
        }

        public void InsertRulesConfig(RulesInfo rulesInfo)
        {
            using (DbCommand cmd = this.database.GetStoredProcCommand("pInsertRuleConfig"))
            {
                this.database.AddInParameter(cmd, "@RuleName", DbType.String, rulesInfo.RuleName);
                this.database.AddInParameter(cmd, "@RuleCondition", DbType.String, rulesInfo.RuleCondition);
                this.database.AddInParameter(cmd, "@ElementName", DbType.String, rulesInfo.ElementName);
                this.database.AddInParameter(cmd, "@ElementType", DbType.String, rulesInfo.ElementType);
                this.database.AddInParameter(cmd, "@IsAutoElementName", DbType.Boolean, rulesInfo.IsAutoElementName);
                this.database.AddInParameter(cmd, "@IsPreviousYear", DbType.Boolean, rulesInfo.IsPreviousYear);
                this.database.AddInParameter(cmd, "@PreviousYearColumns", DbType.String, rulesInfo.PreviousYearColumns);
                this.database.AddInParameter(cmd, "@CompanyHeader", DbType.String, rulesInfo.CompanyHeader);
                this.database.ExecuteNonQuery(cmd);
            }
        }

        public DataSet LoadRules(string companyHeader)
        {
            using (DbCommand cmd = this.database.GetSqlStringCommand("select * from RulesConfig where CompanyHeader = '" + companyHeader + "'"))
            {
                return this.database.ExecuteDataSet(cmd);
            }
        }
    }
}