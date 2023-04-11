using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees
{
    internal class EntryRecord
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }


        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
