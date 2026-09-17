using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace TallerMecanico.Presentation.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción capturada por Middleware: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            object response;

            switch (exception)
            {
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new
                    {
                        status = (int)statusCode,
                        message = "Errores de validación en los datos enviados.",
                        errors = validationEx.Errors.Select(e => new { campo = e.PropertyName, error = e.ErrorMessage })
                    };
                    break;

                case SqlException sqlEx when sqlEx.Number == 547: 
                    statusCode = HttpStatusCode.BadRequest;
                    response = new
                    {
                        status = (int)statusCode,
                        message = "Violación de clave foránea: El ID referenciado (Cliente, Empleado, Servicio, Producto o Agendamiento) no existe en la base de datos.",
                        detail = sqlEx.Message
                    };
                    break;

                case SqlException sqlEx when sqlEx.Number == 2627 || sqlEx.Number == 2601: 
                    statusCode = HttpStatusCode.Conflict;
                    response = new
                    {
                        status = (int)statusCode,
                        message = "El registro ya existe en la base de datos (Violación de restricción única).",
                        detail = sqlEx.Message
                    };
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new
                    {
                        status = (int)statusCode,
                        message = exception.Message
                    };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = new
                    {
                        status = (int)statusCode,
                        message = "Ha ocurrido un error interno en el servidor."
                    };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
