using System;

namespace Quartzified.Input
{
    [Flags]
    public enum InputTypes
    {
        InputAxis = 1 << 0,
        MouseDrag = 1 << 1,
    }

}
