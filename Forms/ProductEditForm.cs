using System;
using System.Windows.Forms;
using AccountingApp.DTO;

namespace AccountingApp.Forms
{
 public class ProductEditForm : Form
 {
 public ProductDto Product { get; private set; }
 private TextBox txtNumber, txtName, txtPrice;
 private CheckBox chkCustom;

 public ProductEditForm(ProductDto existing = null)
 {
 Product = existing ?? new ProductDto();
 InitializeComponent();
 if (existing != null)
 {
 txtNumber.Text = Product.ProductNumber;
 txtName.Text = Product.Name;
 txtPrice.Text = Product.Price.ToString();
 chkCustom.Checked = Product.IsCustom;
 }
 }

 private void InitializeComponent()
 {
 this.Text = "Produkt";
 this.ClientSize = new System.Drawing.Size(400,200);
 var lblNum = new Label() { Text = "Produktnummer", Left =10, Top =10 };
 txtNumber = new TextBox() { Left =150, Top =10, Width =220 };
 var lblName = new Label() { Text = "Navn", Left =10, Top =40 };
 txtName = new TextBox() { Left =150, Top =40, Width =220 };
 var lblPrice = new Label() { Text = "Pris", Left =10, Top =70 };
 txtPrice = new TextBox() { Left =150, Top =70, Width =100 };
 chkCustom = new CheckBox() { Text = "Custom produkt", Left =150, Top =100 };
 var btnSave = new Button() { Text = "Lagre", Left =150, Top =130 };
 btnSave.Click += BtnSave_Click;
 this.Controls.Add(lblNum);
 this.Controls.Add(txtNumber);
 this.Controls.Add(lblName);
 this.Controls.Add(txtName);
 this.Controls.Add(lblPrice);
 this.Controls.Add(txtPrice);
 this.Controls.Add(chkCustom);
 this.Controls.Add(btnSave);
 }

 private void BtnSave_Click(object sender, EventArgs e)
 {
 decimal price;
 if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("Navn må oppgis"); return; }
 if (!decimal.TryParse(txtPrice.Text, out price) || price <0) { MessageBox.Show("Ugyldig pris"); return; }
 Product.ProductNumber = txtNumber.Text?.Trim();
 Product.Name = txtName.Text.Trim();
 Product.Price = price;
 Product.IsCustom = chkCustom.Checked;
 this.DialogResult = DialogResult.OK;
 this.Close();
 }
 }
}
