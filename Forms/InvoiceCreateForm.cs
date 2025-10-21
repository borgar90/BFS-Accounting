using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AccountingApp.DTO;
using AccountingApp.Controller;

namespace AccountingApp.Forms
{
 public class InvoiceCreateForm : Form
 {
 private ComboBox cmbCustomer;
 private DataGridView gridLines;
 private Button btnAddLine, btnRemoveLine, btnSave, btnSelectCustomer;
 private ProductController productController = new ProductController();
 private InvoiceController invoiceController = new InvoiceController();
 private List<ProductDto> products;

 public InvoiceCreateForm()
 {
 InitializeComponent();
 products = productController.GetAll().ToList();
 LoadCustomersToCombo();
 }

 private void InitializeComponent()
 {
 this.Text = "Opprett faktura";
 this.ClientSize = new System.Drawing.Size(800,600);
 cmbCustomer = new ComboBox() { Left =10, Top =10, Width =500, DropDownStyle = ComboBoxStyle.DropDown };
 btnSelectCustomer = new Button() { Left =520, Top =8, Width =100, Text = "Velg" };
 btnSelectCustomer.Click += BtnSelectCustomer_Click;
 gridLines = new DataGridView() { Left =10, Top =50, Width =760, Height =440, AllowUserToAddRows = false };
 btnAddLine = new Button() { Left =10, Top =500, Width =120, Text = "Legg til linje" };
 btnRemoveLine = new Button() { Left =140, Top =500, Width =120, Text = "Fjern linje" };
 btnSave = new Button() { Left =660, Top =540, Width =110, Text = "Lagre faktura" };
 btnAddLine.Click += BtnAddLine_Click;
 btnRemoveLine.Click += BtnRemoveLine_Click;
 btnSave.Click += BtnSave_Click;
 this.Controls.Add(cmbCustomer);
 this.Controls.Add(btnSelectCustomer);
 this.Controls.Add(gridLines);
 this.Controls.Add(btnAddLine);
 this.Controls.Add(btnRemoveLine);
 this.Controls.Add(btnSave);
 SetupGrid();
 }

 private void SetupGrid()
 {
 gridLines.Columns.Add("ProductId", "ProduktId");
 gridLines.Columns.Add("ProductNumber", "Produktnr");
 gridLines.Columns.Add("ProductName", "Navn");
 gridLines.Columns.Add("Description", "Beskrivelse");
 gridLines.Columns.Add("Quantity", "Antall");
 gridLines.Columns.Add("UnitPrice", "Enhetspris");
 gridLines.Columns.Add("LineTotal", "Sum");
 gridLines.Columns["ProductId"].Visible = false;
 }

 private void LoadCustomersToCombo()
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 var adapter = new System.Data.SQLite.SQLiteDataAdapter("SELECT Id, Name, OrgNumber FROM Customers ORDER BY Name", conn);
 var table = new System.Data.DataTable();
 adapter.Fill(table);
 cmbCustomer.DisplayMember = "Name";
 cmbCustomer.ValueMember = "Id";
 cmbCustomer.DataSource = table;
 }
 }

 private void BtnSelectCustomer_Click(object sender, EventArgs e)
 {
 using (var sel = new CustomerSelectForm())
 {
 if (sel.ShowDialog() == DialogResult.OK)
 {
 cmbCustomer.SelectedValue = sel.SelectedCustomerId;
 }
 }
 }

 private void BtnAddLine_Click(object sender, EventArgs e)
 {
 var choose = new ProductSelectDialog(products);
 if (choose.ShowDialog() == DialogResult.OK)
 {
 var prod = choose.SelectedProduct;
 // add row
 int idx = gridLines.Rows.Add();
 var r = gridLines.Rows[idx];
 r.Cells["ProductId"].Value = prod.Id;
 r.Cells["ProductNumber"].Value = prod.ProductNumber;
 r.Cells["ProductName"].Value = prod.Name;
 r.Cells["Description"].Value = prod.Name;
 r.Cells["Quantity"].Value =1;
 r.Cells["UnitPrice"].Value = prod.Price;
 r.Cells["LineTotal"].Value = prod.Price;
 }
 }

 private void BtnRemoveLine_Click(object sender, EventArgs e)
 {
 if (gridLines.CurrentRow != null) gridLines.Rows.Remove(gridLines.CurrentRow);
 }

 private void BtnSave_Click(object sender, EventArgs e)
 {
 if (cmbCustomer.SelectedValue == null) { MessageBox.Show("Velg kunde"); return; }
 var inv = new InvoiceDto();
 inv.InvoiceNumber = null; // generated
 inv.CustomerId = Convert.ToInt32(cmbCustomer.SelectedValue);
 inv.Date = DateTime.Now;
 inv.Total =0;
 foreach (DataGridViewRow r in gridLines.Rows)
 {
 int pid = Convert.ToInt32(r.Cells["ProductId"].Value);
 var qty = Convert.ToInt32(r.Cells["Quantity"].Value);
 var up = Convert.ToDecimal(r.Cells["UnitPrice"].Value);
 var lt = qty * up;
 inv.Lines.Add(new InvoiceLineDto { ProductId = pid, Description = r.Cells["Description"].Value?.ToString(), Quantity = qty, UnitPrice = up, LineTotal = lt });
 inv.Total += lt;
 }
 // validation
 if (inv.Lines.Count ==0) { MessageBox.Show("Legg til minst en linje"); return; }
 var id = invoiceController.CreateInvoice(inv);
 MessageBox.Show($"Faktura lagret med id {id}");
 this.DialogResult = DialogResult.OK;
 this.Close();
 }
 }
}
