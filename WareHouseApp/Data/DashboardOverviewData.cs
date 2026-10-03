using System;
using System.Data;
using System.Data.SqlClient;

namespace WareHouseApp.Data
{
    public sealed class DashboardOverviewData
    {
        public int TotalInventory { get; private set; }
        public int TotalCustomers { get; private set; }
        public int TotalEmployees { get; private set; }
        public int LowStockItems { get; private set; }
        public DataTable LowStockMaterials { get; private set; }

        private DashboardOverviewData()
        {
        }

        public static DashboardOverviewData Load()
        {
            try
            {
                using (SqlConnection connection = WarehouseDatabase.CreateConnection())
                {
                    connection.Open();
                    DashboardOverviewData overview = new DashboardOverviewData();

                    const string summaryQuery =
                        "SELECT (SELECT COUNT(*) FROM Material) AS TotalInventory, " +
                        "(SELECT COUNT(*) FROM Customer) AS TotalCustomers, " +
                        "(SELECT COUNT(*) FROM Employee) AS TotalEmployees, " +
                        "(SELECT COUNT(*) FROM Material WHERE [Count] < 10) AS LowStockItems";
                    using (SqlCommand command = new SqlCommand(summaryQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            overview.TotalInventory = reader.GetInt32(0);
                            overview.TotalCustomers = reader.GetInt32(1);
                            overview.TotalEmployees = reader.GetInt32(2);
                            overview.LowStockItems = reader.GetInt32(3);
                        }
                    }

                    const string lowStockQuery =
                        "SELECT TOP 10 Name, [Count] AS Quantity FROM Material " +
                        "WHERE [Count] < 10 ORDER BY [Count], Name";
                    using (SqlCommand command = new SqlCommand(lowStockQuery, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        overview.LowStockMaterials = new DataTable();
                        adapter.Fill(overview.LowStockMaterials);
                    }

                    return overview;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to load dashboard overview: " + ex.Message, ex);
            }
        }
    }
}
