using System;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public class SettingsForm : Form
    {
        private readonly Person currentUser;
        private readonly AccountService accountService = new AccountService();
        private readonly TextBox currentPasswordTextBox = new TextBox();
        private readonly TextBox newPasswordTextBox = new TextBox();
        private readonly TextBox confirmPasswordTextBox = new TextBox();

        public SettingsForm(Person currentUser)
        {
            if (currentUser == null || !currentUser.IsAuthenticated)
                throw new ArgumentException("Settings require an authenticated user.", "currentUser");

            this.currentUser = currentUser;
            BuildLayout();
            AppLocalization.Apply(this);
        }

        private void BuildLayout()
        {
            Text = "Settings - Change Password";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(440, 260);

            AddPasswordField("Current password", currentPasswordTextBox, 25);
            AddPasswordField("New password (max 10)", newPasswordTextBox, 75);
            AddPasswordField("Confirm new password", confirmPasswordTextBox, 125);
            currentPasswordTextBox.MaxLength = 10;
            newPasswordTextBox.MaxLength = 10;
            confirmPasswordTextBox.MaxLength = 10;

            Button saveButton = new Button
            {
                Text = "Save password",
                Location = new Point(145, 190),
                Size = new Size(130, 36)
            };
            saveButton.Click += SaveButton_Click;
            AcceptButton = saveButton;

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Location = new Point(290, 190),
                Size = new Size(90, 36),
                DialogResult = DialogResult.Cancel
            };
            CancelButton = cancelButton;

            Controls.Add(saveButton);
            Controls.Add(cancelButton);
        }

        private void AddPasswordField(string labelText, TextBox textBox, int top)
        {
            Controls.Add(new Label
            {
                AutoSize = true,
                Location = new Point(25, top + 5),
                Text = labelText
            });
            textBox.Location = new Point(205, top);
            textBox.Size = new Size(205, 25);
            textBox.UseSystemPasswordChar = true;
            Controls.Add(textBox);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string currentPassword = currentPasswordTextBox.Text;
            string newPassword = newPasswordTextBox.Text;
            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                ShowValidationMessage("Enter your current password.");
                currentPasswordTextBox.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length > 10)
            {
                ShowValidationMessage("Enter a new password of 1 to 10 characters.");
                newPasswordTextBox.Focus();
                return;
            }
            if (!string.Equals(newPassword, confirmPasswordTextBox.Text, StringComparison.Ordinal))
            {
                ShowValidationMessage("The new password entries do not match.");
                confirmPasswordTextBox.Focus();
                return;
            }

            try
            {
                int verifiedEmpId;
                if (!accountService.Authenticate(currentUser.Username, currentPassword, out verifiedEmpId) || verifiedEmpId != currentUser.EmpId)
                {
                    AppLocalization.ShowMessage(this, "The current password is incorrect.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    currentPasswordTextBox.Clear();
                    currentPasswordTextBox.Focus();
                    return;
                }

                currentUser.ChangePassword(newPassword);
                currentPasswordTextBox.Clear();
                newPasswordTextBox.Clear();
                confirmPasswordTextBox.Clear();
                AppLocalization.ShowMessage(this, "Your password was changed successfully.", "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Password change error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowValidationMessage(string message)
        {
            AppLocalization.ShowMessage(this, message, "Settings validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
