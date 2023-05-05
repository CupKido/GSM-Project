using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GSM.ViewModel
{
    public class ServerMonitorVM : INotifyPropertyChanged
    {
        public GaugeVM cpuMonitor = new GaugeVM ();
        public GaugeVM memoryMonitor = new GaugeVM ();

        private string cpuUsage;

        public string CpuUsage
        {
            get { return cpuUsage; }
            set
            {
                cpuUsage = value;
                OnPropertyChanged ();
            }
        }

        private string ramUsage;

        public string RamUsage
        {
            get { return ramUsage; }
            set
            {
                ramUsage = value;
                OnPropertyChanged ();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged ([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }
    }
}

