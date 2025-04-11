using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.DTOs.Photo
{
    public class PhotoDto
    {
        public Guid ProductId { get; set; }
        public string Base64Image { get; set; }
    }
}
