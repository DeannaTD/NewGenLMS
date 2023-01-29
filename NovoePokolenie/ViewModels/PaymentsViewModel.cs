using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class PaymentsViewModel
    {
        [Required]
        [HiddenInput]
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Total { get; set; }
        public List<PaymentPeriod> PaymentPeriods { get; set; }
    }
}
