﻿using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Controllers
{
	public class TiposCuentasController: Controller
	{
		private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
			IServicioUsuarios servicioUsuarios)
		{
			this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }

		public async Task<IActionResult> Index()
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
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

			tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId(); ;

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
		public async Task<ActionResult> Editar(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

			if(tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			return View(tipoCuenta);
		}

		[HttpPost]
		public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);

			if (tipoCuentaExiste is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			await repositorioTiposCuentas.Actualizar(tipoCuenta);
			return RedirectToAction("Index");
		}
		//Devuelve la lista de cuentas para la vista borrar
		public async Task<IActionResult> Borrar( int id )
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId( id, usuarioId);

			if(tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			return View(tipoCuenta);
		}

		//Acción para borrar el tipo cuenta
		[HttpPost]
		public async Task<IActionResult> BorrarTipoCuenta(int id)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

			if (tipoCuenta is null)
			{
				return RedirectToAction("NoEncontrado", "Home");
			}

			await repositorioTiposCuentas.Borrar(id);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> VerificarExisteTipoCuenta( string nombre )
		{
			//forzamos a usar el usuario
			var usuarioId = servicioUsuarios.ObtenerUsuarioId(); ;
			var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);

			if ( yaExisteTipoCuenta )
			{
				return Json($"El nombre {nombre} ya existe");
			}

			return Json( true );
		}

		//Actualiza el orden de los Tipos Cuentas
		[HttpPost]
		public async Task<IActionResult> Ordenar([FromBody] int[] ids)
		{
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			//Obtener un usuario por el id parea verificar la lista
			var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
			//Compara los ids ya registrados con los nuevos
			//esto para evitar que el usuario envie datos errados
			var idsTiposCuentas = tiposCuentas.Select(x => x.Id);
			//Existe un id que esté en ids y que no esté en idsTiposCuentas
			var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTiposCuentas).ToList();
			//si lo anterior encuentra un id que no está en la lista de ld BD ...
			if(idsTiposCuentasNoPertenecenAlUsuario.Count > 0)
			{
				return Forbid();
			}
			//mapea y ordena los tipos de cuenta para luego actualizar la lista
			var tiposCuentasOrdenados = ids.Select((valor, indice) => 
			    new TipoCuenta() { Id = valor, Orden = indice +1 }).AsEnumerable();

			await repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);

			return Ok();
		}
	}
}
