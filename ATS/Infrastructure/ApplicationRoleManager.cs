using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATS.Infrastructure
{
    /*
     *  FindByIdAsync(id)	        Find role object based on its unique identifier
        Roles	                    Returns an enumeration of the roles in the system
        FindByNameAsync(Rolename)	Find roled based on its name
        CreateAsync(IdentityRole)	Creates a new role
        DeleteAsync(IdentityRole)	Delete role
        RoleExistsAsync(RoleName)	Returns true if role already exists
     */
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }
}