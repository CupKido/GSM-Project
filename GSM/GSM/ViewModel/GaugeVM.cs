using System.ComponentModel;

namespace GSM.ViewModel
{
    public class GaugeVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged (string info)
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged (this, new PropertyChangedEventArgs (info));
            }
        }

        public GaugeVM ()
        {
            Angle = -90;
            Value = 0;
        }

        private double _angle;
        public double Angle
        {
            get
            {
                return _angle;
            }

            private set
            {
                _angle = value;
                NotifyPropertyChanged ("Angle");
            }
        }

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                if ( value >= 0 && value <= 100 )
                {
                    _value = value;
                    Angle = 1.8 * value - 90;
                    NotifyPropertyChanged ("Value");
                }
            }
        }
    }
}
