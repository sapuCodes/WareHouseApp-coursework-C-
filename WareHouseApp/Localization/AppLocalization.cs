using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WareHouseApp.Localization
{
    public static class AppLocalization
    {
        private static readonly Dictionary<string, string> Sinhala = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            { "Customers", "ගනුදෙනුකරුවන්" }, { "Employees", "සේවකයින්" }, { "Inventory", "තොගය" },
            { "Add Person", "පුද්ගලයෙකු එක් කරන්න" }, { "Logout", "පිටවන්න" }, { "Profile", "පැතිකඩ" },
            { "Settings", "සැකසුම්" }, { "Language", "භාෂාව" }, { "English", "ඉංග්‍රීසි" },
            { "Sinhala", "සිංහල" }, { "WAREHOUSE\nMANAGEMENT", "ගබඩා\nකළමනාකරණය" },
            { "Name", "නම" }, { "Email", "විද්‍යුත් තැපෑල" }, { "Phone", "දුරකථනය" },
            { "Position", "තනතුර" }, { "Salary", "වැටුප" }, { "Person EmpId", "පුද්ගල හැඳුනුම් අංකය" },
            { "Material name", "ද්‍රව්‍යයේ නම" }, { "On hand / qty", "පවතින ප්‍රමාණය" },
            { "No item selected", "අයිතමයක් තෝරා නැත" }, { "Selected ID: ", "තෝරාගත් අංකය: " },
            { "Add", "එක් කරන්න" }, { "Update", "යාවත්කාලීන කරන්න" }, { "Delete", "මකන්න" },
            { "Clear", "හිස් කරන්න" }, { "Refresh", "නැවුම් කරන්න" }, { "Receive stock", "තොගය ලබාගන්න" },
            { "Ship stock", "තොගය යවන්න" }, { "Customer Management", "ගනුදෙනුකරු කළමනාකරණය" },
            { "Employee Management", "සේවක කළමනාකරණය" }, { "Inventory Management", "තොග කළමනාකරණය" },
            { "Create Employee Account", "සේවක ගිණුමක් සාදන්න" }, { "Employee name", "සේවකයාගේ නම" },
            { "Username (max 10)", "පරිශීලක නාමය (උපරිම 10)" }, { "Password (max 10)", "මුරපදය (උපරිම 10)" },
            { "Confirm password", "මුරපදය තහවුරු කරන්න" }, { "Confirm new password", "නව මුරපදය තහවුරු කරන්න" },
            { "Create Account", "ගිණුම සාදන්න" }, { "Cancel", "අවලංගු කරන්න" },
            { "Settings - Change Password", "සැකසුම් - මුරපදය වෙනස් කරන්න" },
            { "Current password", "වත්මන් මුරපදය" }, { "New password (max 10)", "නව මුරපදය (උපරිම 10)" },
            { "Save password", "මුරපදය සුරකින්න" }, { "Employee Profile", "සේවක පැතිකඩ" },
            { "Close", "වසන්න" }, { "Loading...", "පූරණය වෙමින්..." }, { "Employee ID", "සේවක අංකය" },
            { "Username", "පරිශීලක නාමය" }, { "Inventory Management System", "තොග කළමනාකරණ පද්ධතිය" },
            { "Inventory \r\nManagement \r\nSystem", "තොග කළමනාකරණ පද්ධතිය" },
            { "Login", "පිවිසුම" }, { "Login error", "පිවිසුම් දෝෂය" },
            { "Profile error", "පැතිකඩ දෝෂය" }, { "DashBoard", "පාලක පුවරුව" },
            { "WAREHOUSE", "ගබඩාව" },
            { "LOGIN", "පිවිසෙන්න" }, { "Sign up as an employee", "සේවකයෙකු ලෙස ලියාපදිංචි වන්න" },
            { "CustomerId", "ගනුදෙනුකරු අංකය" }, { "EmployeeId", "සේවක අංකය" }, { "EmpId", "පුද්ගල අංකය" },
            { "Count", "ප්‍රමාණය" },
            { "Welcome back, {0}!", "නැවතත් සාදරයෙන් පිළිගනිමු, {0}!" },
            { "Here is what's happening with your warehouse today.", "අද ඔබගේ ගබඩාවේ සිදුවන දේ මෙන්න." },
            { "Total Inventory", "මුළු තොගය" }, { "Total Customers", "මුළු ගනුදෙනුකරුවන්" },
            { "Total Employees", "මුළු සේවකයින්" }, { "Low Stock Items", "අඩු තොග අයිතම" },
            { "Items recorded", "ලියාපදිංචි කළ අයිතම" },
            { "Registered customers", "ලියාපදිංචි ගනුදෙනුකරුවන්" },
            { "Registered employees", "ලියාපදිංචි සේවකයින්" }, { "Below 10 units", "ඒකක 10ට අඩු" },
            { "No low stock items.", "අඩු තොග අයිතම නැත." },
            { "Dashboard data could not be loaded.", "පාලක පුවරුවේ දත්ත පූරණය කළ නොහැකි විය." },
            { "Quantity", "ප්‍රමාණය" },
            { "Customer added successfully.", "ගනුදෙනුකරු සාර්ථකව එක් කරන ලදී." },
            { "Customer updated successfully.", "ගනුදෙනුකරුගේ තොරතුරු යාවත්කාලීන කරන ලදී." },
            { "Customer deleted successfully.", "ගනුදෙනුකරු සාර්ථකව මකා දමන ලදී." },
            { "Select a customer to update.", "යාවත්කාලීන කිරීමට ගනුදෙනුකරුවෙකු තෝරන්න." },
            { "Select a customer to delete.", "මකා දැමීමට ගනුදෙනුකරුවෙකු තෝරන්න." },
            { "Delete the selected customer?", "තෝරාගත් ගනුදෙනුකරු මකා දැමීමට අවශ්‍යද?" },
            { "Enter a customer name of 1 to 50 characters.", "අක්ෂර 1 සිට 50 දක්වා ගනුදෙනුකරුගේ නම ඇතුළත් කරන්න." },
            { "Employee added successfully.", "සේවකයා සාර්ථකව එක් කරන ලදී." },
            { "Employee updated successfully.", "සේවක තොරතුරු යාවත්කාලීන කරන ලදී." },
            { "Employee deleted successfully.", "සේවකයා සාර්ථකව මකා දමන ලදී." },
            { "Select an employee to update.", "යාවත්කාලීන කිරීමට සේවකයෙකු තෝරන්න." },
            { "Select an employee to delete.", "මකා දැමීමට සේවකයෙකු තෝරන්න." },
            { "Delete the selected employee?", "තෝරාගත් සේවකයා මකා දැමීමට අවශ්‍යද?" },
            { "Enter an employee name of 1 to 50 characters.", "අක්ෂර 1 සිට 50 දක්වා සේවක නමක් ඇතුළත් කරන්න." },
            { "Enter a position of 1 to 50 characters.", "අක්ෂර 1 සිට 50 දක්වා තනතුරක් ඇතුළත් කරන්න." },
            { "Enter a valid non-negative salary.", "වලංගු ධන හෝ ශුන්‍ය වැටුපක් ඇතුළත් කරන්න." },
            { "Enter a valid existing Person EmpId.", "පවතින වලංගු පුද්ගල අංකයක් ඇතුළත් කරන්න." },
            { "Material added successfully.", "ද්‍රව්‍යය සාර්ථකව එක් කරන ලදී." },
            { "Material updated successfully.", "ද්‍රව්‍ය තොරතුරු යාවත්කාලීන කරන ලදී." },
            { "Material deleted successfully.", "ද්‍රව්‍යය සාර්ථකව මකා දමන ලදී." },
            { "Select a material to update.", "යාවත්කාලීන කිරීමට ද්‍රව්‍යයක් තෝරන්න." },
            { "Select a material to delete.", "මකා දැමීමට ද්‍රව්‍යයක් තෝරන්න." },
            { "Delete the selected material?", "තෝරාගත් ද්‍රව්‍යය මකා දැමීමට අවශ්‍යද?" },
            { "Select a material before changing its stock.", "තොගය වෙනස් කිරීමට පෙර ද්‍රව්‍යයක් තෝරන්න." },
            { "Enter a positive whole-number stock movement quantity.", "ධන පූර්ණ සංඛ්‍යා තොග ප්‍රමාණයක් ඇතුළත් කරන්න." },
            { "The stock was not changed. Check that the item exists and has enough stock.", "තොගය වෙනස් නොවීය. අයිතමය පවතින බවත් ප්‍රමාණවත් තොග ඇති බවත් පරීක්ෂා කරන්න." },
            { "Stock received.", "තොගය ලැබුණි." }, { "Stock shipped.", "තොගය යවන ලදී." },
            { "Enter a material name of 1 to 50 characters.", "අක්ෂර 1 සිට 50 දක්වා ද්‍රව්‍ය නාමයක් ඇතුළත් කරන්න." },
            { "Enter a non-negative whole-number quantity.", "ශුන්‍ය හෝ ධන පූර්ණ සංඛ්‍යා ප්‍රමාණයක් ඇතුළත් කරන්න." },
            { "Account created successfully. Your employee ID is ", "ගිණුම සාර්ථකව සාදන ලදී. ඔබගේ සේවක අංකය: " },
            { "Enter the employee name and position.", "සේවකයාගේ නම සහ තනතුර ඇතුළත් කරන්න." },
            { "Enter a valid username up to 10 characters long.", "අක්ෂර 10 දක්වා වලංගු පරිශීලක නාමයක් ඇතුළත් කරන්න." },
            { "Enter a username up to 10 characters long.", "අක්ෂර 10 දක්වා පරිශීලක නාමයක් ඇතුළත් කරන්න." },
            { "Enter a password up to 10 characters long.", "අක්ෂර 10 දක්වා මුරපදයක් ඇතුළත් කරන්න." },
            { "The passwords do not match.", "මුරපද ගැළපෙන්නේ නැත." },
            { "Account created successfully.", "ගිණුම සාර්ථකව සාදන ලදී." },
            { "The current password is incorrect.", "වත්මන් මුරපදය වැරදිය." },
            { "Your password was changed successfully.", "ඔබගේ මුරපදය සාර්ථකව වෙනස් කරන ලදී." },
            { "Enter your current password.", "ඔබගේ වත්මන් මුරපදය ඇතුළත් කරන්න." },
            { "Enter a new password of 1 to 10 characters.", "අක්ෂර 1 සිට 10 දක්වා නව මුරපදයක් ඇතුළත් කරන්න." },
            { "The new password entries do not match.", "නව මුරපද ගැළපෙන්නේ නැත." },
            { "No employee record is linked to this account.", "මෙම ගිණුමට සම්බන්ධ සේවක වාර්තාවක් නොමැත." },
            { "Username or password is incorrect.", "පරිශීලක නාමය හෝ මුරපදය වැරදිය." },
            { "Validation", "වලංගුකරණය" }, { "Confirm delete", "මකාදැමීම තහවුරු කරන්න" },
            { "Customer error", "ගනුදෙනුකරු දෝෂය" }, { "Employee error", "සේවක දෝෂය" },
            { "Inventory error", "තොග දෝෂය" }, { "Sign up", "ලියාපදිංචි වීම" },
            { "Sign-up error", "ලියාපදිංචි දෝෂය" }, { "Sign-up validation", "ලියාපදිංචි වලංගුකරණය" },
            { "Password change error", "මුරපද වෙනස් කිරීමේ දෝෂය" },
            { "Settings validation", "සැකසුම් වලංගුකරණය" },
            { "Unable to add customer: ", "ගනුදෙනුකරු එක් කළ නොහැක: " },
            { "Unable to load customers: ", "ගනුදෙනුකරුවන් පූරණය කළ නොහැක: " },
            { "Unable to update customer: ", "ගනුදෙනුකරු යාවත්කාලීන කළ නොහැක: " },
            { "Unable to delete customer: ", "ගනුදෙනුකරු මකා දැමිය නොහැක: " },
            { "Unable to add employee: ", "සේවකයා එක් කළ නොහැක: " },
            { "Unable to load employees: ", "සේවකයින් පූරණය කළ නොහැක: " },
            { "Unable to update employee: ", "සේවකයා යාවත්කාලීන කළ නොහැක: " },
            { "Unable to delete employee: ", "සේවකයා මකා දැමිය නොහැක: " },
            { "Unable to add material: ", "ද්‍රව්‍යය එක් කළ නොහැක: " },
            { "Unable to load inventory: ", "තොග පූරණය කළ නොහැක: " },
            { "Unable to update material: ", "ද්‍රව්‍යය යාවත්කාලීන කළ නොහැක: " },
            { "Unable to delete material: ", "ද්‍රව්‍යය මකා දැමිය නොහැක: " },
            { "Unable to receive stock: ", "තොගය ලබාගත නොහැක: " },
            { "Unable to ship stock: ", "තොගය යැවිය නොහැක: " },
            { "Unable to register the account and employee: ", "ගිණුම සහ සේවක තොරතුරු ලියාපදිංචි කළ නොහැක: " },
            { "Unable to verify the account: ", "ගිණුම තහවුරු කළ නොහැක: " },
            { "Unable to change password: ", "මුරපදය වෙනස් කළ නොහැක: " },
            { "That username is already registered.", "මෙම පරිශීලක නාමය දැනටමත් ලියාපදිංචි කර ඇත." },
            { "The account no longer exists.", "මෙම ගිණුම තවදුරටත් නොපවතී." },
            { "A valid employee ID is required.", "වලංගු සේවක අංකයක් අවශ්‍යයි." },
            { "A valid Person EmpId is required.", "වලංගු පුද්ගල අංකයක් අවශ්‍යයි." },
            { "Customer name is required and must be 50 characters or fewer.", "ගනුදෙනුකරුගේ නම අවශ්‍ය වන අතර අක්ෂර 50ක් හෝ ඊට අඩු විය යුතුය." },
            { "Employee name is required and must be 50 characters or fewer.", "සේවක නම අවශ්‍ය වන අතර අක්ෂර 50ක් හෝ ඊට අඩු විය යුතුය." },
            { "Employee position is required and must be 50 characters or fewer.", "සේවක තනතුර අවශ්‍ය වන අතර අක්ෂර 50ක් හෝ ඊට අඩු විය යුතුය." },
            { "Material name is required and must be 50 characters or fewer.", "ද්‍රව්‍ය නාමය අවශ්‍ය වන අතර අක්ෂර 50ක් හෝ ඊට අඩු විය යුතුය." },
            { "Email must be 50 characters or fewer.", "විද්‍යුත් තැපෑල අක්ෂර 50ක් හෝ ඊට අඩු විය යුතුය." },
            { "Phone must be 20 characters or fewer.", "දුරකථන අංකය අක්ෂර 20ක් හෝ ඊට අඩු විය යුතුය." },
            { "Salary cannot be negative.", "වැටුප ඍණ විය නොහැක." },
            { "Material count cannot be negative.", "ද්‍රව්‍ය ප්‍රමාණය ඍණ විය නොහැක." },
            { "Stock movement quantity must be positive.", "තොග වෙනස් කිරීමේ ප්‍රමාණය ධන විය යුතුය." }
        };

        private static readonly Dictionary<string, string> English = CreateEnglishMap();

        public static bool IsSinhala { get; private set; }

        public static void SetLanguage(bool useSinhala)
        {
            IsSinhala = useSinhala;
        }

        public static string Translate(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string translated;
            if (IsSinhala && Sinhala.TryGetValue(text, out translated))
                return translated;
            if (!IsSinhala && English.TryGetValue(text, out translated))
                return translated;

            if (!IsSinhala)
            {
                foreach (KeyValuePair<string, string> entry in Sinhala)
                {
                    if (text.StartsWith(entry.Value, StringComparison.Ordinal))
                        return entry.Key + text.Substring(entry.Value.Length);
                }
            }

            string selectedIdPrefix = "Selected ID: ";
            string sinhalaSelectedIdPrefix = Sinhala[selectedIdPrefix];
            if (text.StartsWith(selectedIdPrefix, StringComparison.Ordinal))
                return (IsSinhala ? sinhalaSelectedIdPrefix : selectedIdPrefix) + text.Substring(selectedIdPrefix.Length);
            if (text.StartsWith(sinhalaSelectedIdPrefix, StringComparison.Ordinal))
                return (IsSinhala ? sinhalaSelectedIdPrefix : selectedIdPrefix) + text.Substring(sinhalaSelectedIdPrefix.Length);

            string accountMessagePrefix = "Account created successfully. Your employee ID is ";
            string sinhalaAccountMessagePrefix = Sinhala[accountMessagePrefix];
            if (text.StartsWith(accountMessagePrefix, StringComparison.Ordinal))
                return (IsSinhala ? sinhalaAccountMessagePrefix : accountMessagePrefix) + text.Substring(accountMessagePrefix.Length);
            if (text.StartsWith(sinhalaAccountMessagePrefix, StringComparison.Ordinal))
                return (IsSinhala ? sinhalaAccountMessagePrefix : accountMessagePrefix) + text.Substring(sinhalaAccountMessagePrefix.Length);
            return text;
        }

        public static void Apply(Control root)
        {
            if (root == null)
                return;

            root.Text = Translate(root.Text);
            DataGridView grid = root as DataGridView;
            if (grid != null)
            {
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    string originalHeader;
                    if (column.Tag == null)
                        column.Tag = column.HeaderText;
                    originalHeader = Convert.ToString(column.Tag);
                    column.HeaderText = Translate(originalHeader);
                }
                if (!string.Equals(grid.Tag as string, "AppLocalization.GridHooked", StringComparison.Ordinal))
                {
                    grid.DataBindingComplete += Grid_DataBindingComplete;
                    grid.Tag = "AppLocalization.GridHooked";
                }
            }

            foreach (Control child in root.Controls)
                Apply(child);
        }

        private static void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null)
                return;
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.Tag == null)
                    column.Tag = column.HeaderText;
                column.HeaderText = Translate(Convert.ToString(column.Tag));
            }
        }

        public static DialogResult ShowMessage(IWin32Window owner, string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(owner, Translate(message), Translate(caption), buttons, icon);
        }

        private static Dictionary<string, string> CreateEnglishMap()
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (KeyValuePair<string, string> entry in Sinhala)
                if (!result.ContainsKey(entry.Value))
                    result.Add(entry.Value, entry.Key);
            return result;
        }
    }
}
