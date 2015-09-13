using System;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;
using SSExec.Button.Core.Data;
using SSExec.Button.Core.Data.Contract;

namespace SSExec.Button.Core.Security
{
    public class CustomRepoMembershipProvider : MembershipProvider
    {
        private readonly IRepository<XmlUser, string> _userRepository;

        public CustomRepoMembershipProvider(IRepository<XmlUser, string> userRepository)
        {
            _userRepository = userRepository;
        }

        public CustomRepoMembershipProvider()
        {
            _userRepository = new SingleFileRepository<XmlUser, string>( HttpContext.Current.Server.MapPath(@"~\App_Data\users.xml"));
        }

        public override bool EnablePasswordRetrieval { get; }
        public override bool EnablePasswordReset { get; }
        public override bool RequiresQuestionAndAnswer { get; }
        public override string ApplicationName { get; set; }
        public override int MaxInvalidPasswordAttempts { get; }
        public override int PasswordAttemptWindow { get; }
        public override bool RequiresUniqueEmail { get; }
        public override MembershipPasswordFormat PasswordFormat { get; }
        public override int MinRequiredPasswordLength { get; }
        public override int MinRequiredNonAlphanumericCharacters { get; }
        public override string PasswordStrengthRegularExpression { get; }

        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer,
            bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            
            var user = new XmlUser() { Id = username, Name = username, Password = password};
            user = _userRepository.Add(user);

            status = MembershipCreateStatus.Success;

            return new MembershipUser("CustomRepoMembershipProvider",
                username, user.Name, string.Empty,
                string.Empty, string.Empty,
                true, false, DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.Now, DateTime.Now);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            return
                _userRepository.Count(
                    x =>
                        string.CompareOrdinal(x.Name, username) == 0 && string.CompareOrdinal(x.Password, password) == 0) ==
                1;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }
    }


    [Serializable]
    public class XmlUser : IEntity<string>
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Password { get; set; }
    }
}