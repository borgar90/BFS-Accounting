namespace AccountingApp.DTO
{
 public class InvoiceLineDto
 {
 public int Id { get; set; }
 public int InvoiceId { get; set; }
 public int ProductId { get; set; }
 public string Description { get; set; }
 public int Quantity { get; set; }
 public decimal UnitPrice { get; set; }
 public decimal LineTotal { get; set; }
 // optional product info
 public string ProductNumber { get; set; }
 public string ProductName { get; set; }
 }
}