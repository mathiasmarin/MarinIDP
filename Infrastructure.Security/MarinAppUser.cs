using System;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security
{
    public sealed class MarinAppUser : IdentityUser
    {
        public MarinAppUser(string email, string firstName, string lastName) : base(email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Id = Guid.NewGuid().ToString();
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        #region Do not use

        /// <summary>
        /// Ef requires empty Ctor. Do not use
        /// </summary>
        [Obsolete]
        private MarinAppUser()
        {

        }


        #endregion
    }
}