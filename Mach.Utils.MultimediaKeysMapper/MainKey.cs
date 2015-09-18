using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaKeysMapper
{
    public enum MainKey
    {
        [Description("Alt")]
        [CorrespondingKeyCode(0xA2)] //leftctrl
        [CorrespondingKeyCode(0xA3)] //rightctrl
        [CorrespondingKeyCode(0xA0)] //leftshift
        [CorrespondingKeyCode(0xA1)] //rightshit
        [CorrespondingKeyCode(0x5B)] //leftwin
        [CorrespondingKeyCode(0x5C)] //rightwin
        Alt = 0x0001,

        [Description("Ctrl")]
        [CorrespondingKeyCode(0xA2)] //leftctrl
        [CorrespondingKeyCode(0xA3)] //rightctrl
        [CorrespondingKeyCode(0xA0)] //leftshift
        [CorrespondingKeyCode(0xA1)] //rightshit
        [CorrespondingKeyCode(0x5B)] //leftwin
        [CorrespondingKeyCode(0x5C)] //rightwin
        Ctrl = 0x0002,

        [Description("Shift")]
        [CorrespondingKeyCode(0xA0)] //leftshift
        [CorrespondingKeyCode(0xA1)] //rightshit
        [CorrespondingKeyCode(0x5B)] //leftwin
        [CorrespondingKeyCode(0x5C)] //rightwin
        Shift = 0x0004,

        [Description("Win")]
        [CorrespondingKeyCode(91)]
        [CorrespondingKeyCode(0xA2)] //leftctrl
        [CorrespondingKeyCode(0xA3)] //rightctrl
        [CorrespondingKeyCode(0xA0)] //leftshift
        [CorrespondingKeyCode(0xA1)] //rightshit
        [CorrespondingKeyCode(0x5B)] //leftwin
        [CorrespondingKeyCode(0x5C)] //rightwin
        WinKey = 0x0008
    }
}
