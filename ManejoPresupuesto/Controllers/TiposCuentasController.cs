using ManejoPresupuesto.Models;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
	public class TiposCuentasController: Controller
	{
		public IActionResult Crear() 
		{
			return View();
		}

		[HttpPost]
		public IActionResult Crear(TipoCuenta tipoCuenta) 
		{
			//Valida si los datos son correctos
			if( !ModelState.IsValid ) 
			{
				//enviar tipoCuenta sirve para volver a llenar el fornmulario
				return View(tipoCuenta);
			}
			return View();
		}
	}
}
