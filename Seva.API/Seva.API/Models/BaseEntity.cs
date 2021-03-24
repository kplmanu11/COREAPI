using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seva.API.Models
{
    public abstract class BaseEntity<TKey>
    {
        public virtual TKey Id { get; set; }
    }
}
