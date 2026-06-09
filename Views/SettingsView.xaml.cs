using HelpDeskSystem.Data;
using MaterialDesignThemes.Wpf;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace HelpDeskSystem.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            LoadDatabaseInfo();
            LoadStatistics();
        }
        private void LoadDatabaseInfo()
        {
            string path = Path.GetFullPath("HelpDesk.db");
            FileInfo file = new FileInfo(path);
            DatabasePathText.Text = $"Путь к БД: {path}";
            DatabaseSizeText.Text = $"Размер БД: {file.Length / 1024} KB";
        }
        private void LoadStatistics()
        {
            using (var db = new HelpDeskDbContext())
            {
                IncidentsCountText.Text = $"Инцидентов: {db.Incidents.Count()}";
                CommentsCountText.Text = $"Комментариев: {db.Comments.Count()}";
                UsersCountText.Text = $"Пользователей: {db.Users.Count()}";
            }
        }
        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTheme =(ThemeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();
            if (selectedTheme == "Dark")
            {
                theme.SetBaseTheme(BaseTheme.Dark);
            }
            else
            {
                theme.SetBaseTheme(BaseTheme.Light);
            }
            paletteHelper.SetTheme(theme);
        }
    }
}