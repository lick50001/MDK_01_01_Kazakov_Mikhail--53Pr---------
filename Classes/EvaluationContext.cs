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
    public class EvaluationContext : Evaluation
    {
        public EvaluationContext(int id, int idWork, int IdStudent, string value, string Lateness) : base(id, idWork, IdStudent, value, Lateness) { }

        public static List<EvaluationContext> AllEvalut()
        {
            List<EvaluationContext> allEvalut = new List<EvaluationContext>();
            MySqlConnection conn = Connection.OpenConnection();
            MySqlDataReader BDWvalut = Connection.Query("SELECT * FROM evaluation;", conn);
            while (BDWvalut.Read())
            {
                allEvalut.Add(new EvaluationContext(
                    BDWvalut.GetInt32(0),
                    BDWvalut.GetInt32(1),
                    BDWvalut.GetInt32(2),
                    BDWvalut.GetString(3),
                    BDWvalut.GetString(4)));
            }

            Connection.CloseConnection(conn);
            return allEvalut;
        }
    }
}
