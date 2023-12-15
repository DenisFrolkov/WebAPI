using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class DepartmentParameters : RequestParameters
    {
        public string DepartmentName { get; set; }

        public string DepartmentManager { get; set; }
    }
}
