using System;

namespace AccountingApp.DTO
{
 public class InvoicePaymentDto
 {
 public int Id { get; set; }
 public int InvoiceId { get; set; }
 public decimal Amount { get; set; }
 public DateTime Date { get; set; }
 public string Note { get; set; }
 }
}