using System;
using WareHouseApp.Materials;

namespace WareHouseApp.People
{
    public class ShippingOperator : Person
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

        public bool LoadStocks(int materialId, int quantity)
        {
            EnsureAuthenticated();
            return new Material().AddStock(materialId, quantity);
        }

        public bool ShipStocks(int materialId, int quantity)
        {
            EnsureAuthenticated();
            return new Material().RemoveStock(materialId, quantity);
        }

        private void EnsureAuthenticated()
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("Log in before changing stock.");
        }
    }
}
