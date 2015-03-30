using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ATS.Service.Email;

namespace ATS.Infrastructure
{
    //manage instances of ApplicationUser
    /*
    FindByIdAsync(id)	                    Find user object based on its unique identifier
    Users	                                Returns an enumeration of the users
    FindByNameAsync(Username)	            Find user based on its Username
    CreateAsync(User, Password	            Creates a new user with a password
    GenerateEmailConfirmationTokenAsync(Id)	Generate email confirmation token which is used in email confimration
    SendEmailAsync(Id, Subject, Body)	    Send confirmation email to the newly registered user
    ConfirmEmailAsync(Id, token)	        Confirm the user email based on the received token
    ChangePasswordAsync(Id, OldPassword, NewPassword)	Change user password
    DeleteAsync(User)	                    Delete user
    IsInRole(Username, Rolename)	        Check if a user belongs to certain Role
    AddToRoleAsync(Username, RoleName)	    Assign user to a specific Role
    RemoveFromRoleAsync(Username, RoleName	Remove user from specific Role
     */
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {

            var appDbContext = context.Get<ApplicationDbContext>();
            //ApplicationUserManager depends on UserStore(next layer up), and UserStore depends on DbContext
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));
            //Regex abstraction for Registration
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            //Regex abstraction for reset password
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }
            return appUserManager;
        }
    }
}