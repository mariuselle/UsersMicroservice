using System;

namespace Users.Core.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Password { get; set; }
    }
}
