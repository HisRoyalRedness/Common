using HisRoyalRedness.com.ColourConstants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
    Colour components. These are added as fields to the various colour structs, 
    and take care of range checking, conversion etc.

    Keith Fletcher
    Jan 2018

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    #region ByteColourComponent
    public partial struct ByteColourComponent : IComparable, IComparable<ByteColourComponent>
    {
        public ByteColourComponent(byte value)
        {
            _value = value;
        }

        internal const byte MIN_VAL = 0;
        internal const byte MAX_VAL = 255;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"{_value}";

        public static implicit operator ByteColourComponent(byte value) => new ByteColourComponent(value);
    }
    #endregion ByteColourComponent

    #region UnitColourComponent
    public partial struct UnitColourComponent
    {
        internal const ColourPrimitive MIN_VAL = 0;
        internal const ColourPrimitive MAX_VAL = 1.0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"{_value:0.000}";
    }
    #endregion UnitColourComponent

    #region DegreeColourComponent
    public partial struct DegreeColourComponent
    {
        internal const ColourPrimitive MIN_VAL = 0;
        internal const ColourPrimitive MAX_VAL = 360.0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"{_value:0.0}°";
    }
    #endregion DegreeColourComponent

    public static class ComponentConversionExtensions
    {
        public static ByteColourComponent ToByteColour(this UnitColourComponent colourComp) => new ByteColourComponent(colourComp.Value * (ColourPrimitive)ByteColourComponent.MaxValue);
        public static UnitColourComponent ToUnitColour(this ByteColourComponent colourComp) => new UnitColourComponent((ColourPrimitive)colourComp.Value / (ColourPrimitive)ByteColourComponent.MaxValue);

        public static DegreeColourComponent ToDegreeColour(this UnitColourComponent unit) => new DegreeColourComponent(unit.Value * DegreeColourComponent.MaxValue);
        public static UnitColourComponent ToUnitColour(this DegreeColourComponent colourComp) => new UnitColourComponent(colourComp.Value / DegreeColourComponent.MaxValue);
    }
}

/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/
