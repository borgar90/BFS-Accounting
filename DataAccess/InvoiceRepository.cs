using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using AccountingApp.DTO;

namespace AccountingApp.DataAccess
{
 public class InvoiceRepository
 {
 public int AddInvoice(InvoiceDto invoice)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var tran = conn.BeginTransaction())
 {
 using (var cmd = new SQLiteCommand("INSERT INTO Invoices (InvoiceNumber, CustomerId, Date, Total) VALUES (@num,@cid,@date,@total); SELECT last_insert_rowid();", conn, tran))
 {
 cmd.Parameters.AddWithValue("@num", invoice.InvoiceNumber ?? GenerateInvoiceNumber(conn));
 cmd.Parameters.AddWithValue("@cid", invoice.CustomerId.HasValue ? (object)invoice.CustomerId.Value : DBNull.Value);
 cmd.Parameters.AddWithValue("@date", invoice.Date.ToString("yyyy-MM-dd HH:mm:ss"));
 cmd.Parameters.AddWithValue("@total", (double)invoice.Total);
 var id = Convert.ToInt32(cmd.ExecuteScalar());
 // insert lines
 foreach (var line in invoice.Lines)
 {
 using (var lcmd = new SQLiteCommand("INSERT INTO InvoiceLines (InvoiceId, ProductId, Description, Quantity, UnitPrice, LineTotal) VALUES (@inv,@pid,@desc,@qty,@up,@lt)", conn, tran))
 {
 lcmd.Parameters.AddWithValue("@inv", id);
 lcmd.Parameters.AddWithValue("@pid", line.ProductId);
 lcmd.Parameters.AddWithValue("@desc", line.Description ?? string.Empty);
 lcmd.Parameters.AddWithValue("@qty", line.Quantity);
 lcmd.Parameters.AddWithValue("@up", (double)line.UnitPrice);
 lcmd.Parameters.AddWithValue("@lt", (double)line.LineTotal);
 lcmd.ExecuteNonQuery();
 }
 }
 tran.Commit();
 return id;
 }
 }
 }
 }

 private string GenerateInvoiceNumber(SQLiteConnection conn)
 {
 // Very simple: INV-YYYYMMDD-<next>
 var datePart = DateTime.Now.ToString("yyyyMMdd");
 using (var cmd = new SQLiteCommand("SELECT COUNT(1) FROM Invoices WHERE Date LIKE @d||'%'", conn))
 {
 cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("yyyy-MM-dd"));
 var count = Convert.ToInt32(cmd.ExecuteScalar());
 return $"INV-{datePart}-{count +1}";
 }
 }

 public InvoiceDto GetInvoice(int id)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("SELECT Id, InvoiceNumber, CustomerId, Date, Total FROM Invoices WHERE Id=@id", conn))
 {
 cmd.Parameters.AddWithValue("@id", id);
 using (var reader = cmd.ExecuteReader())
 {
 if (reader.Read())
 {
 var inv = new InvoiceDto
 {
 Id = reader.GetInt32(0),
 InvoiceNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
 CustomerId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
 Date = DateTime.Parse(reader.GetString(3)),
 Total = Convert.ToDecimal(reader.GetDouble(4))
 };
 // load lines
 using (var lcmd = new SQLiteCommand("SELECT il.Id, il.InvoiceId, il.ProductId, il.Description, il.Quantity, il.UnitPrice, il.LineTotal, p.ProductNumber, p.Name FROM InvoiceLines il LEFT JOIN Products p ON il.ProductId=p.Id WHERE il.InvoiceId=@inv", conn))
 {
 lcmd.Parameters.AddWithValue("@inv", inv.Id);
 using (var lreader = lcmd.ExecuteReader())
 {
 while (lreader.Read())
 {
 inv.Lines.Add(new InvoiceLineDto
 {
 Id = lreader.GetInt32(0),
 InvoiceId = lreader.GetInt32(1),
 ProductId = lreader.GetInt32(2),
 Description = lreader.IsDBNull(3) ? string.Empty : lreader.GetString(3),
 Quantity = lreader.GetInt32(4),
 UnitPrice = Convert.ToDecimal(lreader.GetDouble(5)),
 LineTotal = Convert.ToDecimal(lreader.GetDouble(6)),
 ProductNumber = lreader.IsDBNull(7) ? string.Empty : lreader.GetString(7),
 ProductName = lreader.IsDBNull(8) ? string.Empty : lreader.GetString(8)
 });
 }
 }
 }
 return inv;
 }
 }
 }
 }
 return null;
 }
 }
}