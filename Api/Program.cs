using Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Application layer (MediatR + FluentValidation + ValidationBehavior pipeline)
builder.Services.AddApplicationServices();

// Infrastructure layer (DbContext + UnitOfWork)
builder.Services.AddInfrastructureServices(builder.Configuration);

// JWT Auth + Authorization Policies
builder.Services.AddJwt(builder.Configuration);

// ── Pipeline ──────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
