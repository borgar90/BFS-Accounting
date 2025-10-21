using System;
using System.Collections.Generic;
using System.Linq;
using AccountingApp.DataAccess;
using CoreDto = AccountingApp.DTO;

namespace AccountingApp.Model
{
 public class InvoiceModel
 {
 private InvoiceRepository repo = new InvoiceRepository();
 private PaymentRepository paymentRepo = new PaymentRepository();

 public int CreateInvoice(CoreDto.InvoiceDto inv)
 {
 var dto = new CoreDto.InvoiceDto
 {
 InvoiceNumber = inv.InvoiceNumber,
 CustomerId = inv.CustomerId,
 Date = inv.Date,
 Total = inv.Total,
 };
 foreach (var l in inv.Lines)
 {
 dto.Lines.Add(new CoreDto.InvoiceLineDto { ProductId = l.ProductId, Description = l.Description, Quantity = l.Quantity, UnitPrice = l.UnitPrice, LineTotal = l.LineTotal });
 }
 return repo.AddInvoice(dto);
 }

 public CoreDto.InvoiceDto GetInvoice(int id)
 {
 var dto = repo.GetInvoice(id);
 if (dto == null) return null;
 var model = new CoreDto.InvoiceDto { Id = dto.Id, InvoiceNumber = dto.InvoiceNumber, CustomerId = dto.CustomerId, Date = dto.Date, Total = dto.Total };
 foreach (var l in dto.Lines)
 {
 model.Lines.Add(new CoreDto.InvoiceLineDto { Id = l.Id, InvoiceId = l.InvoiceId, ProductId = l.ProductId, Description = l.Description, Quantity = l.Quantity, UnitPrice = l.UnitPrice, LineTotal = l.LineTotal, ProductName = l.ProductName, ProductNumber = l.ProductNumber });
 }
 return model;
 }

 public IEnumerable<CoreDto.InvoicePaymentDto> GetPayments(int invoiceId)
 {
 var list = new List<CoreDto.InvoicePaymentDto>();
 foreach (var p in paymentRepo.GetPaymentsForInvoice(invoiceId))
 {
 list.Add(new CoreDto.InvoicePaymentDto { Id = p.Id, InvoiceId = p.InvoiceId, Amount = p.Amount, Date = p.Date, Note = p.Note });
 }
 return list;
 }

 public int RegisterPayment(CoreDto.InvoicePaymentDto p)
 {
 var dto = new CoreDto.InvoicePaymentDto { InvoiceId = p.InvoiceId, Amount = p.Amount, Date = p.Date, Note = p.Note };
 return paymentRepo.AddPayment(dto);
 }
 }
}