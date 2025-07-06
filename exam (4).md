
# ✅ Реализация авторизации по ролям с использованием JWT Bearer

## 🎯 Цель

Реализовать авторизацию по ролям в проекте **ASP.NET Core Web API** с использованием JWT Bearer. Доступ к endpoint'ам должен контролироваться на основе ролей пользователя: `Admin`, `Manager`, `User`.

---

## 📌 Требования

### 1. Настройка Identity и ролей

- Использовать `IdentityUser` и `IdentityRole`.
- При необходимости создать кастомную модель пользователя, унаследовав от `IdentityUser`.

### 2. Настройка JWT аутентификации

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });

services.AddAuthorization();
```

### 3. Генерация JWT с ролями

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(ClaimTypes.NameIdentifier, user.Id)
};

var roles = await _userManager.GetRolesAsync(user);
foreach (var role in roles)
{
    claims.Add(new Claim(ClaimTypes.Role, role));
}
```

---

## 🔐 Контроллер аутентификации (AuthController / AccountController)

### 🔑 POST `/api/auth/login`

Авторизация и получение JWT:

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto model) { ... }
```

### 📝 POST `/api/auth/register`

Регистрация нового пользователя и добавление в роль:

```csharp
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterDto model) { ... }
```

### 🔒 POST `/api/auth/change-password`

Смена пароля текущим пользователем:

```csharp
[Authorize]
[HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model) { ... }
```

---

## 👤 UsersController

Контроллер для управления пользователями (доступ только админам):

- `GET /api/users` — получить список пользователей
- `GET /api/users/{id}` — получить пользователя по ID
- `PUT /api/users/{id}` — обновление пользователя
- `DELETE /api/users/{id}` — удаление пользователя

Используется `UserManager` и `RoleManager`.

---

## 🔒 Защита endpoint'ов по ролям

```csharp
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet("admin-data")]
    public IActionResult GetAdminData()
    {
        return Ok("Только для админов.");
    }
}
```

---

## 🧪 Тестирование

| Сценарий | Результат |
|----------|-----------|
| 🔐 Токен с ролью Admin | ✅ Доступ разрешён |
| 👤 Токен обычного пользователя | ❌ 403 Forbidden |
| 🚫 Без токена или с неверным | ❌ 401 Unauthorized |

---

## ⚙ Инициализация ролей при старте

```csharp
if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}
```

---

## 📌 Рекомендации

- Использовать константы или `enum` для ролей:

```csharp
public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string User = "User";
}
```
---
