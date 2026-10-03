using System;
using System.Data;
using System.Data.SqlClient;
using WareHouseApp.Data;

namespace WareHouseApp.Materials
{
    public class Material
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public Material()
        {
        }

        public Material(int id, string name, int count)
        {
            Id = id;
            Name = name;
            Count = count;
        }

        public bool AddMaterial(string name, int count)
        {
            Validate(name, count);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        int id;
                        using (SqlCommand nextId = new SqlCommand("SELECT ISNULL(MAX(Id), 0) + 1 FROM Material WITH (UPDLOCK, HOLDLOCK)", connection, transaction))
                            id = Convert.ToInt32(nextId.ExecuteScalar());

                        const string query = "INSERT INTO Material (Id, Name, Count) VALUES (@Id, @Name, @Count)";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                            command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                            command.Parameters.Add("@Count", SqlDbType.Int).Value = count;
                            bool added = command.ExecuteNonQuery() > 0;
                            transaction.Commit();
                            return added;
                        }
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
                throw new InvalidOperationException("Unable to add material: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public DataTable GetAllMaterials()
        {
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "SELECT Id, Name, Count FROM Material ORDER BY Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable materials = new DataTable();
                    adapter.Fill(materials);
                    return materials;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load inventory: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool UpdateMaterial(int id, string name, int count)
        {
            Validate(name, count);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "UPDATE Material SET Name = @Name, Count = @Count WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = name.Trim();
                    command.Parameters.Add("@Count", SqlDbType.Int).Value = count;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to update material: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool DeleteMaterial(int id)
        {
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "DELETE FROM Material WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to delete material: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool AddStock(int id, int quantity)
        {
            ValidateStockMovement(id, quantity);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "UPDATE Material SET Count = ISNULL(Count, 0) + @Quantity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to receive stock: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        public bool RemoveStock(int id, int quantity)
        {
            ValidateStockMovement(id, quantity);
            SqlConnection connection = WarehouseDatabase.CreateConnection();
            try
            {
                connection.Open();
                const string query = "UPDATE Material SET Count = ISNULL(Count, 0) - @Quantity WHERE Id = @Id AND ISNULL(Count, 0) >= @Quantity";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                    return command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to ship stock: " + ex.Message, ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Dispose();
            }
        }

        private static void Validate(string name, int count)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
                throw new ArgumentException("Material name is required and must be 50 characters or fewer.", "name");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "Material count cannot be negative.");
        }

        private static void ValidateStockMovement(int id, int quantity)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("id", "A valid material ID is required.");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException("quantity", "Stock movement quantity must be positive.");
        }
    }
}
