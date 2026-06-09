using HelpDeskSystem.Services;
using HelpDeskSystem.Views;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

//MAIN TEST:
// Олег
// admin


// TASKS:
//Добавьте пожалуйста для задач метрику "время решения"
//И запретите редактирование описания после статуса «в работе» (только добавление комментариев)
//еще я хотел попросить цветовую индикацию на просрочки и важность
//на полях ввода есть ограничения?
//на ввод неправильных данных, отрицательных значений

namespace HelpDeskSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ContentControl MainContentControl
        {
            get { return MainContent; }
        }

        public MainWindow()
        {
            InitializeComponent();
            CreateDatabase();
           // DatabaseInitializer.Seed();
            MainContent.Content = new DashboardView();
        }
        private void CreateDatabase()
        {
            if (!File.Exists("HelpDesk.db"))
            {
                SQLiteConnection.CreateFile("HelpDesk.db");

                using (var connection =
                    new SQLiteConnection("Data Source=HelpDesk.db"))
                {
                    ///////////////////////////////////////////////
                    //INCIDENTS TABLE:
                    connection.Open();
                    string sql =@"CREATE TABLE Incidents (Id INTEGER PRIMARY KEY AUTOINCREMENT,Number TEXT,
                    Title TEXT, Priority TEXT, Status TEXT, AssignedUser TEXT, SLA TEXT, Category TEXT, Department TEXT, Author TEXT,
                    CreatedAt TEXT, DueDate TEXT)";
                    ///////////////////////////////////////////////
                    //COMMETS TABLE:
                    string commentsSql = @"CREATE TABLE IncidentComments (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    IncidentId INTEGER, Author TEXT, Message TEXT, CreatedAt TEXT)";
                    ///////////////////////////////////////////////
                    //CREATE Incidents
                    string usersSql = @"CREATE TABLE Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, FullName TEXT, Role TEXT,
                    Department TEXT, Email TEXT, WorkHours TEXT, CreatedAt TEXT)";
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    using (var usersCommand = new SQLiteCommand(usersSql, connection))
                    {
                        usersCommand.ExecuteNonQuery();
                    }
                    //CREATE Comments
                    using (var commentsCommand = new SQLiteCommand(commentsSql, connection))
                    {
                        commentsCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void DashboardButton_Click( object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DashboardView();
        }
        private void IncidentsButton_Click( object sender, RoutedEventArgs e)
        {
            MainContent.Content = new IncidentsView();
        }

        private void CreateIncidentButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CreateIncidentView();
        }
        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UsersView();
        }

        private void ReportsButton_Click( object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReportsView();
        }
        private void SettingsButton_Click( object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SettingsView();
        }
    }
}