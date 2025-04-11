using StoreVisitTracking.Application.DTOs.Photo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.DTOs.Visit
{
    public class GetVisitDto
    {
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Status { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}
