using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeds;

public class DefaultUsers
{
    public static async Task SeedUserAsync(UserManager<IdentityUser> userManager)
    {
        var existing = await userManager.FindByNameAsync("Admin");
        if (existing != null)
        {
            return;
        }

        var user = new IdentityUser()
        {
            UserName = "Admin",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "000000000",
            PhoneNumberConfirmed = true
        };

        await userManager.CreateAsync(user, "0000");
        await userManager.AddToRoleAsync(user, "Admin");
    }
}
