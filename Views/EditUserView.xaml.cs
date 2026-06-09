using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class EditUserView : UserControl
    {
        private User currentUser;
        public EditUserView(int userId)
        {
            InitializeComponent();
            using (var db = new HelpDeskDbContext())
            {
                currentUser = db.Users.FirstOrDefault(u => u.Id == userId);
            }
            if (currentUser != null)
            {
                FullNameTextBox.Text = currentUser.FullName;
                DepartmentTextBox.Text = currentUser.Department;
                EmailTextBox.Text = currentUser.Email;
                WorkHoursTextBox.Text = currentUser.WorkHours;
                SelectComboBoxItem(RoleComboBox, currentUser.Role);
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
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("Введите ФИО");
                return;
            }
            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль");
                return;
            }
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            // защита от снятия админа с самого себя
            if (currentUser.FullName == SessionManager.CurrentUser.FullName && role != "Admin")
            {
                MessageBox.Show("Нельзя снять роль Admin у самого себя!");
                return;
            }
            using (var db = new HelpDeskDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Id == currentUser.Id);
                if (user != null)
                {
                    user.FullName = FullNameTextBox.Text;
                    user.Role = role;
                    user.Department = DepartmentTextBox.Text;
                    user.Email = EmailTextBox.Text;
                    user.WorkHours = WorkHoursTextBox.Text;
                    db.SaveChanges();
                }
            }
            MessageBox.Show("Пользователь обновлён!");
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow.MainContentControl.Content = new UsersView();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow.MainContentControl.Content = new UsersView();
        }
    }
}