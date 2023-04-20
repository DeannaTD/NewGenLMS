using NovoePokolenie.Models;
using NovoePokolenie.UoW;
using NovoePokolenie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Services
{
    public class PaymentPeriodService
    {
        private UnitOfWork _unitOfWork;
        public PaymentPeriodService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentPeriod> GetByIdAsync(string Id)
        {
            return (await _unitOfWork.PaymentPeriods.GetByIdAsync(Id));
        }

        public async Task CreateAsync(PaymentPeriod period)
        {
            await _unitOfWork.PaymentPeriods.CreateAsync(period);
            await _unitOfWork.Save();
        }

        //TODO: косяк с датой последней оплаты
        public async Task<List<PaymentPeriod>> GetStudentsPaymentPeriodsAsync(string studentId)
        {
            var payments = await _unitOfWork.PaymentPeriods.GetCollectionAsync();
            //TODO: ты отупела, тут что-то не так 
            var lastPaymentPeriod = payments.Where(payment => payment.UserId == studentId).ToList();
            lastPaymentPeriod.Sort((p1, p2) => p1.PaymentEnd > p2.PaymentEnd ? 1 : 0);
            PaymentPeriod period = lastPaymentPeriod.LastOrDefault();
            return payments.Where(p => p.UserId == studentId).ToList();
        }

        //NEW
        public async Task<List<NPUser>> GetStudentsWithPaymentsAsync()
        {
            List<PaymentPeriod> payments = await _unitOfWork.PaymentPeriods.GetCollectionAsync();
            List<NPUser> students = await _unitOfWork.Students.GetAllStudentsAsync();
            for (int i = 0; i < students.Count; i++)
            {
                students[i].PaymentPeriods = payments.Where(period => period.UserId == students[i].Id).ToList();
                //TODO: а группы вообще в оплате нужны?
                students[i].Group = await _unitOfWork.Groups.GetByIdAsync(students[i].GroupId);
            }
            return students;
        }


        public async Task RemoveAsync(PaymentPeriod period)
        {
            _unitOfWork.PaymentPeriods.Delete(period);
            await _unitOfWork.Save();
        }

        public async Task ChangePeriodDates(DateTime start, DateTime end, string periodId)
        {
            var period = await GetByIdAsync(periodId);
            period.PaymentStart = start;
            period.PaymentEnd = end;
            await _unitOfWork.Save();
        }

        public async Task<PaymentInfoViewModel> GetPaymentsInfoAsync(string studentId)
        {
            var periods = (await _unitOfWork.PaymentPeriods.GetCollectionAsync()).Where(p => p.UserId == studentId).ToList();
            periods.Sort((x, y) => DateTime.Compare(x.PaymentStart, y.PaymentStart));
            int paid = 0;
            long debt = 0;
            foreach(PaymentPeriod period in periods)
            {
                paid += (from payment in period.Payments select payment.PaymentAmount).Sum();
                debt += period.MustTotal;
            }
            DateTime? last = periods.Last()?.Payments.Last()?.PaymentCreated;

            return new PaymentInfoViewModel()
            {
                StudentId = studentId,
                Paid = paid,
                Duration = periods.Count,
                LastPayment = last ?? new DateTime(1,1,1),
                Debt = debt - paid
            };
        }
    }
}
