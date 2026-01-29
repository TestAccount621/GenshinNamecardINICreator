using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace GenshinNamecardAutomater.ValidationRules
{

    public class HashValuesTextBoxValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string hash)
            {
                return Regex.IsMatch(hash, @"^[a-zA-Z0-9]{8}$")
                    ? ValidationResult.ValidResult
                    : new ValidationResult(false, "Invalid hash format. Must be 8 characters long with only letters and numbers");
            }
            return new ValidationResult(false, "Invalid hash format. Must be 8 characters long with only letters and numbers");
        }
    }

    public class HashNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace(value?.ToString())
                ? new ValidationResult(false, $"Cannot be blank. Please put in a namecard name.")
                : ValidationResult.ValidResult;
        }
    }
}
