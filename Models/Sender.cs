using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParcelWarehouseManagementSupport.Models
{
    [Table("Senders")]
    public class Sender
    {
        [Key]
        public int SenderId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
