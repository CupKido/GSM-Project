using GSM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private string _gamename;

        public string Gamename
        {
            get
            {
                return _gamename;
            }
            set
            {
                _gamename = value;
                OnPropertyChanged (nameof (Gamename));
            }
        }

        private string _imgUrl;

        public string ImgUrl
        {
            get
            {
                return _imgUrl;
            }
            set
            {
                _imgUrl = value;
                OnPropertyChanged (nameof (ImgUrl));
            }
        }
    }
}
