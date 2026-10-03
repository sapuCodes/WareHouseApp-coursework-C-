using System;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public partial class Form1 : Form
    {
        private readonly Employee currentUser = new Employee();

        public Form1()
        {
            InitializeComponent();
            txtName.MaxLength = 10;
            txtPassword.MaxLength = 10;

            ApplyModernStyle();
            AppLocalization.Apply(this);
        }

        private void ApplyModernStyle()
        {
            Color slate = Color.FromArgb(30, 30, 45);
            Color background = Color.FromArgb(245, 246, 250);
            Color accent = Color.FromArgb(59, 130, 246);
            Color muted = Color.FromArgb(103, 112, 130);
            Font bodyFont = new Font("Segoe UI", 10F, FontStyle.Regular);

            BackColor = background;
            Font = bodyFont;

            panel1.BackColor = slate;
            brandLabel.BackColor = Color.Transparent;

            label1.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            label1.ForeColor = slate;

            usernameLabel.Font = bodyFont;
            usernameLabel.ForeColor = muted;
            txtName.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            txtName.BackColor = Color.White;
            txtName.BorderStyle = BorderStyle.FixedSingle;

            passwordLabel.Font = bodyFont;
            passwordLabel.ForeColor = muted;
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            txtPassword.BackColor = Color.White;
            txtPassword.BorderStyle = BorderStyle.FixedSingle;

            butLogin.BackColor = accent;
            butLogin.ForeColor = Color.White;
            butLogin.FlatStyle = FlatStyle.Flat;
            butLogin.FlatAppearance.BorderSize = 0;
            butLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            butLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            butLogin.Cursor = Cursors.Hand;

            signUpButton.BackColor = background;
            signUpButton.ForeColor = accent;
            signUpButton.FlatStyle = FlatStyle.Flat;
            signUpButton.FlatAppearance.BorderSize = 0;
            signUpButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 238, 249);
            signUpButton.Font = bodyFont;
            signUpButton.Cursor = Cursors.Hand;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void butLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!currentUser.Login(txtName.Text.Trim(), txtPassword.Text))
                {
                    AppLocalization.ShowMessage(this, "Username or password is incorrect.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Hide();
                using (DashBoard dashboard = new DashBoard(currentUser))
                    dashboard.ShowDialog(this);
                AppLocalization.Apply(this);
                Show();
                txtName.Focus();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                txtPassword.Clear();
            }
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            using (SignUpForm signUpForm = new SignUpForm())
                signUpForm.ShowDialog(this);
        }
    }
}
