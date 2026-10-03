using System;
using System.Drawing;
using System.Windows.Forms;
using WareHouseApp.People;
using WareHouseApp.Localization;

namespace WareHouseApp
{
    public class ProfileForm : Form
    {
        private readonly Person currentUser;
        private readonly Employee employeeRepository = new Employee();
        private readonly Label nameValueLabel = new Label();
        private readonly Label positionValueLabel = new Label();
        private readonly Label usernameValueLabel = new Label();
        private readonly Label employeeIdValueLabel = new Label();

        public ProfileForm(Person currentUser)
        {
            if (currentUser == null || !currentUser.IsAuthenticated)
                throw new ArgumentException("Profile access requires an authenticated user.", "currentUser");

            this.currentUser = currentUser;
            BuildLayout();
            AppLocalization.Apply(this);
            Load += ProfileForm_Load;
        }

        private void BuildLayout()
        {
            Text = "Employee Profile";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(460, 270);

            AddProfileField("Name", nameValueLabel, 35);
            AddProfileField("Position", positionValueLabel, 80);
            AddProfileField("Username", usernameValueLabel, 125);
            AddProfileField("Employee ID", employeeIdValueLabel, 170);

            Button closeButton = new Button
            {
                Text = "Close",
                Location = new Point(340, 215),
                Size = new Size(85, 32),
                DialogResult = DialogResult.Cancel
            };
            CancelButton = closeButton;
            Controls.Add(closeButton);
        }

        private void AddProfileField(string caption, Label valueLabel, int top)
        {
            Controls.Add(new Label
            {
                AutoSize = true,
                Location = new Point(35, top),
                Text = caption + ":"
            });
            valueLabel.AutoSize = true;
            valueLabel.Location = new Point(160, top);
            valueLabel.Text = "Loading...";
            Controls.Add(valueLabel);
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            try
            {
                Employee profile = employeeRepository.GetProfileByPersonId(currentUser.EmpId);
                if (profile == null)
                {
                    AppLocalization.ShowMessage(this, "No employee record is linked to this account.", "Profile", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                nameValueLabel.Text = profile.Name;
                positionValueLabel.Text = profile.Position;
                usernameValueLabel.Text = currentUser.Username;
                employeeIdValueLabel.Text = profile.EmployeeId.ToString();
            }
            catch (Exception ex)
            {
                AppLocalization.ShowMessage(this, ex.Message, "Profile error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
