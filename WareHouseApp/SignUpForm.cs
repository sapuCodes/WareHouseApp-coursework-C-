using System;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public class SignUpForm : Form
    {
        private readonly TextBox nameTextBox = new TextBox();
        private readonly TextBox positionTextBox = new TextBox();
        private readonly TextBox salaryTextBox = new TextBox();
        private readonly TextBox usernameTextBox = new TextBox();
        private readonly TextBox passwordTextBox = new TextBox();
        private readonly TextBox confirmPasswordTextBox = new TextBox();
        private readonly AccountService accountService = new AccountService();

        public SignUpForm()
        {
            BuildLayout();
            AppLocalization.Apply(this);
        }

        private void BuildLayout()
        {
            Text = "Create Employee Account";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(480, 430);

            AddField("Employee name", nameTextBox, 20);
            AddField("Position", positionTextBox, 70);
            AddField("Salary", salaryTextBox, 120);
            AddField("Username (max 10)", usernameTextBox, 170);
            AddField("Password (max 10)", passwordTextBox, 220);
            AddField("Confirm password", confirmPasswordTextBox, 270);
            passwordTextBox.UseSystemPasswordChar = true;
            confirmPasswordTextBox.UseSystemPasswordChar = true;
            usernameTextBox.MaxLength = 10;
            passwordTextBox.MaxLength = 10;
            confirmPasswordTextBox.MaxLength = 10;

            Button registerButton = new Button
            {
                Text = "Create Account",
                Location = new Point(150, 335),
                Size = new Size(130, 38)
            };
            registerButton.Click += RegisterButton_Click;
            AcceptButton = registerButton;

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(295, 335),
                Size = new Size(100, 38),
                DialogResult = DialogResult.Cancel
            };
            CancelButton = cancelButton;

            Controls.Add(registerButton);
            Controls.Add(cancelButton);
        }

        private void AddField(string label, TextBox textBox, int top)
        {
            Controls.Add(new Label
            {
                AutoSize = true,
                Location = new Point(35, top + 5),
                Text = label
            });
            textBox.Location = new Point(190, top);
            textBox.Size = new Size(250, 25);
            Controls.Add(textBox);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            decimal salary;
            if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(positionTextBox.Text))
            {
                ShowValidationMessage("Enter the employee name and position.");
                return;
            }
            if (!decimal.TryParse(salaryTextBox.Text.Trim(), out salary) || salary < 0)
            {
                ShowValidationMessage("Enter a valid non-negative salary.");
                return;
            }
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) || usernameTextBox.Text.Trim().Length > 10)
            {
                ShowValidationMessage("Enter a username up to 10 characters long.");
                return;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text) || passwordTextBox.Text.Length > 10)
            {
                ShowValidationMessage("Enter a password up to 10 characters long.");
                return;
            }
            if (!string.Equals(passwordTextBox.Text, confirmPasswordTextBox.Text, StringComparison.Ordinal))
            {
                ShowValidationMessage("The passwords do not match.");
                confirmPasswordTextBox.Focus();
                return;
            }

            try
            {
                int empId = accountService.RegisterEmployee(
                    nameTextBox.Text,
                    positionTextBox.Text,
                    salary,
                    usernameTextBox.Text,
                    passwordTextBox.Text);
                AppLocalization.ShowMessage(this, "Account created successfully. Your employee ID is " + empId + ".", "Sign up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Sign-up error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowValidationMessage(string message)
        {
            AppLocalization.ShowMessage(this, message, "Sign-up validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
