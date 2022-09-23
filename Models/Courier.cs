using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Couriers")]
    public class Courier
    {
        [Key]
        public String CourierId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        [EmailAddress]
        public String Email { get; set; }

        public String FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
