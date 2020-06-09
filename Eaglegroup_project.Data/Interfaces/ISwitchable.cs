using Eaglegroup_project.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eaglegroup_project.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
