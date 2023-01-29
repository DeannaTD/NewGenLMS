using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class PaymentService
    {
        private UnitOfWork _unitOfWork;
        public PaymentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Payment payment)
        {
            await _unitOfWork.Payments.CreateAsync(payment);
            await _unitOfWork.Save();
        }

        public async Task ErasePaymentsByPeriodAsync(string PaymentPeriodId)
        {
            var payments = await _unitOfWork.Payments.GetCollectionAsync();
            foreach(var payment in payments)
            {
                if(payment.PaymentPeriodId == PaymentPeriodId)
                    _unitOfWork.Payments.Delete(payment);
            }
            await _unitOfWork.Save();
        }

        public async Task<List<Payment>> GetPaymentsByPeriodAsync(string PeriodId)
        {
            var payments = await _unitOfWork.Payments.GetCollectionAsync();
            return payments.Where(p => p.PaymentPeriodId == PeriodId).ToList();
        }

        public async Task UpdatePayment(string PaymentId, DateTime PaymentCreated, int PaymentAmount)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(PaymentId);
            payment.PaymentAmount = PaymentAmount;
            payment.PaymentCreated = PaymentCreated;
            await _unitOfWork.Save();
        }

        public async Task<Payment> GetByIdAsync(string PaymentId)
        {
            Payment payment = await _unitOfWork.Payments.GetByIdAsync(PaymentId);
            return payment;
        }

        public async Task DeletePaymentAsync(string PaymentId)
        {
            Payment payment = await _unitOfWork.Payments.GetByIdAsync(PaymentId);
            _unitOfWork.Payments.Delete(payment);
            await _unitOfWork.Save();
        }


    }
}
