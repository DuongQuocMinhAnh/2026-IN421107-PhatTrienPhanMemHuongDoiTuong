using EduHub.Application.Clubs;
using EduHub.Domain.Interfaces;
using EduHub.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ── Đăng ký services (Dependency Injection) ──────────────────────────────
// TODO 6a: đăng ký IClubRepository → InMemoryClubRepository (AddScoped)
// TODO 6b: đăng ký IMemberRepository → InMemoryMemberRepository (AddScoped)
// TODO 6c: đăng ký 4 handler: GetClubsQueryHandler, GetClubByIdQueryHandler,
//          CreateClubCommandHandler, DeleteClubCommandHandler (AddScoped)
builder.Services.AddScoped<IClubRepository, InMemoryClubRepository>();
builder.Services.AddScoped<IMemberRepository, InMemoryMemberRepository>();

builder.Services.AddScoped<GetClubsQueryHandler>();
builder.Services.AddScoped<GetClubByIdQueryHandler>();
builder.Services.AddScoped<CreateClubCommandHandler>();
builder.Services.AddScoped<DeleteClubCommandHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// TODO 7: cấu hình Swagger — SwaggerDoc "v1" với Title = "EduHub API"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "EduHub API", Version = "v1" });
});

var app = builder.Build();

// ── Middleware pipeline ───────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
