using MySql.Data.MySqlClient;
using ReportGeneration_Kazakov.Classes.Common;
using ReportGeneration_Kazakov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = ReportGeneration_Kazakov.Models.Group;

namespace ReportGeneration_Kazakov.Classes
{
    public class GroupContext : Group
    {
        public GroupContext(int Id, string Name) : base(Id, Name) { }

        public static List<GroupContext> AllGroups()
        {
            List<GroupContext> allGroups = new List<GroupContext>();
            MySqlConnection conn = Connection.OpenConnection();
            MySqlDataReader BDWGroups = Connection.Query("SELECT * FROM group ORDER BY Name;", conn);
            while (BDWGroups.Read())
            {
                allGroups.Add(new GroupContext(
                    BDWGroups.GetInt32(0),
                    BDWGroups.GetString(1)));
            }

            Connection.CloseConnection(conn);
            return allGroups;
        }
    }
}
