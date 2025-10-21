using System;

namespace AccountingApp.DTO
{
 public class TransactionDto
 {
 public int Id { get; set; }
 public string Type { get; set; }
 public string Description { get; set; }
 public decimal Amount { get; set; }
 // Stored in DB as text (yyyy-MM-dd HH:mm:ss) but expose as DateTime for consumers
 public DateTime Date { get; set; }
 public int? CustomerId { get; set; }

 // Optional related customer info populated by queries that join Customers
 public string CustomerName { get; set; }
 public string CustomerOrgNumber { get; set; }
 }
}