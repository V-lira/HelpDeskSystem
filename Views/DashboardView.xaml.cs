using HelpDeskSystem.Data;
using System;
using System.Linq;
using System.Windows.Controls;

namespace HelpDeskSystem.Views
{
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();

            LoadDashboard();
        }
        private void LoadDashboard()
        {
            using (var db = new HelpDeskDbContext())
            {
                int total = db.Incidents.Count();
                TotalText.Text = total.ToString();
                int open = db.Incidents.Count(i => i.Status != "Закрыт");
                OpenText.Text = open.ToString();
                int overdue = db.Incidents.Count(i => i.DueDate < DateTime.Now && i.Status != "Закрыт");
                OverdueText.Text = overdue.ToString();
                int critical = db.Incidents.Count(i => i.Priority == "Критический");
                CriticalText.Text = critical.ToString();
                RecentIncidentsGrid.ItemsSource = db.Incidents
                    .OrderByDescending(i => i.CreatedAt)
                    .Take(5)
                    .ToList();
            }
        }
    }
}