using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chat.Domain.Entities
{
    [Index(nameof(ID), IsUnique = true)]
    public class Message : IBaseEntity
    {
        [Required]
        public Guid ID { get; set; } = Guid.NewGuid();

        public long? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        [MaxLength(2000)]
        public string Text { get; set; } = default;

        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public long? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public bool SoftDeleted { get; set; } = false;
    }
}
