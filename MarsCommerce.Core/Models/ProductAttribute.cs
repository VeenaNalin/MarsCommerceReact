using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MarsCommerce.Core.Models
{
    public class ProductAttribute : BaseEntity
    {
        public ProductAttribute() { 
        Name = string.Empty;
        }
        public string Name { get; set; }

        public virtual List<ProductAttributeMapping>? ProductAttributeMappings { get; set; }
    }
}
