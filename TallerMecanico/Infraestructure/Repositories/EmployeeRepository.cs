using System.Data;
using Microsoft.Data.SqlClient;
using TallerMecanico.Domain.Agregates;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;
using TallerMecanico.Infraestructure.Data;

namespace TallerMecanico.Infraestructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IConnectionSQL _connectionSQL;

        public EmployeeRepository(IConnectionSQL connectionSQL)
        {
            _connectionSQL = connectionSQL;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var list = new List<Employee>();
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Employee_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapEmployee(reader));
            }

            return list;
        }

        public async Task<Employee?> GetById(long id)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Employee_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmployeeId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapEmployee(reader);
            }

            return null;
        }

        public async Task<Employee?> GetByEmail(string email)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Employee_GetByEmail", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmailAddress", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapEmployee(reader);
            }

            return null;
        }

        public async Task<long> Insert(Employee employee)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Employee_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Dni", employee.Dni);
            command.Parameters.AddWithValue("@Address", employee.Address);
            command.Parameters.AddWithValue("@Phone", employee.Phone);
            command.Parameters.AddWithValue("@Role", employee.Role);
            command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
            command.Parameters.AddWithValue("@Password", employee.Password);

            var insertedId = await command.ExecuteScalarAsync();
            return Convert.ToInt64(insertedId);
        }

        public async Task<Employee?> Update(Employee employee)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Employee_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Dni", employee.Dni);
            command.Parameters.AddWithValue("@Address", employee.Address);
            command.Parameters.AddWithValue("@Phone", employee.Phone);
            command.Parameters.AddWithValue("@Role", employee.Role);
            command.Parameters.AddWithValue("@EmailAddress", employee.EmailAddress);
            command.Parameters.AddWithValue("@Password", employee.Password);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapEmployee(reader);
            }

            return null;
        }

        public async Task<Employee?> UpdateStatus(long id, string status)
        {
            using var connection = (SqlConnection)_connectionSQL.GetConnection();
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_Employee_UpdateStatus", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EmployeeId", id);
            command.Parameters.AddWithValue("@EmployeeStatus", status);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapEmployee(reader);
            }

            return null;
        }

        private static Employee MapEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = reader.GetInt64(0),
                FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Dni = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Address = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Phone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                Role = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                EmailAddress = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                Password = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                EmployeeStatus = Enum.Parse<Status>(reader.GetString(9))
            };
        }
    }
}
