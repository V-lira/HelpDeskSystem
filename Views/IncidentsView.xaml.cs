using HelpDeskSystem.Data;
using System.Linq;
using System.Windows;
using HelpDeskSystem.Models;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class IncidentsView : UserControl
    {
        public IncidentsView()
        {
            InitializeComponent();
            LoadIncidents();
        }
        private void IncidentsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IncidentsGrid.SelectedItem is Incident incident)
            {
                var mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.MainContent.Content = new IncidentDetailsView(incident.Id);
            }
        }
        private void LoadIncidents()
        {
            using (var db = new HelpDeskDbContext())
            {
                IncidentsGrid.ItemsSource = db.Incidents.ToList();
            }
        }
        private void CreateIncidentButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentControl.Content = new CreateIncidentView();
        }
        private void EditIncidentButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int incidentId = (int)button.Tag;
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentControl.Content = new EditIncidentView(incidentId);
        }
        //работает так:
        //ввод:
        //inc - номера
        //outbook - темы
        //Иванов - исполнителей
        private void SearchTextBox_TextChanged(object sender,TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();
            using (var db = new HelpDeskDbContext())
            {
                //Если строка пустая — показать всё
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    IncidentsGrid.ItemsSource = db.Incidents.ToList();
                    return;
                }

                var filteredIncidents = db.Incidents
                    .ToList()
                    .Where(i => i.Number.ToLower().Contains(searchText) || i.Title.ToLower().Contains(searchText) || i.AssignedUser.ToLower()
                    .StartsWith(searchText) || i.Status.ToLower() .Contains(searchText) || i.Priority.ToLower().Contains(searchText)).ToList();
                IncidentsGrid.ItemsSource = filteredIncidents;
            }
        }
        private void DeleteIncidentButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int incidentId = (int)button.Tag;
            MessageBoxResult result = MessageBox.Show(
                "Удалить инцидент?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
                return;
            using (var db = new HelpDeskDbContext())
            {
                var incident = db.Incidents.FirstOrDefault(i => i.Id == incidentId);
                if (incident != null)
                {
                    db.Incidents.Remove(incident);
                    db.SaveChanges();
                }
            }
            LoadIncidents();
            MessageBox.Show("Инцидент удалён!");
        }
    }
}