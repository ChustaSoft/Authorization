using ChustaSoft.Tools.Authorization.Models;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class CredentialsBuilder : ICredentialsBuilder, IAutomaticCredentialsBuilder
    {

        private readonly ICollection<(AutomaticCredentials Credentials, IEnumerable<string> Roles)> _usersCredentials;
       
        private AutomaticCredentials _credentials;
        private ICollection<string> _rolesAssigned;


        public CredentialsBuilder()
        {
            _usersCredentials = new List<(AutomaticCredentials, IEnumerable<string>)>();
        }


        public ICredentialsBuilder Add(string userName, string userPassword)
        {
            CheckIfExistingCredentials();

            _credentials = new AutomaticCredentials { Username = userName, Password = userPassword, Email = $"{userName}@noreply.com" };
            _rolesAssigned = _rolesAssigned = new List<string>();

            return this;
        }

        public ICredentialsBuilder WithEmail(string email)
        {
            _credentials.Email = email;

            return this;
        }

        public ICredentialsBuilder WithCulture(string culture)
        {
            _credentials.Culture = culture;

            return this;
        }

        public ICredentialsBuilder WithRole(string roleName)
        {
            _rolesAssigned.Add(roleName);

            return this;
        }

        public ICredentialsBuilder WithFullAccess()
        {
            _credentials.FullAccess = true;

            return this;
        }
        
        public IEnumerable<(AutomaticCredentials Credentials, IEnumerable<string> Roles)> Build()
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



    #region Contracts

    public interface ICredentialsBuilder
    {

        ICredentialsBuilder Add(string userName, string userPassword);
        ICredentialsBuilder WithCulture(string culture);
        ICredentialsBuilder WithRole(string roleName);
        ICredentialsBuilder WithFullAccess();

    }


    public interface IAutomaticCredentialsBuilder : ICredentialsBuilder
    {
        IEnumerable<(AutomaticCredentials Credentials, IEnumerable<string> Roles)> Build();
    }

    #endregion

}
