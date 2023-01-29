using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.ViewModels
{
    public class PaymentInfoViewModel
    {
        /*
         *  ФИО
                Долг
                Стоимость
                Всего
                Оплачено
                Количество месяцев
                Последняя оплата
         */
        public string StudentId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Paid { get; set; }
        public int Duration { get; set; }
        public long Debt { get; set; }
        public DateTime LastPayment { get; set; }
    }
}
