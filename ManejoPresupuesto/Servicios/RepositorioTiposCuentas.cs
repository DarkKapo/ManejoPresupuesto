using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
	public interface IRepositorioTiposCuentas
	{
		Task Crear(TipoCuenta tipoCuenta);
		Task<bool> Existe(string nombre, int usuarioId);
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
			var id = await connection.QuerySingleAsync<int>(@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden)
													Values(@Nombre, @UsuarioId, 0);
													SELECT SCOPE_IDENTITY();", tipoCuenta);
			//Agrega el id extraído a tipoCuenta
			tipoCuenta.Id = id;
		}

		//Verifica si existe el tipo cuenta ingresado
		public async Task<bool> Existe( string nombre, int usuarioId )
		{
			using var connection = new SqlConnection( connectionString );
			//Devuelve el primer registro o un valor por defecto (0)
			var existe = await connection.QueryFirstOrDefaultAsync<int>(
										@"SELECT 1
										FROM TiposCuentas
										WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
										new { nombre, usuarioId });
			return existe == 1;
		}
	}
}
