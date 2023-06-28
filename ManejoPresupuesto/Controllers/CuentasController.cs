using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;
		private readonly IRepositorioCuentas repositorioCuentas;

		//Para obtener la lista de los tipos de cuentas del usuario
		public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, 
            IServicioUsuarios servicioUsuarios, IRepositorioCuentas repositorioCuentas)
        {
            //Obtener los tipos de cuentas del usuario
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            //Obtener el id del usuario
            this.servicioUsuarios = servicioUsuarios;
			this.repositorioCuentas = repositorioCuentas;
		}
        //Retorna la vista para crear una Cuenta
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            //Crea un modelo con la apariencia de CuentaCreacionViewModel
            var modelo = new CuentaCreacionViewModel();
            
            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }

        //Creamos la Cuenta
        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            //Verificar que los datos enviados por el usuario son válidos
			var usuarioId = servicioUsuarios.ObtenerUsuarioId();
			var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(cuenta.TipoCuentaId, usuarioId);

            if (tipoCuenta is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid) 
            {
				cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
			}

            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");
		}

        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId)
		{
			var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
			//Crea una lista de items y los retorna
			//Para tener un IEnumerable, debemos tener un SelectListItem, por eso se mapea
			return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
		}
    }
}
