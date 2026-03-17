using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StayEase.DAL.Models;
using System.Security.Claims;

namespace StayEase.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelTranslation> HotelTranslations { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor)
         : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");


            builder.Entity<Room>()
                .HasIndex(r => new { r.HotelId, r.RoomNumber })
                .IsUnique();

        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var entries = ChangeTracker.Entries<BaseModel>();
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }





        public override int SaveChanges()
        {
            if (_httpContextAccessor.HttpContext != null)
            {

                var entries = ChangeTracker.Entries<BaseModel>();
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entityEntry in entries)
                {
                    if (entityEntry.State == EntityState.Added)
                    {
                        entityEntry.Property(x => x.CreatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    }
                    else if (entityEntry.State == EntityState.Modified)
                    {
                        entityEntry.Property(x => x.UpdatedBy).CurrentValue = currentUserId;
                        entityEntry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
