using HelpDeskSystem.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class UserDetailsView : UserControl
    {
        private int userId;

        public UserDetailsView(int id)
        {
            InitializeComponent();
            userId = id;
            LoadUser();
            LoadStatistics();
        }
        private void LoadUser()
        {
            using (var db = new HelpDeskDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Id == userId);

                if (user == null)
                    return;

                FullNameText.Text = user.FullName;
                RoleText.Text = $"Роль: {user.Role}";
                DepartmentText.Text = $"Отдел: {user.Department}";
                EmailText.Text = $"Email: {user.Email}";
                WorkHoursText.Text = $"Рабочие часы: {user.WorkHours}";
                CreatedAtText.Text = $"Создан: {user.CreatedAt}";
            }
        }
        private void BackButton_Click( object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContent.Content = new UsersView();
        }
        private void LoadStatistics()
        {
            using (var db = new HelpDeskDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Id == userId);

                if (user == null)
                    return;

                int total = db.Incidents
                    .Count(i => i.AssignedUser == user.FullName);
                int closed = db.Incidents
                    .Count(i => i.AssignedUser == user.FullName && i.Status == "Закрыт");
                int active = db.Incidents
                    .Count(i => i.AssignedUser == user.FullName && i.Status == "В работе");

                TotalIncidentsText.Text = total.ToString();
                ClosedIncidentsText.Text = closed.ToString();
                ActiveIncidentsText.Text = active.ToString();
            }
        }
    }
}