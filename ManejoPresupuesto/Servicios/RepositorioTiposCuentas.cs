using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
	public interface IRepositorioTiposCuentas
	{
		Task Crear(TipoCuenta tipoCuenta);
	}
	public class RepositorioTiposCuentas: IRepositorioTiposCuentas
	{
		private readonly string connectionString;
		public RepositorioTiposCuentas(IConfiguration configuration) 
		{
			connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task Crear( TipoCuenta tipoCuenta )
		{
			using var connection = new SqlConnection( connectionString );
			//QuerySingle se usa cuando el resultado de la query tiene 1 solo resultado
			//SCOPE_IDENTITY() retirna el Id del registro creado
			var id = await connection.QuerySingleAsync<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden)
													Values(@Nombre, @UsuarioId, 0);
													SELECT SCOPE_IDENTITY();", tipoCuenta);
			//Agrega el id extraído a tipoCuenta
			tipoCuenta.Id = id;
		}
	}
}
