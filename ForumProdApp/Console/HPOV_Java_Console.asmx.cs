using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;


namespace HPOV_Java_Console_APP_Project
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : WebService
    {

        [WebMethod]
        public string Get_All_Nodes()
        {
            ForumProdApp.hjyhj.WebService1SoapClient cc = new ForumProdApp.hjyhj.WebService1SoapClient();
            return cc.Get_All_Nodes();
        }

        [WebMethod]
        public string GetMessages_ByNodes(string Selected_NodeNames, char Delimiter)
        {
            string json = "";
            ForumProdApp.hjyhj.WebService1SoapClient cc = new ForumProdApp.hjyhj.WebService1SoapClient();
            json = cc.GetMessages_ByNodes(Selected_NodeNames, Delimiter);
            return json;
        }


        [WebMethod]
        public string GetMessages_ByParamSeverity(string Selected_NodeNames, string msgSeverity, char Delimiter)
        {
            string json = "";
             ForumProdApp.hjyhj.WebService1SoapClient cc = new ForumProdApp.hjyhj.WebService1SoapClient();
            json = cc.GetMessages_ByParamSeverity(Selected_NodeNames, msgSeverity, Delimiter);
            return json;
        }

        [WebMethod]
        public string getName()
        {
            string cli = "";
            bool flagCli;
            try
            {
                cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                flagCli = true;
            }
            catch
            {
                flagCli = false;
            }

           

            string username = "";
            string sqlquery = "";

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlquery = "select distinct USER_NAME from [JC_AllowedUsers] where CLI = '" + cli + "'";
                }
                else
                {
                    
                    //System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;

                    //cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToLower();
                    cli = User.Identity.Name.ToUpper().Replace("CELLCOM_NT\\", "");
                    sqlquery = "select distinct USER_NAME from [JC_AllowedUsers] where USER_NAME = '" + cli + "'";

                }

                DataTable t = GetDataTable(sqlquery, "OVOMNG");
                if (t.Rows.Count == 0)
                {
                    username = "NotAllowed" + cli;
                }
                else
                {
                    username = t.Rows[0]["USER_NAME"].ToString();
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
        public string Get_UserName()
        {
            //string json = "";
            int x = 0;
            // ForumProdApp.hjyhj.WebService1SoapClient cc = new ForumProdApp.hjyhj.WebService1SoapClient();
            //json = cc.Get_UserName();
            //return json;
            string cli = "";
            bool flagCli;
            string json = "";
            try
            {
                cli = HttpContext.Current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"];
                //cli = "0528907275";
                //cli = null;

                flagCli = true;
            }
            catch (Exception)
            {
                flagCli = false;
            }


            string sqlQuery = "";

            if (flagCli)
            {
                if (cli != null)
                {
                    cli = cli.Replace("972", "0");
                    sqlQuery = "select distinct USER_NAME from [JC_AllowedUsers] where CLI = '" + cli + "'";
                }

                ///Using computer System_NT
                else
                {
                    System.Security.Principal.WindowsPrincipal p = System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal;
                    cli = p.Identity.Name.Replace("CELLCOM_NT\\", "").ToUpper();
                    sqlQuery = "select distinct USER_NAME from [JC_AllowedUsers] where USER_NAME = '" + cli + "'";
                }


                string username = "";
                try
                {
                    ///Run SQL SELECT
                    SqlConnection myConnection = new SqlConnection("Data Source=BLTSD;Initial Catalog=OVOMNG;Persist Security Info=True;User ID=td;Password=tdtdtd");
                    SqlCommand myCommand = new SqlCommand(sqlQuery, myConnection);
                    myConnection.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            username = myReader.GetString(myReader.GetOrdinal("USER_NAME"));
                        }
                    }
                    else
                    {
                        username = "NotAllowed " + cli;
                    }
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    username = "ErrorFromDB: " + ex.Message;
                }

                json = JsonConvert.SerializeObject(username, Formatting.Indented);
            }
            else
            {
                json = JsonConvert.SerializeObject("ErrorFromServer", Formatting.Indented);
            }
            return json;
        }

        public DataTable GetDataTable(string sqlQuery, string dbname)
        {
            DataTable myTable = new DataTable();
            try
            {
                SqlConnection myConnection = new SqlConnection("Data Source=BLTSD;Initial Catalog=" + dbname + ";Persist Security Info=True;User ID=td;Password=tdtdtd");
                SqlCommand myCommand = new SqlCommand(sqlQuery, myConnection);
                myConnection.Open();
                SqlDataReader myReader = myCommand.ExecuteReader();
                myTable.Load(myReader);
                myConnection.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return myTable;
        }
       

    }
}
