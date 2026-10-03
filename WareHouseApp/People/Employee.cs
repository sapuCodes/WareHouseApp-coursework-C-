using System;
using System.Data;
using System.Data.SqlClient;
using WareHouseApp.Data;

namespace WareHouseApp.People
{
    // Inheritance reuses Person identity/authentication while adding employee-specific state.
    public class Employee : Person
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }

        public Employee()
        {
        }

        public Employee(int employeeId, string name, string position, decimal salary, int empId)
        {
            EmployeeId = employeeId;
            Name = name;
            Position = position;
            Salary = salary;
            EmpId = empId;
        }

        public bool AddEmployee(string name, string position, decimal salary, int empId)
        {
            ValidateEmployeeData(name, position, salary, empId);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "INSERT INTO Employee (Name, Position, Salary, EmpId) VALUES (@Name, @Position, @Salary, @EmpId)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                    command.Parameters.Add("@Position", SqlDbType.NVarChar, 50).Value = position.Trim();
                    SqlParameter salaryParameter = command.Parameters.Add("@Salary", SqlDbType.Decimal);
                    salaryParameter.Precision = 18;
                    salaryParameter.Scale = 2;
                    salaryParameter.Value = salary;
                    command.Parameters.Add("@EmpId", SqlDbType.Int).Value = empId;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                // Preserve database failure context for the form's user-facing MessageBox.
                throw new InvalidOperationException("Unable to add employee: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public DataTable GetAllEmployees()
        {
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "SELECT EmployeeId, Name, Position, Salary, EmpId FROM Employee ORDER BY EmployeeId";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable employees = new DataTable();
                    adapter.Fill(employees);
                    return employees;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load employees: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public Employee GetProfileByPersonId(int personId)
        {
            if (personId <= 0)
                throw new ArgumentOutOfRangeException("personId", "A valid Person EmpId is required.");

            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "SELECT EmployeeId, Name, Position FROM Employee WHERE EmpId = @EmpId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmpId", SqlDbType.Int).Value = personId;
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!reader.Read())
                            return null;

                        return new Employee(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            0m,
                            personId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load the employee profile: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool UpdateEmployee(int employeeId, string name, string position, decimal salary, int empId)
        {
            ValidateEmployeeData(name, position, salary, empId);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "UPDATE Employee SET Name = @Name, Position = @Position, Salary = @Salary, EmpId = @EmpId WHERE EmployeeId = @EmployeeId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                    command.Parameters.Add("@Position", SqlDbType.NVarChar, 50).Value = position.Trim();
                    SqlParameter salaryParameter = command.Parameters.Add("@Salary", SqlDbType.Decimal);
                    salaryParameter.Precision = 18;
                    salaryParameter.Scale = 2;
                    salaryParameter.Value = salary;
                    command.Parameters.Add("@EmpId", SqlDbType.Int).Value = empId;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to update employee: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool DeleteEmployee(int employeeId)
        {
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        object personId;
                        using (SqlCommand findEmployee = new SqlCommand("SELECT EmpId FROM Employee WITH (UPDLOCK, HOLDLOCK) WHERE EmployeeId = @EmployeeId", connection, transaction))
                        {
                            findEmployee.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                            personId = findEmployee.ExecuteScalar();
                        }
                        if (personId == null)
                        {
                            transaction.Commit();
                            return false;
                        }

                        using (SqlCommand removeEmployee = new SqlCommand("DELETE FROM Employee WHERE EmployeeId = @EmployeeId", connection, transaction))
                        {
                            removeEmployee.Parameters.Add("@EmployeeId", SqlDbType.Int).Value = employeeId;
                            if (removeEmployee.ExecuteNonQuery() == 0)
                            {
                                transaction.Commit();
                                return false;
                            }
                        }

                        if (personId != DBNull.Value)
                        {
                            using (SqlCommand removeAccount = new SqlCommand("DELETE FROM Person WHERE EmpId = @EmpId AND NOT EXISTS (SELECT 1 FROM Employee WHERE EmpId = @EmpId)", connection, transaction))
                            {
                                removeAccount.Parameters.Add("@EmpId", SqlDbType.Int).Value = Convert.ToInt32(personId);
                                removeAccount.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to delete employee: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

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

        private static void ValidateEmployeeData(string name, string position, decimal salary, int empId)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
                throw new ArgumentException("Employee name is required and must be 50 characters or fewer.", "name");
            if (string.IsNullOrWhiteSpace(position) || position.Trim().Length > 50)
                throw new ArgumentException("Employee position is required and must be 50 characters or fewer.", "position");
            if (salary < 0)
                throw new ArgumentOutOfRangeException("salary", "Salary cannot be negative.");
            if (empId <= 0)
                throw new ArgumentOutOfRangeException("empId", "A valid Person EmpId is required.");
        }
    }
}
