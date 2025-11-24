using System.ComponentModel.DataAnnotations;

namespace Consultorio_PDS2.Validaciones
{
    public class Telefono10DigitosAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // checa si es null y regresa error
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("El teléfono es obligatorio.");
            }

            var telefono = value.ToString();

            // checa puros numeros
            if (!telefono.All(char.IsDigit))
            {
                return new ValidationResult("El teléfono debe contener solo números.");
            }

            // checa longitud exacta
            if (telefono.Length != 10)
            {
                return new ValidationResult("El teléfono debe tener exactamente 10 dígitos.");
            }

            return ValidationResult.Success;
        }
    }
}
