using MySql.Data.MySqlClient;
using ReportGeneration_Kazakov.Classes.Common;
using ReportGeneration_Kazakov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration_Kazakov.Classes
{
    public class WorkContext : Work
    {
        public WorkContext(int d, int dDiscipline, int dType, DateTime date, string name, int semester) : base(d, dDiscipline, dType, date, name, semester) { }

        public static List<WorkContext> AllWorks()
        {
            List<WorkContext> allWorks = new List<WorkContext>();
            MySqlConnection conn = Connection.OpenConnection();
            MySqlDataReader BDWorks = Connection.Query("SELECT * FROM work ORDER BY Date", conn);
            while (BDWorks.Read())
            {
                allWorks.Add(new WorkContext(
                    BDWorks.GetInt32(0),
                    BDWorks.GetInt32(1),
                    BDWorks.GetInt32(2),
                    BDWorks.GetDateTime(3),
                    BDWorks.GetString(4),
                    BDWorks.GetInt32(5)));
            }

            Connection.CloseConnection(conn);
            return allWorks;
        }
    }
}
