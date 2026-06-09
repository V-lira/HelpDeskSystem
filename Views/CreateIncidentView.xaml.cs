using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class CreateIncidentView : UserControl
    {
        public CreateIncidentView()
        {
            InitializeComponent();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var incident = new Incident
            {
                Number = NumberTextBox.Text,
                Title = TitleTextBox.Text,
                AssignedUser = AssignedUserTextBox.Text,
                Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Author = AuthorTextBox.Text,
                Department = DepartmentTextBox.Text,
                Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                SLA = SLATextBox.Text
            };
            if (string.IsNullOrWhiteSpace(NumberTextBox.Text))
            {
                MessageBox.Show("Введите номер инцидента");
                return;
            }
            if (int.TryParse(NumberTextBox.Text, out int number))
            {
                if (number < 0)
                {
                    MessageBox.Show("Номер инцидента не может быть отрицательным");
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(AssignedUserTextBox.Text))
            {
                MessageBox.Show("Введите исполнителя");
                return;
            }
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название инцидента");
                return;
            }
            if (string.IsNullOrWhiteSpace(AuthorTextBox.Text))
            {
                MessageBox.Show("Введите автора");
                return;
            }
            if (PriorityComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите приоритет");
                return;
            }
            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус");
                return;
            }
            if (string.IsNullOrWhiteSpace(SLATextBox.Text))
            {
                MessageBox.Show("Введите SLA");
                return;
            }
            if (DeadlinePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите deadline");
                return;
            }
            if (DeadlinePicker.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Deadline не может быть в прошлом");
                return;
            }
            using (var db = new HelpDeskDbContext())
            {
                bool exists = db.Incidents.Any(i => i.Number == NumberTextBox.Text);

                if (exists)
                {
                    MessageBox.Show("Инцидент с таким номером уже существует");
                    return;
                }
            }
            string priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime dueDate;
            switch (priority)
            {
                case "Критический":
                    dueDate = DateTime.Now.AddHours(2);
                    break;

                case "Высокий":
                    dueDate = DateTime.Now.AddHours(8);
                    break;

                case "Средний":
                    dueDate = DateTime.Now.AddDays(1);
                    break;

                default:
                    dueDate = DateTime.Now.AddDays(3);
                    break;
            }
            incident.SLA = SLATextBox.Text;
            incident.DueDate =
                DeadlinePicker.SelectedDate
                ?? dueDate;
            //incident.DueDate = dueDate;
            incident.CreatedAt = DateTime.Now;
            using (var db = new HelpDeskDbContext())
            {
                db.Incidents.Add(incident);
                db.SaveChanges();
            }
            MessageBox.Show("Инцидент сохранён!");
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentControl.Content = new IncidentsView();


        }
    }
}