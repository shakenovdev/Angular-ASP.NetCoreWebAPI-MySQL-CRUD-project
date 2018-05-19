using System;

namespace DataAccessLayer.Models
{
    public abstract class BaseModel
    {
        public long Id { get; set; }

        public ApplicationUser CreatorUser { get; set; }
        public long CreatorId { get; set; }

        public long ModifierId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        // HVersion gets +1 whenever a record is updated
        public int HVersion { get; set; }
        // Mark a record as deleted instead of deletion
        public bool IsDeleted { get; set; }
    }
}