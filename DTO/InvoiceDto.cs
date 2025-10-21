using System;
using System.Collections.Generic;

namespace AccountingApp.DTO
{
 public class InvoiceDto
 {
 public int Id { get; set; }
 public string InvoiceNumber { get; set; }
 public int? CustomerId { get; set; }
 public DateTime Date { get; set; }
 public decimal Total { get; set; }
 public List<InvoiceLineDto> Lines { get; set; } = new List<InvoiceLineDto>();
 }
}
