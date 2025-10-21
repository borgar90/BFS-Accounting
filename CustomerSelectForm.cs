using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AccountingApp
{
 public class CustomerSelectForm : Form
 {
 private DataGridView grid;
 private TextBox txtSearch;
 private Button btnSearch;
 private Button btnOk;
 private Button btnCancel;
 public int? SelectedCustomerId { get; private set; }
 public string SelectedCustomerDisplay { get; private set; }

 public CustomerSelectForm()
 {
 InitializeComponent();
 LoadCustomers();
 }

 private void InitializeComponent()
 {
 this.Text = "Velg kunde";
 this.ClientSize = new System.Drawing.Size(600,400);
 txtSearch = new TextBox() { Left =10, Top =10, Width =350 };
 btnSearch = new Button() { Text = "Søk", Left =370, Top =8, Width =80 };
 grid = new DataGridView() { Left =10, Top =40, Width =560, Height =300, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AllowUserToAddRows = false };
 btnOk = new Button() { Text = "OK", Left =370, Top =350, Width =80 };
 btnCancel = new Button() { Text = "Avbryt", Left =460, Top =350, Width =80 };
 btnSearch.Click += BtnSearch_Click;
 btnOk.Click += BtnOk_Click;
 btnCancel.Click += (s,e) => this.DialogResult = DialogResult.Cancel;
 grid.DoubleClick += Grid_DoubleClick;
 this.Controls.Add(txtSearch);
 this.Controls.Add(btnSearch);
 this.Controls.Add(grid);
 this.Controls.Add(btnOk);
 this.Controls.Add(btnCancel);
 }

 private void LoadCustomers(string filter = null)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 string sql = "SELECT Id, Name, Company, OrgNumber FROM Customers";
 if (!string.IsNullOrWhiteSpace(filter))
 {
 sql += " WHERE Id LIKE @q OR OrgNumber LIKE @q OR Name LIKE @q";
 }
 sql += " ORDER BY Name";
 using (var cmd = new SQLiteCommand(sql, conn))
 {
 if (!string.IsNullOrWhiteSpace(filter)) cmd.Parameters.AddWithValue("@q", "%" + filter + "%");
 var adapter = new SQLiteDataAdapter(cmd);
 var table = new DataTable();
 adapter.Fill(table);
 grid.DataSource = table;
 }
 }
 }

 private void BtnSearch_Click(object sender, EventArgs e)
 {
 LoadCustomers(txtSearch.Text);
 }

 private void BtnOk_Click(object sender, EventArgs e)
 {
 SelectCurrent();
 }

 private void Grid_DoubleClick(object sender, EventArgs e)
 {
 SelectCurrent();
 }

 private void SelectCurrent()
 {
 if (grid.SelectedRows.Count ==0) return;
 var row = grid.SelectedRows[0];
 SelectedCustomerId = Convert.ToInt32(row.Cells["Id"].Value);
 SelectedCustomerDisplay = $"{row.Cells["Name"].Value} ({row.Cells["OrgNumber"].Value})";
 this.DialogResult = DialogResult.OK;
 }
 }
}
