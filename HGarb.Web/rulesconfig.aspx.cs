using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hydra.DBRS;
using Hydra.DBRS.Business;
using System.Web.UI.HtmlControls;
using Hydra.DBRS.Models;

namespace HGarb.Web
{
    public partial class rulesconfig : System.Web.UI.Page
    {
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
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}