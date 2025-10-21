using System.Windows.Forms;

namespace AccountingApp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridView1;
        private Button btnAdd;
        private Button btnKunder;
        private Button btnRegninger;
        private Button btnUtgifter;
        private Button btnKvitteringer;
        private Button btnRapporter;
        private TextBox txtCustomerFilter;
        private Button btnFilterCustomer;
        private Button btnClearFilter;
        private Button btnProducts;
        private Button btnInvoices;
        private Button btnPrint;
        private Label lblBills;
        private Label lblPayments;
        private Label lblExpenses;
        private Label lblBalance;

        private void InitializeComponent()
        {
            this.dataGridView1 = new DataGridView();
            this.btnAdd = new Button();
            this.btnKunder = new Button();
            this.btnRegninger = new Button();
            this.btnUtgifter = new Button();
            this.btnKvitteringer = new Button();
            this.btnRapporter = new Button();
            this.txtCustomerFilter = new TextBox();
            this.btnFilterCustomer = new Button();
            this.btnClearFilter = new Button();
            this.btnProducts = new Button();
            this.btnInvoices = new Button();
            this.btnPrint = new Button();
            this.lblBills = new Label();
            this.lblPayments = new Label();
            this.lblExpenses = new Label();
            this.lblBalance = new Label();
            this.SuspendLayout();
            // Toolbar buttons (single row)
            int x = 10;
            int y = 10;
            int spacing = 8;
            int btnW = 110;
            int btnH = 32;
            // btnProducts
            this.btnProducts.Location = new System.Drawing.Point(x, y);
            this.btnProducts.Size = new System.Drawing.Size(btnW, btnH);
            this.btnProducts.Text = "Produkter";
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            x += btnW + spacing;
            // btnInvoices
            this.btnInvoices.Location = new System.Drawing.Point(x, y);
            this.btnInvoices.Size = new System.Drawing.Size(btnW, btnH);
            this.btnInvoices.Text = "Fakturaer";
            this.btnInvoices.Click += new System.EventHandler(this.btnInvoices_Click);
            x += btnW + spacing;
            // btnKunder
            this.btnKunder.Location = new System.Drawing.Point(x, y);
            this.btnKunder.Size = new System.Drawing.Size(btnW, btnH);
            this.btnKunder.Text = "Kunder";
            this.btnKunder.Click += new System.EventHandler(this.btnKunder_Click);
            x += btnW + spacing;
            // btnRegninger
            this.btnRegninger.Location = new System.Drawing.Point(x, y);
            this.btnRegninger.Size = new System.Drawing.Size(btnW + 40, btnH); // wider for text
            this.btnRegninger.Text = "Regninger & Betalinger";
            this.btnRegninger.Click += new System.EventHandler(this.btnRegninger_Click);
            x += (btnW + 40) + spacing;
            // btnUtgifter
            this.btnUtgifter.Location = new System.Drawing.Point(x, y);
            this.btnUtgifter.Size = new System.Drawing.Size(btnW, btnH);
            this.btnUtgifter.Text = "Utgifter";
            this.btnUtgifter.Click += new System.EventHandler(this.btnUtgifter_Click);
            x += btnW + spacing;
            // btnKvitteringer
            this.btnKvitteringer.Location = new System.Drawing.Point(x, y);
            this.btnKvitteringer.Size = new System.Drawing.Size(btnW, btnH);
            this.btnKvitteringer.Text = "Kvitteringer";
            this.btnKvitteringer.Click += new System.EventHandler(this.btnKvitteringer_Click);
            x += btnW + spacing;
            // btnRapporter
            this.btnRapporter.Location = new System.Drawing.Point(x, y);
            this.btnRapporter.Size = new System.Drawing.Size(btnW, btnH);
            this.btnRapporter.Text = "Rapporter";
            this.btnRapporter.Click += new System.EventHandler(this.btnRapporter_Click);
            x += btnW + spacing;
            // btnPrint
            this.btnPrint.Location = new System.Drawing.Point(x, y);
            this.btnPrint.Size = new System.Drawing.Size(btnW, btnH);
            this.btnPrint.Text = "Skriv ut";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            // dataGridView1 - moved down below toolbar
            this.dataGridView1.Location = new System.Drawing.Point(10, 52);
            this.dataGridView1.Size = new System.Drawing.Size(760, 380);
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);

            // btnAdd (bottom left)
            this.btnAdd.Location = new System.Drawing.Point(10, 444);
            this.btnAdd.Size = new System.Drawing.Size(160, 32);
            this.btnAdd.Text = "Legg til transaksjon";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // txtCustomerFilter and filter buttons (bottom area)
            this.txtCustomerFilter.Location = new System.Drawing.Point(180, 448);
            this.txtCustomerFilter.Size = new System.Drawing.Size(220, 22);
            this.btnFilterCustomer.Location = new System.Drawing.Point(410, 444);
            this.btnFilterCustomer.Size = new System.Drawing.Size(110, 28);
            this.btnFilterCustomer.Text = "Filter kunde";
            this.btnFilterCustomer.Click += new System.EventHandler(this.btnFilterCustomer_Click);
            this.btnClearFilter.Location = new System.Drawing.Point(526, 444);
            this.btnClearFilter.Size = new System.Drawing.Size(80, 28);
            this.btnClearFilter.Text = "Tøm filter";
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);

            // Totals labels (bottom right)
            this.lblBills.Location = new System.Drawing.Point(10, 486);
            this.lblBills.Size = new System.Drawing.Size(220, 24);
            this.lblPayments.Location = new System.Drawing.Point(240, 486);
            this.lblPayments.Size = new System.Drawing.Size(220, 24);
            this.lblExpenses.Location = new System.Drawing.Point(470, 486);
            this.lblExpenses.Size = new System.Drawing.Size(220, 24);
            this.lblBalance.Location = new System.Drawing.Point(10, 516);
            this.lblBalance.Size = new System.Drawing.Size(680, 24);

            // MainForm
            this.ClientSize = new System.Drawing.Size(784, 560);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnAdd);
            // toolbar buttons
            this.Controls.Add(this.btnProducts);
            this.Controls.Add(this.btnInvoices);
            this.Controls.Add(this.btnKunder);
            this.Controls.Add(this.btnRegninger);
            this.Controls.Add(this.btnUtgifter);
            this.Controls.Add(this.btnKvitteringer);
            this.Controls.Add(this.btnRapporter);
            this.Controls.Add(this.btnPrint);
            // bottom controls
            this.Controls.Add(this.txtCustomerFilter);
            this.Controls.Add(this.btnFilterCustomer);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.lblBills);
            this.Controls.Add(this.lblPayments);
            this.Controls.Add(this.lblExpenses);
            this.Controls.Add(this.lblBalance);
            this.Text = "Regnskap - Oversikt";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
