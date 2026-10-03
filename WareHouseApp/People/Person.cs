using System;

namespace WareHouseApp.People
{
    public abstract class Person
    {
        public int EmpId { get; protected set; }
        public string Username { get; protected set; }
        public bool IsAuthenticated { get; private set; }
        protected string Password { get; private set; }

        public abstract bool Login(string username, string password);
        public abstract void ChangePassword(string newPassword);
        public abstract void Logout(int userID);

        protected bool AuthenticateCredentials(string username, string password)
        {
            int authenticatedId;
            if (!new AccountService().Authenticate(username, password, out authenticatedId))
            {
                EmpId = 0;
                Username = null;
                Password = null;
                IsAuthenticated = false;
                return false;
            }

            EmpId = authenticatedId;
            Username = username.Trim();
            Password = password;
            IsAuthenticated = true;
            return true;
        }

        protected void ChangeStoredPassword(string newPassword)
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Log in before changing the password.");

            new AccountService().ChangePassword(EmpId, newPassword);
            Password = newPassword;
        }

        protected void EndSession(int userId)
        {
            if (IsAuthenticated && EmpId == userId)
            {
                IsAuthenticated = false;
                EmpId = 0;
                Username = null;
                Password = null;
            }
        }
    }
}
