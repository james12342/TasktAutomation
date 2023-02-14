using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ConsoleTest
{
    class Program
    {
        public static SqlConnection dbConn= new SqlConnection("server=DESKTOP-HP16UDT\\SQLEXPRESS;" +
                                    "User Id=workad0098;Pwd=kentxy_01;" +
                                    "database=taskt;MultipleActiveResultSets=True");
        static void Main(string[] args)
        {
            string cmdsql = "SELECT [WorkerID],[MachineName],[UserName],[LastCheckIn],[Status] FROM [taskt].[dbo].[Workers] ";
            dbConn.Open();
            SqlCommand cmd = new SqlCommand(cmdsql, dbConn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string WorkerID = reader.GetValue(0).ToString();
                Console.WriteLine("WorkerID:{0}",WorkerID);
            }
            dbConn.Close();
            Console.ReadLine();
        }
    }
}
