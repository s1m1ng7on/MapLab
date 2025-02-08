using MapLab.Data.Entities;
using MapLab.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MapLab.Data.Managers
{
    public class ProfileManager<TUser> : UserManager<TUser> where TUser : class, IAuditInfo
    {
        public ProfileManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            if (user.CreatedOn == default) user.CreatedOn = DateTime.Now;
            return base.CreateAsync(user, password);
        }

        public override Task<IdentityResult> UpdateAsync(TUser user)
        {
            user.UpdatedOn = DateTime.Now;
            return base.UpdateAsync(user);
        }
    }
}
