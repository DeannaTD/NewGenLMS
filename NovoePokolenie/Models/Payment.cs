using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PaymentPeriodId { get; set; }
        public int PaymentAmount { get; set; }
        public DateTime PaymentCreated { get; set; }
        public string Comment { get; set; }

        public virtual PaymentPeriod PaymentPeriod { get; set; }
    }
}
