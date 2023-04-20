using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovoePokolenie.Helpers;
using NovoePokolenie.Models;
using NovoePokolenie.Services;
using NovoePokolenie.ViewModels;

namespace NovoePokolenie.Controllers
{
    public class PaymentController : Controller
    {
        PaymentService _paymentService;
        PaymentPeriodService _paymentPeriodService;
        StudentService _studentService;
        GroupService _groupService;
       
        public PaymentController(PaymentService paymentService, PaymentPeriodService paymentPeriodService, StudentService studentService, GroupService groupService)
        {
            _paymentService = paymentService;
            _paymentPeriodService = paymentPeriodService;
            _studentService = studentService;
            _groupService = groupService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("SortedIndex", new StFilter());
        }

        public async Task<IActionResult> SortedIndex(StFilter filter)
        {
            List<NPUser> students = await _paymentPeriodService.GetStudentsWithPaymentsAsync();
            if(filter.Option != FilterOption.None)
            {
                if(filter.Option == FilterOption.Branch)
                {
                    List<int> groupsToSave =
                        (await _groupService.GetGroupsByBranchIdAsync(filter.Parameter)).
                        Select(group => group.Id).
                        ToList();

                    students.RemoveAll(student => !groupsToSave.Contains(student.GroupId ?? 0));
                }
                else if(filter.Option == FilterOption.Status)
                {
                    students.RemoveAll(student => student.StatusId != filter.Parameter);
                }
            }
            return View("Index", students);
            
            /*
             * comparators:
             *  debt
             *  firstname
             *  lastname
             *  date
             *  
             *  filters:
             *  status
             *  branch
             *  
             *  search:
             *  name
             *  surname
             *  number
             */
        }

        public async Task<IActionResult> StudentPaymentCard(string studentId)
        {
            List<NPUser> students = await _paymentPeriodService.GetStudentsWithPaymentsAsync();
            NPUser student = students.Find(x => x.Id == studentId);
            student.PaymentPeriods = student.PaymentPeriods.OrderBy(x => x.PaymentStart).ToList();
            return View("PaymentStudentCard", student);
        }

        public async Task<IActionResult> StudentCard(string UserId)
        {
            var Student = await _studentService.GetStudentById(UserId);
            PaymentsViewModel model = new PaymentsViewModel()
            {
                StudentId = UserId,
                FirstName = Student.FirstName,
                LastName = Student.Lastname,
                Total = Student.PaymentCharge,
                PaymentPeriods = await _paymentPeriodService.GetStudentsPaymentPeriodsAsync(UserId)
            };

            model.PaymentPeriods.Sort((x, y) => y.PaymentStart.CompareTo(x.PaymentStart));
            return View(model);
        }

        public async Task<IActionResult> GetStudentPeriodsList(string UserId)
        {
            var Student = await _studentService.GetStudentById(UserId);
            PaymentsViewModel model = new PaymentsViewModel()
            {
                StudentId = UserId,
                FirstName = Student.FirstName,
                LastName = Student.Lastname,
                Total = Student.PaymentCharge,
                PaymentPeriods = await _paymentPeriodService.GetStudentsPaymentPeriodsAsync(UserId)
            };

            model.PaymentPeriods.Sort((x, y) => y.PaymentStart.CompareTo(x.PaymentStart));
            // return View("StudentCard", model); OLD VIEW
            return View("TableStudentPayment", model);
        }

        public async Task ChangePeriodDates(DateTime PaymentStart, DateTime PaymentEnd, string periodId)
        {
            await _paymentPeriodService.ChangePeriodDates(PaymentStart, PaymentEnd, periodId);
        }

        public async Task<IActionResult> GroupPayment(int groupId)
        {
            var students = await _studentService.GetStudentsAndPayments(groupId);
            List<PaymentsViewModel> model = new List<PaymentsViewModel>();
            foreach(var student in students)
            {
                model.Add(new PaymentsViewModel()
                {
                    StudentId = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.Lastname,
                    Total = student.PaymentCharge,
                    PaymentPeriods = await _paymentPeriodService.GetStudentsPaymentPeriodsAsync(student.Id)
                });
            }
            return PartialView("GroupPayment", model);
        }

        public async Task<IActionResult> BranchPayment(int BranchId)
        {
            var students = await _studentService.GetStudentsAndPaymentsByBranch(BranchId);
            List<PaymentsViewModel> model = new List<PaymentsViewModel>();
            foreach (var student in students)
            {
                model.Add(new PaymentsViewModel()
                {
                    StudentId = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.Lastname,
                    Total = student.PaymentCharge,
                    PhoneNumber = student.PhoneNumber,
                    PaymentPeriods = await _paymentPeriodService.GetStudentsPaymentPeriodsAsync(student.Id)
                });
            }

            model.Sort((x, y) => CompareDebt(x.PaymentPeriods, y.PaymentPeriods, x.Total, y.Total));
            return PartialView("GroupPayment", model);
        }

