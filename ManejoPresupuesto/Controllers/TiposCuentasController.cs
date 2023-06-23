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

		public async Task<IActionResult> Index()
		{
			var usuarioId = 2;
			var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
				return View(tiposCuentas);
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

			tipoCuenta.UsuarioId = 2;

			//Agregamos la validación
			var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
			if(yaExisteTipoCuenta)
			{
				ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe");
				return View(tipoCuenta);
			}
			await repositorioTiposCuentas.Crear(tipoCuenta);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> VerificarExisteTipoCuenta( string nombre )
		{
			//forzamos a usar el usuario
			var usuarioId = 1;
			var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);

			if ( yaExisteTipoCuenta )
			{
				return Json($"El nombre {nombre} ya existe");
			}

			return Json( true );
		}
	}
}
