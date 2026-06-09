using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class EditIncidentView : UserControl
    {
        private Incident currentIncident;

        public EditIncidentView(int incidentId)
        {
            InitializeComponent();

            using (var db = new HelpDeskDbContext())
            {
                currentIncident = db.Incidents
                    .FirstOrDefault(i => i.Id == incidentId);
            }

            if (currentIncident != null)
            {
                NumberTextBox.Text = currentIncident.Number;
                TitleTextBox.Text = currentIncident.Title;
                AssignedUserTextBox.Text = currentIncident.AssignedUser;

                SelectComboBoxItem(PriorityComboBox, currentIncident.Priority);
                SelectComboBoxItem(StatusComboBox, currentIncident.Status);

                SLATextBox.Text = currentIncident.SLA;
                DeadlinePicker.SelectedDate = currentIncident.DueDate;



                AuthorTextBox.Text = currentIncident.Author;
                DepartmentTextBox.Text = currentIncident.Department;
            }
        }

        private void SelectComboBoxItem(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumberTextBox.Text))
            {
                MessageBox.Show("Введите номер инцидента");
                return;
            }

            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Введите название инцидента");
                return;
            }

            if (string.IsNullOrWhiteSpace(AssignedUserTextBox.Text))
            {
                MessageBox.Show("Введите исполнителя");
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

            string priority =
                (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            string status =
                (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            using (var db = new HelpDeskDbContext())
            {
                var incident = db.Incidents
                    .FirstOrDefault(i => i.Id == currentIncident.Id);

                if (incident != null)
                {
                    incident.Number = NumberTextBox.Text;
                    incident.Title = TitleTextBox.Text;
                    incident.AssignedUser = AssignedUserTextBox.Text;
                    incident.Priority = priority;
                    incident.Status = status;
                    incident.SLA = SLATextBox.Text;
                    incident.Author = AuthorTextBox.Text;
                    incident.Department = DepartmentTextBox.Text;
                    incident.Category =
                        (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    incident.DueDate = DeadlinePicker.SelectedDate.Value;

                    db.SaveChanges();
                }
            }

            MessageBox.Show("Сохранено");

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentControl.Content = new IncidentsView();
        }
        //    private void SaveButton_Click(object sender, RoutedEventArgs e)
        //    {
        //        // Validation

        //        if (string.IsNullOrWhiteSpace(NumberTextBox.Text))
        //        {
        //            MessageBox.Show("Введите номер инцидента");
        //            return;
        //        }

        //        if (int.TryParse(NumberTextBox.Text, out int number))
        //        {
        //            if (number < 0)
        //            {
        //                MessageBox.Show("Номер инцидента не может быть отрицательным");
        //                return;
        //            }
        //        }

        //        if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
        //        {
        //            MessageBox.Show("Введите название инцидента");
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(AssignedUserTextBox.Text))
        //        {
        //            MessageBox.Show("Введите исполнителя");
        //            return;
        //        }

        //        if (PriorityComboBox.SelectedItem == null)
        //        {
        //            MessageBox.Show("Выберите приоритет");
        //            return;
        //        }

        //        if (StatusComboBox.SelectedItem == null)
        //        {
        //            MessageBox.Show("Выберите статус");
        //            return;
        //        }
        //        if (string.IsNullOrWhiteSpace(
        //SLATextBox.Text))
        //        {
        //            MessageBox.Show(
        //                "Введите SLA");

        //            return;
        //        }

        //        if (DeadlinePicker.SelectedDate == null)
        //        {
        //            MessageBox.Show(
        //                "Выберите deadline");

        //            return;
        //        }

        //        if (DeadlinePicker.SelectedDate <
        //            DateTime.Today)
        //        {
        //            MessageBox.Show(
        //                "Deadline не может быть в прошлом");

        //            return;
        //        }

        //        string priority =
        //            (PriorityComboBox.SelectedItem as ComboBoxItem)?
        //            .Content.ToString();

        //        string status =
        // (StatusComboBox.SelectedItem as ComboBoxItem)?
        // .Content.ToString();

        //        if (status == "Закрыт"
        //            && currentIncident.Status != "Решен"
        //            && status != "Решен")
        //        {
        //            MessageBox.Show(
        //                "Сначала переведите инцидент в статус 'Решен' 😭");

        //            return;
        //        }

        // SLA Calculation

        //DateTime dueDate;

        //switch (priority)
        //{
        //    case "Критический":
        //        dueDate = DateTime.Now.AddHours(2);
        //        break;

        //    case "Высокий":
        //        dueDate = DateTime.Now.AddHours(8);
        //        break;

        //    case "Средний":
        //        dueDate = DateTime.Now.AddDays(1);
        //        break;

        //    default:
        //        dueDate = DateTime.Now.AddDays(3);
        //        break;
        //}

        //        using (var db = new HelpDeskDbContext())
        //        {
        //            // Duplicate check

        //            bool exists = db.Incidents.Any(
        //                i => i.Number == NumberTextBox.Text
        //                && i.Id != currentIncident.Id);

        //            if (exists)
        //            {
        //                MessageBox.Show("Инцидент с таким номером уже существует");
        //                return;
        //            }

        //            // Find incident

        //            var incident = db.Incidents
        //                .FirstOrDefault(i => i.Id == currentIncident.Id);

        //            if (incident != null)
        //            {
        //                incident.Number = NumberTextBox.Text;

        //                incident.Title = TitleTextBox.Text;

        //                incident.AssignedUser =
        //                    AssignedUserTextBox.Text;

        //                incident.Priority = priority;

        //                incident.Status =
        //                    (StatusComboBox.SelectedItem as ComboBoxItem)?
        //                    .Content.ToString();

        //                // SLA update
        //                incident.SLA =
        //SLATextBox.Text;

        //                //incident.DueDate = DeadlinePicker.SelectedDate ?? dueDate;
        //                incident.DueDate =
        //DeadlinePicker.SelectedDate.Value;
        //                //incident.DueDate = dueDate;

        //                db.SaveChanges();
        //            }
        //        }

        //        MessageBox.Show("Изменения сохранены 😭");

        //        var mainWindow =
        //            (MainWindow)Application.Current.MainWindow;

        //        mainWindow.MainContentControl.Content =
        //            new IncidentsView();
        //    }
        ////////////////////////////////
        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    using (var db = new HelpDeskDbContext())
        //    {
        //        var incident = db.Incidents
        //            .FirstOrDefault(i => i.Id == currentIncident.Id);

        //        if (incident != null)
        //        {
        //            incident.Number = NumberTextBox.Text;
        //            incident.Title = TitleTextBox.Text;
        //            incident.AssignedUser = AssignedUserTextBox.Text;

        //            incident.Priority =
        //                (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        //            incident.Status =
        //                (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

        //            db.SaveChanges();
        //        }
        //    }

        //    MessageBox.Show("Изменения сохранены 😭");

        //    var mainWindow = (MainWindow)Application.Current.MainWindow;

        //    mainWindow.MainContentControl.Content = new IncidentsView();
        //}
    }
}