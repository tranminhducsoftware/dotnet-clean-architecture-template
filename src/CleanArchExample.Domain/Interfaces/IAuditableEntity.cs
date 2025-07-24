using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchExample.Domain.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime  CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}