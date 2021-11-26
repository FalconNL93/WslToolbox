using System.Text.RegularExpressions;

namespace WslToolbox.Gui.Validators
{
    public static class DistributionNameValidator
    {
        public static bool ValidateName(string name)
        {
            Regex validCharacters = new("^[a-zA-Z0-9]*$");
            return validCharacters.IsMatch(name) && name.Length >= 3;
        }

        public static bool ValidateRename(string newName, string oldName)
        {
            Regex validCharacters = new("^[a-zA-Z0-9]*$");

            return newName != oldName && validCharacters.IsMatch(newName) && newName.Length >= 3;
        }
    }
}