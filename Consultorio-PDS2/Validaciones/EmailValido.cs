using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Consultorio_PDS2.Validaciones
{
    public class EmailValidoAttribute : ValidationAttribute
    {
        private const string EmailRegex =
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("El correo es obligatorio.");
            }

            var email = value.ToString();

            if (!Regex.IsMatch(email, EmailRegex))
            {
                return new ValidationResult("El correo no tiene un formato válido.");
            }

            return ValidationResult.Success;
        }
    }
}
