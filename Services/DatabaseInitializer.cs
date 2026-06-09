using HelpDeskSystem.Data;
using HelpDeskSystem.Models;
using System;
using System.Linq;

namespace HelpDeskSystem.Services
{
    public static class DatabaseInitializer
    {
        public static void Seed()
        {
            using (var db = new HelpDeskDbContext())
            {
                ////INCIDENTS TEST FOR TABLE
                if (!db.Incidents.Any())
                {
                    db.Incidents.Add(new Incident
                    {
                        Number = "INC-2026-001",
                        Title = "Не работает Outlook",
                        Priority = "Высокий",
                        Status = "В работе",
                        AssignedUser = "Иванов",
                        SLA = "2ч",
                        CreatedAt = new DateTime(2026, 5, 10, 10, 45, 0),
                        DueDate = new DateTime(2026, 5, 12, 10, 45, 0)
                    });
                    db.Incidents.Add(new Incident
                    {
                        Number = "INC-2026-002",
                        Title = "Сбой VPN подключения",
                        Priority = "Критический",
                        Status = "Просрочен",
                        AssignedUser = "Петров",
                        SLA = "Просрочен",
                        CreatedAt = new DateTime(2026, 5, 14, 14, 20, 0),
                        DueDate = new DateTime(2026, 5, 14, 16, 20, 0)
                    });
                    db.Incidents.Add(new Incident
                    {
                        Number = "INC-2026-003",
                        Title = "Замена пароля",
                        Priority = "Низкий",
                        Status = "Закрыт",
                        AssignedUser = "Сидоров",
                        SLA = "Выполнен",
                        CreatedAt = new DateTime(2026, 5, 15, 9, 30, 0),
                        DueDate = new DateTime(2026, 5, 15, 11, 30, 0)
                    });

                    db.SaveChanges();
                }
                //USERS TEST
                //if (!db.Users.Any())
                //{
                //    db.Users.Add(new User
                //    {
                //        FullName = "Администратор",
                //        Role = "Admin",
                //        Department = "IT",
                //        Email = "admin@helpdesk.local",
                //        WorkHours = "09:00 - 18:00"
                //    });
                //    db.Users.Add(new User
                //    {
                //        FullName = "Иванов Иван",
                //        Role = "Support",
                //        Department = "Техподдержка",
                //        Email = "ivanov@helpdesk.local",
                //        WorkHours = "09:00 - 18:00"
                //    });
                //    db.Users.Add(new User
                //    {
                //        FullName = "Петров Петр",
                //        Role = "Support",
                //        Department = "Сеть",
                //        Email = "petrov@helpdesk.local",
                //        WorkHours = "10:00 - 19:00"
                //    });
                  ////  db.SaveChanges();
               // }
            }
        }
    }
}