using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class CreateUserView : UserControl
    {
        public CreateUserView()
        {
            InitializeComponent();
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
            if (string.IsNullOrWhiteSpace(DepartmentTextBox.Text))
            {
                MessageBox.Show("Введите отдел");
                return;
            }
            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Введите email");
                return;
            }
            using (var db = new HelpDeskDbContext())
            {
                bool exists =
                    db.Users.Any(u => u.Email == EmailTextBox.Text);
                if (exists)
                {
                    MessageBox.Show("Пользователь с таким email уже существует");
                    return;
                }
            }
            //CREATE USERS::::
            var user = new User
            {
                FullName = FullNameTextBox.Text,
                Role =(RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Department = DepartmentTextBox.Text,
                Email = EmailTextBox.Text,
                WorkHours = WorkHoursTextBox.Text,
                CreatedAt = DateTime.Now
            };
            using (var db = new HelpDeskDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            MessageBox.Show("Пользователь создан!");
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContent.Content = new UsersView();
        }
    }
}