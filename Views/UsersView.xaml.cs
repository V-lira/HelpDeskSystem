using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HelpDeskSystem.Services;

namespace HelpDeskSystem.Views
{
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();

            LoadUsers();
        }
        private void LoadUsers()
        {
            using (var db = new HelpDeskDbContext())
            {
                UsersGrid.ItemsSource = db.Users.ToList();
            }
        }
        //по ФИО
        //по роли
        //по отделу
        //по email
        //моментальненько (на ввод)
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            using (var db = new HelpDeskDbContext())
            {
                var query = db.Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(u =>
                        u.FullName.ToLower().Contains(searchText) ||
                        u.Role.ToLower().Contains(searchText) ||
                        u.Department.ToLower().Contains(searchText) ||
                        u.Email.ToLower().Contains(searchText)
                    );
                }

                UsersGrid.ItemsSource = query.ToList();
            }
        }
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow =(MainWindow)Application.Current.MainWindow;
            mainWindow.MainContent.Content = new CreateUserView();
        }
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.CurrentUser.Role != "Admin")
            {
                MessageBox.Show("ВЫ НЕ АДМИН!");
                return;
            }
            Button button = sender as Button;
            int userId = (int)button.Tag;
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow.MainContentControl.Content = new EditUserView(userId);
        }
        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (SessionManager.CurrentUser.Role != "Admin")
            {
                MessageBox.Show("ВЫ НЕ АДМИН!");
                return;
            }
            Button button = sender as Button;
            int userId = (int)button.Tag;
            using (var db = new HelpDeskDbContext())
            {
                var selectedUser = db.Users
                    .FirstOrDefault(u => u.Id == userId);

                if (selectedUser == null)
                    return;

                //нельзя удалить самого себя
                if (selectedUser.FullName ==
                    SessionManager.CurrentUser.FullName)
                {
                    MessageBox.Show("Нельзя удалить свою учетную запись!");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Удалить пользователя?","Подтверждение", MessageBoxButton.YesNo,MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes)
                    return;
                db.Users.Remove(selectedUser);
                db.SaveChanges();
            }
            LoadUsers();
            MessageBox.Show("Пользователь удалён!");
        }
        private void UsersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UsersGrid.SelectedItem is User user)
            {
                var mainWindow =(MainWindow)Application.Current.MainWindow;
                mainWindow.MainContent.Content = new UserDetailsView(user.Id);
            }
        }
    }
}