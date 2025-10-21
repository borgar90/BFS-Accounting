using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AccountingApp
{
    public partial class AddTransactionForm : Form
    {
        private int? selectedCustomerId;

        public AddTransactionForm()
        {
            InitializeComponent();
        }

        private void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            using (var sel = new CustomerSelectForm())
            {
                if (sel.ShowDialog() == DialogResult.OK)
                {
                    selectedCustomerId = sel.SelectedCustomerId;
                    txtCustomer.Text = sel.SelectedCustomerDisplay;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string type = cmbType.SelectedItem?.ToString();
            string desc = txtDescription.Text;
            decimal amount;
            if (!decimal.TryParse(txtAmount.Text, out amount) || amount <= 0)
            {
                MessageBox.Show("Oppgi et gyldig beløp.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("Velg en transaksjonstype.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Enforce customer selection for bills and payments
            if ((type == "Bill" || type == "Payment") && !selectedCustomerId.HasValue)
            {
                MessageBox.Show("Velg en kunde for regninger og innbetalinger.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new SQLiteCommand("INSERT INTO Transactions (Type, Description, Amount, Date, CustomerId) VALUES (@type, @desc, @amount, @date, @customer)", conn);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (selectedCustomerId.HasValue)
                    cmd.Parameters.AddWithValue("@customer", selectedCustomerId.Value);
                else
                    cmd.Parameters.AddWithValue("@customer", DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
