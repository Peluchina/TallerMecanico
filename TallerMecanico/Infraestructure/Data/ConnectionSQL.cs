using System.Data;
using Microsoft.Data.SqlClient;

namespace TallerMecanico.Infraestructure.Data
{
    public class ConnectionSQL : IConnectionSQL
    {
        private readonly string _connectionString;

        public ConnectionSQL(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PPA")
                                ?? throw new InvalidOperationException("La cadena de conexión 'PPA' no se encuentra configurada en appsettings.json.");
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
