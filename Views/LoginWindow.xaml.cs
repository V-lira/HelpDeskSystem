using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void LoginButton_Click( object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace( FullNameTextBox.Text))
            {
                MessageBox.Show("Введите ФИО");
                return;
            }
            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль");
                return;
            }
            string role =(RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            using (var db = new HelpDeskDbContext())
            {
                var user =db.Users.FirstOrDefault(u => u.FullName == FullNameTextBox.Text && u.Role == role);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден");
                    return;
                }
                SessionManager.CurrentUser = user;
            }
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }
    }
}