using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoofingConcept.Data.Entities;

namespace RoofingConcept.Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUserEntity>(options)
{

}

    

