using System.Linq;
using System.Web;
using SSExec.Button.Core.Data;
using SSExec.Button.Core.Data.Contract;

namespace SSExec.Button.Core.Security
{
    public class CustomRepoRoleProvider : System.Web.Security.RoleProvider
    {
        private readonly IRepository<XmlUser, string> _userRepository;

        public CustomRepoRoleProvider()
        {
            _userRepository = new SingleFileRepository<XmlUser, string>(HttpContext.Current.Server.MapPath(@"~\App_Data\users.xml"));
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var user = _userRepository.Get(username);
            return user?.Roles?.Any(x => string.CompareOrdinal(x, roleName) == 0) ?? false;
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new System.NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new System.NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new System.NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}