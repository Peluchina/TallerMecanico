using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IConnectionSQL _connectionSQL;

        public SupplierRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            var list = new List<Supplier>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Supplier_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Supplier
                {
                    SupplierId = reader.GetInt64(0),
                    Ruc = reader.GetString(1),
                    LegalName = reader.GetString(2),
                    Address = reader.GetString(3),
                    Phone = reader.GetString(4),
                    SupplierStatus = Enum.Parse <Status>(reader.GetString(5))
                });
            }

            return list;
        }

        public async Task<Supplier?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Supplier_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@SupplierId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Supplier
                {
                    SupplierId = reader.GetInt64(0),
                    Ruc = reader.GetString(1),
                    LegalName = reader.GetString(2),
                    Address = reader.GetString(3),
                    Phone = reader.GetString(4),
                    SupplierStatus = Enum.Parse<Status>(reader.GetString(5))
                };
            }

            return null;
        }

        public async Task<long> Insert(Supplier supplier)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Supplier_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Ruc", supplier.Ruc);
            command.Parameters.AddWithValue("@LegalName", supplier.LegalName);
            command.Parameters.AddWithValue("@Address", supplier.Address);
            command.Parameters.AddWithValue("@Phone", supplier.Phone);

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<Supplier?> Update(Supplier supplier)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Supplier_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
            command.Parameters.AddWithValue("@Ruc", supplier.Ruc);
            command.Parameters.AddWithValue("@LegalName", supplier.LegalName);
            command.Parameters.AddWithValue("@Address", supplier.Address);
            command.Parameters.AddWithValue("@Phone", supplier.Phone);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Supplier
                {
                    SupplierId = reader.GetInt64(0),
                    Ruc = supplier.Ruc ?? "",
                    LegalName = reader.GetString(1),
                    Address = reader.GetString(2),
                    Phone = reader.GetString(3)
                };
            }

            return null;
        }

        public async Task<Supplier?> UpdateStatus(long id, string status)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Supplier_UpdateStatus", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SupplierId", id);
            command.Parameters.AddWithValue("@SupplierStatus", status);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Supplier
                {
                    SupplierId = reader.GetInt64(0),
                    Ruc = "",
                    LegalName = reader.GetString(1),
                    Address = reader.GetString(2),
                    Phone = reader.GetString(3),
                    SupplierStatus = Enum.Parse<Status>(reader.GetString(4))
                };
            }

            return null;
        }
    }
}

