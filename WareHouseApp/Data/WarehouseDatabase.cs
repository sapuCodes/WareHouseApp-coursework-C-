using System;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace WareHouseApp.Data
{
    public static class WarehouseDatabase
    {
        public static void Initialize()
        {
            string dataDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "WareHouseApp");
            Directory.CreateDirectory(dataDirectory);

            string databasePath = Path.Combine(dataDirectory, "WarehouseDB.mdf");
            if (!File.Exists(databasePath))
            {
                string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WarehouseDB.mdf");
                if (!File.Exists(sourcePath))
                    throw new FileNotFoundException("The initial WarehouseDB.mdf file could not be found.", sourcePath);

                File.Copy(sourcePath, databasePath);

                string sourceLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WarehouseDB_log.ldf");
                if (File.Exists(sourceLogPath))
                    File.Copy(sourceLogPath, Path.Combine(dataDirectory, "WarehouseDB_log.ldf"));
            }

            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
        }

        public static SqlConnection CreateConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["WarehouseDb"];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new InvalidOperationException("The WarehouseDb connection string is missing from App.config.");

            return new SqlConnection(settings.ConnectionString);
        }
    }
}
