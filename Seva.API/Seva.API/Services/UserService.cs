namespace Seva.API.Services
{
    using Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Models.Auth;
    using static Google.Apis.Auth.GoogleJsonWebSignature;

    public interface IUserService
    {
        Task<LoginUser> AuthenticateGoogleUserAsync(GoogleUserRequest request);
    }

    public class UserService : IUserService
    {
        protected readonly UserManager<LoginUser> _userManager;
        public UserService(UserManager<LoginUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<LoginUser> AuthenticateGoogleUserAsync(GoogleUserRequest request)
        {
            Payload payload = await ValidateAsync(request.IdToken, new ValidationSettings
            {
                Audience = new[] { Startup.StaticConfig["Authentication:Google:ClientId"] }
            });

            return await GetOrCreateExternalLoginUser(GoogleUserRequest.PROVIDER, payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);
        }


        private async Task<LoginUser> GetOrCreateExternalLoginUser(string provider, string key, string email, string firstName, string lastName)
        {
            var user = await _userManager.FindByLoginAsync(provider, key);
            if (user != null)
                return user;
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new LoginUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Id = key,
                };
                await _userManager.CreateAsync(user);
            }

            var info = new UserLoginInfo(provider, key, provider.ToUpperInvariant());
            var result = await _userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
                return user;
            return null;

        }
    }
}
