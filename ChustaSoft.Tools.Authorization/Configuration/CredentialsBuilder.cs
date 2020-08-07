using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization.Configuration
{
    public class CredentialsBuilder : ICredentialsBuilder
    {

        private readonly ICollection<(Credentials Credentials, IEnumerable<string> Roles)> _usersCredentials;
       
        private Credentials _credentials;
        private ICollection<string> _rolesAssigned;


        public CredentialsBuilder()
        {
            _usersCredentials = new List<(Credentials, IEnumerable<string>)>();
        }


        public ICredentialsBuilder Add(string userName, string userPassword)
        {
            CheckIfExistingCredentials();

            _credentials = new Credentials { Username = userName, Password = userPassword, Email = $"{userName}@noreply.com" };
            _rolesAssigned = _rolesAssigned = new List<string>();

            return this;
        }

        public ICredentialsBuilder WithEmail(string email)
        {
            _credentials.Email = email;

            return this;
        }

        public ICredentialsBuilder WithRole(string roleName)
        {
            _rolesAssigned.Add(roleName);

            return this;
        }

        public IEnumerable<(Credentials Credentials, IEnumerable<string> Roles)> Build()
        {
            CheckIfExistingCredentials();

            return _usersCredentials;
        }


        private void CheckIfExistingCredentials()
        {
            if (_credentials != null)
                _usersCredentials.Add((_credentials, _rolesAssigned));
        }

    }



    #region Contract

    public interface ICredentialsBuilder
    {

        ICredentialsBuilder Add(string userName, string userPassword);
        ICredentialsBuilder WithRole(string roleName);

        IEnumerable<(Credentials Credentials, IEnumerable<string> Roles)> Build();

    }

    #endregion

}
