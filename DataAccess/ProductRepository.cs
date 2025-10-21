using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using AccountingApp.DTO;

namespace AccountingApp.DataAccess
{
 public class ProductRepository
 {
 public IEnumerable<ProductDto> GetAll()
 {
 var list = new List<ProductDto>();
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("SELECT Id, ProductNumber, Name, Price, IsCustom FROM Products ORDER BY Name", conn))
 using (var reader = cmd.ExecuteReader())
 {
 while (reader.Read())
 {
 list.Add(new ProductDto
 {
 Id = reader.GetInt32(0),
 ProductNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
 Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
 Price = Convert.ToDecimal(reader.GetDouble(3)),
 IsCustom = reader.GetInt32(4) !=0
 });
 }
 }
 }
 return list;
 }

 public ProductDto GetById(int id)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("SELECT Id, ProductNumber, Name, Price, IsCustom FROM Products WHERE Id=@id", conn))
 {
 cmd.Parameters.AddWithValue("@id", id);
 using (var reader = cmd.ExecuteReader())
 {
 if (reader.Read())
 {
 return new ProductDto
 {
 Id = reader.GetInt32(0),
 ProductNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
 Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
 Price = Convert.ToDecimal(reader.GetDouble(3)),
 IsCustom = reader.GetInt32(4) !=0
 };
 }
 }
 }
 }
 return null;
 }

 public int Add(ProductDto p)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("INSERT INTO Products (ProductNumber, Name, Price, IsCustom) VALUES (@pn,@name,@price,@isCustom); SELECT last_insert_rowid();", conn))
 {
 cmd.Parameters.AddWithValue("@pn", p.ProductNumber ?? string.Empty);
 cmd.Parameters.AddWithValue("@name", p.Name ?? string.Empty);
 cmd.Parameters.AddWithValue("@price", (double)p.Price);
 cmd.Parameters.AddWithValue("@isCustom", p.IsCustom ?1 :0);
 var obj = cmd.ExecuteScalar();
 return Convert.ToInt32(obj);
 }
 }
 }

 public void Update(ProductDto p)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("UPDATE Products SET ProductNumber=@pn, Name=@name, Price=@price, IsCustom=@isCustom WHERE Id=@id", conn))
 {
 cmd.Parameters.AddWithValue("@pn", p.ProductNumber ?? string.Empty);
 cmd.Parameters.AddWithValue("@name", p.Name ?? string.Empty);
 cmd.Parameters.AddWithValue("@price", (double)p.Price);
 cmd.Parameters.AddWithValue("@isCustom", p.IsCustom ?1 :0);
 cmd.Parameters.AddWithValue("@id", p.Id);
 cmd.ExecuteNonQuery();
 }
 }
 }

 public void Delete(int id)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("DELETE FROM Products WHERE Id=@id", conn))
 {
 cmd.Parameters.AddWithValue("@id", id);
 cmd.ExecuteNonQuery();
 }
 }
 }
 }
}
