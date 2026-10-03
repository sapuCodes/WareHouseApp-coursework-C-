using System;
using System.Data;
using System.Data.SqlClient;
using WareHouseApp.Data;

namespace WareHouseApp.People
{
    public sealed class AccountService
    {
        public int RegisterEmployee(string name, string position, decimal salary, string username, string password)
        {
            ValidateRegistration(name, position, salary, username, password);

            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        using (SqlCommand checkUsername = new SqlCommand("SELECT COUNT(*) FROM Person WITH (UPDLOCK, HOLDLOCK) WHERE RTRIM(Username) = @Username", connection, transaction))
                        {
                            checkUsername.Parameters.Add("@Username", SqlDbType.NChar, 10).Value = username.Trim();
                            if (Convert.ToInt32(checkUsername.ExecuteScalar()) > 0)
                                throw new InvalidOperationException("That username is already registered.");
                        }

                        int empId;
                        using (SqlCommand nextId = new SqlCommand("SELECT ISNULL(MAX(EmpId), 0) + 1 FROM Person WITH (UPDLOCK, HOLDLOCK)", connection, transaction))
                            empId = Convert.ToInt32(nextId.ExecuteScalar());

                        using (SqlCommand addPerson = new SqlCommand("INSERT INTO Person (EmpId, Username, Password) VALUES (@EmpId, @Username, @Password)", connection, transaction))
                        {
                            addPerson.Parameters.Add("@EmpId", SqlDbType.Int).Value = empId;
                            addPerson.Parameters.Add("@Username", SqlDbType.NChar, 10).Value = username.Trim();
                            addPerson.Parameters.Add("@Password", SqlDbType.NChar, 10).Value = password;
                            addPerson.ExecuteNonQuery();
                        }

                        using (SqlCommand addEmployee = new SqlCommand("INSERT INTO Employee (Name, Position, Salary, EmpId) VALUES (@Name, @Position, @Salary, @EmpId)", connection, transaction))
                        {
                            addEmployee.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                            addEmployee.Parameters.Add("@Position", SqlDbType.NVarChar, 50).Value = position.Trim();
                            SqlParameter salaryParameter = addEmployee.Parameters.Add("@Salary", SqlDbType.Decimal);
                            salaryParameter.Precision = 18;
                            salaryParameter.Scale = 2;
                            salaryParameter.Value = salary;
                            addEmployee.Parameters.Add("@EmpId", SqlDbType.Int).Value = empId;
                            addEmployee.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return empId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to register the account and employee: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool Authenticate(string username, string password, out int empId)
        {
            empId = 0;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || username.Trim().Length > 10 || password.Length > 10)
                return false;

            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "SELECT EmpId FROM Person WHERE RTRIM(Username) = @Username AND RTRIM(Password) = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.NChar, 10).Value = username.Trim();
                    command.Parameters.Add("@Password", SqlDbType.NChar, 10).Value = password;
                    object result = command.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;

                    empId = Convert.ToInt32(result);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to verify the account: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public void ChangePassword(int empId, string newPassword)
        {
            if (empId <= 0)
                throw new ArgumentOutOfRangeException("empId", "A valid employee ID is required.");
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length > 10)
                throw new ArgumentException("Password is required and must be 10 characters or fewer by the current database schema.", "newPassword");

            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "UPDATE Person SET Password = @Password WHERE EmpId = @EmpId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Password", SqlDbType.NChar, 10).Value = newPassword;
                    command.Parameters.Add("@EmpId", SqlDbType.Int).Value = empId;
                    if (command.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("The account no longer exists.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to change password: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        private static void ValidateRegistration(string name, string position, decimal salary, string username, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
                throw new ArgumentException("Employee name is required and must be 50 characters or fewer.", "name");
            if (string.IsNullOrWhiteSpace(position) || position.Trim().Length > 50)
                throw new ArgumentException("Employee position is required and must be 50 characters or fewer.", "position");
            if (string.IsNullOrWhiteSpace(username) || username.Trim().Length > 10)
                throw new ArgumentException("Username is required and must be 10 characters or fewer.", "username");
            if (string.IsNullOrWhiteSpace(password) || password.Length > 10)
                throw new ArgumentException("Password is required and must be 10 characters or fewer by the current database schema.", "password");
            if (salary < 0)
                throw new ArgumentOutOfRangeException("salary", "Salary cannot be negative.");
        }
    }
}
