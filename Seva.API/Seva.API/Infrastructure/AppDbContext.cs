namespace Seva.API.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Seva.API.Infrastructure.Entities;

    public class AppDbContext : IdentityDbContext<LoginUser>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LoginUser>(entity => { entity.ToTable(name: "LoginUsers"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });

            builder.Entity<Employee>(entity => { entity.ToTable("Employees"); });
            builder.Entity<Project>(entity => { entity.ToTable("Projects"); });
            builder.Entity<ProjectEmployeeMap>(entity => { entity.ToTable("ProjectEmployeeMap"); });

            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }
}
