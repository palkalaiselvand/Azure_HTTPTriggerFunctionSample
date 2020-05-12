using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApp.Shared.Data
{
    [Table("RequestAudit")]
    public class RequestAudit
    {
        [Key]
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public DateTime META_DateCreated { get; set; }
        public string META_CreatedBy { get; set; }
        public string META_Application { get; set; }
        public DateTime? META_DateUpdated { get; set; }
        public string META_UpdatedBy { get; set; }
    }
}
