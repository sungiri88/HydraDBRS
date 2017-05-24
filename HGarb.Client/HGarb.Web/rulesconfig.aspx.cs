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

namespace HGarb.Web
{
    public partial class rulesconfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

                this.LoadGenericComponents();
            }
            else
            {
                //this.RefreshRulesGrid();
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

        protected void ddlCompanyHeaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            RulesConfig rulesConfig = new RulesConfig();
            List<string> lstStdFields = rulesConfig.LoadStandardFieldNames(ddlCompanyHeaders.SelectedItem.Text);
            foreach (var item in lstStdFields)
            {
                lbDEFields.Items.Add(new ListItem(item, item));
            }

            Dictionary<string, RulesInfo> dictRules = rulesConfig.LoadRules(ddlCompanyHeaders.SelectedItem.Text);
            Session["Rules"] = dictRules;
            if (dictRules != null && dictRules.Count > 0)
            {
                foreach (var rule in dictRules)
                {
                    TableRow tr = new TableRow();
                    TableCell tcRuleName = new TableCell();
                    tcRuleName.Text = rule.Key;
                    TableCell tcRuleData = new TableCell();
                    tcRuleData.Text = rule.Value.RuleCondition;
                    TableCell tcElemType = new TableCell();
                    tcElemType.Text = rule.Value.ElementType;
                    TableCell tcElemName = new TableCell();
                    tcElemName.Text = rule.Value.ElementName;
                    TableCell tcIsAutolem = new TableCell();
                    tcIsAutolem.Text = rule.Value.IsAutoElementName.ToString();
                    TableCell tcIsPrevYear = new TableCell();
                    tcIsPrevYear.Text = rule.Value.IsPreviousYear.ToString();
                    TableCell tcPrevYrVal = new TableCell();
                    tcPrevYrVal.Text = rule.Value.PreviousYearColumns;

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

                    tbRuleName.Text = string.Empty;
                    tbRuleData.Text = string.Empty;
                    tbPrevPeriodValues.Text = string.Empty;
                    lbDEFields.ClearSelection();
                    lbOperator.ClearSelection();
                }
            }
        }

        protected void btnAddRule_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbRuleName.Text))
            {
                Dictionary<string, RulesInfo> dictRules = new Dictionary<string, RulesInfo>();
                if (Session["Rules"] != null)
                {
                    dictRules = Session["Rules"] as Dictionary<string, RulesInfo>;
                }

                if (!dictRules.ContainsKey(tbRuleName.Text.Trim()))
                {
                    RulesInfo rulesInfo = new RulesInfo()
                    {
                        ElementName = txtElementName.Text,
                        ElementType = txtElementType.Text,
                        IsAutoElementName = cbIsAutoElemName.Checked,
                        IsPreviousYear = cbPreviousYear.Checked,
                        PreviousYearColumns = tbPrevPeriodValues.Text,
                        RuleCondition = tbRuleData.Text,
                        RuleName = tbRuleName.Text
                    };

                    dictRules.Add(tbRuleName.Text, rulesInfo);
                    Session["Rules"] = dictRules;
                    foreach (var rule in dictRules)
                    {
                        TableRow tr = new TableRow();
                        TableCell tcRuleName = new TableCell();
                        tcRuleName.Text = rule.Key;
                        TableCell tcRuleData = new TableCell();
                        tcRuleData.Text = rule.Value.RuleCondition;
                        TableCell tcElemType = new TableCell();
                        tcElemType.Text = rule.Value.ElementType;
                        TableCell tcElemName = new TableCell();
                        tcElemName.Text = rule.Value.ElementName;
                        TableCell tcIsAutolem = new TableCell();
                        tcIsAutolem.Text = rule.Value.IsAutoElementName.ToString();
                        TableCell tcIsPrevYear = new TableCell();
                        tcIsPrevYear.Text = rule.Value.IsPreviousYear.ToString();
                        TableCell tcPrevYrVal = new TableCell();
                        tcPrevYrVal.Text = rule.Value.PreviousYearColumns;

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

                        tbRuleName.Text = string.Empty;
                        tbRuleData.Text = string.Empty;
                        tbPrevPeriodValues.Text = string.Empty;
                        lbDEFields.ClearSelection();
                        lbOperator.ClearSelection();
                    }
                }
                else
                {
                    tbRuleName.Focus();
                }
            }
            else
            {
                tbRuleName.Focus();
            }
            //trRuleRow.InnerHtml += "<td>" + tbRuleName.Text + "</td><td>" + tbRuleData.Text + "</td><td><a href = \"\"><i class=\"fa fa-2x fa-edit\"></i></a><a href = \"\"><i class=\"fa fa-2x fa-remove\"></i></a></td>";
        }

        protected void cbIsAutoElemName_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void cbPreviousYear_CheckedChanged(object sender, EventArgs e)
        {
            tbPrevPeriodValues.Enabled = !tbPrevPeriodValues.Enabled;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, RulesInfo> dictRules = default(Dictionary<string, RulesInfo>);
                if (Session["Rules"] != null)
                {
                    dictRules = Session["Rules"] as Dictionary<string, RulesInfo>;
                }

                if (dictRules != null)
                {
                    foreach (var rule in dictRules)
                    {
                        RulesConfig rulesConfig = new RulesConfig();
                        rule.Value.CompanyHeader = ddlCompanyHeaders.SelectedItem.Text;
                        rulesConfig.InsertRulesConfig(rule.Value);
                        HGarb.Business.Rules.BuildRule buildRule = new HGarb.Business.Rules.BuildRule();
                        buildRule.StandardFieldNames = new List<string>();
                        foreach (var item in lbDEFields.Items)
                        {
                            buildRule.StandardFieldNames.Add((item as ListItem).Value);
                        }

                        buildRule.ConstructRuleCode(ddlCompanyHeaders.SelectedItem.Text);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadGenericComponents()
        {
            RulesConfig rulesConfig = new RulesConfig();
            List<string> lstStdFields = rulesConfig.LoadGenericStandardFieldNames();
            foreach (var item in lstStdFields)
            {
                generic_stdFieldName.Items.Add(new ListItem(item, item));
            }

            this.LoadGenericRules();
        }

        protected void generic_chkboxIsPreviousYear_CheckedChanged(object sender, EventArgs e)
        {
            generic_txtPreviousYearColumn.Enabled = !generic_txtPreviousYearColumn.Enabled;
        }

        protected void generic_btnAddRule_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(generic_txtRuleName.Text))
            {
                Dictionary<string, RulesInfo> dictRules = new Dictionary<string, RulesInfo>();
                if (Session["GenericRules"] != null)
                {
                    dictRules = Session["GenericRules"] as Dictionary<string, RulesInfo>;
                }

                if (!dictRules.ContainsKey(generic_txtRuleName.Text.Trim()))
                {
                    RulesInfo rulesInfo = new RulesInfo()
                    {
                        ElementName = generic_txtElementName.Text,
                        ElementType = generic_txtElementType.Text,
                        IsAutoElementName = generic_chkboxIsAutoElementName.Checked,
                        IsPreviousYear = generic_chkboxIsPreviousYear.Checked,
                        PreviousYearColumns = generic_txtPreviousYearColumn.Text,
                        RuleCondition = generic_txtRuleCode.Text,
                        RuleName = generic_txtRuleName.Text
                    };

                    dictRules.Add(generic_txtRuleName.Text, rulesInfo);
                    Session["GenericRules"] = dictRules;
                    foreach (var rule in dictRules)
                    {
                        TableRow tr = new TableRow();
                        TableCell tcRuleName = new TableCell();
                        tcRuleName.Text = rule.Key;
                        TableCell tcRuleData = new TableCell();
                        tcRuleData.Text = rule.Value.RuleCondition;
                        TableCell tcElemType = new TableCell();
                        tcElemType.Text = rule.Value.ElementType;
                        TableCell tcElemName = new TableCell();
                        tcElemName.Text = rule.Value.ElementName;
                        TableCell tcIsAutolem = new TableCell();
                        tcIsAutolem.Text = rule.Value.IsAutoElementName.ToString();
                        TableCell tcIsPrevYear = new TableCell();
                        tcIsPrevYear.Text = rule.Value.IsPreviousYear.ToString();
                        TableCell tcPrevYrVal = new TableCell();
                        tcPrevYrVal.Text = rule.Value.PreviousYearColumns;

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
                        genric_TableRules.Rows.Add(tr);

                        generic_txtRuleName.Text = string.Empty;
                        generic_txtRuleCode.Text = string.Empty;
                        generic_txtPreviousYearColumn.Text = string.Empty;
                        generic_stdFieldName.ClearSelection();
                        generic_lstboxOperator.ClearSelection();
                    }
                }
                else
                {
                    generic_txtRuleName.Focus();
                }
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
                Dictionary<string, RulesInfo> dictRules = default(Dictionary<string, RulesInfo>);
                if (Session["GenericRules"] != null)
                {
                    dictRules = Session["GenericRules"] as Dictionary<string, RulesInfo>;
                }

                if (dictRules != null)
                {
                    foreach (var rule in dictRules)
                    {
                        RulesConfig rulesConfig = new RulesConfig();
                        rulesConfig.InsertGenericRulesConfig(rule.Value);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void cbIsInheritRule_CheckedChanged(object sender, EventArgs e)
        {
            InheritGenericRules();
        }

        private void LoadGenericRules()
        {
            RulesConfig rulesConfig = new RulesConfig();
            Dictionary<string, RulesInfo> dictRules = rulesConfig.LoadGenericRules();
            Session["GenericRules"] = dictRules;
            if (dictRules != null && dictRules.Count > 0)
            {
                foreach (var rule in dictRules)
                {
                    TableRow tr = new TableRow();
                    TableCell tcRuleName = new TableCell();
                    tcRuleName.Text = rule.Key;
                    TableCell tcRuleData = new TableCell();
                    tcRuleData.Text = rule.Value.RuleCondition;
                    TableCell tcElemType = new TableCell();
                    tcElemType.Text = rule.Value.ElementType;
                    TableCell tcElemName = new TableCell();
                    tcElemName.Text = rule.Value.ElementName;
                    TableCell tcIsAutolem = new TableCell();
                    tcIsAutolem.Text = rule.Value.IsAutoElementName.ToString();
                    TableCell tcIsPrevYear = new TableCell();
                    tcIsPrevYear.Text = rule.Value.IsPreviousYear.ToString();
                    TableCell tcPrevYrVal = new TableCell();
                    tcPrevYrVal.Text = rule.Value.PreviousYearColumns;

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
                    genric_TableRules.Rows.Add(tr);

                    generic_txtRuleName.Text = string.Empty;
                    generic_txtRuleCode.Text = string.Empty;
                    generic_txtPreviousYearColumn.Text = string.Empty;
                    generic_stdFieldName.ClearSelection();
                    generic_lstboxOperator.ClearSelection();
                }
            }
        }

        private void InheritGenericRules()
        {
            RulesConfig rulesConfig = new RulesConfig();
            Dictionary<string, RulesInfo> dictRules = new Dictionary<string, RulesInfo>();
            if (Session["Rules"] != null)
            {
                dictRules = Session["Rules"] as Dictionary<string, RulesInfo>;
            }

            Dictionary<string, RulesInfo> dictGenericRules = rulesConfig.LoadGenericRules();
            foreach (var item in dictGenericRules)
            {
                dictRules.Add("Gen_" + item.Key, item.Value);
            }

            Session["Rules"] = dictRules;
            tblRules.Rows.Clear();
            foreach (var rule in dictRules)
            {
                TableRow tr = new TableRow();
                TableCell tcRuleName = new TableCell();
                tcRuleName.Text = rule.Key;
                TableCell tcRuleData = new TableCell();
                tcRuleData.Text = rule.Value.RuleCondition;
                TableCell tcElemType = new TableCell();
                tcElemType.Text = rule.Value.ElementType;
                TableCell tcElemName = new TableCell();
                tcElemName.Text = rule.Value.ElementName;
                TableCell tcIsAutolem = new TableCell();
                tcIsAutolem.Text = rule.Value.IsAutoElementName.ToString();
                TableCell tcIsPrevYear = new TableCell();
                tcIsPrevYear.Text = rule.Value.IsPreviousYear.ToString();
                TableCell tcPrevYrVal = new TableCell();
                tcPrevYrVal.Text = rule.Value.PreviousYearColumns;

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

                tbRuleName.Text = string.Empty;
                tbRuleData.Text = string.Empty;
                tbPrevPeriodValues.Text = string.Empty;
                lbDEFields.ClearSelection();
                lbOperator.ClearSelection();
            }
        }

        private void RefreshRulesGrid()
        {
            Dictionary<string, RulesInfo> dictRules = new Dictionary<string, RulesInfo>();
            if (Session["Rules"] != null)
            {
                dictRules = Session["Rules"] as Dictionary<string, RulesInfo>;
            }

            foreach (var rule in dictRules)
            {
                TableRow tr = new TableRow();
                TableCell tcRuleName = new TableCell();
                tcRuleName.Text = rule.Key;
                TableCell tcRuleData = new TableCell();
                tcRuleData.Text = rule.Value.RuleCondition;
                TableCell tcElemType = new TableCell();
                tcElemType.Text = rule.Value.ElementType;
                TableCell tcElemName = new TableCell();
                tcElemName.Text = rule.Value.ElementName;
                TableCell tcIsAutolem = new TableCell();
                tcIsAutolem.Text = rule.Value.IsAutoElementName.ToString();
                TableCell tcIsPrevYear = new TableCell();
                tcIsPrevYear.Text = rule.Value.IsPreviousYear.ToString();
                TableCell tcPrevYrVal = new TableCell();
                tcPrevYrVal.Text = rule.Value.PreviousYearColumns;

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

        private void BuildRules()
        {

        }
    }
}