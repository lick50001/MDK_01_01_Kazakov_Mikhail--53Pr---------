using MySql.Data.MySqlClient;
using ReportGeneration_Kazakov.Models;
using ReportGeneration_Kazakov.Classes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration_Kazakov.Classes
{
    public class DisciplineContext : Discipline
    {
        public DisciplineContext(int Id, string Name, int IdGroup) : base(Id, Name, IdGroup) { }

        public static List<DisciplineContext> AllDisc()
        {
            List<DisciplineContext> allDisc = new List<DisciplineContext>();
            MySqlConnection connection = Connection.OpenConnection();
            MySqlDataReader BDDiscip = Connection.Query("SELECT * FROM discilpine ORDER BY Name;", connection);
            while (BDDiscip.Read())
            {
                allDisc.Add(new DisciplineContext(
                    BDDiscip.GetInt32(0),
                    BDDiscip.GetString(1),
                    BDDiscip.GetInt32(2)));
            }
            Connection.CloseConnection(connection);
            return allDisc;
        }
    }
}
