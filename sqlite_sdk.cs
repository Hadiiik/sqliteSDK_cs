using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
namespace sqlLiteClassApi
{
    public class Table
    {
        public Table(string tableName) 
        { 
            this.tableName = tableName;
        }
        public string tableName;

        // Function to create a table with specified columns and types
        public void CreateTable( string[,] columns)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Construct the SQL command for creating the table
                StringBuilder sql = new StringBuilder();
                sql.Append($"CREATE TABLE IF NOT EXISTS {tableName} (");
                sql.Append("id INTEGER PRIMARY KEY AUTOINCREMENT, "); // Adding primary key column

                for (int i = 0; i < columns.GetLength(0); i++)
                {
                    string columnName = columns[i, 0];
                    string columnType = columns[i, 1];
                    sql.Append($"{columnName} {columnType}");
                    if (i < columns.GetLength(0) - 1)
                    {
                        sql.Append(", ");
                    }
                }

                sql.Append(");");

                // Execute the SQL command
                using (SQLiteCommand command = new SQLiteCommand(sql.ToString(), connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // Function to insert data into a specified table
        public void InsertRow( string[] columns, object[] values)
        {
            if (columns.Length != values.Length)
            {
                throw new ArgumentException("The number of columns must match the number of values.");
            }

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Construct the SQL command for inserting data
                StringBuilder sql = new StringBuilder();
                sql.Append($"INSERT INTO {tableName} (");

                for (int i = 0; i < columns.Length; i++)
                {
                    sql.Append(columns[i]);

                    if (i < columns.Length - 1)
                    {
                        sql.Append(", ");
                    }
                }

                sql.Append(") VALUES (");

                for (int i = 0; i < values.Length; i++)
                {
                    sql.Append($"@value{i}");

                    if (i < values.Length - 1)
                    {
                        sql.Append(", ");
                    }
                }

                sql.Append(");");

                // Prepare the command
                using (SQLiteCommand command = new SQLiteCommand(sql.ToString(), connection))
                {
                    // Add parameters to prevent SQL injection
                    for (int i = 0; i < values.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@value{i}", values[i]);
                    }

                    // Execute the command
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        //insert rows 
        public void InsertRows(string[] columns, object[,] valuesArray)
        {
            // Iterate over the rows of the 2D array
            for (int i = 0; i < valuesArray.GetLength(0); i++)
            {
                // Create an array to hold the current row values
                object[] rowValues = new object[valuesArray.GetLength(1)];

                // Copy each value from the 2D array row to the rowValues array
                for (int j = 0; j < valuesArray.GetLength(1); j++)
                {
                    rowValues[j] = valuesArray[i, j];
                }

                // Insert the current row into the table
                this.InsertRow(columns, rowValues);
            }
        }
        //update 

        public void UpdateRow( string[] columns, object[] values, string condition="1=1")
        {
            if (columns.Length != values.Length)
            {
                throw new ArgumentException("The number of columns must match the number of values.");
            }

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Construct the SQL command for updating data
                StringBuilder sql = new StringBuilder();
                sql.Append($"UPDATE {tableName} SET ");

                for (int i = 0; i < columns.Length; i++)
                {
                    sql.Append($"{columns[i]} = @value{i}");

                    if (i < columns.Length - 1)
                    {
                        sql.Append(", ");
                    }
                }

                sql.Append($" WHERE {condition};");

                // Prepare the command
                using (SQLiteCommand command = new SQLiteCommand(sql.ToString(), connection))
                {
                    // Add parameters to prevent SQL injection
                    for (int i = 0; i < values.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@value{i}", values[i]);
                    }

                    // Execute the command
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // update value in row
        public void UpdateValue(string column, object value, string condition="1=1")
        {
            
            
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Construct the SQL command for updating data
                StringBuilder sql = new StringBuilder();
                sql.Append($"UPDATE {tableName} SET ");

                sql.Append($"{column} = @value");

                sql.Append($" WHERE {condition};");

                // Prepare the command
                using (SQLiteCommand command = new SQLiteCommand(sql.ToString(), connection))
                {
                    command.Parameters.AddWithValue($"@value", value);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        //serach tabel

        public List<List<object>> SearchTable( string[] columns = null, string condition = "1=1")
        {
            // List to store the results, where each row is a List of objects
            List<List<object>> result = new List<List<object>>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Build SQL query for searching data
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT ");

                // Check columns: if not specified, select all columns
                if (columns == null || columns.Length == 0)
                {
                    sql.Append("*");
                }
                else
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        sql.Append(columns[i]);

                        if (i < columns.Length - 1)
                        {
                            sql.Append(", ");
                        }
                    }
                }

                sql.Append($" FROM {tableName}");

                // Add the condition if provided, otherwise return all rows
                if (!string.IsNullOrEmpty(condition))
                {
                    sql.Append($" WHERE {condition}");
                }

                sql.Append(";");

                // Execute the SQL command
                using (SQLiteCommand command = new SQLiteCommand(sql.ToString(), connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Read each row that matches the condition or all rows if no condition is provided
                    while (reader.Read())
                    {
                        List<object> row = new List<object>();

                        // Populate the list with values from the database
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetValue(i));
                        }

                        // Add the row to the result list
                        result.Add(row);
                    }
                }

                connection.Close();
            }

            return result;
        }
        //list all coulmns info
        public List<List<string>> GetColumnInfo()
        {
            // Define a list to store column names and types
            List<List<string>> columnsInfo = new List<List<string>>();

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Query to get the column information for the given table
                string sql = $"PRAGMA table_info({tableName});";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Read each column info row
                    while (reader.Read())
                    {
                        // Column name and type
                        string columnName = reader["name"].ToString();
                        string columnType = reader["type"].ToString();

                        // Create a list for the column info and add it to the main list
                        List<string> columnDetails = new List<string> { columnName, columnType };
                        columnsInfo.Add(columnDetails);
                    }
                }

                connection.Close();
            }

            return columnsInfo;
        }
        // Function to print the contents of the table
        public void PrintTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {
                connection.Open();

                // Construct the SQL command to select all data from the table
                string sql = $"SELECT * FROM {tableName};";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    // Print column names
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetName(i) + "\t");
                    }
                    Console.WriteLine();

                    // Print each row
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write(reader.GetValue(i) + "\t");
                        }
                        Console.WriteLine();
                    }
                }

                connection.Close();
            }
        }
    }

    
}
