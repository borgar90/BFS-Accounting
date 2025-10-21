using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AccountingApp
{
 public partial class CustomersManagementForm : Form
 {
 private DataGridView grid;
 private Button btnAdd;
 private Button btnEdit;
 private Button btnDelete;
 private TextBox txtSearch;
 private Button btnSearch;
 private Button btnClear;

 public CustomersManagementForm()
 {
 InitializeComponent();
 LoadCustomers();
 }

 private void InitializeComponent()
 {
 this.Text = "Kundeadministrasjon";
 this.ClientSize = new System.Drawing.Size(800,450);
 txtSearch = new TextBox() { Left =10, Top =10, Width =400 };
 btnSearch = new Button() { Text = "Søk", Left =420, Top =8, Width =80 };
 btnClear = new Button() { Text = "Tøm", Left =506, Top =8, Width =80 };
 grid = new DataGridView() { Left =10, Top =40, Width =760, Height =330, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false };
 btnAdd = new Button() { Text = "Legg til", Left =10, Top =380, Width =100 };
 btnEdit = new Button() { Text = "Rediger", Left =120, Top =380, Width =100 };
 btnDelete = new Button() { Text = "Slett", Left =230, Top =380, Width =100 };
 btnAdd.Click += BtnAdd_Click;
 btnEdit.Click += BtnEdit_Click;
 btnDelete.Click += BtnDelete_Click;
 btnSearch.Click += BtnSearch_Click;
 btnClear.Click += BtnClear_Click;
 txtSearch.KeyDown += TxtSearch_KeyDown;
 this.Controls.Add(txtSearch);
 this.Controls.Add(btnSearch);
 this.Controls.Add(btnClear);
 this.Controls.Add(grid);
 this.Controls.Add(btnAdd);
 this.Controls.Add(btnEdit);
 this.Controls.Add(btnDelete);
 }

 private void LoadCustomers(string filter = null)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 string sql = "SELECT Id, Name, Company, ContactPerson, Email, Phone, OrgNumber, CreatedAt FROM Customers";
 if (!string.IsNullOrWhiteSpace(filter))
 {
 sql += " WHERE Name LIKE @q OR Company LIKE @q OR ContactPerson LIKE @q OR Email LIKE @q OR Phone LIKE @q OR OrgNumber LIKE @q";
 }
 sql += " ORDER BY Name";
 using (var cmd = new SQLiteCommand(sql, conn))
 {
 if (!string.IsNullOrWhiteSpace(filter))
 {
 string q = "%" + filter + "%";
 cmd.Parameters.AddWithValue("@q", q);
 }
 var adapter = new SQLiteDataAdapter(cmd);
 var table = new DataTable();
 adapter.Fill(table);
 grid.DataSource = table;
 }
 }
 }

 private int? GetSelectedCustomerId()
 {
 if (grid.SelectedRows.Count ==0) return null;
 var row = grid.SelectedRows[0];
 if (row.Cells["Id"].Value == null) return null;
 return Convert.ToInt32(row.Cells["Id"].Value);
 }

 private void BtnAdd_Click(object sender, EventArgs e)
 {
 var form = new AddCustomerForm();
 if (form.ShowDialog() == DialogResult.OK)
 {
 LoadCustomers();
 }
 }

 private void BtnEdit_Click(object sender, EventArgs e)
 {
 var id = GetSelectedCustomerId();
 if (!id.HasValue)
 {
 MessageBox.Show("Velg en kunde først.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }
 var form = new AddCustomerForm(id.Value);
 if (form.ShowDialog() == DialogResult.OK)
 {
 LoadCustomers();
 }
 }

 private void BtnDelete_Click(object sender, EventArgs e)
 {
 var id = GetSelectedCustomerId();
 if (!id.HasValue)
 {
 MessageBox.Show("Velg en kunde først.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }
 if (MessageBox.Show("Er du sikker på at du vil slette denne kunden?", "Bekreft", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 var cmd = new SQLiteCommand("DELETE FROM Customers WHERE Id=@id", conn);
 cmd.Parameters.AddWithValue("@id", id.Value);
 cmd.ExecuteNonQuery();
 }
 LoadCustomers();
 }
 }
 
 private void BtnSearch_Click(object sender, EventArgs e)
 {
 LoadCustomers(txtSearch.Text);
 }
 
 private void BtnClear_Click(object sender, EventArgs e)
 {
 txtSearch.Text = string.Empty;
 LoadCustomers();
 }
 
 private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
 {
 if (e.KeyCode == Keys.Enter)
 {
 e.Handled = true;
 e.SuppressKeyPress = true;
 LoadCustomers(txtSearch.Text);
 }
 }
 }
}
