using HelpDeskSystem.Models;

namespace HelpDeskSystem.Services
{
    public static class SessionManager
    {
        public static User CurrentUser
        {
            get;
            set;
        }
    }
}