using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HGarb;
using HGarb.Business;
using System.Web.UI.HtmlControls;
using HGarb.Models;
using System.Xml.Serialization;
using HGarb.Business.Rules;

namespace HGarb.Web
{
    public partial class rulesconfig : System.Web.UI.Page
    {     
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RulesConfig rulesConfig = new RulesConfig();
                ddlCompany.Items.Add("");
                List<string> lstCompanies = rulesConfig.LoadCompanies();
                int index = 0;
                foreach (var item in lstCompanies)
                {
                    ddlCompany.Items.Add(new ListItem(item, item));
                    index += 1;
                }
                ddlAssetClass.DataSource = this.getAssetClass();
                ddlAssetClass.DataBind();
                this.LoadGenericComponents();
            }
            else
            {
                this.RefreshRulesGrid();
            }
        }
        protected List<string> getAssetClass()
        {
            RulesConfig rulesConfig = new RulesConfig();
            return rulesConfig.LoadAssetClass();
        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            RulesConfig rulesConfig = new RulesConfig();
            ddlCompanyHeaders.Items.Add("");
            List<string> lstCompanies = rulesConfig.LoadCompanyHeader(ddlCompany.SelectedItem.Text);
            foreach (var item in lstCompanies)
            {
                ddlCompanyHeaders.Items.Add(new ListItem(item, item));
            }
        }
        protected void ddlCompanyHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            RulesConfig rulesConfig = new RulesConfig();
            List<string> lstStdFields = rulesConfig.LoadStandardFieldNames(ddlCompanyHeaders.SelectedItem.Text);
            foreach (var item in lstStdFields)
            {
                lbDEFields.Items.Add(new ListItem(item, item));
            }
        }
        protected void btnAddRule_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbRuleName.Text))
            {
                Rule rule = new Rule();
                rule.IsPreviousYear = cbPreviousYear.Checked;
                rule.PreviousYearColumns = tbPrevPeriodValues.Text;
                rule.RuleName = tbRuleName.Text;
                rule.RuleData = tbRuleData.Text;
                RootObject rootObject;
                if (Session["Rules"] != null)
                {
                    rootObject = Session["Rules"] as RootObject;
                    rootObject.Rules.Add(rule);
                    Session["Rules"] = rootObject;
                }
                else
                {
                    rootObject = new RootObject();
                    rootObject.ElementName = txtElementName.Text;
                    rootObject.ElementType = txtElementType.Text;
                    rootObject.IsAutoElementName = cbIsAutoElemName.Checked;
                    List<Rule> ruleList = new List<Rule>();
                    ruleList.Add(rule);
                    rootObject.Rules = ruleList;
                    Session["Rules"] = rootObject;
                }
                rootObject = Session["Rules"] as RootObject;
                LoadTable(rootObject);
            }
            else
            {
                tbRuleName.Focus();
            }
            
        }
        protected void cbPreviousYear_CheckedChanged(object sender, EventArgs e)
        {
            tbPrevPeriodValues.Enabled = !tbPrevPeriodValues.Enabled;
        }
        protected void cbIsInheritRule_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsInheritRule.Checked)
            {
                panel_GenericRuleType.Visible = true;
                ListGenericRuleTypes();
            }
            else
                panel_GenericRuleType.Visible = false;
        }
        private void ListGenericRuleTypes()
        {
            ddlSelectAssetClass.DataSource = this.getAssetClass();
            ddlSelectAssetClass.DataBind();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Rules"] != null)
                {
                    RootObject rootObject = Session["Rules"] as RootObject;
                    rootObject.CompanyName = ddlCompany.Text;
                    rootObject.CompanyHeader = ddlCompanyHeaders.Text;
                    RulesConfig rulesConfig = new RulesConfig();
                    rulesConfig.InsertRulesConfigV1(rootObject);
                    BuildRule buildRule = new BuildRule();
                    buildRule.ConstructRuleCode(ddlCompanyHeaders.Text);
                }
                ResetFields();
            }
            catch (Exception ex)
            {
            }
        }
        protected void btnAddGenericRule_Click(object sender, EventArgs e)
        {
            RulesConfig rulesConfig = new RulesConfig();

            GenericRootObject genericRootObject = rulesConfig.LoadGenericRulesByKeyV1(ddlSelectAssetClass.SelectedItem.ToString().Trim());
            Rule rule = new Rule();
            RootObject rootObject;
            if (Session["Rules"] != null)
            {
                rootObject = Session["Rules"] as RootObject;
                rootObject.IsGenericRuleInherited = true;
                rootObject.Rules = rootObject.Rules.Concat(genericRootObject.Rules).ToList();
                Session["Rules"] = rootObject;
            }
            else
            {
                rootObject = new RootObject();
                rootObject.IsGenericRuleInherited = true;
                rootObject.ElementName = txtElementName.Text;
                rootObject.ElementType = txtElementType.Text;
                rootObject.IsAutoElementName = cbIsAutoElemName.Checked;
                List<Rule> ruleList = new List<Rule>();
                ruleList.Add(rule);
                rootObject.Rules = ruleList;
                Session["Rules"] = rootObject;
            }
            rootObject = Session["Rules"] as RootObject;
            LoadTable(rootObject);
            ResetFields();

        }
        


        #region Generic Rule Funtions
        private void LoadGenericComponents()
        {
            RulesConfig rulesConfig = new RulesConfig();
            List<string> lstStdFields = rulesConfig.LoadGenericStandardFieldNames();
            foreach (var item in lstStdFields)
            {
                generic_stdFieldName.Items.Add(new ListItem(item, item));
            }
        }
        protected void generic_chkboxIsPreviousYear_CheckedChanged(object sender, EventArgs e)
        {
            generic_txtPreviousYearColumn.Enabled = !generic_txtPreviousYearColumn.Enabled;
        }
        protected void generic_btnAddRule_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(generic_txtRuleName.Text))
            {
                Rule rule = new Rule();
                rule.IsPreviousYear = generic_chkboxIsPreviousYear.Checked;
                rule.PreviousYearColumns = generic_txtPreviousYearColumn.Text;
                rule.RuleName = generic_txtRuleName.Text;
                rule.RuleData = generic_txtRuleCode.Text;
                GenericRootObject genericRootObject;
                if (Session["GenericRules"] != null)
                {
                    genericRootObject = Session["GenericRules"] as GenericRootObject;
                    genericRootObject.Rules.Add(rule);
                    Session["GenericRules"] = genericRootObject;
                }
                else
                {
                    genericRootObject = new GenericRootObject();
                    genericRootObject.AssestClass = ddlAssetClass.SelectedValue.ToString();
                    genericRootObject.ElementName = generic_txtElementName.Text;
                    genericRootObject.ElementType = generic_txtElementType.Text;
                    genericRootObject.IsAutoElementName = generic_chkboxIsAutoElementName.Checked;
                    List<Rule> ruleList = new List<Rule>();
                    ruleList.Add(rule);
                    genericRootObject.Rules = ruleList;
                    Session["GenericRules"] = genericRootObject;
                }
                genericRootObject = Session["GenericRules"] as GenericRootObject;
                LoadGenericTable(genericRootObject);
                ResetFields();
            }
            else
            {
                generic_txtRuleName.Focus();
            }
        }
        protected void generic_btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["GenericRules"] != null)
                {
                    GenericRootObject genericRootObject = Session["GenericRules"] as GenericRootObject;
                    RulesConfig rulesConfig = new RulesConfig();
                    rulesConfig.InsertGenericRulesConfigV1(genericRootObject);  
                }
                ResetFields();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Common Functions
        private void RefreshRulesGrid()
        {
            if (Session["GenericRules"] != null)
            {
                GenericRootObject genericRootObject = Session["GenericRules"] as GenericRootObject;
                LoadGenericTable(genericRootObject);
            }
            if (Session["Rules"] != null)
            {
                RootObject rootObject = Session["Rules"] as RootObject;
                LoadTable(rootObject);
            }
        }
        private void LoadTable(RootObject rootObject)
        {
            for(int i = 1; i < tblRules.Rows.Count; i++)
            {
                tblRules.Rows.RemoveAt(i);
            }
            foreach (var rule in rootObject.Rules)
            {
                TableRow tr = new TableRow();
                TableCell tcRuleName = new TableCell();
                tcRuleName.Text = rule.RuleName;
                TableCell tcRuleData = new TableCell();
                tcRuleData.Text = rule.RuleData;
                TableCell tcElemType = new TableCell();
                tcElemType.Text = rootObject.ElementType;
                TableCell tcElemName = new TableCell();
                tcElemName.Text = rootObject.ElementName;
                TableCell tcIsAutolem = new TableCell();
                tcIsAutolem.Text = rootObject.IsAutoElementName.ToString();
                TableCell tcIsPrevYear = new TableCell();
                tcIsPrevYear.Text = rule.IsPreviousYear.ToString();
                TableCell tcPrevYrVal = new TableCell();
                tcPrevYrVal.Text = rule.PreviousYearColumns;

                TableCell tcAction = new TableCell();
                HtmlGenericControl gc = new HtmlGenericControl();
                gc.InnerHtml = "<a href = \"#\"><i class=\"fa fa-2x fa-edit\"></i></a><a href = \"#\"><i class=\"fa fa-2x fa-remove\"></i></a>";
                tcAction.Controls.Add(gc);
                tr.Cells.Add(tcRuleName);
                tr.Cells.Add(tcElemType);
                tr.Cells.Add(tcElemName);
                tr.Cells.Add(tcIsAutolem);
                tr.Cells.Add(tcRuleData);
                tr.Cells.Add(tcIsPrevYear);
                tr.Cells.Add(tcPrevYrVal);
                tr.Cells.Add(tcAction);
                tblRules.Rows.Add(tr);
                
            }
        }
        private void LoadGenericTable(GenericRootObject genericRootObject)
        {
            for(int i=1;i< generic_TableRules.Rows.Count; i++)
            {
                generic_TableRules.Rows.RemoveAt(i); 
            }
            foreach (var _rule in genericRootObject.Rules)
            {
                TableRow tr = new TableRow();
                TableCell tcRuleName = new TableCell();
                tcRuleName.Text = _rule.RuleName;
                TableCell tcRuleData = new TableCell();
                tcRuleData.Text = _rule.RuleData;
                TableCell tcElemType = new TableCell();
                tcElemType.Text = genericRootObject.ElementType;
                TableCell tcElemName = new TableCell();
                tcElemName.Text = genericRootObject.ElementName;
                TableCell tcIsAutolem = new TableCell();
                tcIsAutolem.Text = genericRootObject.IsAutoElementName.ToString();
                TableCell tcIsPrevYear = new TableCell();
                tcIsPrevYear.Text = _rule.IsPreviousYear.ToString();
                TableCell tcPrevYrVal = new TableCell();
                tcPrevYrVal.Text = _rule.PreviousYearColumns;
                TableCell tcAction = new TableCell();
                HtmlGenericControl gc = new HtmlGenericControl();
                gc.InnerHtml = "<a href = \"#\"><i class=\"fa fa-2x fa-edit\"></i></a><a href = \"#\"><i class=\"fa fa-2x fa-remove\"></i></a>";
                tcAction.Controls.Add(gc);
                tr.Cells.Add(tcRuleName);
                tr.Cells.Add(tcElemType);
                tr.Cells.Add(tcElemName);
                tr.Cells.Add(tcIsAutolem);
                tr.Cells.Add(tcRuleData);
                tr.Cells.Add(tcIsPrevYear);
                tr.Cells.Add(tcPrevYrVal);
                tr.Cells.Add(tcAction);
                generic_TableRules.Rows.Add(tr);
            }
        }
        private void ResetFields()
        {
            
            generic_txtRuleName.Text = string.Empty;
            generic_txtRuleCode.Text = string.Empty;
            generic_txtPreviousYearColumn.Text = string.Empty;
            generic_stdFieldName.ClearSelection();
            generic_lstboxOperator.ClearSelection();
            tbRuleName.Text = string.Empty;
            tbRuleData.Text = string.Empty;
            tbPrevPeriodValues.Text = string.Empty;
            lbDEFields.ClearSelection();
            lbOperator.ClearSelection();
        }
        #endregion

        protected void cbIsAutoElemName_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}