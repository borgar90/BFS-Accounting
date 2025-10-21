using System;
using System.Windows.Forms;
using AccountingApp.DTO;

namespace AccountingApp.Forms
{
 public class RegisterPaymentForm : Form
 {
 public InvoicePaymentDto Payment { get; private set; }
 private NumericUpDown numAmount;
 private DateTimePicker dtDate;
 private TextBox txtNote;
 private decimal invoiceTotal;

 public RegisterPaymentForm(decimal invoiceTotal)
 {
 this.invoiceTotal = invoiceTotal;
 InitializeComponent();
 }

 private void InitializeComponent()
 {
 this.Text = "Registrer innbetaling";
 this.ClientSize = new System.Drawing.Size(400,200);
 var lblAmount = new Label() { Text = "Beløp", Left =10, Top =10 };
 numAmount = new NumericUpDown() { Left =120, Top =10, Width =120, DecimalPlaces =2, Maximum =100000000, Minimum =0 };
 var lblDate = new Label() { Text = "Dato", Left =10, Top =40 };
 dtDate = new DateTimePicker() { Left =120, Top =40, Width =200 }; 
 var lblNote = new Label() { Text = "Merknad", Left =10, Top =70 };
 txtNote = new TextBox() { Left =120, Top =70, Width =240 };
 var btnSave = new Button() { Text = "Registrer", Left =120, Top =110 };
 btnSave.Click += BtnSave_Click;
 this.Controls.Add(lblAmount);
 this.Controls.Add(numAmount);
 this.Controls.Add(lblDate);
 this.Controls.Add(dtDate);
 this.Controls.Add(lblNote);
 this.Controls.Add(txtNote);
 this.Controls.Add(btnSave);
 }

 private void BtnSave_Click(object sender, EventArgs e)
 {
 if (numAmount.Value <=0) { MessageBox.Show("Beløp må være større enn0"); return; }
 Payment = new InvoicePaymentDto { Amount = numAmount.Value, Date = dtDate.Value, Note = txtNote.Text };
 this.DialogResult = DialogResult.OK;
 this.Close();
 }
 }
}
