using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Entities.RequestFeatures
{
    public class ProjectParameters : RequestParameters
    {
        public string SearchTerm { get; set; }
    }
}
