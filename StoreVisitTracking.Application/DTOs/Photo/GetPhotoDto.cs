using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreVisitTracking.Application.DTOs.Photo
{
    public class GetPhotoDto
    {
        public Guid ProductId { get; set; }  
        public string Base64Image { get; set; }  
        public DateTime UploadedAt { get; set; } 
    }
}
