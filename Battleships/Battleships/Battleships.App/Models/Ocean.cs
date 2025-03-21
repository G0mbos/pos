using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.App.Models
{
    public class Ocean : INotifyPropertyChanged
    {
        public int Row { get; set; }
        
        public int Column { get; set; }

        private bool isHit = false;

        public bool IsHit 
        {
            get => isHit;
            set
            {
                isHit = value;
                OnPropertyChanged();
            }
        }

        private bool isShip = false;

        public bool IsShip
        {
            get => isShip;
            set
            {
                isShip = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
