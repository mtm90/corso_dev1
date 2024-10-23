using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    // Aggiungi qui le proprietà personalizzate
    public string Codice {get; set;}

}