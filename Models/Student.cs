using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGeneration_Kazakov.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int IdGroup { get; set; }
        public bool Expelled { get; set; }
        public DateTime DateExpelled { get; set; }
    
        public Student(int Id, string Firstnamev, string Lastname, int IdGroup, bool Expelled, DateTime DateExpelled)
        {
            this.Id = Id;
            this.Firstname = Firstnamev;
            this.Lastname = Lastname;
            this.IdGroup = IdGroup;
            this.Expelled = Expelled;
            this.DateExpelled = DateExpelled;
        }
    }
}