        public int CompareDebt(List<PaymentPeriod> p1, List<PaymentPeriod> p2, int t1, int t2)
        {
            int sum1 = 0;
            foreach (var period in p1)
            {
                foreach (var check in period.Payments)
                {
                    sum1 += check.PaymentAmount;
                }
            }
            sum1 = p1.Count * t1 - sum1;
            int sum2 = 0;
            foreach (var period in p2)
            {
                foreach (var check in period.Payments)
                {
                    sum1 += check.PaymentAmount;
                }
            }
            sum2 = p2.Count * t2 - sum2;

            return sum1 > sum2 ? -1 : sum1 == sum2 ? 0 : 1;
        }

        [HttpGet]
        public async Task<IActionResult> PeriodInfo(string periodId, string UserId)
        {
            var payments = await _paymentService.GetPaymentsByPeriodAsync(periodId);
            ViewBag.PeriodId = periodId;
            ViewBag.UserId = UserId;
            ViewBag.Price = (await _paymentPeriodService.GetByIdAsync(periodId)).MustTotal;
            return PartialView("PeriodPaymentsView", payments);
        }

        public async Task<IActionResult> GetPayments(string periodId)
        {
            List<Payment> payments = await _paymentService.GetPaymentsByPeriodAsync(periodId);
            payments.Sort((x,y) => x.PaymentCreated.CompareTo(y.PaymentCreated));
            ViewBag.PeriodId = periodId;
            ViewBag.Amount = (await _paymentPeriodService.GetByIdAsync(periodId)).MustTotal;
            return PartialView("Payments", payments);
        }

        [HttpGet]
        public IActionResult CreatePaymentPeriod(string UserId, int MustTotal)
        {
            ViewBag.UserId = UserId;
            ViewBag.MustTotal = MustTotal;
            return View("AddNewPeriod");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentPeriod(PaymentPeriod period)
        {
            await _paymentPeriodService.CreateAsync(period);
            return RedirectToAction("GetStudentPeriodsList", "Payment", new { UserId = period.UserId });
        }

        public async Task<string> CreatePaymentPeriod2(string studentId, int amount, DateTime start, DateTime end)
        {
            //TODO: Rename after finishing
            PaymentPeriod period = new PaymentPeriod()
            {
                UserId = studentId,
                MustTotal = amount,
                PaymentStart = start,
                PaymentEnd = end
            };
            await _paymentPeriodService.CreateAsync(period);

            return period.Id;
        }

        [HttpGet]
        public async Task<IActionResult> CreatePayment(string PeriodId)
        {
            PaymentPeriod period = await _paymentPeriodService.GetByIdAsync(PeriodId);
            ViewBag.Period = period;
            return PartialView("AddNewPayment");
        }
        
        [HttpPost]
        public async Task<IActionResult> CreatePayment(string UserId, string PaymentPeriodId, int PaymentAmount, DateTime PaymentCreated, string Comment)
        {
            Payment payment = new Payment()
            {
                UserId = UserId,
                PaymentAmount = PaymentAmount,
                PaymentPeriodId = PaymentPeriodId,
                PaymentCreated = PaymentCreated,
                Comment = Comment
            };
            await _paymentService.CreateAsync(payment);
            return RedirectToAction("PeriodInfo", new { periodId = PaymentPeriodId, UserId = UserId });
        }

        [HttpPost]
        public async Task<string> CreateNewPayment(string periodId, int amount, DateTime dateCreated, string comment="")
        {
            PaymentPeriod period = await _paymentPeriodService.GetByIdAsync(periodId);
            Payment payment = new Payment()
            {
                UserId = period.UserId,
                PaymentAmount = amount,
                PaymentPeriodId = periodId,
                PaymentCreated = dateCreated,
                Comment = comment
            };
            await _paymentService.CreateAsync(payment);
            return payment.Id;
        }

        public async Task DeletePaymentPeriod(string PeriodId)
        {
            PaymentPeriod period = await _paymentPeriodService.GetByIdAsync(PeriodId);
            await _paymentService.ErasePaymentsByPeriodAsync(PeriodId);
            await _paymentPeriodService.RemoveAsync(period);
        }

        public async Task<IActionResult> Payment(string studentId)
        {
            List<PaymentPeriod> payments = await _paymentPeriodService.GetStudentsPaymentPeriodsAsync(studentId);
            payments.Sort((x, y) => DateTime.Compare(x.PaymentStart, y.PaymentStart));
            payments.Reverse();
            return View("StudentPayment", payments);
        }

        [HttpPost]
        public async Task UpdatePayment(string PaymentId, DateTime PaymentCreated, int PaymentAmount)
        {
            await _paymentService.UpdatePayment(PaymentId, PaymentCreated, PaymentAmount);
            return;
        }

        public async Task DeletePayment(string paymentId)
        {
            await _paymentService.DeletePaymentAsync(paymentId);
        }
    }
}