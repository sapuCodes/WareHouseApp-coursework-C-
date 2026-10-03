using System;
using System.Data;
using System.Data.SqlClient;
using WareHouseApp.Data;

namespace WareHouseApp.People
{
    // Encapsulation keeps customer state behind properties and CRUD persistence in the model.
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Customer()
        {
        }

        public Customer(int customerId, string name, string email, string phone)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public bool AddCustomer(string name, string email, string phone)
        {
            Validate(name, email, phone);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "INSERT INTO Customer (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = email == null ? (object)DBNull.Value : email.Trim();
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone == null ? (object)DBNull.Value : phone.Trim();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                // Preserve database failure context for the form's user-facing MessageBox.
                throw new InvalidOperationException("Unable to add customer: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public DataTable GetAllCustomers()
        {
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "SELECT CustomerId, Name, Email, Phone FROM Customer ORDER BY CustomerId";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable customers = new DataTable();
                    adapter.Fill(customers);
                    return customers;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load customers: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool UpdateCustomer(int customerId, string name, string email, string phone)
        {
            Validate(name, email, phone);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "UPDATE Customer SET Name = @Name, Email = @Email, Phone = @Phone WHERE CustomerId = @CustomerId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = email == null ? (object)DBNull.Value : email.Trim();
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 20).Value = phone == null ? (object)DBNull.Value : phone.Trim();
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to update customer: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "DELETE FROM Customer WHERE CustomerId = @CustomerId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to delete customer: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        private static void Validate(string name, string email, string phone)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
                throw new ArgumentException("Customer name is required and must be 50 characters or fewer.", "name");
            if (email != null && email.Trim().Length > 50)
                throw new ArgumentException("Email must be 50 characters or fewer.", "email");
            if (phone != null && phone.Trim().Length > 20)
                throw new ArgumentException("Phone must be 20 characters or fewer.", "phone");
        }
    }
}
