using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration_Kazakov.Classes.Common
{
    public class Connection
    {
        public static string config = "server=localhost;uid=root;pwd=;database=journal;";

        public static MySqlConnection OpenConnection()
        {
            MySqlConnection conn = new MySqlConnection(config);
            conn.Open();

            return conn;
        }

        public static MySqlDataReader Query(string SQL, MySqlConnection conn)
        {
            return new MySqlCommand(SQL, conn).ExecuteReader();
        }

        public static void CloseConnection(MySqlConnection conn)
        {
            conn.Close();
            MySqlConnection.ClearPool(conn);
        }
    }
}
