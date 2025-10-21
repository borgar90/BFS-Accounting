using System;
using System.Collections.Generic;
using System.Data.SQLite;
using AccountingApp.DTO;

namespace AccountingApp.DataAccess
{
 public class PaymentRepository
 {
 public int AddPayment(InvoicePaymentDto p)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("INSERT INTO InvoicePayments (InvoiceId, Amount, Date, Note) VALUES (@inv,@amt,@date,@note); SELECT last_insert_rowid();", conn))
 {
 cmd.Parameters.AddWithValue("@inv", p.InvoiceId);
 cmd.Parameters.AddWithValue("@amt", (double)p.Amount);
 cmd.Parameters.AddWithValue("@date", p.Date.ToString("yyyy-MM-dd HH:mm:ss"));
 cmd.Parameters.AddWithValue("@note", p.Note ?? string.Empty);
 var obj = cmd.ExecuteScalar();
 return Convert.ToInt32(obj);
 }
 }
 }

 public IEnumerable<InvoicePaymentDto> GetPaymentsForInvoice(int invoiceId)
 {
 var list = new List<InvoicePaymentDto>();
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 using (var cmd = new SQLiteCommand("SELECT Id, InvoiceId, Amount, Date, Note FROM InvoicePayments WHERE InvoiceId=@inv ORDER BY Date", conn))
 {
 cmd.Parameters.AddWithValue("@inv", invoiceId);
 using (var reader = cmd.ExecuteReader())
 {
 while (reader.Read())
 {
 list.Add(new InvoicePaymentDto
 {
 Id = reader.GetInt32(0),
 InvoiceId = reader.GetInt32(1),
 Amount = Convert.ToDecimal(reader.GetDouble(2)),
 Date = DateTime.Parse(reader.GetString(3)),
 Note = reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
 });
 }
 }
 }
 }
 return list;
 }
 }
}