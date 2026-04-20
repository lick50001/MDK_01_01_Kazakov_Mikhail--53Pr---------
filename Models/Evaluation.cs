using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration_Kazakov.Models
{
    public class Evaluation
    {
        public int Id { get; set; }
        public int IdWork{ get; set; }
        public int IdStudent { get; set; }
        public string Value{ get; set; }
        public string Lateness{ get; set; }

        public Evaluation(int id, int idWork, int IdStudent, string value, string Lateness)
        {
            this.Id = id;
            this.IdWork = idWork;
            this.IdStudent = IdStudent;
            this.Value = value;
            this.Lateness = Lateness;
        }
    }
}
