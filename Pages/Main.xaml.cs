using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReportGeneration_Kazakov.Models;
using ReportGeneration_Kazakov.Classes.Common;
using ReportGeneration_Kazakov.Classes;

namespace ReportGeneration_Kazakov.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public List<DisciplineContext> DisCont = DisciplineContext.AllDisc();
        public List<EvaluationContext> EvaCont = EvaluationContext.AllEvalut();
        public List<GroupContext> GrpCont = GroupContext.AllGroups();
        public List<StudentContext> StdCont = StudentContext.AllStud();
        public List<WorkContext> WrkCont = WorkContext.AllWorks();
        public Main()
        {
            InitializeComponent();
            CreateGroupUI();
        }

        public void CreateGroupUI()
        {
            foreach (GroupContext group in GrpCont)
                CBGroups.Items.Add(group.Name);

            CBGroups.Items.Add("Выберите");
            CBGroups.SelectedIndex = CBGroups.Items.Count - 1;
        }

        public void CreateStudents(List<StudentContext> AllStudents)
        {
            Parent.Children.Clear();
            foreach (StudentContext student in AllStudents) 
                Parent.Children.Add(new Items.Student(student, this));
        }

        private void SelectGroup(object sender, SelectionChangedEventArgs e)
        {
            if (CBGroups.SelectedIndex != CBGroups.Items.Count - 1)
            {
                var selectedGroup = GrpCont.Find(x => x.Name == CBGroups.SelectedItem.ToString());
                if (selectedGroup != null)
                {
                    CreateStudents(StdCont.FindAll(x => x.IdGroup == selectedGroup.Id));
                }
            }
        }

        private void SelectStudents(object sender, KeyEventArgs e)
        {
            List<StudentContext> SearchStudent = StdCont;

            if (CBGroups.SelectedIndex != -1 && CBGroups.SelectedIndex != CBGroups.Items.Count - 1)
            {
                var selectedGroup = GrpCont.Find(x => x.Name == CBGroups.SelectedItem.ToString());
                if (selectedGroup != null)
                {
                    SearchStudent = StdCont.FindAll(x => x.IdGroup == selectedGroup.Id);
                }
            }
            CreateStudents(SearchStudent.FindAll(x =>
                $"{x.Lastname} {x.Firstname}".ToLower().Contains(TBFIO.Text.ToLower())));
        }
    }
}
