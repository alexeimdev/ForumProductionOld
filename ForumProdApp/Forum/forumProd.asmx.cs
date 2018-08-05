using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;


namespace ForumProdApp.WebServer
{
    /// <summary>
    /// Summary description for forumProd
    /// </summary>
    [WebService(Namespace = "http://tempuri.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class forumProd : System.Web.Services.WebService
    {
        private static string cliConf = ConfigurationManager.AppSettings["cli"];
       // private string clifromclient = "";

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetForumForMe()
        {
            string cli = "";
            bool flagCli;
            string sqlquery;

            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }

                DBStuff c = new DBStuff();

                DataTable t = new DataTable();
                DataTable roles = c.GetDataTable(sqlquery, "default_VersionStatus");
                int amntofJobs = roles.Rows.Count;
                DataTable merged = new DataTable();
                for (int MRoles = 0; MRoles < amntofJobs; MRoles++)
                {

                    string username = roles.Rows[MRoles]["username"].ToString();
                    string role = roles.Rows[MRoles]["role"].ToString();

                    if (role == "deptmngr")
                    {
                        //sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                        //          "from BUG Where " +
                        //          "BG_USER_04 = '" + username + "' " +
                        //          "and BG_STATUS <> 'אושר לפרודקשן' " +
                        //          "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_22 is null)";

                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                                    "from BUG Where " +
                                    "BG_USER_04 = '" + username + "' " +
                                    "and BG_STATUS <> 'אושר לפרודקשן' ";
                    }
                    else if (role == "tchum")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                               "from BUG Where " +
                               "BG_USER_09 = '" + username + "' " +
                               "and BG_STATUS <> 'אושר לפרודקשן' " +
                               "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_21 is null)";
                    }
                    else if (role == "tl")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                               "from BUG Where " +
                               "BG_USER_42 = '" + username + "' " +
                               "and BG_STATUS <> 'אושר לפרודקשן' " +
                               "and BG_DETECTION_DATE > getdate() - 1 and ( BG_USER_43 is null)";
                    }
                    else if (role == "tlQA")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                               "from BUG Where " +
                               "BG_USER_02 = '" + username + "' " +
                               "and BG_STATUS <> 'אושר לפרודקשן' " +
                               "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_23 is null )"; //and ((BG_USER_23 <> 'כן' and BG_USER_23 <> 'טנטטיבי') or BG_USER_23 is null )";
                    }
                    else if (role == "tiful")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                               "from BUG Where " +
                               "BG_USER_11 = '" + username + "' " +
                               "and BG_STATUS <> 'אושר לפרודקשן' " +
                               "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_27 is null )"; //and ((BG_USER_23 <> 'כן' and BG_USER_23 <> 'טנטטיבי') or BG_USER_23 is null )";
                    }
                    t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {

                        DateTime d = Convert.ToDateTime(t.Rows[i]["BG_DETECTION_DATE"]);
                        t.Rows[i]["BG_DETECTION_DATE"] = d.ToString("dd-MM-yyyy");
                    }

