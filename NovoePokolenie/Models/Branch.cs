using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NovoePokolenie.Models
{
    public class Branch
    {
        public int Id { get; set; }
        [Display(Name = "Название филиала")]
        public string Name { get; set; }
        [Display(Name = "Адрес филиала")]
        public string Adress { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
