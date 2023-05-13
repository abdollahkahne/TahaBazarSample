using Microsoft.EntityFrameworkCore;
using SampleForTahaBazar.Entities;
using SampleForTahaBazar.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

// Add services to the container.
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
builder.Services.AddRazorPages().AddMvcLocalization();
builder.Services.AddDbContext<SaleDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));
builder.Services.AddScoped<ISaleService, SaleService>();// This is scoped since DbContext is scoped so this should be scoped or transient
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IUnitOfMeasurementService, UnitOfMeasurementService>();

var app = builder.Build();

// Use ServerLocator (anti)pattern to create database if it does not exist yet
using (var scope = app.Services.CreateScope())
{
    var _dbContext = scope.ServiceProvider.GetRequiredService<SaleDbContext>();
    await _dbContext.Database.EnsureCreatedAsync();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
