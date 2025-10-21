using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace AccountingApp
{
 public partial class AddCustomerForm : Form
 {
 private TextBox txtName;
 private TextBox txtCompany;
 private TextBox txtContact;
 private TextBox txtEmail;
 private TextBox txtPhone;
 private TextBox txtOrg;
 private Button btnSave;
 private int? customerId;

 public AddCustomerForm(int? id = null)
 {
 customerId = id;
 InitializeComponent();
 if (customerId.HasValue)
 LoadCustomer(customerId.Value);
 }

 private void InitializeComponent()
 {
 this.Text = customerId.HasValue ? "Rediger kunde" : "Legg til kunde"; // Norwegian
 this.ClientSize = new System.Drawing.Size(420,260);
 var lblName = new Label() { Text = "Navn", Left =10, Top =10 };
 txtName = new TextBox() { Name = "txtName", Left =150, Top =10, Width =240 };
 var lblCompany = new Label() { Text = "Firma", Left =10, Top =40 };
 txtCompany = new TextBox() { Name = "txtCompany", Left =150, Top =40, Width =240 };
 var lblContact = new Label() { Text = "Kontaktperson", Left =10, Top =70 };
 txtContact = new TextBox() { Name = "txtContact", Left =150, Top =70, Width =240 };
 var lblEmail = new Label() { Text = "E-post", Left =10, Top =100 };
 txtEmail = new TextBox() { Name = "txtEmail", Left =150, Top =100, Width =240 };
 var lblPhone = new Label() { Text = "Telefon", Left =10, Top =130 };
 txtPhone = new TextBox() { Name = "txtPhone", Left =150, Top =130, Width =240 };
 var lblOrg = new Label() { Text = "Organisasjonsnummer", Left =10, Top =160 };
 txtOrg = new TextBox() { Name = "txtOrg", Left =150, Top =160, Width =240 };
 btnSave = new Button() { Text = "Lagre", Left =150, Top =200, Width =80 };
 btnSave.Click += BtnSave_Click;

 this.Controls.Add(lblName);
 this.Controls.Add(txtName);
 this.Controls.Add(lblCompany);
 this.Controls.Add(txtCompany);
 this.Controls.Add(lblContact);
 this.Controls.Add(txtContact);
 this.Controls.Add(lblEmail);
 this.Controls.Add(txtEmail);
 this.Controls.Add(lblPhone);
 this.Controls.Add(txtPhone);
 this.Controls.Add(lblOrg);
 this.Controls.Add(txtOrg);
 this.Controls.Add(btnSave);
 }

 private void LoadCustomer(int id)
 {
 using (var conn = Database.GetConnection())
 {
 conn.Open();
 var cmd = new SQLiteCommand("SELECT Name, Company, ContactPerson, Email, Phone, OrgNumber FROM Customers WHERE Id = @id", conn);
 cmd.Parameters.AddWithValue("@id", id);
 using (var reader = cmd.ExecuteReader())
 {
 if (reader.Read())
 {
 txtName.Text = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
 txtCompany.Text = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
 txtContact.Text = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
 txtEmail.Text = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
 txtPhone.Text = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
 txtOrg.Text = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
 }
 }
 }
 }

 private void BtnSave_Click(object sender, EventArgs e)
 {
 string name = txtName.Text?.Trim();
 string company = txtCompany.Text?.Trim();
 string contact = txtContact.Text?.Trim();
 string email = txtEmail.Text?.Trim();
 string phone = txtPhone.Text?.Trim();
 string org = txtOrg.Text?.Trim();

 if (string.IsNullOrEmpty(name))
 {
 MessageBox.Show("Oppgi kundens navn.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }

 // Normalize OrgNumber (remove spaces/non-digits)
 string normalizedOrg = Validators.NormalizeOrgNumber(org);

 // Validate organization number if provided
 if (!string.IsNullOrEmpty(normalizedOrg) && !Validators.IsValidOrgNumber(normalizedOrg))
 {
 MessageBox.Show("Ugyldig organisasjonsnummer. Det må være 9 siffer og ha et gyldig kontrollsiffer.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }

 // Validate email if provided
 if (!string.IsNullOrEmpty(email) && !Validators.IsValidEmail(email))
 {
 MessageBox.Show("Ugyldig e-postadresse.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }

 // Validate phone if provided
 if (!string.IsNullOrEmpty(phone) && !Validators.IsValidPhone(phone))
 {
 MessageBox.Show("Ugyldig telefonnummer.", "Feil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 return;
 }

 using (var conn = Database.GetConnection())
 {
 conn.Open();
 if (customerId.HasValue)
 {
 // Update existing
 var cmd = new SQLiteCommand("UPDATE Customers SET Name=@name, Company=@company, ContactPerson=@contact, Email=@email, Phone=@phone, OrgNumber=@org WHERE Id=@id", conn);
 cmd.Parameters.AddWithValue("@name", name);
 cmd.Parameters.AddWithValue("@company", company ?? string.Empty);
 cmd.Parameters.AddWithValue("@contact", contact ?? string.Empty);
 cmd.Parameters.AddWithValue("@email", email ?? string.Empty);
 cmd.Parameters.AddWithValue("@phone", phone ?? string.Empty);
 cmd.Parameters.AddWithValue("@org", normalizedOrg ?? string.Empty);
 cmd.Parameters.AddWithValue("@id", customerId.Value);
 cmd.ExecuteNonQuery();
 }
 else
 {
 var cmd = new SQLiteCommand("INSERT INTO Customers (Name, Company, ContactPerson, Email, Phone, OrgNumber, CreatedAt) VALUES (@name, @company, @contact, @email, @phone, @org, @created)", conn);
 cmd.Parameters.AddWithValue("@name", name);
 cmd.Parameters.AddWithValue("@company", company ?? string.Empty);
 cmd.Parameters.AddWithValue("@contact", contact ?? string.Empty);
 cmd.Parameters.AddWithValue("@email", email ?? string.Empty);
 cmd.Parameters.AddWithValue("@phone", phone ?? string.Empty);
 cmd.Parameters.AddWithValue("@org", normalizedOrg ?? string.Empty);
 cmd.Parameters.AddWithValue("@created", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
 cmd.ExecuteNonQuery();
 }
 }

 this.DialogResult = DialogResult.OK;
 this.Close();
 }
 }
}
