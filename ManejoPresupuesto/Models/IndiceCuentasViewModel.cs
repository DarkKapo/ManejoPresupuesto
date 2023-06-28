namespace ManejoPresupuesto.Models
{
	public class IndiceCuentasViewModel
	{
		public string TipoCuenta { get; set; }
		//Guarda todas las cuentas del usuario
		public IEnumerable<Cuenta> Cuentas { get; set; }
		//Suma las cuentas del usuario
		public decimal Balance => Cuentas.Sum(x => x.Balance);
	}
}
