using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Addresses")]
    public class Address
    {
        public int AddressId { get; set; }
        [DisplayName("Recipient")]
        [ForeignKey("RecipientId")]
        public int RecipientId { get; set; }
        [DisplayName("Street")]
        public String StreetName { get; set; }
        public int StreetNumber { get; set; }
        [DisplayName]
        public String PostCode { get; set; }
        public String City { get; set; }
        public String FullStreet 
        {
            get { return StreetName + " " + StreetNumber; }
        }
        public String FullCode
        {
            get { return PostCode + " " + City;  }
        }

    }
}
