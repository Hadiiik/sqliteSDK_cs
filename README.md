# SQLite Class API

This project contains a Table class that provides methods to create, modify, insert, and query data from a SQLite database.
Make sure to install sqllite core pakage

## `Table` Class

### Constructor
```csharp
public Table(string tableName)
```
- *Parameters*:
  - `tableName`: The name of the table to be created or interacted with.

### Methods
* CreateTable
* InsertRow
* InsertRows
* UpdateRow
* UpdateValue
* PrintTable
---
#### `CreateTable`
```csharp
public void CreateTable(string[,] columns)
```
- *Parameters*:
  - columns: A 2D array containing column names and their types.

- *Description*:
  Creates a new table in the database if it does not already exist, with the specified columns.

#### `InsertRow`
```csharp
public void InsertRow(string[] columns, object[] values)
```
- *Parameters*:
  - columns: An array containing column names.
  - values: An array containing the corresponding values for each column.

- *Description*:
  Inserts a new row into the table.

#### `InsertRows`
```csharp
public void InsertRows(string[] columns, object[,] valuesArray)
```
- *Parameters*:
  - columns: An array containing column names.
  - valuesArray: A 2D array containing values to be inserted.

- *Description*:
  Inserts multiple rows into the table based on the provided values.

#### `UpdateRow`
```csharp
public void UpdateRow(string[] columns, object[] values, string condition="1=1")
```
- *Parameters*:
  - columns: An array containing column names.
  - values: An array containing new values.
  - condition: A condition to specify which rows to update.

- *Description*:
  Updates the values in table rows that match the specified condition.

#### `UpdateValue`
```csharp
public void UpdateValue(string column, object value, string condition="1=1")
```
- *Parameters*:
  - column: The name of the column to be updated.
  - value: The new value.
  - condition: A condition to specify which rows to update.

- *Description*:
  Updates a specific column's value in table rows that match the specified condition.

#### `SearchTable`
```csharp
public List<List<object>> SearchTable(string[] columns = null, string condition = "1=1")
```
- *Parameters*:
  - columns: An array of column names to be selected (or null to select all columns).
  - condition: A condition to specify which rows to retrieve.

- *Description*:
  Retrieves data from the table based on the specified columns and condition.

#### `PrintTable`
```csharp
public void PrintTable()
```
- *Description*:
  Prints the contents of the table to the console.

## Example Usage

Here's an example of how to use the Table class:

```csharp
Table employees = new Table("employees");

// Create a new table
employees.CreateTable(new string[,] {
    { "name", "TEXT" },
    { "age", "INTEGER" }
});

// Insert new rows
employees.InsertRow(new string[] { "name", "age" }, new object[] { "John Doe", 30 });
employees.InsertRows(new string[] { "name", "age" }, new object[,] {
    { "Jane Doe", 25 },
    { "Mike Smith", 40 }
});

// Update rows
employees.UpdateRow(new string[] { "age" }, new object[] { 35 }, "name = 'John Doe'");

// Search the table
List<List<object>> employeesList = employees.SearchTable(condition: "age > 30");

// Print the table
employees.PrintTable();
```
