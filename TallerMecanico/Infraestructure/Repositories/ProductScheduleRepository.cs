using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class ProductScheduleRepository : IProductScheduleRepository
    {
        private readonly IConnectionSQL _connectionSQL;

        public ProductScheduleRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<ProductSchedule>> GetAll()
        {
            var list = new List<ProductSchedule>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ProductSchedule_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ProductSchedule
                {
                    ProductScheduleId = reader.GetInt64(0),
                    ProductId = reader.GetInt64(1),
                    ServiceScheduleId = reader.GetInt64(2),
                    Quantity = reader.GetInt64(3),
                    ProductUseDate = reader.GetDateTime(4),
                    ProductScheduleStatus = Enum.Parse<Status>(reader.GetString(5))
                });
            }

            return list;
        }

        public async Task<ProductSchedule?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ProductSchedule_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductScheduleId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ProductSchedule
                {
                    ProductScheduleId = reader.GetInt64(0),
                    ProductId = reader.GetInt64(1),
                    ServiceScheduleId = reader.GetInt64(2),
                    Quantity = reader.GetInt64(3),
                    ProductUseDate = reader.GetDateTime(4),
                    ProductScheduleStatus = Enum.Parse<Status>(reader.GetString(5))
                };
            }

            return null;
        }

        public async Task<long> Insert(ProductSchedule productSchedule)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ProductSchedule_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", productSchedule.ProductId);
            command.Parameters.AddWithValue("@ServiceScheduleId", productSchedule.ServiceScheduleId);
            command.Parameters.AddWithValue("@Quantity", productSchedule.Quantity);
            command.Parameters.AddWithValue("@ProductUseDate", productSchedule.ProductUseDate);

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<ProductSchedule?> Update(ProductSchedule productSchedule)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ProductSchedule_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductScheduleId", productSchedule.ProductScheduleId);
            command.Parameters.AddWithValue("@ProductId", productSchedule.ProductId);
            command.Parameters.AddWithValue("@ServiceScheduleId", productSchedule.ServiceScheduleId);
            command.Parameters.AddWithValue("@Quantity", productSchedule.Quantity);
            command.Parameters.AddWithValue("@ProductUseDate", productSchedule.ProductUseDate);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ProductSchedule
                {
                    ProductScheduleId = reader.GetInt64(0),
                    ProductId = reader.GetInt64(1),
                    ServiceScheduleId = reader.GetInt64(2),
                    Quantity = reader.GetInt64(3),
                    ProductUseDate = reader.GetDateTime(4)
                    //ProductScheduleStatus = Enum.Parse<Status>(reader.GetString(5))
                };
            }

            return null;
        }

        public async Task<ProductSchedule?> UpdateStatus(long id, string status)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_ProductSchedule_UpdateStatus", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductScheduleId", id);
            command.Parameters.AddWithValue("@ProductScheduleStatus", status.ToString());

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ProductSchedule
                {
                    ProductScheduleId = reader.GetInt64(0),
                    ProductId = reader.GetInt64(1),
                    ServiceScheduleId = reader.GetInt64(2),
                    Quantity = reader.GetInt64(3),
                    ProductUseDate = reader.GetDateTime(4),
                    ProductScheduleStatus = Enum.Parse<Status>(reader.GetString(5))
                };
            }

            return null;
        }
    }
}

