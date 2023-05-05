using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM.ViewModel
{
    public class pieChartVM
    {
        public int lowLoad { get; set; }
        public int mediumLoad { get; set; }
        public int highLoad { get; set; }

        public Dictionary<string, int> load= new Dictionary<string, int>
{
    {"low", 0},
    {"medium", 0},
    {"high", 0},
};

    }
}
