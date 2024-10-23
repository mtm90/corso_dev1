# MVCUser 

dotnet new mvc  --auth Individual -o MVCUser

 ### Installo Identity UI che serve a creare le pagine per la registrazione e il login

dotnet add package Microsoft.AspNetCore.Identity.UI


### Creazione di un utente che estende IdentityUser Nella cartella Models, creo un nuovo file chiamato AppUser.cs e definisco il modello come segue:

```csharp

public class AppUser : IdentityUser
{
    // Aggiungi qui le proprietà personalizzate
    public string Codice {get; set;}
}
```

### Nella cartella Data, aggiorno ApplicationDbContext per utilizzare il nuoco modello utente esteso invece di IdentityUser.

```csharp
// cambio questa riga
public class ApplicationDbContext : IdentityDbContext
// in
public class ApplicationDbContext : IdentityDbContext<AppUser>
```

Modifiche al Program.cs

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true) // cambio identityUser in AppUser
    .AddRoles<IdentityRole>() // aggiunta del ruolo
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();


// aggiunto routing gestito da program cs dove andiamo a defniire rotte specifiche
app.UseEndpoints(endpoints =>
{
    endpoints.MapcontrollerRoute(
        name: "user",
        pattern: "User/{email}",
        defaults: new { controller = "Users", action = "Index"}
    );
});



// Seeding del Database
using (var scope = App.Services.CreateScope())
{
    var serviceprovider = scope.ServiceProvider;
    try
    {
        var userManager = serviceProvider.GetrequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetrequiredService<RoleManager<IdentityRole>>();
        await SeedData.InitializeAsync( userManager, roleManager);
    }
    catch (Exceeption ex)
    {
        var logger = serviceProvider.GetRequiredService<Ilogger<Program>>();
        logger.LogError(ex, "Un errore è avvenuto durante il seeding del Database.");
    }
}

// dobbaimo crare il file SeedData con i ruoli nella cartella Data

public class SeedData
{
    public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Creazione dei ruoli se non esistono 
        string[] roleNames = {"Admin", "Fornitore", "Cliente"};
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Creazione dell'utente Admin se non esiste
        if (await userManager.FindByEmailAsync("admin@admin.com") == null)
        {
            var adminUser = new AppUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Nome = "Admin",
                Codice = "12345678", // Genera un codice univoco
                EmailConfirmed = true // Accettazione in automatico
            };
            await userManager.CreateAsync(adminUser, "AdminPass1!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else 
        {
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

```


## Migrazione ed aggiornamento del Database


dotnet ef migrations add InitialCreate
dotnet ef database update