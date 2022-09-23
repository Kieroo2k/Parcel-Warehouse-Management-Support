using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Returns")]
    public class Return
    {
        public int ReturnId { get; set; }
        public String Description { get; set; }
        [DisplayName("Courier")]
        [ForeignKey("CourierId")]
        public String CourierId { get; set; }
        public int ParcelsNUMBER { get; set; }
        [DisplayName("Delivery")]
        public int DeliveryId { get; set; }
        public String DateAdded { get; set; }

        public virtual Delivery delivery { get; set; }
        public virtual Courier courier { get; set; }
    }
}
