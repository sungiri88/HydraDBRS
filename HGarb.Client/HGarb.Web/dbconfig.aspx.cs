using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HGarb.Business;
using HGarb.Models;

namespace HGarb.Web
{
    public partial class dbconfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadCompanies();
            }
        }
        protected void btnAddCompanyHeader_Click(object sender, EventArgs e)
        {
            string CompanyName = ddlCompany.SelectedItem.ToString().Trim();
            string CompanyHeader = txt_companyHeader_1.Text.ToString().Trim();
            RulesConfig rc = new RulesConfig();
            bool status = rc.InsertCompanyHeader(CompanyName, CompanyHeader);
            if(status)
            txt_companyHeader_1.Text = string.Empty;
            this.LoadCompanyHeaders(CompanyName);
        }
        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string CompanyName = ddlCompany.SelectedValue;
            this.LoadCompanyHeaders(CompanyName);
        }
        private void LoadCompanyHeaders(string CompanyName)
        {
            RulesConfig rc = new RulesConfig();
            var lstCompanyHeaders = rc.LoadCompanyHeader(CompanyName);
            int index = 0;
            foreach (var item in lstCompanyHeaders)
            {
                ddlCompanyHeaders.Items.Add(new ListItem(item, item));
                index += 1;
            }
        }
        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            string CompanyName = txt_add_company.Text.ToString().Trim();
            RulesConfig rc = new RulesConfig();
            rc.InsertCompany(CompanyName);
            txt_add_company.Text = string.Empty;
            this.LoadCompanies();
        }
        private void LoadCompanies()
        {
            ddlCompany.Items.Clear();
            RulesConfig rc = new RulesConfig();
            var lstCompanies = rc.LoadCompanies();
            int index = 0;
            foreach (var item in lstCompanies)
            {
                ddlCompany.Items.Add(new ListItem(item, item));
                index += 1;
            }
            if (lstCompanies.Count > 0)
                this.LoadCompanyHeaders(ddlCompany.Text);
        }
    }
}