using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Parcels")]
    public class Parcel
    {
        [Key]
        public int ParcelId { get; set; }
        [Required(ErrorMessage = "Parcel number is required")]
        public int DeliveryId { get; set; }
        public int DeliveryNumber { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Weight { get; set; }
        
    }
}
