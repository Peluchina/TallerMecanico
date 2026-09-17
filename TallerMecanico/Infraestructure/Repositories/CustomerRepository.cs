using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConnectionSQL _connectionSQL;

        public CustomerRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            var customers = new List<Customer>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Customer_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(MapCustomer(reader));
            }

            return customers;
        }

        public async Task<Customer?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Customer_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustomerId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapCustomer(reader);
            }

            return null;
        }

        public async Task<long> Insert(Customer customer)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Customer_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Dni", customer.Dni);
            command.Parameters.AddWithValue("@Address", customer.Address);
            command.Parameters.AddWithValue("@Phone", customer.Phone);

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<Customer?> Update(Customer customer)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Customer_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Dni", customer.Dni);
            command.Parameters.AddWithValue("@Address", customer.Address);
            command.Parameters.AddWithValue("@Phone", customer.Phone);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapCustomer(reader);
            }

            return null;
        }

        private static Customer MapCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerId = reader.GetInt64(0),
                FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Dni = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Phone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            };
        }
    }
}
