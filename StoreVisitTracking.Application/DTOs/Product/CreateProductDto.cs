using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public Guid StoreId { get; set; }
    }

}
