using SistemaVoltCar.Models;
using SistemaVoltCar.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Injeção de dependência
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<FornecedorRepositorio>();
builder.Services.AddScoped<VeiculoRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Login}/{id?}");

app.Run();
