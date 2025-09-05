using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Campaign:BaseModel
    {
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
    }
}
