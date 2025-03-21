using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRobotServer.Model
{
    public enum SpaceValue { OBSTACLE, RED, GREEN, BLUE, ROBOT, EMPTY }
    public class Space : INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }
        SpaceValue _value;
        public SpaceValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
