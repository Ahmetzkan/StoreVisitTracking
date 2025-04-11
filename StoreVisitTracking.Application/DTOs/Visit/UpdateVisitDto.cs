using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.DTOs.Visit
{
    public class UpdateVisitDto
    {
        public Guid StoreId { get; set; }  
        public string Status { get; set; }
    }
}
