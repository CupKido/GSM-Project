using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GSM.ViewModel
{
    public class GameDescriptionVM : INotifyPropertyChanged
    {
        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged ();
            }
        }

        private string GPTdescription;

        public string GPTDescription
        {
            get { return GPTdescription; }
            set
            {
                GPTdescription = value;
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
