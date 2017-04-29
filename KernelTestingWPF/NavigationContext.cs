using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelTestingWPF
{
    class NavigationContext
    {
        public string fileName { get; set; }
        public int numFastCores { get; set; }
        public int numSlowCores { get; set; }
        public int policyType { get; set; }
    }
}
