using AdminDashboardApi.Data;
using AdminDashboardApi.Models;
using AdminDashboardApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Подключение контекста базы данных PostgreSQL
builder.Services.AddDbContext<AppDbContext>(opt =>
	opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем Identity с пользовательскими настройками паролей
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 6;
})
.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("AdminDashboard")
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Добавляем реализацию IAuthManager
builder.Services.AddScoped<IAuthManager, AuthManager>();

// Настройка Kestrel-сервера для прослушивания порта 5000
builder.WebHost.ConfigureKestrel(opts => opts.ListenAnyIP(80));

// Настройка аутентификации через JWT
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
	};
});

// Включаем авторизацию
builder.Services.AddAuthorization();

// Добавляем Swagger для документации API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настраиваем CORS (разрешаем всё)
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyOrigin()
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

// Создаём и запускаем приложение
var app = builder.Build();

// Инициализация базы данных и заполнение начальными данными
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var db = services.GetRequiredService<AppDbContext>();
	var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

	await db.Database.MigrateAsync();
	await SeedData.SeedDatabaseAsync(db, userManager);
}


if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}

app.UseRouting();
app.UseCors(); 
app.UseAuthentication(); 
app.UseAuthorization(); 


if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// ======= API-маршруты =======

// Группа маршрутов
var api = app.MapGroup("/api");

// Проверка авторизации
api.MapGet("/", () => "Server is running")
	.RequireAuthorization();

// ====== Аутентификация ======

// Вход пользователя
api.MapPost("/auth/login", async (LoginDto req, IAuthManager authManager) =>
{
	var authResponse = await authManager.Login(req);
	if (authResponse == null)
		return Results.Unauthorized();
	return Results.Ok(authResponse);
});

// Обновление токена
api.MapPost("/auth/refreshtoken", async (AuthResponse request, IAuthManager authManager) =>
{
	var authResponse = await authManager.VertifyRefreshToken(request);
	if (authResponse == null)
		return Results.Unauthorized();
	return Results.Ok(authResponse);
});

// ====== Работа с клиентами ======

// Получить всех клиентов с расчётом баланса
api.MapGet("/clients", async (AppDbContext db) =>
{
	var clients = db.Clients.Select(c => new ClientDto
	{
		Id = c.Id,
		Name = c.Name,
		Email = c.Email,
		Balance = db.Payments.Where(p => p.ClientId == c.Id).Sum(p => p.Amount)
	});
	return Results.Ok(await clients.ToListAsync());
});

// Получить клиента по Id
api.MapGet("/clients/{id:int}", async (int id, AppDbContext db) =>
{
	var client = await db.Clients.FindAsync(id);
	return client != null ? Results.Ok(client) : Results.NotFound();
});

// Создать нового клиента
api.MapPost("/clients", async (Client client, AppDbContext db) =>
{
	db.Clients.Add(client);
	await db.SaveChangesAsync();
	return Results.Created($"/api/clients/{client.Id}", client);
});

// Обновить клиента по Id
api.MapPut("/clients/{id:int}", async (int id, Client updatedClient, AppDbContext db) =>
{
	var client = await db.Clients.FindAsync(id);
	if (client == null)
		return Results.NotFound();

	client.Name = updatedClient.Name;
	client.Email = updatedClient.Email;
	await db.SaveChangesAsync();

	return Results.Ok(client);
});

// Удалить клиента по Id
api.MapDelete("/clients/{id:int}", async (int id, AppDbContext db) =>
{
	var client = await db.Clients.FindAsync(id);
	if (client == null)
		return Results.NotFound();

	db.Clients.Remove(client);
	await db.SaveChangesAsync();

	return Results.NoContent();
});

// ====== Платежи ======

// Получить последние N платежей
api.MapGet("/payments", async (AppDbContext db, int take = 5) =>
{
	var payments = await db.Payments
		.Include(p => p.Client)
		.OrderByDescending(p => p.Date)
		.Take(take)
		.ToListAsync();

	var paymentsDto = payments.Select(p => new
	{
		p.Id,
		p.Amount,
		p.Date,
		ClientName = p.Client.Name,
		ClientEmail = p.Client.Email
	});

	return Results.Ok(paymentsDto);
});

// ====== Курс валюты ======

// Получить текущий курс
api.MapGet("/rate", async (AppDbContext db) =>
{
	var rate = await db.ExchangeRates.OrderByDescending(r => r.UpdatedAt).FirstOrDefaultAsync();
	return rate != null ? Results.Ok(rate) : Results.NotFound();
});

// Обновить курс
api.MapPost("/rate", async (AppDbContext db, ExchangeRate updatedRate) =>
{
	var rate = await db.ExchangeRates.OrderByDescending(r => r.UpdatedAt).FirstOrDefaultAsync();

	if (rate == null)
	{
		updatedRate.UpdatedAt = DateTime.UtcNow;
		db.ExchangeRates.Add(updatedRate);
	}
	else
	{
		rate.Rate = updatedRate.Rate;
		rate.UpdatedAt = DateTime.UtcNow;
	}

	await db.SaveChangesAsync();
	return Results.Ok(updatedRate);
});


app.Run();
