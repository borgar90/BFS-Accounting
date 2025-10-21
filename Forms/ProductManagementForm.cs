using System;
using System.Linq;
using System.Windows.Forms;
using AccountingApp.Controller;
using AccountingApp.DTO;

namespace AccountingApp.Forms
{
 public class ProductManagementForm : Form
 {
 private DataGridView grid;
 private Button btnAdd, btnEdit, btnDelete;
 private ProductController controller = new ProductController();

 public ProductManagementForm()
 {
 InitializeComponent();
 LoadProducts();
 }

 private void InitializeComponent()
 {
 this.Text = "Produktkatalog";
 this.ClientSize = new System.Drawing.Size(700,400);
 grid = new DataGridView() { Left =10, Top =10, Width =680, Height =320, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
 btnAdd = new Button() { Left =10, Top =340, Width =100, Text = "Legg til" };
 btnEdit = new Button() { Left =120, Top =340, Width =100, Text = "Rediger" };
 btnDelete = new Button() { Left =230, Top =340, Width =100, Text = "Slett" };
 btnAdd.Click += (s, e) => AddProduct();
 btnEdit.Click += (s, e) => EditProduct();
 btnDelete.Click += (s, e) => DeleteProduct();
 this.Controls.Add(grid);
 this.Controls.Add(btnAdd);
 this.Controls.Add(btnEdit);
 this.Controls.Add(btnDelete);
 }

 private void LoadProducts()
 {
 var list = controller.GetAll().ToList();
 grid.DataSource = list;
 grid.AutoResizeColumns();
 }

 private void AddProduct()
 {
 var dlg = new ProductEditForm();
 if (dlg.ShowDialog() == DialogResult.OK)
 {
 controller.Add(dlg.Product);
 LoadProducts();
 }
 }

 private void EditProduct()
 {
 if (grid.CurrentRow == null) return;
 var dto = grid.CurrentRow.DataBoundItem as ProductDto;
 var dlg = new ProductEditForm(dto);
 if (dlg.ShowDialog() == DialogResult.OK)
 {
 controller.Update(dlg.Product);
 LoadProducts();
 }
 }

 private void DeleteProduct()
 {
 if (grid.CurrentRow == null) return;
 var dto = grid.CurrentRow.DataBoundItem as ProductDto;
 if (MessageBox.Show($"Slette produkt {dto.Name}?", "Bekreft", MessageBoxButtons.YesNo) == DialogResult.Yes)
 {
 controller.Delete(dto.Id);
 LoadProducts();
 }
 }
 }
}
