using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using HGarb.Infrastructure;

namespace HGarb.Web
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = Helper.GetAppSetting("ConnectionString");
            SampleVBApp.EValuate.EvalRunTime evalRun = new SampleVBApp.EValuate.EvalRunTime("SLM Student Loan", "SLM Student Loan 2009-3", connectionString, "102016");
            
        }
    }
}