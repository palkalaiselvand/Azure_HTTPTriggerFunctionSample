using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp.Shared.Data
{
    [Table("UserDetails")]
    public class UserDetails
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
        public DateTime META_DateCreated { get; set; }
        public string META_CreatedBy { get; set; }
        public string META_Application { get; set; }
        public DateTime? META_DateUpdated { get; set; }
        public string META_UpdatedBy { get; set; }
    }
}

