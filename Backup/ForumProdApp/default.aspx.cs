using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.IO;

using System.Collections.Specialized;
using System.Configuration;

namespace ForumProdApp
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] cliarr;
            

            string cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];

            //cliarr = HttpContext.Current.Request.Headers.AllKeys;
            cliarr = HttpContext.Current.Request.ServerVariables.AllKeys;
            

            for (int i = 0; i < cliarr.Length; i++)
            {
                Label1.Text += cliarr[i].ToString();
                Label1.Text += HttpContext.Current.Request.ServerVariables[cliarr[i].ToString()] + "::::";
            }
        }
    }
}