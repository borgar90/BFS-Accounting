using System.Windows.Forms;

namespace AccountingApp
{
    partial class AddTransactionForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbType;
        private TextBox txtDescription;
        private TextBox txtAmount;
        private TextBox txtCustomer;
        private Button btnSelectCustomer;
        private Button btnSave;
        private Label lblType;
        private Label lblDescription;
        private Label lblAmount;

        private void InitializeComponent()
        {
            this.cmbType = new ComboBox();
            this.txtDescription = new TextBox();
            this.txtCustomer = new TextBox();
            this.btnSelectCustomer = new Button();
            this.txtAmount = new TextBox();
            this.btnSave = new Button();
            this.lblType = new Label();
            this.lblDescription = new Label();
            this.lblAmount = new Label();
            this.SuspendLayout();
            // cmbType
            this.cmbType.Items.AddRange(new object[] { "Bill", "Payment", "Expense" });
            this.cmbType.Location = new System.Drawing.Point(120, 20);
            this.cmbType.Size = new System.Drawing.Size(150, 21);
            // lblType
            this.lblType.Text = "Type:";
            this.lblType.Location = new System.Drawing.Point(20, 20);
            this.lblType.Size = new System.Drawing.Size(100, 21);
            // txtCustomer
            this.txtCustomer.Location = new System.Drawing.Point(120, 50);
            this.txtCustomer.Size = new System.Drawing.Size(120, 21);
            this.txtCustomer.ReadOnly = true;
            // btnSelectCustomer
            this.btnSelectCustomer.Text = "Velg kunde";
            this.btnSelectCustomer.Location = new System.Drawing.Point(250, 48);
            this.btnSelectCustomer.Size = new System.Drawing.Size(80, 24);
            this.btnSelectCustomer.Click += new System.EventHandler(this.btnSelectCustomer_Click);
            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(120, 90);
            this.txtDescription.Size = new System.Drawing.Size(150, 21);
            // lblDescription
            this.lblDescription.Text = "Description:";
            this.lblDescription.Location = new System.Drawing.Point(20, 90);
            this.lblDescription.Size = new System.Drawing.Size(100, 21);
            // txtAmount
            this.txtAmount.Location = new System.Drawing.Point(120, 130);
            this.txtAmount.Size = new System.Drawing.Size(150, 21);
            // lblAmount
            this.lblAmount.Text = "Amount:";
            this.lblAmount.Location = new System.Drawing.Point(20, 130);
            this.lblAmount.Size = new System.Drawing.Size(100, 21);
            // btnSave
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(120, 170);
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // AddTransactionForm
            this.ClientSize = new System.Drawing.Size(360, 220);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.txtCustomer);
            this.Controls.Add(this.btnSelectCustomer);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblAmount);
            this.Text = "Add Transaction";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
