using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class CompanyParameters : RequestParameters
    {
        public string CompamyName { get; set; }
        public bool ValidCompanytNameRange => CompamyName == CompamyName;

    }
}
