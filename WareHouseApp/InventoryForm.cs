using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.Materials;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public class InventoryForm : Form
    {
        private readonly Material materialRepository = new Material();
        private readonly DataGridView materialsGrid = new DataGridView();
        private readonly TextBox nameTextBox = new TextBox();
        private readonly TextBox countTextBox = new TextBox();
        private readonly Label selectedIdLabel = new Label();
        private int? selectedMaterialId;

        public InventoryForm()
        {
            BuildLayout();
            AppLocalization.Apply(this);
            Load += InventoryForm_Load;
        }

        private void BuildLayout()
        {
            Text = "Inventory Management";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(750, 460);
            Size = new Size(900, 560);

            Panel inputPanel = new Panel { Dock = DockStyle.Top, Height = 164, Padding = new Padding(12) };
            TableLayoutPanel fieldsPanel = CreateFieldsPanel(3);
            AddField(fieldsPanel, "Material name", nameTextBox, 0);
            AddField(fieldsPanel, "On hand / qty", countTextBox, 1);
            selectedIdLabel.Dock = DockStyle.Fill;
            selectedIdLabel.Margin = new Padding(4, 0, 4, 0);
            selectedIdLabel.Text = "No item selected";
            selectedIdLabel.TextAlign = ContentAlignment.MiddleLeft;
            fieldsPanel.Controls.Add(selectedIdLabel, 2, 1);

            FlowLayoutPanel actionsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            actionsPanel.Controls.Add(CreateButton("Add", AddMaterial_Click));
            actionsPanel.Controls.Add(CreateButton("Update", UpdateMaterial_Click));
            actionsPanel.Controls.Add(CreateButton("Delete", DeleteMaterial_Click));
            actionsPanel.Controls.Add(CreateButton("Receive stock", ReceiveStock_Click));
            actionsPanel.Controls.Add(CreateButton("Ship stock", ShipStock_Click));
            actionsPanel.Controls.Add(CreateButton("Clear", ClearFields_Click));
            actionsPanel.Controls.Add(CreateButton("Refresh", RefreshMaterials_Click));

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

            materialsGrid.Dock = DockStyle.Fill;
            materialsGrid.ReadOnly = true;
            materialsGrid.AllowUserToAddRows = false;
            materialsGrid.AllowUserToDeleteRows = false;
            materialsGrid.MultiSelect = false;
            materialsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            materialsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            materialsGrid.SelectionChanged += MaterialsGrid_SelectionChanged;

            Controls.Add(materialsGrid);
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

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            LoadMaterials();
        }

        private void LoadMaterials()
        {
            try
            {
                materialsGrid.DataSource = materialRepository.GetAllMaterials();
                materialsGrid.ClearSelection();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Inventory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddMaterial_Click(object sender, EventArgs e)
        {
            int count;
            if (!ValidateFields(out count))
                return;

            try
            {
                if (materialRepository.AddMaterial(nameTextBox.Text.Trim(), count))
                {
                    AppLocalization.ShowMessage(this, "Material added successfully.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadMaterials();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Inventory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMaterial_Click(object sender, EventArgs e)
        {
            if (!selectedMaterialId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select a material to update.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int count;
            if (!ValidateFields(out count))
                return;

            try
            {
                if (materialRepository.UpdateMaterial(selectedMaterialId.Value, nameTextBox.Text.Trim(), count))
                {
                    AppLocalization.ShowMessage(this, "Material updated successfully.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadMaterials();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Inventory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteMaterial_Click(object sender, EventArgs e)
        {
            if (!selectedMaterialId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select a material to delete.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AppLocalization.ShowMessage(this, "Delete the selected material?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                if (materialRepository.DeleteMaterial(selectedMaterialId.Value))
                {
                    AppLocalization.ShowMessage(this, "Material deleted successfully.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadMaterials();
                }
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Inventory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshMaterials_Click(object sender, EventArgs e)
        {
            LoadMaterials();
        }

        private void ReceiveStock_Click(object sender, EventArgs e)
        {
            ApplyStockMovement(true);
        }

        private void ShipStock_Click(object sender, EventArgs e)
        {
            ApplyStockMovement(false);
        }

        private void ApplyStockMovement(bool receive)
        {
            if (!selectedMaterialId.HasValue)
            {
                AppLocalization.ShowMessage(this, "Select a material before changing its stock.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity;
            if (!int.TryParse(countTextBox.Text.Trim(), out quantity) || quantity <= 0)
            {
                AppLocalization.ShowMessage(this, "Enter a positive whole-number stock movement quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                countTextBox.Focus();
                return;
            }

            try
            {
                bool updated = receive
                    ? materialRepository.AddStock(selectedMaterialId.Value, quantity)
                    : materialRepository.RemoveStock(selectedMaterialId.Value, quantity);
                if (!updated)
                {
                    AppLocalization.ShowMessage(this, "The stock was not changed. Check that the item exists and has enough stock.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AppLocalization.ShowMessage(this, receive ? "Stock received." : "Stock shipped.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadMaterials();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Inventory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            selectedMaterialId = null;
            selectedIdLabel.Text = "No item selected";
            nameTextBox.Clear();
            countTextBox.Clear();
            materialsGrid.ClearSelection();
        }

        private bool ValidateFields(out int count)
        {
            count = 0;
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || nameTextBox.Text.Trim().Length > 50)
            {
                AppLocalization.ShowMessage(this, "Enter a material name of 1 to 50 characters.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
                return false;
            }
            if (!int.TryParse(countTextBox.Text.Trim(), out count) || count < 0)
            {
                AppLocalization.ShowMessage(this, "Enter a non-negative whole-number quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                countTextBox.Focus();
                return false;
            }
            return true;
        }

        private void MaterialsGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (materialsGrid.SelectedRows.Count == 0 || materialsGrid.SelectedRows[0].IsNewRow)
                return;

            DataGridViewRow row = materialsGrid.SelectedRows[0];
            object id = row.Cells["Id"].Value;
            if (id == null || id == DBNull.Value)
                return;

            selectedMaterialId = Convert.ToInt32(id);
            selectedIdLabel.Text = "Selected ID: " + selectedMaterialId.Value;
            nameTextBox.Text = Convert.ToString(row.Cells["Name"].Value);
            countTextBox.Text = Convert.ToString(row.Cells["Count"].Value);
        }
    }
}
