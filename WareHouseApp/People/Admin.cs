namespace WareHouseApp.People
{
    public class Admin : Person
    {
        public override bool Login(string username, string password)
        {
            return AuthenticateCredentials(username, password);
        }

        public override void ChangePassword(string newPassword)
        {
            ChangeStoredPassword(newPassword);
        }

        public override void Logout(int userID)
        {
            EndSession(userID);
        }
    }
}