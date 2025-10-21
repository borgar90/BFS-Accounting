using System.Collections.Generic;
using AccountingApp.Model;
using AccountingApp.DTO;

namespace AccountingApp.Controller
{
 public class InvoiceController
 {
 private InvoiceModel model = new InvoiceModel();
 public int CreateInvoice(InvoiceDto inv) => model.CreateInvoice(inv);
 public InvoiceDto GetInvoice(int id) => model.GetInvoice(id);
 public IEnumerable<InvoicePaymentDto> GetPayments(int invoiceId) => model.GetPayments(invoiceId);
 public int RegisterPayment(InvoicePaymentDto p) => model.RegisterPayment(p);
 }
}