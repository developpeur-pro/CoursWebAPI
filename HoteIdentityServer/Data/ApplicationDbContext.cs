using HoteIdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HoteIdentityServer.Data
{
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
   {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
      {
      }

      protected override void OnModelCreating(ModelBuilder builder)
      {
         base.OnModelCreating(builder);

         builder.Entity<ApplicationUser>(entity =>
         {
            entity.Property(e => e.Fonction).HasMaxLength(40)
               .IsUnicode(false).HasDefaultValue("");
         });
      }
   }
}
