using System;
using System.Linq;
using System.Windows.Forms;
using AccountingApp.DTO;
using AccountingApp.Controller;

namespace AccountingApp.Forms
{
 public class InvoiceDetailForm : Form
 {
 private DTO.InvoiceDto invoice;
 private Label lblHeader;
 private DataGridView grid;
 private DataGridView gridPayments;
 private Button btnClose;
 private Button btnRegisterPayment;
 private InvoiceController controller = new InvoiceController();

 public InvoiceDetailForm(DTO.InvoiceDto inv)
 {
 invoice = inv;
 InitializeComponent();
 LoadInvoice();
 LoadPayments();
 UpdateStatus();
 }

 private void InitializeComponent()
 {
 this.Text = "Faktura detaljer";
 this.ClientSize = new System.Drawing.Size(700,600);
 lblHeader = new Label() { Left =10, Top =10, Width =660 };
 grid = new DataGridView() { Left =10, Top =40, Width =660, Height =250, ReadOnly = true };
 gridPayments = new DataGridView() { Left =10, Top =300, Width =660, Height =180, ReadOnly = true };
 btnRegisterPayment = new Button() { Left =10, Top =490, Text = "Registrer innbetaling", Width =160 };
 btnClose = new Button() { Left =580, Top =520, Text = "Lukk", Width =80 };
 btnRegisterPayment.Click += BtnRegisterPayment_Click;
 btnClose.Click += (s, e) => this.Close();
 this.Controls.Add(lblHeader);
 this.Controls.Add(grid);
 this.Controls.Add(gridPayments);
 this.Controls.Add(btnRegisterPayment);
 this.Controls.Add(btnClose);
 }

 private void LoadInvoice()
 {
 lblHeader.Text = $"Faktura {invoice.InvoiceNumber} - Dato: {invoice.Date.ToShortDateString()} - Total: {invoice.Total:C}";
 grid.DataSource = invoice.Lines.Select(l => new { l.ProductNumber, l.ProductName, l.Description, l.Quantity, UnitPrice = l.UnitPrice, LineTotal = l.LineTotal }).ToList();
 grid.AutoResizeColumns();
 }

 private void LoadPayments()
 {
 var payments = controller.GetPayments(invoice.Id).ToList();
 gridPayments.DataSource = payments.Select(p => new { p.Date, p.Amount, p.Note }).ToList();
 gridPayments.AutoResizeColumns();
 }

 private void UpdateStatus()
 {
 var payments = controller.GetPayments(invoice.Id).Sum(p => p.Amount);
 var outstanding = invoice.Total - payments;
 if (outstanding <=0)
 {
 // paid or overpaid
 if (outstanding ==0)
 {
 this.BackColor = System.Drawing.Color.LightGreen;
 lblHeader.Text += " - Betalt";
 }
 else
 {
 this.BackColor = System.Drawing.Color.LightCoral;
 lblHeader.Text += $" - Overbetalt ({-outstanding:C}) - Sjekk og returner differansen";
 }
 }
 else
 {
 this.BackColor = System.Drawing.Color.LightYellow;
 lblHeader.Text += $" - Utestående: {outstanding:C}";
 }
 }

 private void BtnRegisterPayment_Click(object sender, EventArgs e)
 {
 using (var dlg = new RegisterPaymentForm(invoice.Total))
 {
 if (dlg.ShowDialog() == DialogResult.OK)
 {
 var p = dlg.Payment;
 p.InvoiceId = invoice.Id;
 controller.RegisterPayment(p);
 LoadPayments();
 UpdateStatus();
 }
 }
 }
 }
}
