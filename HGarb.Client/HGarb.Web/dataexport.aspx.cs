using HGarb.Business;
using HGarb.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGarb.Web
{
    public partial class dataexport : System.Web.UI.Page
    {
        private Dictionary<string, RuleInstance> ruleInstanceList = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            RulesConfig rulesConfig = new RulesConfig();
            ddlCOmpany.Items.Add("");
            List<string> lstCompanies = rulesConfig.LoadCompanies();
            int index = 0;
            foreach (var item in lstCompanies)
            {
                ddlCOmpany.Items.Add(new ListItem(item, item));
                index += 1;
            }
        }

        protected void ddlCOmpany_SelectedIndexChanged(object sender, EventArgs e)
        {
            RulesConfig rulesConfig = new RulesConfig();
            ddlCompanyHeaders.Items.Add("");
            List<string> lstCompanies = rulesConfig.LoadCompanyHeader(ddlCOmpany.SelectedItem.Text);
            foreach (var item in lstCompanies)
            {
                ddlCompanyHeaders.Items.Add(new ListItem(item, item));
            }
        }

        protected void btnExeRules_Click(object sender, EventArgs e)
        {
            string compHeader = System.Text.RegularExpressions.Regex.Replace(ddlCompanyHeaders.Text, "[^0-9a-zA-Z]+", "");
            Dictionary<string, string> dictResult = this.InvokeRuleDll(System.IO.Path.Combine(Helper.GetAppSetting("RuleDllSavePath"), compHeader + ".dll"), ddlCOmpany.SelectedValue, ddlCompanyHeaders.Text, Helper.GetAppSetting("ConnectionString"), txtYear.Text);
            pnlResult.Visible = true;
            var data = dictResult.Select(x => new RulesResult() { RuleName = x.Key, Status = x.Value });
            gvRulesResult.DataSource = data;
            gvRulesResult.DataBind();
        }

        private Dictionary<string, string> InvokeRuleDll(string ruleDllPath, string compId, string companyHeader, string connectionString, string year)
        {
            //log.WriteLog("Invoking rule dll...", LogTypes.TRACE);
            MethodInfo methodObj = null;
            object MyObj = null;
            if (ruleInstanceList == null)
            {
                MyObj = CreateInstance(ruleDllPath);
                //log.WriteLog("RulePath..." + rulePath, LogTypes.TRACE);
                ruleInstanceList = new Dictionary<string, RuleInstance>();
                MethodInfo omthd = (MyObj.GetType()).GetMethod("runRules");
                AddInstance(ruleDllPath, MyObj, omthd);
            }

            if (CheckRuleInstanceList(ruleDllPath) == true)
            {
                RuleInstance ins = ruleInstanceList[ruleDllPath];
                MyObj = ins.RuleInvoker;
                methodObj = ins.MethodToInvoke;
            }

            if (MyObj != null && methodObj != null)
            {
                var ctor = MyObj.GetType().GetConstructor(new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) });
                //log.WriteLog("Rule connstr " + connectionString, LogTypes.TRACE);
                //log.WriteLog("Rule connstr " + connectionString, LogTypes.ERROR);
                var obj = ctor.Invoke(new object[] { compId, companyHeader, connectionString, year });
                //PropertyInfo traceProp = obj.GetType().GetProperty("IsTrace");
                //if (traceProp != null) traceProp.SetValue(obj, IsRuleTraceLog, null);
                //PropertyInfo errorProp = obj.GetType().GetProperty("IsError");
                //if (errorProp != null) errorProp.SetValue(obj, IsRuleErrorLog, null);
                PropertyInfo tracelogProp = obj.GetType().GetProperty("TraceLog");
                //if (tracelogProp != null) tracelogProp.SetValue(obj, traceStr, null);
                PropertyInfo errorlogProp = obj.GetType().GetProperty("errorLog");
                //if (errorlogProp != null) errorlogProp.SetValue(obj, errStr, null);
                //object[] myparam = new object[6];
                //myparam[0] = "";
                //myparam[1] = pageData;
                //myparam[2] = loanNumber.ToString();
                //myparam[3] = borrowerId.ToString();
                //myparam[4] = isSecReIssue;
                //myparam[5] = fieldString;
                object result = methodObj.Invoke(obj, null);
                Dictionary<string, string> dictResult = obj.GetType().GetProperty("DictRulesResult").GetValue(obj, null) as Dictionary<string, string>;
                return dictResult;
            }

            return null;
        }

        private void AddInstance(string path, object ruleObject, MethodInfo methodToInvoke)
        {
            RuleInstance objInstance = new RuleInstance();
            objInstance.RuleInvoker = ruleObject;
            objInstance.MethodToInvoke = methodToInvoke;
            ruleInstanceList.Add(path, objInstance);
        }

        private Boolean CheckRuleInstanceList(string rulePath)
        {
            if (string.IsNullOrEmpty(rulePath) == false)
            {
                if (ruleInstanceList.ContainsKey(rulePath) == true)
                    return true;
                else
                {
                    object classObj = CreateInstance(rulePath);
                    MethodInfo methodToinvoke = (classObj.GetType()).GetMethod("runRule");
                    AddInstance(rulePath, classObj, methodToinvoke);
                    return true;
                }
            }
            return false;
        }

        private object CreateInstance(string rulePath)
        {
            Assembly ruleAssembly = null;
            ruleAssembly = Assembly.LoadFile(rulePath);
            if (ruleAssembly != null)
            {
                object MyObj = ruleAssembly.CreateInstance("EValuate.EvalRunTime");
                return MyObj;
            }
            return null;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        protected void ExportToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "RulesResults" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvRulesResult.GridLines = GridLines.Both;
            gvRulesResult.HeaderStyle.Font.Bold = true;
            gvRulesResult.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
    }

    public class RuleInstance
    {
        private MethodInfo _methodInfo = null;
        private object _ruleObject = null;

        public object RuleInvoker
        {
            get { return _ruleObject; }
            set { _ruleObject = value; }
        }
        public MethodInfo MethodToInvoke
        {
            get { return _methodInfo; }
            set { _methodInfo = value; }
        }
    }

    public class RulesResult
    {
        public String RuleName { get; set; }
        public String Status { get; set; }
    }
}