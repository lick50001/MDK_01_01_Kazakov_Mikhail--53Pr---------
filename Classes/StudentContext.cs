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
    public class StudentContext : Student
    {
        public StudentContext(int Id, string Firstnamev, string Lastname, int IdGroup, bool Expelled, DateTime DateExpelled) : base(Id, Firstnamev, Lastname, IdGroup, Expelled, DateExpelled) { }

        public static List<StudentContext> AllStud()
        {
            List<StudentContext> allStud = new List<StudentContext>();
            MySqlConnection conn = Connection.OpenConnection();
            MySqlDataReader BDWStud = Connection.Query("SELECT * FROM student ORDER BY LastName;", conn);
            while (BDWStud.Read())
            {
                allStud.Add(new StudentContext(
                    BDWStud.GetInt32(0),
                    BDWStud.GetString(1),
                    BDWStud.GetString(2),
                    BDWStud.GetInt32(3),
                    BDWStud.GetBoolean(4),
                    BDWStud.IsDBNull(5) ? DateTime.Now : BDWStud.GetDateTime(5)));
            }

            Connection.CloseConnection(conn);
            return allStud;
        }
    }
}
