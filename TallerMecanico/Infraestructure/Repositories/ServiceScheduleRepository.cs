using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class ServiceScheduleRepository : IServiceScheduleRepository
    {
        private readonly IConnectionSQL _connectionSQL;

        public ServiceScheduleRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<ServiceSchedule>> GetAll()
        {
            var list = new List<ServiceSchedule>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ServiceSchedule_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ServiceSchedule
                {
                    ServiceScheduleId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    EmployeeId = reader.GetInt64(2),
                    ServiceId = reader.GetInt64(3),
                    ScheduledDate = reader.GetDateTime(4),
                    ServiceScheduleStatus = Enum.Parse<StatusSchedule>(reader.GetString(5))
                });
            }

            return list;
        }

        public async Task<ServiceSchedule?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ServiceSchedule_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ServiceScheduleId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ServiceSchedule
                {
                    ServiceScheduleId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    EmployeeId = reader.GetInt64(2),
                    ServiceId = reader.GetInt64(3),
                    ScheduledDate = reader.GetDateTime(4),
                    ServiceScheduleStatus = Enum.Parse<StatusSchedule>(reader.GetString(5))
                };
            }

            return null;
        }

        public async Task<long> Insert(ServiceSchedule serviceSchedule)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ServiceSchedule_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerId", serviceSchedule.CustomerId);
            command.Parameters.AddWithValue("@EmployeeId", serviceSchedule.EmployeeId);
            command.Parameters.AddWithValue("@ServiceId", serviceSchedule.ServiceId);
            command.Parameters.AddWithValue("@ScheduledDate", serviceSchedule.ScheduledDate);
            

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<ServiceSchedule?> Update(ServiceSchedule serviceSchedule)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ServiceSchedule_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ServiceScheduleId", serviceSchedule.ServiceScheduleId);
            command.Parameters.AddWithValue("@CustomerId", serviceSchedule.CustomerId);
            command.Parameters.AddWithValue("@EmployeeId", serviceSchedule.EmployeeId);
            command.Parameters.AddWithValue("@ServiceId", serviceSchedule.ServiceId);
            command.Parameters.AddWithValue("@ScheduledDate", serviceSchedule.ScheduledDate);
            

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ServiceSchedule
                {
                    ServiceScheduleId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    EmployeeId = reader.GetInt64(2),
                    ServiceId = reader.GetInt64(3),
                    ScheduledDate = reader.GetDateTime(4)
                };
            }

            return null;
        }

        public async Task<ServiceSchedule?> UpdateStatus(long id, string status)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ServiceSchedule_UpdateStatus", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ServiceScheduleId", id);
            command.Parameters.AddWithValue("@ServiceScheduleStatus", status);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ServiceSchedule
                {
                    ServiceScheduleId = reader.GetInt64(0),
                    CustomerId = reader.GetInt64(1),
                    EmployeeId = reader.GetInt64(2),
                    ServiceId = reader.GetInt64(3),
                    ScheduledDate = reader.GetDateTime(4),
                    ServiceScheduleStatus = Enum.Parse<StatusSchedule>(reader.GetString(5))
                };
            }

            return null;
        }
    }
}

