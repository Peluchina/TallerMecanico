using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TallerMecanico.Application.Commands;
using TallerMecanico.Application.Queries;
using TallerMecanico.Application.Services;
using TallerMecanico.Application.Validators;
using TallerMecanico.Domain.Contracts;
using TallerMecanico.Infraestructure.Data;
using TallerMecanico.Infraestructure.Repositories;
using TallerMecanico.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<TimeOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new Microsoft.OpenApi.Any.OpenApiString("00:00:00")
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT enviado por la API después del inicio de sesión con Google (ejemplo: Bearer eyJhbGci...)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuración de Autenticación JWT Bearer
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ?? "SuperSecretKeyForTallerMecanicoJwtAuthentication12345!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "TallerMecanicoAPI",
        ValidAudience = jwtSettings["Audience"] ?? "TallerMecanicoClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Infrastructure DI & Services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IConnectionSQL, ConnectionSQL>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IServiceScheduleRepository, ServiceScheduleRepository>();
builder.Services.AddScoped<IProductScheduleRepository, ProductScheduleRepository>();
builder.Services.AddScoped<IProductRespository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

// Application DI & Handlers - Customer
builder.Services.AddScoped<CustomerCreateCommandHandler>();
builder.Services.AddScoped<CustomerUpdateCommandHandler>();
builder.Services.AddScoped<CustomerGetQueryHandler>();
builder.Services.AddScoped<CustomerListQueryHandler>();

// Application DI & Handlers - ServiceSchedule
builder.Services.AddScoped<ServiceScheduleCreateCommandHandler>();
builder.Services.AddScoped<ServiceScheduleUpdateCommandHandler>();
builder.Services.AddScoped<ServiceScheduleUpdateStatusCommandHandler>();
builder.Services.AddScoped<ServiceScheduleGetQueryHandler>();
builder.Services.AddScoped<ServiceScheduleListQueryHandler>();

// Application DI & Handlers - ProductSchedule
builder.Services.AddScoped<ProductScheduleCreateCommandHandler>();
builder.Services.AddScoped<ProductScheduleUpdateCommandHandler>();
builder.Services.AddScoped<ProductScheduleUpdateStatusCommandHandler>();
builder.Services.AddScoped<ProductScheduleGetQueryHandler>();
builder.Services.AddScoped<ProductScheduleListQueryHandler>();

// Application DI & Handlers - Product
builder.Services.AddScoped<ProductCreateCommandHandler>();
builder.Services.AddScoped<ProductUpdateCommandHandler>();
builder.Services.AddScoped<ProductUpdateStatusCommandHandler>();
builder.Services.AddScoped<ProductGetQueryHandler>();
builder.Services.AddScoped<ProductListQueryHandler>();

// Application DI & Handlers - Supplier
builder.Services.AddScoped<SupplierCreateCommandHandler>();
builder.Services.AddScoped<SupplierUpdateCommandHandler>();
builder.Services.AddScoped<SupplierUpdateStatusCommandHandler>();
builder.Services.AddScoped<SupplierGetQueryHandler>();
builder.Services.AddScoped<SupplierListQueryHandler>();

// Application DI & Handlers - Employee
builder.Services.AddScoped<EmployeeCreateCommandHandler>();
builder.Services.AddScoped<EmployeeUpdateCommandHandler>();
builder.Services.AddScoped<EmployeeUpdateStatusCommandHandler>();
builder.Services.AddScoped<EmployeeGetQueryHandler>();
builder.Services.AddScoped<EmployeeListQueryHandler>();

// Application DI & Handlers - Service
builder.Services.AddScoped<ServiceCreateCommandHandler>();
builder.Services.AddScoped<ServiceUpdateCommandHandler>();
builder.Services.AddScoped<ServiceUpdateStatusCommandHandler>();
builder.Services.AddScoped<ServiceGetQueryHandler>();
builder.Services.AddScoped<ServiceListQueryHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CustomerCreateCommandValidator>();

// Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
