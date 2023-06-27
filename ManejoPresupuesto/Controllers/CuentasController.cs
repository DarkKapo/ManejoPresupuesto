using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        //Para obtener la lista de los tipos de cuentas del usuario
        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
        {
            //Obtener los tipos de cuentas del usuario
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            //Obtener el id del usuario
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            //Crea un modelo con la apariencia de CuentaCreacionViewModel
            var modelo = new CuentaCreacionViewModel();
            //Crea una lista de items y los guarda en la variable TiposCuenta
            //Para tener un IEnumerable, debemos tener un SelectListItem, por eso se mapea
            modelo.TiposCuentas = tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            return View(modelo);
        }
    }
}