                    t = DoesExist(merged, t);
                    merged.Merge(t);
                }

                
                string json = JsonConvert.SerializeObject(merged, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

       [WebMethod]
        public string GetDoneForumForMe()
        {
            string cli = "";
            bool flagCli;
            string sqlquery;


            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }

                DBStuff c = new DBStuff();

                DataTable t = new DataTable();
                DataTable roles = c.GetDataTable(sqlquery, "default_VersionStatus");
                int amntofJobs = roles.Rows.Count;
                DataTable merged = new DataTable();
                for (int MRoles = 0; MRoles < amntofJobs; MRoles++)
                {

                    string username = roles.Rows[MRoles]["username"].ToString();
                    string role = roles.Rows[MRoles]["role"].ToString();

                    if (role == "deptmngr")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                 "from BUG Where " +
                                "BG_USER_04 = '" + username + "' " +
                                 "and BG_STATUS <> 'אושר לפרודקשן' " +
                                 "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_22 = 'כן' or BG_USER_22 = 'לא')";
                    }
                    else if (role == "tchum")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                 "from BUG Where " +
                                "BG_USER_09 = '" + username + "' " +
                                 "and BG_STATUS <> 'אושר לפרודקשן' " +
                                 "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_21 = 'כן' or BG_USER_21 = 'לא')";
                    }
                    else if (role == "tl")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                "from BUG Where " +
                               "BG_USER_42 = '" + username + "' " +
                                "and BG_STATUS <> 'אושר לפרודקשן' " +
                                "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_43 = 'כן' or BG_USER_43 = 'לא')";
                    }
                    else if (role == "tlQA")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                               "from BUG Where " +
                               "BG_USER_02 = '" + username + "' " +
                               "and BG_STATUS <> 'אושר לפרודקשן' " +
                               "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_23 = 'כן' or BG_USER_23 = 'לא' or BG_USER_23 = 'טנטטיבי' or BG_USER_23 = 'נבדק על ידי הפיתוח' )";
                    }
                    else if (role == "tiful")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24,BG_DESCRIPTION " +
                               "from BUG Where " +
                               "BG_USER_11 = '" + username + "' " +
                               "and BG_STATUS <> 'אושר לפרודקשן' " +
                               "and BG_DETECTION_DATE > getdate() - 1 and (BG_USER_27 = 'כן' or BG_USER_27 = 'לא')";
                    }
                    t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        DateTime d = Convert.ToDateTime(t.Rows[i]["BG_USER_74"]);
                        t.Rows[i]["BG_USER_74"] = d.ToString("dd-MM-yyyy");
                        d = Convert.ToDateTime(t.Rows[i]["BG_DETECTION_DATE"]);
                        t.Rows[i]["BG_DETECTION_DATE"] = d.ToString("dd-MM-yyyy");
                    }

                    t = DoesExist(merged, t);
                    merged.Merge(t); 
                }

              

                string json = JsonConvert.SerializeObject(merged, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

       [WebMethod]
       public string ApproveRequest(string requestID,string statuschange)
        {
            //
            string cli = "";
            bool flagCli;
            string sqlquery;

            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

            if (flagCli)
            {

                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }
         
                DBStuff c = new DBStuff();
                string k = "";
                string [] sucs; // declare numbers as an int array of any size
                sucs = new string[4];  // numbers is a 10-element array

                DataTable t = new DataTable();
                DataTable roles = c.GetDataTable(sqlquery, "default_VersionStatus");
                int amntofJobs = roles.Rows.Count;
                DataTable merged = new DataTable();

                for (int MRoles = 0; MRoles < amntofJobs; MRoles++)
                {
                    string username = roles.Rows[MRoles]["username"].ToString();
                    string role = roles.Rows[MRoles]["role"].ToString();

                    string sade = "";
                    string sadeMain = "";

                    if (statuschange == "טנטטיבי" || statuschange == "נבדק על ידי הפיתוח")
                    {
                        if (role == "tlQA")
                        {
                            sade = "BG_USER_23";
                            sadeMain = "BG_USER_02";

                            sqlquery = "UPDATE [default_alm_forumprod_db].[dbo].[BUG] SET [" + sade + "] = '" + statuschange + "' WHERE BG_BUG_ID = '" + requestID + "' and " + sadeMain + " = '" + username + "'";
                            k += c.RunQuery(sqlquery, "default_alm_forumprod_db") + ":";
                            amntofJobs = 1;
                            break;
                        }
                    }
                    else
                    {
                        if (role == "deptmngr")
                        {
                            sade = "BG_USER_22";
                            sadeMain = "BG_USER_04";
                        }
                        else if (role == "tchum")
                        {
                            sade = "BG_USER_21";
                            sadeMain = "BG_USER_09";
                        }
                        else if (role == "tl")
                        {
                            sade = "BG_USER_43";
                            sadeMain = "BG_USER_42";
                        }
                        else if (role == "tlQA")
                        {        
                                sade = "BG_USER_23";
                                sadeMain = "BG_USER_02";
                        }
                         else if (role == "tiful")
                        {
                            sade = "BG_USER_27";
                            sadeMain = "BG_USER_11";
                        }

                        sqlquery = "UPDATE [default_alm_forumprod_db].[dbo].[BUG] SET [" + sade + "] = '" + statuschange + "' WHERE BG_BUG_ID = '" + requestID + "' and " + sadeMain + " = '" + username + "'";
                        k += c.RunQuery(sqlquery, "default_alm_forumprod_db") + ":";   
                    }         
                }

                if (amntofJobs > 1)
                {
                    sucs = k.Split(':');
                    for (int jj = 0; jj < sucs.Length; jj++)
                    {
                        if (sucs[jj] == "Fail")
                        {
                            k = "fail";
                            break;
                        }
                    }

                    if (k != "fail")
                        k = "Success";
                }
                else
                {
                    k = k.Replace(":", "");
                }

                string json = JsonConvert.SerializeObject(k, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

       [WebMethod]
       public string TentException(string requestID, string statuschange,string currTentStatus)
       {
           //
           string cli = "";
           bool flagCli;
           string sqlquery;

           try
           {
               //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
               cli = cliConf;
               flagCli = true;
           }
           catch
           {
               flagCli = false;
           }

           if (flagCli)
           {

               if (cli != null)
               {
                   cli = cli.Replace("972", "0");
                   sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
               }
               else
               {
                   System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                   cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                   sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
               }

               DBStuff c = new DBStuff();
               string k = "";
               string[] sucs; // declare numbers as an int array of any size
               sucs = new string[4];  // numbers is a 10-element array

               DataTable t = new DataTable();
               DataTable roles = c.GetDataTable(sqlquery, "default_VersionStatus");
               int amntofJobs = roles.Rows.Count;
               DataTable merged = new DataTable();

               for (int MRoles = 0; MRoles < amntofJobs; MRoles++)
               {
                   string username = roles.Rows[MRoles]["username"].ToString();
                   string role = roles.Rows[MRoles]["role"].ToString();

                   string sade = "";
                   string sadeMain = "";

                   
                   if (role == "tlQA")
                   {
                           k += "Success:";      
                   }
                   else
                   {
                       if (role == "deptmngr")
                       {
                           sade = "BG_USER_22";
                           sadeMain = "BG_USER_04";
                       }
                       else if (role == "tchum")
                       {
                           sade = "BG_USER_21";
                           sadeMain = "BG_USER_09";
                       }
                       else if (role == "tl")
                       {
                           sade = "BG_USER_43";
                           sadeMain = "BG_USER_42";
                       }
                   
                        else if (role == "tiful")
                       {
                           sade = "BG_USER_27";
                           sadeMain = "BG_USER_11";
                       }

                       sqlquery = "UPDATE [default_alm_forumprod_db].[dbo].[BUG] SET [" + sade + "] = '" + statuschange + "' WHERE BG_BUG_ID = '" + requestID + "' and " + sadeMain + " = '" + username + "'";
                       k += c.RunQuery(sqlquery, "default_alm_forumprod_db") + ":";
                   }
               }

               if (amntofJobs > 1)
               {
                   sucs = k.Split(':');
                   for (int jj = 0; jj < sucs.Length; jj++)
                   {
                       if (sucs[jj] == "Fail")
                       {
                           k = "fail";
                           break;
                       }
                   }

                   if (k != "fail")
                       k = "Success";
               }
               else
               {
                   k = k.Replace(":", "");
               }

               string json = JsonConvert.SerializeObject(k, Formatting.Indented);
               return json;
           }
           else
           {
               string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
               return json;
           }
       }

       [WebMethod]
       public string testgetName()
       {
           string username = "";
           string cli = "";
           bool odflag;
           string nu = "";
           bool flagCli;
           try
           {
               //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
               cli = cliConf;
               flagCli = true;
           }
           catch
           {
               flagCli = false;
               

           }

           DBStuff c = new DBStuff();

           
           string sqlquery = "";

           if (flagCli)
           {
               if (cli != null)
               {
                   odflag = true;
                   cli = cli.Replace("972", "0");
                   sqlquery = "select distinct username from [ForumRoles] where cli = '" + cli + "'";
               }
               else
               {
                   odflag = false; ;
                   System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                   cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                   sqlquery = "select distinct username from [ForumRoles] where username = '" + cli + "'";
               }

               
               DataTable t = c.GetDataTable(sqlquery, "default_VersionStatus");
               if (t.Rows.Count == 0)
               {
                   username = "notAllowed";
               }
               else
               {
                   username = t.Rows[0]["username"].ToString() + "cli: " + cli;
               }

               string json = JsonConvert.SerializeObject(username, Formatting.Indented);
               return json;
           }
           else
           {
               username = "ErrorFromServer: flagcli = " + flagCli + " cli  = " + cli; 
               //string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
               string json = JsonConvert.SerializeObject(username, Formatting.Indented);
               return json;
           }
       }

       [WebMethod]
        public string getName()
        {
           string cli = "";
           bool flagCli;
            try
            {
               //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
               cli = cliConf;
               flagCli = true;
            }
            catch
            {
                flagCli = false;
            }
            
            //cli = cliConf;
            DBStuff c = new DBStuff();
           
            string username = "";            
            string sqlquery = "";
            string howgetname = "";
            if (flagCli)
            {
                c.RunQuery("INSERT INTO [dbo].[LoggedUsersForum]([loggedUser]) VALUES ('cli - " + cli + "')", "default_VersionStatus");
                if (cli != null)
                {
                    
                    cli = cli.Replace("972", "0");

                    sqlquery = "select distinct username from [ForumRoles] where cli = '" + cli + "'";
                    howgetname = "3G";

                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    string aaa = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();                    
                    sqlquery = "select distinct username from [ForumRoles] where username = '" + cli + "'";
                    
                    c.RunQuery("INSERT INTO [dbo].[LoggedUsersForum]([loggedUser]) VALUES ('" + p.Identity.Name + " :: " + aaa + "')", "default_VersionStatus");
                    howgetname = p.Identity.Name;
                }

                DataTable t = c.GetDataTable(sqlquery, "default_VersionStatus");
                if (t.Rows.Count == 0)
                {
                    username = "NA" + howgetname + " " + cli;
                }
                else
                {
                    username = t.Rows[0]["username"].ToString();
                }

                if (username == "")
                {
                    username = "noname";
                }
                string json = JsonConvert.SerializeObject(username, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }
       
        [WebMethod]
        public string RefreshData(string requestID)
        {
            string cli = "";
            bool flagCli;
            string sqlquery = "";
            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;

            }

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }
  
                


                DBStuff c = new DBStuff();

                DataTable t = new DataTable();
                DataTable roles = c.GetDataTable(sqlquery, "default_VersionStatus");
                int amntofJobs = roles.Rows.Count;
                DataTable merged = new DataTable();

                for (int MRoles = 0; MRoles < amntofJobs; MRoles++)
                {

                    string username = roles.Rows[MRoles]["username"].ToString();
                    string role = roles.Rows[MRoles]["role"].ToString();

                    if (role == "deptmngr")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                 "from BUG Where " +
                                "BG_USER_04 = '" + username + "' " +
                                 "and BG_BUG_ID = '" + requestID + "' " +
                                 "and BG_STATUS <> 'אושר לפרודקשן' " +
                                 "and BG_DETECTION_DATE > getdate() - 1 "; //and (BG_USER_22 = 'כן' or BG_USER_22 = 'לא')";
                    }
                    else if (role == "tchum")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                 "from BUG Where " +
                                "BG_USER_09 = '" + username + "' " +
                                "and BG_BUG_ID = '" + requestID + "' " +
                                 "and BG_STATUS <> 'אושר לפרודקשן' " +
                                 "and BG_DETECTION_DATE > getdate() - 1 "; //and (BG_USER_21 = 'כן' or BG_USER_21 = 'לא')";
                    }
                    else if (role == "tl")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                "from BUG Where " +
                               "BG_USER_42 = '" + username + "' " +
                               "and BG_BUG_ID = '" + requestID + "' " +
                                "and BG_STATUS <> 'אושר לפרודקשן' " +
                                "and BG_DETECTION_DATE > getdate() - 1 "; //and (BG_USER_43 = 'כן' or BG_USER_43 = 'לא')";
                    }
                    else if (role == "tlQA")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                "from BUG Where " +
                               "BG_USER_02 = '" + username + "' " +
                               "and BG_BUG_ID = '" + requestID + "' " +
                                "and BG_STATUS <> 'אושר לפרודקשן' " +
                                "and BG_DETECTION_DATE > getdate() - 1 "; //and (BG_USER_23 = 'כן' or BG_USER_23 = 'לא' or BG_USER_23 = 'טנטטיבי')";
                    }
                    else if (role == "tiful")
                    {
                        sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                                "from BUG Where " +
                               "BG_USER_11 = '" + username + "' " +
                               "and BG_BUG_ID = '" + requestID + "' " +
                                "and BG_STATUS <> 'אושר לפרודקשן' " +
                                "and BG_DETECTION_DATE > getdate() - 1 "; //and (BG_USER_23 = 'כן' or BG_USER_23 = 'לא' or BG_USER_23 = 'טנטטיבי')";
                    }

                    t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");

                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        DateTime d = Convert.ToDateTime(t.Rows[i]["BG_USER_74"]);
                        t.Rows[i]["BG_USER_74"] = d.ToString("dd-MM-yyyy");
                        d = Convert.ToDateTime(t.Rows[i]["BG_DETECTION_DATE"]);
                        t.Rows[i]["BG_DETECTION_DATE"] = d.ToString("dd-MM-yyyy");
                    }

                    t = DoesExist(merged, t);
                    merged.Merge(t);
                }
                string json = JsonConvert.SerializeObject(merged, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

        [WebMethod]
        public string resetDesc(string requestID)
        {
            string cli = "";
            bool flagCli;
            string sqlquery;

            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

            if (flagCli)
            {

                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }

                DBStuff c = new DBStuff();
                string k = "";
                string[] sucs; // declare numbers as an int array of any size
                sucs = new string[4];  // numbers is a 10-element array

                DataTable t = new DataTable();
                DataTable roles = c.GetDataTable(sqlquery, "default_VersionStatus");
                int amntofJobs = roles.Rows.Count;
                DataTable merged = new DataTable();

                for (int MRoles = 0; MRoles < amntofJobs; MRoles++)
                {
                    string username = roles.Rows[MRoles]["username"].ToString();
                    string role = roles.Rows[MRoles]["role"].ToString();

                    string sade = "";
                    string sadeMain = "";
                    if (role == "deptmngr")
                    {
                        sade = "BG_USER_22";
                        sadeMain = "BG_USER_04";
                    }
                    else if (role == "tchum")
                    {
                        sade = "BG_USER_21";
                        sadeMain = "BG_USER_09";
                    }
                    else if (role == "tl")
                    {
                        sade = "BG_USER_43";
                        sadeMain = "BG_USER_42";
                    }
                    else if (role == "tlQA")
                    {
                        sade = "BG_USER_23";
                        sadeMain = "BG_USER_02";
                    }
                     else if (role == "tiful")
                     {
                         sade = "BG_USER_27";
                         sadeMain = "BG_USER_11";
                     }


                    //sqlquery = "UPDATE [default_alm_forumprod_db].[dbo].[BUG] SET [" + sade + "] = null WHERE BG_BUG_ID = '" + requestID + "'";
                    sqlquery = "UPDATE [default_alm_forumprod_db].[dbo].[BUG] SET [" + sade + "] = null WHERE BG_BUG_ID = '" + requestID + "' and " + sadeMain + " = '" + username + "'";
                    k += c.RunQuery(sqlquery, "default_alm_forumprod_db");

                }
                if (amntofJobs > 1)
                {
                    sucs = k.Split(':');
                    for (int jj = 0; jj < sucs.Length; jj++)
                    {
                        if (sucs[jj] == "Fail")
                        {
                            k = "fail";
                            break;
                        }
                    }

                    if (k != "fail")
                        k = "Success";
                }
                else
                {
                    k = k.Replace(":", "");
                }

                string json = JsonConvert.SerializeObject(k, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

        [WebMethod]
        public string getRole(string requestID)
        {
            string cli = "";
            bool flagCli;
            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;

            }

            DBStuff c = new DBStuff();
            string json = "";
            string role = "";
            string sqlquery = "";

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct role,username from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct role,username from [ForumRoles] where username = '" + cli + "'";
                }

                DataTable t = c.GetDataTable(sqlquery, "default_VersionStatus");
                
                int amntofRoles = t.Rows.Count;
                if (t.Rows.Count == 0)
                {
                    role = "notAllowed:0";
                }
                else
                {
                    //tlQA 02
                    //deptmngr 04
                    //tchum 09
                    //tl 42
                    //tiful 11
                    string username = t.Rows[0]["username"].ToString();
                    sqlquery = "select BG_USER_02 as 'tlQA' from BUG Where BG_BUG_ID = '" + requestID.Replace("_Done","") + "'";
                    
                    t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");
                    if (t.Rows[0]["tlQA"].ToString() == username)
                        role = "tlQA:" + amntofRoles;
                    else
                        role = "none:0";
                }

                json = JsonConvert.SerializeObject(role, Formatting.Indented);
                return json;
            }
            else
            {
               json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
               return json;
            }
        }

         [WebMethod]
        public string getAllRole(string requestID)
        {
            string cli = "";
            bool flagCli;
            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;

            }

            DBStuff c = new DBStuff();
            string json = "";
            string role = "";
            string sqlquery = "";

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct role,username from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct role,username from [ForumRoles] where username = '" + cli + "'";
                }

                DataTable t = c.GetDataTable(sqlquery, "default_VersionStatus");
                
                int amntofRoles = t.Rows.Count;
                if (t.Rows.Count == 0)
                {
                    role = "notAllowed:0";
                }
                else
                {
                    string username = t.Rows[0]["username"].ToString();
                    int tmpamntofroles = 0;
                    sqlquery = "select BG_USER_04,BG_USER_23,BG_USER_02,BG_USER_09,BG_USER_42,BG_USER_11,BG_USER_21,BG_USER_27 from BUG Where BG_BUG_ID = '" + requestID.Replace("_Done", "") + "'";
                    
                    t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");
                    if (t.Rows[0]["BG_USER_02"].ToString() == username)
                        if(t.Rows[0]["BG_USER_23"].ToString() == "")
                            tmpamntofroles++;
                    if (t.Rows[0]["BG_USER_04"].ToString() == username)

                        tmpamntofroles++;
                    if (t.Rows[0]["BG_USER_09"].ToString() == username)
                        if (t.Rows[0]["BG_USER_21"].ToString() == "")
                            tmpamntofroles++;
                    if (t.Rows[0]["BG_USER_42"].ToString() == username)
                        if (t.Rows[0]["BG_USER_43"].ToString() == "")
                            tmpamntofroles++;
                    if (t.Rows[0]["BG_USER_11"].ToString() == username)
                        if (t.Rows[0]["BG_USER_27"].ToString() == "")
                            tmpamntofroles++;

                    if (tmpamntofroles == 0)
                        role = "none:0";
                    else
                        role = "allroles:" + tmpamntofroles.ToString();
                }

                json = JsonConvert.SerializeObject(role, Formatting.Indented);
                return json;
            }
            else
            {
               json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
               return json;
            }
        }

        [WebMethod]
        public string GetApprovedRequestsToday(string all)
        {
            string cli = "";
            bool flagCli;
            string sqlquery;

            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

            if (flagCli)
            {

                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }

                DBStuff c = new DBStuff();


                DataTable t = c.GetDataTable(sqlquery, "default_VersionStatus");
                string username = t.Rows[0]["username"].ToString();
                string role = t.Rows[0]["role"].ToString();

                string sade = "";
                if (role == "deptmngr")
                {
                    sade = "BG_USER_04";
                }
                else if (role == "tchum")
                {
                    sade = "BG_USER_09";
                }
                else if (role == "tl")
                {
                    sade = "BG_USER_42";
                }
                else if (role == "tlQA")
                {
                    sade = "BG_USER_02";
                }
                else if (role == "tiful")
                {
                    sade = "BG_USER_11";
                }

                if (all == "false")
                {
                    sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                               "from BUG WHERE BG_DETECTION_DATE = CONVERT(VARCHAR(10),GETDATE(),101) AND " +
                               "BG_STATUS = 'אושר לפרודקשן' and " + sade + " = '" + username + "'" +
                               " order by BG_PROJECT";
                }
                else
                {
                    sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                              "from BUG WHERE BG_DETECTION_DATE = CONVERT(VARCHAR(10),GETDATE(),101) AND " +
                              "BG_STATUS = 'אושר לפרודקשן' " + // and " + sade + " = '" + username + "'" +
                              " order by BG_PROJECT";
                }

                t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");
              

                string json = JsonConvert.SerializeObject(t, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

        [WebMethod]
        public string GetRequestsToday()
        {
            string cli = "";
            bool flagCli;
            string sqlquery;

            try
            {
                //cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                cli = cliConf;
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

            if (flagCli)
            {

                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct username,role from [ForumRoles] where cli = '" + cli + "'";
                }
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    sqlquery = "select distinct username,role from [ForumRoles] where username = '" + cli + "'";
                }

                DBStuff c = new DBStuff();


                DataTable t = c.GetDataTable(sqlquery, "default_VersionStatus");
                string username = t.Rows[0]["username"].ToString();
                string role = t.Rows[0]["role"].ToString();

                string sade = "";
                if (role == "deptmngr")
                {
                    sade = "BG_USER_04";
                }
                else if (role == "tchum")
                {
                    sade = "BG_USER_09";
                }
                else if (role == "tl")
                {
                    sade = "BG_USER_42";
                }
                else if (role == "tlQA")
                {
                    sade = "BG_USER_02";
                }
                else if (role == "tiful")
                {
                    sade = "BG_USER_11";
                }

                
                
                    sqlquery = "select BG_BUG_ID,BG_SUMMARY,BG_RESPONSIBLE,BG_PROJECT ,BG_DETECTED_BY ,BG_DETECTION_DATE ,BG_USER_42,BG_USER_43,BG_USER_23,BG_USER_75,BG_USER_26,BG_USER_27,BG_USER_22 , BG_USER_21 ,BG_USER_78 ,BG_USER_74,BG_USER_01,BG_USER_10,BG_USER_11,BG_USER_02 ,BG_USER_76 ,BG_USER_04 ,BG_USER_09,BG_USER_03,BG_USER_24 " +
                              "from BUG WHERE BG_DETECTION_DATE = CONVERT(VARCHAR(10),GETDATE(),101) AND " +
                              "BG_STATUS <> 'אושר לפרודקשן' " + // and " + sade + " = '" + username + "'" +
                              " order by BG_PROJECT";
                

                t = c.GetDataTable(sqlquery, "default_alm_forumprod_db");


                string json = JsonConvert.SerializeObject(t, Formatting.Indented);
                return json;
            }
            else
            {
                string json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
                return json;
            }
        }

        private DataTable DoesExist(DataTable merged,DataTable newtb)
       {
          
           if (merged.Rows.Count > 0)
           {
               for (int k = 0; k < newtb.Rows.Count; k++)
               {
                   string newBugID = newtb.Rows[k]["BG_BUG_ID"].ToString();
                   for (int l = 0; l < merged.Rows.Count; l++)
                   {
                       string currBugId = merged.Rows[l]["BG_BUG_ID"].ToString();
                       if (currBugId == newBugID)
                       {
                           newtb.Rows[k].Delete();
                           newtb.AcceptChanges();
                           break;
                       }
                   }
               }
           }

           return newtb;
       }

        private int isAlreadyTentativeOrDevChecked(string requestID,string username)
        {
            string sqlQuery = "select * from BUG where BG_BUG_ID = '" + requestID + "' and (BG_USER_23 = 'טנטטיבי' or BG_USER_23 = 'נבדק על ידי הפיתוח')  and BG_USER_02 = '" + username + "'";
            DBStuff c = new DBStuff();
            DataTable t = c.GetDataTable(sqlQuery, "default_alm_forumprod_db");

            return t.Rows.Count;
        }

    }
}


