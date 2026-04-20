using ReportGeneration_Kazakov.Classes;
using ReportGeneration_Kazakov.Pages;
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

namespace ReportGeneration_Kazakov.Items
{
    /// <summary>
    /// Логика взаимодействия для Student.xaml
    /// </summary>
    public partial class Student : UserControl
    {
        private StudentContext student;
        private Main main;

        public Student(StudentContext student, Main main)
        {
            InitializeComponent();
            this.student = student;
            this.main = main;

            // Присваиваем значения UI элементам
            // ВАЖНО: Проверьте, что в Student.xaml есть <TextBlock x:Name="TBFio" />
            if (TBFio != null)
                TBFio.Text = $"{student.Lastname} {student.Firstname}";

            CBExpelled.IsChecked = student.Expelled;

            // 1. Используем списки из экземпляра main (DisCont, WrkCont и т.д.)
            var studentDisciplines = main.DisCont
                .Where(x => x.IdGroup == student.IdGroup)
                .ToList();

            var disciplineIds = studentDisciplines.Select(d => d.Id).ToList();

            // 2. Считаем обязательные работы
            var necessarilyWorks = main.WrkCont
                .Where(w => (w.IdType == 1 || w.IdType == 2 || w.IdType == 3) &&
                             disciplineIds.Contains(w.IdDiscipline))
                .ToList();

            int necessarilyCount = necessarilyWorks.Count;
            int doneCount = 0;

            // Оценки студента
            var studentEvaluations = main.EvaCont
                .Where(e => e.IdStudent == student.Id)
                .ToList();

            foreach (var work in necessarilyWorks)
            {
                var eval = studentEvaluations.FirstOrDefault(e => e.IdWork == work.Id);
                if (eval != null && !string.IsNullOrWhiteSpace(eval.Value) && eval.Value.Trim() != "2")
                {
                    doneCount++;
                }
            }

            // 3. Считаем все занятия
            var regularWorks = main.WrkCont
                .Where(w => w.IdType != 4 && w.IdType != 3 &&
                             disciplineIds.Contains(w.IdDiscipline))
                .ToList();

            int worksCount = regularWorks.Count;
            int missedCount = 0;

            foreach (var work in regularWorks)
            {
                var eval = studentEvaluations.FirstOrDefault(e => e.IdWork == work.Id);
                if (eval != null && !string.IsNullOrWhiteSpace(eval.Lateness))
                {
                    if (int.TryParse(eval.Lateness, out int lateness))
                    {
                        missedCount += lateness;
                    }
                }
            }

            // 4. Вывод в Progress Bars
            // ВАЖНО: Проверьте x:Name="doneWorks" и x:Name="missedCountBar" в XAML
            doneWorks.Value = necessarilyCount > 0 ? (100f / necessarilyCount) * doneCount : 0;

            if (missedCountBar != null)
                missedCountBar.Value = worksCount > 0 ? (100f / (worksCount * 90f)) * missedCount : 0;

            // Устанавливаем название группы
            var group = main.GrpCont.FirstOrDefault(x => x.Id == student.IdGroup);

            if (TBGroup != null)
                TBGroup.Text = group?.Name ?? "Группа не найдена";
        }
    }
}
