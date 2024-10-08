using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_App.Core.Identity
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [AllowedValues('f', 'm', 'F', 'M')]
        public char Gender { get; set; }

        [Required]
        public string Salt { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string? VerifecationCode { get; set; }
        public DateTime FinalTimeForVerify { get; set; }
        public int NumberOfTries { get; set; } = 0;

        public string? RefreshToken { get; set; }

        public bool AccountLocked { get; set; }

    }
}
