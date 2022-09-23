using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Transmissions")]
    public class Transmission
    {
        [Key]
        public int TransmissionId { get; set; }
        public int DeliveryId { get; set; }
        public int DeliveryNumber { get; set; }
        [ForeignKey("ParcelNumber")]
        [DisplayName("Number of parcels")]
        public int ParcelsNUMBER { get; set; }
        [DisplayName("Courier")]
        [ForeignKey("CourierId")]
        public String CourierId { get; set; }
        [DisplayName("Sender")]
        [ForeignKey("SenderId")]
        public int SenderId { get; set; }
        [DisplayName("Recipient")]
        [ForeignKey("RecipientId")]
        public int RecipientId { get; set; }
        public int TotalWeight { get; set; }
        [DisplayName("Destination")]
        [ForeignKey("DestinationId")]
        public int DestinationId { get; set; }
        public int Fee { get; set; }
        public bool Paid { get; set; }
        public virtual Courier courier { get; set; }
        public virtual Sender sender { get; set; }
        public virtual Recipient recipient { get; set; }
        public virtual Destination destination { get; set; }
    }
}
