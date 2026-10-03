using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public class EmployeeForm : Form
    {
        private readonly Employee employeeRepository = new Employee();
        private readonly DataGridView employeesGrid = new DataGridView();
        private readonly TextBox nameTextBox = new TextBox();
        private readonly TextBox positionTextBox = new TextBox();
        private readonly TextBox salaryTextBox = new TextBox();
        private readonly TextBox personIdTextBox = new TextBox();
        private int? selectedEmployeeId;

        public EmployeeForm()
        {
            BuildLayout();
            AppLocalization.Apply(this);
            Load += EmployeeForm_Load;
        }

        private void BuildLayout()
        {
            Text = "Employee Management";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(850, 500);
            Size = new Size(1000, 600);

            Panel inputPanel = new Panel { Dock = DockStyle.Top, Height = 164, Padding = new Padding(12) };
            nameTextBox.MaxLength = 50;
            positionTextBox.MaxLength = 50;
            personIdTextBox.MaxLength = 10;
            TableLayoutPanel fieldsPanel = CreateFieldsPanel(4);
            AddField(fieldsPanel, "Name", nameTextBox, 0);
            AddField(fieldsPanel, "Position", positionTextBox, 1);
            AddField(fieldsPanel, "Salary", salaryTextBox, 2);
            AddField(fieldsPanel, "Person EmpId", personIdTextBox, 3);

            FlowLayoutPanel actionsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            actionsPanel.Controls.Add(CreateButton("Add", AddEmployee_Click));
            actionsPanel.Controls.Add(CreateButton("Update", UpdateEmployee_Click));
            actionsPanel.Controls.Add(CreateButton("Delete", DeleteEmployee_Click));
            actionsPanel.Controls.Add(CreateButton("Clear", ClearFields_Click));
            actionsPanel.Controls.Add(CreateButton("Refresh", RefreshEmployees_Click));

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

            employeesGrid.Dock = DockStyle.Fill;
            employeesGrid.ReadOnly = true;
            employeesGrid.AllowUserToAddRows = false;
            employeesGrid.AllowUserToDeleteRows = false;
            employeesGrid.MultiSelect = false;
            employeesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            employeesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            employeesGrid.SelectionChanged += EmployeesGrid_SelectionChanged;

            Controls.Add(employeesGrid);
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

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                DataTable employees = employeeRepository.GetAllEmployees();
                employeesGrid.DataSource = employees;
                employeesGrid.ClearSelection();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Employee error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddEmployee_Click(object sender, EventArgs e)
        {
            decimal salary;
            int empId;
            if (!ValidateFields(out salary, out empId))
                return;

            try
            {
                if (employeeRepository.AddEmployee(nameTextBox.Text.Trim(), positionTextBox.Text.Trim(), salary, empId))
                {
                    AppLocalization.ShowMessage(this, "Employee added successfully.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadEmployees();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Employee error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateEmployee_Click(object sender, EventArgs e)
        {
            if (!selectedEmployeeId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select an employee to update.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal salary;
            int empId;
            if (!ValidateFields(out salary, out empId))
                return;

            try
            {
                if (employeeRepository.UpdateEmployee(selectedEmployeeId.Value, nameTextBox.Text.Trim(), positionTextBox.Text.Trim(), salary, empId))
                {
                    AppLocalization.ShowMessage(this, "Employee updated successfully.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadEmployees();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Employee error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteEmployee_Click(object sender, EventArgs e)
        {
            if (!selectedEmployeeId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select an employee to delete.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AppLocalization.ShowMessage(this, "Delete the selected employee?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                if (employeeRepository.DeleteEmployee(selectedEmployeeId.Value))
                {
                    AppLocalization.ShowMessage(this, "Employee deleted successfully.", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadEmployees();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Employee error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshEmployees_Click(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void ClearFields_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            selectedEmployeeId = null;
            nameTextBox.Clear();
            positionTextBox.Clear();
            salaryTextBox.Clear();
            personIdTextBox.Clear();
            personIdTextBox.ReadOnly = false;
            employeesGrid.ClearSelection();
        }

        private bool ValidateFields(out decimal salary, out int empId)
        {
            salary = 0;
            empId = 0;
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || nameTextBox.Text.Trim().Length > 50)
            {
                AppLocalization.ShowMessage(this, "Enter an employee name of 1 to 50 characters.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(positionTextBox.Text) || positionTextBox.Text.Trim().Length > 50)
            {
                AppLocalization.ShowMessage(this, "Enter a position of 1 to 50 characters.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                positionTextBox.Focus();
                return false;
            }
            if (!decimal.TryParse(salaryTextBox.Text.Trim(), out salary) || salary < 0)
            {
                AppLocalization.ShowMessage(this, "Enter a valid non-negative salary.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                salaryTextBox.Focus();
                return false;
            }
            if (!int.TryParse(personIdTextBox.Text.Trim(), out empId) || empId <= 0)
            {
                AppLocalization.ShowMessage(this, "Enter a valid existing Person EmpId.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                personIdTextBox.Focus();
                return false;
            }
            return true;
        }

        private void EmployeesGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (employeesGrid.SelectedRows.Count == 0 || employeesGrid.SelectedRows[0].IsNewRow)
                return;

            DataGridViewRow selectedRow = employeesGrid.SelectedRows[0];
            object id = selectedRow.Cells["EmployeeId"].Value;
            if (id == null || id == DBNull.Value)
                return;

            selectedEmployeeId = Convert.ToInt32(id);
            personIdTextBox.ReadOnly = true;
            nameTextBox.Text = Convert.ToString(selectedRow.Cells["Name"].Value);
            positionTextBox.Text = Convert.ToString(selectedRow.Cells["Position"].Value);
            salaryTextBox.Text = Convert.ToString(selectedRow.Cells["Salary"].Value);
            personIdTextBox.Text = Convert.ToString(selectedRow.Cells["EmpId"].Value);
        }
    }
}
