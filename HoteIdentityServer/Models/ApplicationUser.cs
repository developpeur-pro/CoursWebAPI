using Microsoft.AspNetCore.Identity;

namespace HoteIdentityServer.Models
{
   public class ApplicationUser : IdentityUser<int>
   {
      public string Fonction { get; set; } = "";
   }
}
