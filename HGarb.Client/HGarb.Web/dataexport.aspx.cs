using HGarb.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HGarb.Web
{
    public partial class dataexport : System.Web.UI.Page
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

        protected void btnExeRules_Click(object sender, EventArgs e)
        {
            pnlResult.Visible = true;
        }
    }
}