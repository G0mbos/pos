using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierGewinnt.Model
{
    public partial class Cell : ObservableObject
    {
        public int X { get; set; }
        public int Y { get; set; }

        private int _status; // 0 = leer, 1 = X, 2 = O

        public int Status
        {
            get => _status;
            set => SetProperty(ref _status, value); // Automatische Benachrichtigung bei Wertänderung
        }
    }

}
