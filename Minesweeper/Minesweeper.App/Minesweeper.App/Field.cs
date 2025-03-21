using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.App;

public partial class Field : ObservableObject
{
    public int Row { get; set; }
    public int Column { get; set; }

    [ObservableProperty]
    private int value;

    public Field()
    {
        value = -1;
    }

}
