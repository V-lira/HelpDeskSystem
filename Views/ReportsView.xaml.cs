using HelpDeskSystem.Data;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
            LoadStatistics();
        }
        private void LoadStatistics()
        {
            using (var db = new HelpDeskDbContext())
            {
                TotalText.Text = db.Incidents.Count().ToString();
                ClosedText.Text = db.Incidents
                    .Count(i => i.Status == "Закрыт")
                    .ToString();

                CriticalText.Text = db.Incidents
                    .Count(i => i.Priority == "Критический")
                    .ToString();

                OverdueText.Text = db.Incidents
                    .Count(i => i.DueDate < DateTime.Now && i.Status != "Закрыт")
                    .ToString();
            }
        }
        private void ExportCsvButton_Click( object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FileName = "incidents_report.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var db = new HelpDeskDbContext())
                {
                    var incidents = db.Incidents.ToList();
                    StringBuilder csv = new StringBuilder();
                    //head:
                    csv.AppendLine("Номер;Название;Приоритет;Статус;Исполнитель;Создан;Deadline");
                    //rows:
                    foreach (var incident in incidents)
                    {
                        csv.AppendLine(
                            $"{incident.Number};" +
                            $"{incident.Title};" +
                            $"{incident.Priority};" +
                            $"{incident.Status};" +
                            $"{incident.AssignedUser};" +
                            $"{incident.CreatedAt};" +
                            $"{incident.DueDate}");
                    }
                    File.WriteAllText(saveFileDialog.FileName, csv.ToString(), Encoding.UTF8);
                }
                MessageBox.Show("CSV отчёт успешно экспортирован!");
            }
        }
        private void ExportTxtButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.FileName = "incidents_report.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (var db = new HelpDeskDbContext())
                {
                    var incidents = db.Incidents.ToList();
                    StringBuilder txt = new StringBuilder();
                    txt.AppendLine("ОТЧЁТ ПО ИНЦИДЕНТАМ");
                    txt.AppendLine("--------------------------------");
                    foreach (var incident in incidents)
                    {
                        txt.AppendLine($"Номер: {incident.Number}");
                        txt.AppendLine($"Название: {incident.Title}");
                        txt.AppendLine($"Приоритет: {incident.Priority}");
                        txt.AppendLine($"Статус: {incident.Status}");
                        txt.AppendLine($"Исполнитель: {incident.AssignedUser}");
                        txt.AppendLine($"Создан: {incident.CreatedAt}");
                        txt.AppendLine($"Deadline: {incident.DueDate}");
                        txt.AppendLine("--------------------------------");
                    }
                    File.WriteAllText(saveFileDialog.FileName, txt.ToString(), Encoding.UTF8);
                }
                MessageBox.Show("TXT отчёт успешно экспортирован!");
            }
        }
    }
}