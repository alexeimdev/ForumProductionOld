using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;


namespace ForumProdApp
{
    public class DBStuff
    {

        #region Get Data Table
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
       

        public string RunQuery(string sqlQuery, string dbname)
        {
            string stringtoRet = "";
            string rowsAffected = "";
            try
            {
                SqlConnection myConnection = new SqlConnection("Data Source=BLTSD;Initial Catalog=" + dbname + ";Persist Security Info=True;User ID=td;Password=tdtdtd");
                SqlCommand myCommand = new SqlCommand(sqlQuery, myConnection);
                myConnection.Open();
                rowsAffected = myCommand.ExecuteNonQuery().ToString();
                myConnection.Close();
                if (Convert.ToInt16(rowsAffected) > 0)
                    stringtoRet = "Success";
                else
                    stringtoRet = "Warning";
            }
            catch (Exception ex)
            {
                stringtoRet = "Fail";

                ex.ToString();
            }

            return stringtoRet;
        }

        #endregion
    }
     
}