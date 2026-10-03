using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;
using WareHouseApp.Data;

namespace WareHouseApp
{
    public partial class DashBoard : Form
    {
        private readonly Person currentUser;
        private Form activeContentForm;
        private DashboardOverviewControl overviewControl;

        public DashBoard()
            : this(null)
        {
        }

        public DashBoard(Person currentUser)
        {
            this.currentUser = currentUser;
            InitializeComponent();
            mainDash1.AutoScroll = true;
            mainDash1.Controls.Clear();
            button1.Text = "Customers";
            button2.Text = "Employees";
            button3.Text = "Inventory";
            button4.Text = "Add Person";
            button6.Text = "Logout";
            button1.Click += CustomerButton_Click;
            button2.Click += EmployeeButton_Click;
            button3.Click += InventoryButton_Click;
            button4.Click += AddPersonButton_Click;
            button7.Click += ProfileButton_Click;
            button8.Click += SettingsButton_Click;
            button6.Click += LogoutButton_Click;
            button9.Click += LanguageButton_Click;

            ApplyModernStyle();
            DashboardOverviewData overviewData;
            try
            {
                overviewData = DashboardOverviewData.Load();
            }
            catch (InvalidOperationException)
            {
                overviewData = null;
            }
            overviewControl = new DashboardOverviewControl(
                currentUser == null ? null : currentUser.Username,
                overviewData);
            mainDash1.Controls.Add(overviewControl);
            AppLocalization.Apply(this);
            overviewControl.UpdateWelcomeText();
        }

        private void ApplyModernStyle()
        {
            Color sidebarColor = Color.FromArgb(30, 30, 45);
            Color backgroundColor = Color.FromArgb(245, 246, 250);
            Color accentColor = Color.FromArgb(59, 130, 246);
            Color secondaryColor = Color.FromArgb(43, 45, 62);
            Font navigationFont = new Font("Segoe UI", 10F, FontStyle.Regular);

            SuspendLayout();
            BackColor = backgroundColor;
            Font = navigationFont;

            panel1.BackColor = sidebarColor;

            panel2.BackColor = Color.White;

            mainDash1.BackColor = backgroundColor;

            brandLabel.BackColor = Color.Transparent;
            StyleNavigationButton(button1, accentColor, navigationFont);
            StyleNavigationButton(button2, accentColor, navigationFont);
            StyleNavigationButton(button3, accentColor, navigationFont);
            StyleNavigationButton(button4, accentColor, navigationFont);
            StyleNavigationButton(button6, accentColor, navigationFont);

            StyleTopButton(button7, accentColor, navigationFont);
            button7.Text = "Profile";

            StyleTopButton(button8, accentColor, navigationFont);
            button8.Text = "Settings";

            button9.Font = navigationFont;
            button9.BackColor = secondaryColor;
            button9.ForeColor = Color.White;
            button9.FlatStyle = FlatStyle.Flat;
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 60, 82);
            button9.Cursor = Cursors.Hand;

            ResumeLayout(false);
        }

        private static void StyleNavigationButton(Button button, Color accentColor, Font font)
        {
            button.BackColor = Color.FromArgb(43, 45, 62);
            button.ForeColor = Color.FromArgb(231, 234, 243);
            button.Font = font;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(14, 0, 0, 0);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = accentColor;
            button.Cursor = Cursors.Hand;
        }

        private static void StyleTopButton(Button button, Color accentColor, Font font)
        {
            button.BackColor = Color.White;
            button.ForeColor = accentColor;
            button.Font = font;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 238, 249);
            button.Cursor = Cursors.Hand;
        }

        private void CustomerButton_Click(object sender, EventArgs e)
        {
            DisplayContent(new CustomerForm());
        }

        private void EmployeeButton_Click(object sender, EventArgs e)
        {
            DisplayContent(new EmployeeForm());
        }

        private void InventoryButton_Click(object sender, EventArgs e)
        {
            DisplayContent(new InventoryForm());
        }

        private void AddPersonButton_Click(object sender, EventArgs e)
        {
            DisplayContent(new SignUpForm());
        }

        private void DisplayContent(Form contentForm)
        {
            if (overviewControl != null)
            {
                mainDash1.Controls.Remove(overviewControl);
                overviewControl.Dispose();
                overviewControl = null;
            }

            if (activeContentForm != null)
            {
                mainDash1.Controls.Remove(activeContentForm);
                activeContentForm.Dispose();
                activeContentForm = null;
            }

            mainDash1.Controls.Clear();

            contentForm.TopLevel = false;
            contentForm.FormBorderStyle = FormBorderStyle.None;
            contentForm.StartPosition = FormStartPosition.Manual;
            contentForm.ShowInTaskbar = false;
            contentForm.Dock = DockStyle.Fill;
            mainDash1.Controls.Add(contentForm);
            activeContentForm = contentForm;
            contentForm.Show();
            contentForm.BringToFront();
            AppLocalization.Apply(contentForm);
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            ContextMenuStrip languageMenu = new ContextMenuStrip();
            languageMenu.Items.Add("English", null, delegate { SetLanguage(false); });
            languageMenu.Items.Add("Sinhala", null, delegate { SetLanguage(true); });
            languageMenu.Show(button9, new Point(0, button9.Height));
        }

        private void SetLanguage(bool useSinhala)
        {
            AppLocalization.SetLanguage(useSinhala);
            AppLocalization.Apply(this);
            if (overviewControl != null)
                overviewControl.UpdateWelcomeText();
            if (activeContentForm != null)
                AppLocalization.Apply(activeContentForm);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            DisplayContent(new SettingsForm(currentUser));
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            DisplayContent(new ProfileForm(currentUser));
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            if (currentUser != null)
                currentUser.Logout(currentUser.EmpId);
            Close();
        }
    }
}
