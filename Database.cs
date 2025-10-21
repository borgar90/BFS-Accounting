using System;
using System.Data.SQLite;
using System.IO;

namespace AccountingApp
{
    public static class Database
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounting.db");
        private static string connectionString = $"Data Source={dbPath};Version=3;";

        public static void Initialize()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Ensure Transactions table exists (add CustomerId)
                string sql = @"CREATE TABLE IF NOT EXISTS Transactions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Type TEXT NOT NULL,
                    Description TEXT,
                    Amount REAL NOT NULL,
                    Date TEXT NOT NULL,
                    CustomerId INTEGER
                );";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Ensure Customers table exists
                string customerSql = @"CREATE TABLE IF NOT EXISTS Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Company TEXT,
                    ContactPerson TEXT,
                    Email TEXT,
                    Phone TEXT,
                    OrgNumber TEXT,
                    CreatedAt TEXT NOT NULL
                );";
                using (var cmd = new SQLiteCommand(customerSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Ensure Products table exists (product catalog)
                string productSql = @"CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductNumber TEXT UNIQUE,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    IsCustom INTEGER DEFAULT 0
                );";
                using (var cmd = new SQLiteCommand(productSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Ensure Invoices table exists
                string invoiceSql = @"CREATE TABLE IF NOT EXISTS Invoices (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    InvoiceNumber TEXT UNIQUE,
                    CustomerId INTEGER,
                    Date TEXT NOT NULL,
                    Total REAL,
                    FOREIGN KEY(CustomerId) REFERENCES Customers(Id)
                );";
                using (var cmd = new SQLiteCommand(invoiceSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Ensure InvoiceLines table exists
                string lineSql = @"CREATE TABLE IF NOT EXISTS InvoiceLines (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    InvoiceId INTEGER NOT NULL,
                    ProductId INTEGER NOT NULL,
                    Description TEXT,
                    Quantity INTEGER NOT NULL,
                    UnitPrice REAL NOT NULL,
                    LineTotal REAL NOT NULL,
                    FOREIGN KEY(InvoiceId) REFERENCES Invoices(Id),
                    FOREIGN KEY(ProductId) REFERENCES Products(Id)
                );";
                using (var cmd = new SQLiteCommand(lineSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Ensure InvoicePayments table exists
                string paymentSql = @"CREATE TABLE IF NOT EXISTS InvoicePayments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    InvoiceId INTEGER NOT NULL,
                    Amount REAL NOT NULL,
                    Date TEXT NOT NULL,
                    Note TEXT,
                    FOREIGN KEY(InvoiceId) REFERENCES Invoices(Id)
                );";
                using (var cmd = new SQLiteCommand(paymentSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Company settings table (one row)
                string companySql = @"CREATE TABLE IF NOT EXISTS Company (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT,
                    OrgNumber TEXT,
                    Address TEXT,
                    Email TEXT,
                    Phone TEXT
                );";
                using (var cmd = new SQLiteCommand(companySql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Insert dummy company data if none
                using (var cmd = new SQLiteCommand("SELECT COUNT(1) FROM Company", conn))
                {
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        using (var ins = new SQLiteCommand("INSERT INTO Company (Id, Name, OrgNumber, Address, Email, Phone) VALUES (1, @name, @org, @addr, @email, @phone)", conn))
                        {
                            ins.Parameters.AddWithValue("@name", "Demo Firma AS");
                            ins.Parameters.AddWithValue("@org", "912345678");
                            ins.Parameters.AddWithValue("@addr", "Demo Gate 1, 0123 Oslo");
                            ins.Parameters.AddWithValue("@email", "post@demofirma.no");
                            ins.Parameters.AddWithValue("@phone", "+4712345678");
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // Insert sample product if none
                using (var cmd = new SQLiteCommand("SELECT COUNT(1) FROM Products", conn))
                {
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        using (var ins = new SQLiteCommand("INSERT INTO Products (ProductNumber, Name, Price, IsCustom) VALUES (@pn, @name, @price, 0)", conn))
                        {
                            ins.Parameters.AddWithValue("@pn", "P-1000");
                            ins.Parameters.AddWithValue("@name", "Demo produkt");
                            ins.Parameters.AddWithValue("@price", 199.50);
                            ins.ExecuteNonQuery();
                        }
                    }
                }

                // For older DBs: add CustomerId column if missing in Transactions
                using (var pragma = new SQLiteCommand("PRAGMA table_info('Transactions')", conn))
                using (var reader = pragma.ExecuteReader())
                {
                    bool hasCustomerId = false;
                    while (reader.Read())
                    {
                        var colName = reader[1]?.ToString();
                        if (string.Equals(colName, "CustomerId", StringComparison.OrdinalIgnoreCase))
                        {
                            hasCustomerId = true;
                            break;
                        }
                    }

                    if (!hasCustomerId)
                    {
                        using (var add = new SQLiteCommand("ALTER TABLE Transactions ADD COLUMN CustomerId INTEGER", conn))
                        {
                            add.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
    }
}
