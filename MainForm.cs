using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using AccountingApp.Forms;
using AccountingApp.DataAccess;

namespace AccountingApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Database.Initialize();
            ShowOverview();
        }

        private void ShowOverview()
        {
            LoadTransactions();
            UpdateTotals();
        }

        private void LoadTransactions(int? customerId = null)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string sql = "SELECT t.Id, t.Type, t.Description, t.Amount, t.Date, c.Name AS CustomerName, c.OrgNumber AS CustomerOrg FROM Transactions t LEFT JOIN Customers c ON t.CustomerId = c.Id";
                if (customerId.HasValue)
                    sql += " WHERE t.CustomerId = @cid";
                sql += " ORDER BY t.Date DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    if (customerId.HasValue) cmd.Parameters.AddWithValue("@cid", customerId.Value);
                    var adapter = new SQLiteDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
        }

        private void LoadCustomers()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var adapter = new SQLiteDataAdapter("SELECT Id, Name, Company, ContactPerson, Email, Phone, OrgNumber, CreatedAt FROM Customers ORDER BY Name", conn);
                var table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void UpdateTotals()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand(
                    "SELECT " +
                    "COALESCE(SUM(CASE WHEN Type='Bill' THEN Amount ELSE 0 END),0) AS Bills, " +
                    "COALESCE(SUM(CASE WHEN Type='Payment' THEN Amount ELSE 0 END),0) AS Payments, " +
                    "COALESCE(SUM(CASE WHEN Type='Expense' THEN Amount ELSE 0 END),0) AS Expenses " +
                    "FROM Transactions", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        decimal bills = reader.IsDBNull(0) ?0 : Convert.ToDecimal(reader.GetDouble(0));
                        decimal payments = reader.IsDBNull(1) ?0 : Convert.ToDecimal(reader.GetDouble(1));
                        decimal expenses = reader.IsDBNull(2) ?0 : Convert.ToDecimal(reader.GetDouble(2));
                        lblBills.Text = $"Totale regninger: {bills:C}";
                        lblPayments.Text = $"Totale innbetalinger: {payments:C}";
                        lblExpenses.Text = $"Totale utgifter: {expenses:C}";
                        lblBalance.Text = $"Saldo: {(payments - expenses):C}";
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new AddTransactionForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                LoadTransactions();
                UpdateTotals();
            }
        }

        private void btnKunder_Click(object sender, EventArgs e)
        {
            var mgmt = new CustomersManagementForm();
            mgmt.ShowDialog();
            LoadTransactions();
            UpdateTotals();
        }

        private void btnRegninger_Click(object sender, EventArgs e)
        {
            // show Bills & Payments
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var adapter = new SQLiteDataAdapter("SELECT t.Id, t.Type, t.Description, t.Amount, t.Date, c.Name AS CustomerName, c.OrgNumber AS CustomerOrg FROM Transactions t LEFT JOIN Customers c ON t.CustomerId = c.Id WHERE t.Type IN ('Bill','Payment') ORDER BY t.Date DESC", conn);
                var table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void btnUtgifter_Click(object sender, EventArgs e)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var adapter = new SQLiteDataAdapter("SELECT t.Id, t.Type, t.Description, t.Amount, t.Date, c.Name AS CustomerName, c.OrgNumber AS CustomerOrg FROM Transactions t LEFT JOIN Customers c ON t.CustomerId = c.Id WHERE t.Type='Expense' ORDER BY t.Date DESC", conn);
                var table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void btnKvitteringer_Click(object sender, EventArgs e)
        {
            // receipts not implemented yet - placeholder
            var table = new DataTable();
            table.Columns.Add("Melding");
            table.Rows.Add("Ingen kvitteringer enda.");
            dataGridView1.DataSource = table;
        }

        private void btnRapporter_Click(object sender, EventArgs e)
        {
            // simple report: totals
            var table = new DataTable();
            table.Columns.Add("Rapport");
            table.Columns.Add("Verdi");
            table.Rows.Add("Totale regninger", lblBills.Text);
            table.Rows.Add("Totale innbetalinger", lblPayments.Text);
            table.Rows.Add("Totale utgifter", lblExpenses.Text);
            table.Rows.Add("Saldo", lblBalance.Text);
            dataGridView1.DataSource = table;
        }

        private void btnFilterCustomer_Click(object sender, EventArgs e)
        {
            // allow entering id, orgnr or name
            var txt = txtCustomerFilter.Text?.Trim();
            if (string.IsNullOrEmpty(txt)) { LoadTransactions(); return; }
            int id;
            if (int.TryParse(txt, out id))
            {
                LoadTransactions(id);
            }
            else
            {
                // search by orgnr or name
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand("SELECT Id FROM Customers WHERE OrgNumber LIKE @q OR Name LIKE @q LIMIT1", conn);
                    cmd.Parameters.AddWithValue("@q", "%" + txt + "%");
                    var obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        LoadTransactions(Convert.ToInt32(obj));
                    }
                    else
                    {
                        MessageBox.Show("Fant ingen kunde med det søket.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtCustomerFilter.Text = string.Empty;
            LoadTransactions();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            var f = new ProductManagementForm();
            f.ShowDialog();
        }

        private void btnInvoices_Click(object sender, EventArgs e)
        {
            var f = new InvoiceCreateForm();
            f.ShowDialog();
            LoadTransactions();
            UpdateTotals();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // For now, simple: if a single invoice selected in grid, print details via PrintDocument or export to text
            int id;
            if (!TryGetSelectedId(out id)) { MessageBox.Show("Velg en faktura i listen først."); return; }
            var repo = new InvoiceRepository();
            var inv = repo.GetInvoice(id);
            if (inv == null) { MessageBox.Show("Fant ikke fakturaen."); return; }
            // Simple export to text file
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"invoice_{inv.InvoiceNumber}.txt");
            using (var w = System.IO.File.CreateText(path))
            {
                w.WriteLine($"Faktura: {inv.InvoiceNumber}");
                w.WriteLine($"Dato: {inv.Date}");
                w.WriteLine($"Total: {inv.Total:C}");
                w.WriteLine("Linjer:");
                foreach (var l in inv.Lines)
                {
                    w.WriteLine($" - {l.ProductName} ({l.ProductNumber}) x{l.Quantity} {l.LineTotal:C}");
                }
            }
            MessageBox.Show($"Faktura eksportert til {path}");
        }

        // When double-click on grid and it contains an invoice, open detail/edit
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            int id;
            if (!TryGetSelectedId(out id)) return;

            // open invoice detail form
            var repo = new InvoiceRepository();
            var inv = repo.GetInvoice(id);
            if (inv != null)
            {
                var dlg = new InvoiceDetailForm(inv);
                dlg.ShowDialog();
                // allow edit? not yet; after closing refresh
                LoadTransactions();
                UpdateTotals();
            }
            else
            {
                // fallback to existing behavior
                // old details display
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    var cmd = new SQLiteCommand("SELECT t.Id, t.Type, t.Description, t.Amount, t.Date, t.CustomerId, c.Name, c.OrgNumber FROM Transactions t LEFT JOIN Customers c ON t.CustomerId=c.Id WHERE t.Id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string customerName = reader.IsDBNull(6) ? "-" : reader.GetString(6);
                            string customerOrg = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                            string details = $"Type: {reader.GetString(1)}\r\nBeskrivelse: {reader.GetString(2)}\r\nBeløp: {reader.GetDouble(3):C}\r\nDato: {reader.GetString(4)}\r\nKunde: {customerName} ({customerOrg})";
                            MessageBox.Show(details, "Transaksjon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        // Try to get an integer id from the selected row. First try a column named "Id", otherwise scan cells for an integer.
        private bool TryGetSelectedId(out int id)
        {
            id =0;
            if (dataGridView1.CurrentRow == null) return false;
            var row = dataGridView1.CurrentRow;
            // preferred column
            if (dataGridView1.Columns.Contains("Id"))
            {
                var val = row.Cells["Id"].Value;
                if (val != null && int.TryParse(val.ToString(), out id)) return true;
            }
            // fallback: scan all cells for first integer value
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.Value == null) continue;
                int v;
                if (int.TryParse(cell.Value.ToString(), out v))
                {
                    id = v;
                    return true;
                }
            }
            return false;
        }
    }
}
