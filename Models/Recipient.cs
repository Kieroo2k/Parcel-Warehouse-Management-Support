using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Recipients")]
    public class Recipient
    {
        [Key]
        public int RecipientId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public virtual Address Address { get; set; }
    }
}
