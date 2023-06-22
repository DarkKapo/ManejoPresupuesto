using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Validaciones
{
	public class PrimeraLetraMayusculaAttribute: ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			//Si no hay datos, lo da como correcto porque solo validamos la primera letra
			//Para validar si hay datos o no, usamos otra validación
			if ( value == null || string.IsNullOrEmpty(value.ToString()) ) 
			{
				return ValidationResult.Success;
			}
			//Obtiene la primera letra
			var primeraLetra = value.ToString()[0].ToString();

			if( primeraLetra != primeraLetra.ToUpper() )
			{
				return new ValidationResult("La primera letra debe ser mayúscula");
			}

			return ValidationResult.Success;
		}
	}
}
