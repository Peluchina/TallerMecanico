using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class ProductRepository : IProductRespository
    {
        private readonly IConnectionSQL _connectionSQL;

        public ProductRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var list = new List<Product>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Product_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Product
                {
                    ProductId = reader.GetInt64(0),
                    ProductName = reader.GetString(1),
                    SupplierId = reader.GetInt64(2),
                    Brand = reader.GetString(3),
                    Price = reader.GetDecimal(4),
                    Stock = reader.GetInt64(5),
                    ProductStatus = Enum.Parse <Status> (reader.GetString(6))
                });
            }

            return list;
        }

        public async Task<Product?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Product_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    ProductId = reader.GetInt64(0),
                    ProductName = reader.GetString(1),
                    SupplierId = reader.GetInt64(2),
                    Brand = reader.GetString(3),
                    Price = reader.GetDecimal(4),
                    Stock = reader.GetInt64(5),
                    ProductStatus = Enum.Parse<Status>(reader.GetString(6))
                };
            }

            return null;
        }

        public async Task<long> Insert(Product product)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Product_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@SupplierId", product.SupplierId);
            command.Parameters.AddWithValue("@Brand", product.Brand);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Stock", product.Stock);
            

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<Product?> Update(Product product)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Product_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", product.ProductId);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@SupplierId", product.SupplierId);
            command.Parameters.AddWithValue("@Brand", product.Brand);
            command.Parameters.AddWithValue("@Price", product.Price);
            command.Parameters.AddWithValue("@Stock", product.Stock);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    ProductId = reader.GetInt64(0),
                    ProductName = reader.GetString(1),
                    SupplierId = reader.GetInt64(2),
                    Brand = reader.GetString(3),
                    Price = reader.GetDecimal(4),
                    Stock = reader.GetInt64(5),
                    ProductStatus = Enum.Parse<Status>(reader.GetString(6))
                };
            }

            return null;
        }

        public async Task<Product?> UpdateStatus(long id, string status)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Product_UpdateStatus", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", id);
            command.Parameters.AddWithValue("@ProductStatus", status);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Product
                {
                    ProductId = reader.GetInt64(0),
                    ProductName = reader.GetString(1),
                    SupplierId = reader.GetInt64(2),
                    Brand = reader.GetString(3),
                    Price = reader.GetDecimal(4),
                    Stock = reader.GetInt64(5),
                    ProductStatus = Enum.Parse<Status>(reader.GetString(6))
                };
            }

            return null;
        }
    }
}

