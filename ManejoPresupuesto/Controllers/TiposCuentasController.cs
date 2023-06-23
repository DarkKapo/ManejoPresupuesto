using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
	public class TiposCuentasController: Controller
	{
		private readonly IRepositorioTiposCuentas repositorioTiposCuentas;

		public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas)
		{
			this.repositorioTiposCuentas = repositorioTiposCuentas;
		}

		public IActionResult Crear()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Crear(TipoCuenta tipoCuenta) 
		{
			//Valida si los datos son correctos
			if( !ModelState.IsValid ) 
			{
				//enviar tipoCuenta sirve para volver a llenar el fornmulario
				return View(tipoCuenta);
			}

			tipoCuenta.UsuarioId = 1;
			await repositorioTiposCuentas.Crear(tipoCuenta);
			return View();
		}
	}
}
