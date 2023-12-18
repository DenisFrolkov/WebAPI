using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class DepartmentParameters : RequestParameters
    {

        public DepartmentParameters()
        {
            OrderBy = "name";
        }

        public string SearchTerm { get; set; }

    }
}
