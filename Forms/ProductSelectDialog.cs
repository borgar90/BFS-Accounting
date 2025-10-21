using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AccountingApp.DTO;

namespace AccountingApp.Forms
{
 public class ProductSelectDialog : Form
 {
 private DataGridView grid;
 private Button btnOk, btnCancel;
 public ProductDto SelectedProduct { get; private set; }
 public ProductSelectDialog(List<ProductDto> products)
 {
 InitializeComponent();
 grid.DataSource = products;
 }
 private void InitializeComponent()
 {
 this.Text = "Velg produkt";
 this.ClientSize = new System.Drawing.Size(600,400);
 grid = new DataGridView() { Left =10, Top =10, Width =580, Height =320, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
 btnOk = new Button() { Left =400, Top =340, Width =80, Text = "OK" };
 btnCancel = new Button() { Left =490, Top =340, Width =80, Text = "Avbryt" };
 btnOk.Click += BtnOk_Click;
 btnCancel.Click += (s,e) => this.DialogResult = DialogResult.Cancel;
 this.Controls.Add(grid);
 this.Controls.Add(btnOk);
 this.Controls.Add(btnCancel);
 }
 private void BtnOk_Click(object sender, EventArgs e)
 {
 if (grid.CurrentRow == null) return;
 SelectedProduct = grid.CurrentRow.DataBoundItem as ProductDto;
 this.DialogResult = DialogResult.OK;
 }
 }
}
