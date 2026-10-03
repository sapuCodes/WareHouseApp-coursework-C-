using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public class CustomerForm : Form
    {
        private readonly Customer customerRepository = new Customer();
        private readonly DataGridView customersGrid = new DataGridView();
        private readonly TextBox nameTextBox = new TextBox();
        private readonly TextBox emailTextBox = new TextBox();
        private readonly TextBox phoneTextBox = new TextBox();
        private int? selectedCustomerId;

        public CustomerForm()
        {
            BuildLayout();
            AppLocalization.Apply(this);
            Load += CustomerForm_Load;
        }

        private void BuildLayout()
        {
            Text = "Customer Management";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(800, 500);
            Size = new Size(950, 600);

            Panel inputPanel = new Panel { Dock = DockStyle.Top, Height = 164, Padding = new Padding(12) };
            nameTextBox.MaxLength = 50;
            emailTextBox.MaxLength = 50;
            phoneTextBox.MaxLength = 20;
            TableLayoutPanel fieldsPanel = CreateFieldsPanel(3);
            AddField(fieldsPanel, "Name", nameTextBox, 0);
            AddField(fieldsPanel, "Email", emailTextBox, 1);
            AddField(fieldsPanel, "Phone", phoneTextBox, 2);

            FlowLayoutPanel actionsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            actionsPanel.Controls.Add(CreateButton("Add", AddCustomer_Click));
            actionsPanel.Controls.Add(CreateButton("Update", UpdateCustomer_Click));
            actionsPanel.Controls.Add(CreateButton("Delete", DeleteCustomer_Click));
            actionsPanel.Controls.Add(CreateButton("Clear", ClearFields_Click));
            actionsPanel.Controls.Add(CreateButton("Refresh", RefreshCustomers_Click));

            TableLayoutPanel inputLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                RowCount = 2
            };
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            inputLayout.Controls.Add(fieldsPanel, 0, 0);
            inputLayout.Controls.Add(actionsPanel, 0, 1);
            inputPanel.Controls.Add(inputLayout);

            customersGrid.Dock = DockStyle.Fill;
            customersGrid.ReadOnly = true;
            customersGrid.AllowUserToAddRows = false;
            customersGrid.AllowUserToDeleteRows = false;
            customersGrid.MultiSelect = false;
            customersGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            customersGrid.SelectionChanged += CustomersGrid_SelectionChanged;

            Controls.Add(customersGrid);
            Controls.Add(inputPanel);
        }

        private static TableLayoutPanel CreateFieldsPanel(int columns)
        {
            TableLayoutPanel fieldsPanel = new TableLayoutPanel
            {
                ColumnCount = columns,
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                RowCount = 2
            };
            for (int i = 0; i < columns; i++)
                fieldsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / columns));
            fieldsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            fieldsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            return fieldsPanel;
        }

        private static void AddField(TableLayoutPanel parent, string labelText, TextBox textBox, int column)
        {
            Label label = new Label
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 0, 4, 0),
                Text = labelText,
                TextAlign = ContentAlignment.BottomLeft
            };
            textBox.Dock = DockStyle.Fill;
            textBox.Margin = new Padding(4, 3, 4, 4);
            parent.Controls.Add(label, column, 0);
            parent.Controls.Add(textBox, column, 1);
        }

        private static Button CreateButton(string text, EventHandler clickHandler)
        {
            Button button = new Button
            {
                AutoSize = true,
                Margin = new Padding(4),
                MinimumSize = new Size(76, 34),
                Padding = new Padding(8, 2, 8, 2),
                Text = text
            };
            button.Click += clickHandler;
            return button;
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                DataTable customers = customerRepository.GetAllCustomers();
                customersGrid.DataSource = customers;
                customersGrid.ClearSelection();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Customer error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddCustomer_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                if (customerRepository.AddCustomer(nameTextBox.Text.Trim(), emailTextBox.Text.Trim(), phoneTextBox.Text.Trim()))
                {
                    AppLocalization.ShowMessage(this, "Customer added successfully.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Customer error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCustomer_Click(object sender, EventArgs e)
        {
            if (!selectedCustomerId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select a customer to update.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateFields())
                return;

            try
            {
                if (customerRepository.UpdateCustomer(selectedCustomerId.Value, nameTextBox.Text.Trim(), emailTextBox.Text.Trim(), phoneTextBox.Text.Trim()))
                {
                    AppLocalization.ShowMessage(this, "Customer updated successfully.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Customer error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteCustomer_Click(object sender, EventArgs e)
        {
            if (!selectedCustomerId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select a customer to delete.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AppLocalization.ShowMessage(this, "Delete the selected customer?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                if (customerRepository.DeleteCustomer(selectedCustomerId.Value))
                {
                    AppLocalization.ShowMessage(this, "Customer deleted successfully.", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadCustomers();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Customer error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshCustomers_Click(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void ClearFields_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            selectedCustomerId = null;
            nameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            customersGrid.ClearSelection();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || nameTextBox.Text.Trim().Length > 50)
            {
                AppLocalization.ShowMessage(this, "Enter a customer name of 1 to 50 characters.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return false;
            }
            return true;
        }

        private void CustomersGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (customersGrid.SelectedRows.Count == 0 || customersGrid.SelectedRows[0].IsNewRow)
                return;

            DataGridViewRow selectedRow = customersGrid.SelectedRows[0];
            object id = selectedRow.Cells["CustomerId"].Value;
            if (id == null || id == DBNull.Value)
                return;

            selectedCustomerId = Convert.ToInt32(id);
            nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
            emailTextBox.Text = Convert.ToString(selectedRow.Cells["Email"].Value);
            phoneTextBox.Text = Convert.ToString(selectedRow.Cells["Phone"].Value);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomerForm
            // 
            this.ClientSize = new System.Drawing.Size(506, 382);
            this.Name = "CustomerForm";
            this.ResumeLayout(false);

        }
    }
}
