using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authentication.Data
{
    public class MyUserClaimsPrincipalFactory:UserClaimsPrincipalFactory<ApplicationUser>
    {
        public MyUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor): base(userManager, optionsAccessor)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Name", user.Name??"[Click to Edit Profile]"));
            identity.AddClaim(new Claim("CellPhone", user.CellPhone ?? "[Click to Edit Profile]"));
            identity.AddClaim(new Claim("Country", user.Country ?? "[Click to Edit Profile]"));
            return identity;
        }
    }
}
