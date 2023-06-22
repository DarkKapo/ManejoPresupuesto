using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
	public class TipoCuenta
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[StringLength(maximumLength: 50, MinimumLength = 2, 
			ErrorMessage = "Longitud del campo {0} es entre {2} y {1}")]
		public string Nombre { get; set; }
		public int UsuarioId { get; set; }
		public int Orden { get; set; }

		//Prueba de otras validaciones
		[Required(ErrorMessage = "El campo {0} es requerido")]
		[EmailAddress(ErrorMessage = "Debe ser un Email válido")]
		public string Email { get; set; }
		[Range(minimum: 18, maximum:130, ErrorMessage = "La edad debe estar entre {1} y {2} años")]
		public int Edad { get; set; }
		[Url(ErrorMessage = "Debe ser una Url válida")]
		public string URL { get; set; }
		[CreditCard(ErrorMessage = "Tarjeta no válida")]
		public string TarjetaDeCredito { get; set; }
	}
}
