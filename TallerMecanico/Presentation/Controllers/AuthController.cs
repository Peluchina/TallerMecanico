using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Application.DTOs;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Domain.Enums;

namespace TallerMecanico.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;

        public AuthController(
            IConfiguration configuration,
            IEmployeeRepository employeeRepository)
        {
            _configuration = configuration;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("google-login")]
        public async Task<ActionResult<UserAuthResponse>> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.IdToken))
            {
                return BadRequest(new { message = "El token de Google es requerido." });
            }

            try
            {
                string email;
                string? picture = null;

                // Modo prueba / Demo si se envía token de prueba en desarrollo
                if (request.IdToken == "demo-token-google")
                {
                    email = "jhoel@gmail.com";
                    picture = "https://lh3.googleusercontent.com/a/default-user";
                }
                else
                {
                    var googleClientId = _configuration["Google:ClientId"];
                    
                    var settings = new GoogleJsonWebSignature.ValidationSettings();
                    if (!string.IsNullOrEmpty(googleClientId) && googleClientId != "YOUR_GOOGLE_CLIENT_ID.apps.googleusercontent.com")
                    {
                        settings.Audience = new[] { googleClientId };
                    }

                    var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
                    email = payload.Email;
                    picture = payload.Picture;
                }

                // Verificar existencia y estado del empleado en dbo.Employee
                var employee = await _employeeRepository.GetByEmail(email);

                if (employee == null)
                {
                    return Unauthorized(new { message = $"El correo de Google '{email}' no está registrado como empleado en el taller." });
                }

                if (employee.EmployeeStatus != Status.ACTIVO)
                {
                    return Unauthorized(new { message = $"El empleado '{employee.FirstName} {employee.LastName}' ({email}) no está en estado ACTIVO." });
                }

                return Ok(new UserAuthResponse
                {
                    Email = employee.EmailAddress,
                    Name = $"{employee.FirstName} {employee.LastName}".Trim(),
                    Picture = picture,
                    Token = request.IdToken
                });
            }
            catch (InvalidJwtException ex)
            {
                return Unauthorized(new { message = "Token de Google no válido.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al autenticar con Google.", details = ex.Message });
            }
        }
    }
}
