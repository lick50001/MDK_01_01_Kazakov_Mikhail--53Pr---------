using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration_Kazakov.Models
{
    public class Work
    {
        public int Id { get; set; }
        public int IdDiscipline { get; set; }
        public int IdType { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }

        public Work(int d, int dDiscipline, int dType, DateTime date, string name, int semester)
        {
            Id = d;
            IdDiscipline = dDiscipline;
            IdType = dType;
            Date = date;
            Name = name;
            Semester = semester;
        }
    }
}
