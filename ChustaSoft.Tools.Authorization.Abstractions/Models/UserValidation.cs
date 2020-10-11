using System;
using System.ComponentModel.DataAnnotations;

namespace ChustaSoft.Tools.Authorization.Models
{
    [Serializable]
    public class UserValidation
    {

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string ConfirmationToken { get; set; }

    }
}
