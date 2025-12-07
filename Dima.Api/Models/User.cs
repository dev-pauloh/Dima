using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Models;

public class User : IdentityUser<long> 
{
    //RBAC - Role Based Access Control
    public List<IdentityRole<long>>? Roles { get; set; }
}