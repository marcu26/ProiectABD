using System;

namespace Infrastructure.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
