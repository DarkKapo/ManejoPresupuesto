using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejoPresupuesto.Models
{
    public class CuentaCreacionViewModel : Cuenta
    {
        //SelectList permite crear una lista de objetos fácilmente
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}
