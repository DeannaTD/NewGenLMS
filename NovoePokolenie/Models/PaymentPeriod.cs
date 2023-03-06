using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovoePokolenie.Models
{
    public class PaymentPeriod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public long MustTotal { get; set; }
        public DateTime PaymentStart { get; set; }
        public DateTime PaymentEnd { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual NPUser NPUser { get; set; }
    }
}
