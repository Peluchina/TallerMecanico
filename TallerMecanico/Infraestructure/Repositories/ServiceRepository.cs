using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IConnectionSQL _connectionSQL;

        public ServiceRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            var list = new List<Service>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Service_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapService(reader));
            }

            return list;
        }

        public async Task<Service?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Service_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ServiceId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapService(reader);
            }

            return null;
        }

        public async Task<long> Insert(Service service)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Service_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ServiceName", service.ServiceName);
            command.Parameters.AddWithValue("@Description", service.Description);
            command.Parameters.AddWithValue("@Price", service.Price);
            command.Parameters.AddWithValue("@EstimatedDuration", service.EstimatedDuration.ToTimeSpan());

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<Service?> Update(Service service)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Service_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ServiceId", service.ServiceId);
            command.Parameters.AddWithValue("@ServiceName", service.ServiceName);
            command.Parameters.AddWithValue("@Description", service.Description);
            command.Parameters.AddWithValue("@Price", service.Price);
            command.Parameters.AddWithValue("@EstimatedDuration", service.EstimatedDuration.ToTimeSpan());

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapService(reader);
            }

            return null;
        }

        public async Task<Service?> UpdateStatus(long id, string status)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Service_UpdateStatus", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ServiceId", id);
            command.Parameters.AddWithValue("@ServiceStatus", status);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapService(reader);
            }

            return null;
        }

        private static Service MapService(SqlDataReader reader)
        {
            return new Service
            {
                ServiceId = reader.GetInt64(0),
                ServiceName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Price = reader.IsDBNull(3) ? 0m : reader.GetDecimal(3),
                EstimatedDuration = reader.IsDBNull(4) ? TimeOnly.MinValue : TimeOnly.FromTimeSpan(reader.GetTimeSpan(4)),
                ServiceStatus = Enum.Parse<Status>(reader.GetString(5))
            };
        }
    }
}
