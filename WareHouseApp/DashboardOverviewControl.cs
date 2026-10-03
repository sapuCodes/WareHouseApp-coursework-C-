using System;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.Data;

namespace WareHouseApp
{
    public sealed class DashboardOverviewControl : UserControl
    {
        private readonly string username;
        private readonly Label welcomeLabel;

        public DashboardOverviewControl(string username, DashboardOverviewData overview)
        {
            this.username = string.IsNullOrWhiteSpace(username) ? "User" : username;
            BackColor = Color.FromArgb(245, 246, 250);
            Dock = DockStyle.Fill;
            AutoScroll = true;

            TableLayoutPanel layout = new TableLayoutPanel
            {
                BackColor = BackColor,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(24),
                RowCount = 4
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 112F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 142F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Controls.Add(layout);

            Panel welcomeCard = CreateCard();
            TableLayoutPanel welcomeLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(18, 12, 18, 10),
                RowCount = 2
            };
            welcomeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            welcomeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            welcomeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            welcomeLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 45),
                TextAlign = ContentAlignment.MiddleLeft
            };
            Label subtitleLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(103, 112, 130),
                Text = "Here is what's happening with your warehouse today.",
                TextAlign = ContentAlignment.MiddleLeft
            };
            welcomeLayout.Controls.Add(welcomeLabel, 0, 0);
            welcomeLayout.Controls.Add(subtitleLabel, 0, 1);
            welcomeCard.Controls.Add(welcomeLayout);
            layout.Controls.Add(welcomeCard, 0, 0);

            TableLayoutPanel metricsPanel = new TableLayoutPanel
            {
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 12, 0, 0),
                RowCount = 1
            };
            for (int column = 0; column < 4; column++)
                metricsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            metricsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            metricsPanel.Controls.Add(CreateMetricCard("Total Inventory", overview == null ? (int?)null : overview.TotalInventory, "Items recorded"), 0, 0);
            metricsPanel.Controls.Add(CreateMetricCard("Total Customers", overview == null ? (int?)null : overview.TotalCustomers, "Registered customers"), 1, 0);
            metricsPanel.Controls.Add(CreateMetricCard("Total Employees", overview == null ? (int?)null : overview.TotalEmployees, "Registered employees"), 2, 0);
            metricsPanel.Controls.Add(CreateMetricCard("Low Stock Items", overview == null ? (int?)null : overview.LowStockItems, "Below 10 units"), 3, 0);
            layout.Controls.Add(metricsPanel, 0, 1);

            Label lowStockHeading = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 45),
                Text = "Low Stock Items",
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(lowStockHeading, 0, 2);

            Panel lowStockPanel = CreateCard();
            DataGridView lowStockGrid = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 36,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            lowStockGrid.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lowStockGrid.DefaultCellStyle.ForeColor = Color.FromArgb(43, 45, 62);
            lowStockGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 246, 250);
            lowStockGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(103, 112, 130);
            lowStockGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lowStockGrid.EnableHeadersVisualStyles = false;
            lowStockGrid.DataSource = overview == null ? null : overview.LowStockMaterials;

            Label statusLabel = new Label
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(103, 112, 130),
                Text = overview == null ? "Dashboard data could not be loaded." : "No low stock items.",
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = overview == null || overview.LowStockMaterials == null || overview.LowStockMaterials.Rows.Count == 0
            };
            lowStockGrid.DataBindingComplete += delegate
            {
                statusLabel.Visible = lowStockGrid.Rows.Count == 0;
            };
            lowStockPanel.Controls.Add(lowStockGrid);
            lowStockPanel.Controls.Add(statusLabel);
            statusLabel.BringToFront();
            layout.Controls.Add(lowStockPanel, 0, 3);

            UpdateWelcomeText();
        }

        public void UpdateWelcomeText()
        {
            welcomeLabel.Text = string.Format(
                WareHouseApp.Localization.AppLocalization.Translate("Welcome back, {0}!"),
                username);
        }

        private static Panel CreateMetricCard(string title, int? value, string description)
        {
            Panel card = CreateCard();
            card.Margin = new Padding(0, 0, 12, 0);

            TableLayoutPanel cardLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 8, 8, 6),
                RowCount = 3
            };
            cardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
            cardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 23F));

            cardLayout.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Color.FromArgb(103, 112, 130),
                Text = title,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);
            cardLayout.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 45),
                Text = value.HasValue ? value.Value.ToString("N0") : "—",
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 1);
            cardLayout.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(145, 151, 163),
                Text = description,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 2);

            card.Controls.Add(cardLayout);
            return card;
        }

        private static Panel CreateCard()
        {
            return new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
        }
    }
}
