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
            //string connectionString = Helper.GetAppSetting("ConnectionString");
            //RuleTester.EValuate.EvalRunTime evalRun = new RuleTester.EValuate.EvalRunTime("Ford Credit Auto Owner Trust", "Ford Credit Auto Owner Trust 2016-C", connectionString, "052017");
        }
    }
}