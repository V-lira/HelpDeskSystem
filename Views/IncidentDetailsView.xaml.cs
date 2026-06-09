using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using HelpDeskSystem.Services;
using Microsoft.VisualBasic;
using System;
using System.Data.Entity;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class IncidentDetailsView : UserControl
    {
        private int incidentId;

        public IncidentDetailsView(int id)
        {
            InitializeComponent();
            incidentId = id;
            LoadIncident();
            LoadComments();
        }

        private void LoadIncident()
        {
            using (var db = new HelpDeskDbContext())
            {
                var incident = db.Incidents
                    .FirstOrDefault(i => i.Id == incidentId);

                if (incident == null)
                    return;
                ///////////////////////////////////////////
                TitleText.Text = incident.Title;
                NumberText.Text = $"Номер: {incident.Number}";
                PriorityText.Text = $"Приоритет: {incident.Priority}";
                StatusText.Text =$"Статус: {incident.Status}";
                AssignedText.Text =$"Исполнитель: {incident.AssignedUser}";
                CreatedText.Text = $"Создан: {incident.CreatedAt}";
                DeadlineText.Text = $"Deadline: {incident.DueDate}";
                CategoryText.Text = "Категория: " + incident.Category;
                DepartmentText.Text = "Подразделение: " + incident.Department;
                AuthorText.Text = "Автор: " + incident.Author;
            }
        }
        private void LoadComments()
        {
            using (var db = new HelpDeskDbContext())
            {
                CommentsListBox.ItemsSource = null;
                CommentsListBox.ItemsSource = db.Comments
                    .Where(c => c.IncidentId == incidentId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow.MainContentControl.Content = new IncidentsView();
        }
        private void EditCommentButton_Click( object sender, RoutedEventArgs e)
        {
            int commentId = int.Parse((sender as Button).Tag.ToString());
            using (var db = new HelpDeskDbContext())
            {
                /////////////////////////////
                var comment = db.Comments
                    .FirstOrDefault(c => c.Id == commentId);
                /////////////////////
                if (comment == null)
                    return;
                ////////////////
                string newMessage = Microsoft.VisualBasic.Interaction.InputBox(
                        "Редактирование комментария:",
                        "Edit Comment",
                        comment.Message);

                if (string.IsNullOrWhiteSpace(newMessage))
                    return;
                ////////////////////////////
                comment.Message = newMessage;
                db.SaveChanges();
            }
            LoadComments();
            MessageBox.Show("Комментарий обновлён!");
        }
        private void DeleteCommentButton_Click( object sender, RoutedEventArgs e)
        {
            int commentId = int.Parse((sender as Button).Tag.ToString());
            var result = MessageBox.Show(
                    "Удалить комментарий?",
                    "Подтверждение",
                    MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
                return;

            using (var db = new HelpDeskDbContext())
            {
                var comment = db.Comments
                    .FirstOrDefault(c => c.Id == commentId);

                if (comment != null)
                {
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                }
            }
            LoadComments();
            MessageBox.Show("Комментарий удалён!");
        }
        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CommentTextBox.Text))
            {
                MessageBox.Show("Введите комментарий");
                return;
            }
            using (var db = new HelpDeskDbContext())
            {
                var comment = new IncidentComment
                    {
                        IncidentId = incidentId,
                        Author = $"{SessionManager.CurrentUser.FullName} " + $"({SessionManager.CurrentUser.Role})",
                        //Author = "Оператор",
                        Message = CommentTextBox.Text,
                        CreatedAt = DateTime.Now
                    };
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            CommentTextBox.Clear();
            LoadComments();
            MessageBox.Show("Комментарий добавлен!");
        }
    }
}