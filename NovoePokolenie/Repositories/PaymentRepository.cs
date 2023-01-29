using Microsoft.EntityFrameworkCore;
using NovoePokolenie.Data;
using NovoePokolenie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>
    {
        public PaymentRepository(NewGenContext context) : base(context) { }

    }
}
