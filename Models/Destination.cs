using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Destinations")]
    public class Destination
    {
        public int DestinationId { get; set; }
        public String DestinationCountry { get; set; }
        public bool BelongToEuropeanUnion { get; set; }
    }
}
