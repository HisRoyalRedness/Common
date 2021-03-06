﻿using HisRoyalRedness.com.ColourConstants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
    Defines a colour instance using the HSV / HSB colour space
    https://en.wikipedia.org/wiki/HSL_and_HSV

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

    /// <summary>
    /// HSV (or HSB) is a remapping of the RGB colour space.
    /// Hue (H) is attribute of a visual sensation according to which an area appears to be similar to one of the perceived colors: red, yellow, green, and blue, or to a combination of two of them.
    /// Saturation (S) it the coluorfulness of a stimulus relative to its own brightness.
    /// Value or Brightness (V) is the brightness relative to the brightness of a similarly illuminated white.
    /// 
    /// Ranges:
    /// Hue: 0° - 360°. 0° = Red, 120° = Green, 240° = Blue
    /// Saturation: 0 - 1. 0 = No saturation (greyscale), 1 = Full saturation (full colour)
    /// Value: 0 - 1. 0 = Darkest (black), 1 = Brightest (full colour)
    /// </summary>
    [DebuggerDisplay("{DisplayString}")]
    public struct HSVColour
    {
        public HSVColour(DegreeColourComponent h, UnitColourComponent s, UnitColourComponent v)
        {
            _h = h;
            _s = s;
            _v = v;
            _a = ColourSpaceConstants.ONE;
        }

        public HSVColour(DegreeColourComponent h, UnitColourComponent s, UnitColourComponent v, UnitColourComponent a)
        {
            _h = h;
            _s = s;
            _v = v;
            _a = a;
        }

        public HSVColour(ColourVector v, UnitColourComponent a)
        {
            _h = v.X;
            _s = v.Y;
            _v = v.Z;
            _a = a;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"H: {(ColourPrimitive)H:0.0°}, S: {(ColourPrimitive)S:0.000}, V: {(ColourPrimitive)V:0.000}, A: {(ColourPrimitive)A:0.000}";
        public override string ToString() => $"H: {(ColourPrimitive)H}, S: {(ColourPrimitive)S}, V: {(ColourPrimitive)V}, A: {(ColourPrimitive)A}";

        /// <summary>
        /// Hue
        /// 0° - 360°. 0° = Red, 120° = Green, 240° = Blue
        /// </summary>
        public DegreeColourComponent H => _h;
        readonly DegreeColourComponent _h;
        /// <summary>
        /// Saturation
        /// 0 - 1. 0 = No saturation (greyscale), 1 = Full saturation (full colour)
        /// </summary>
        public UnitColourComponent S => _s;
        readonly UnitColourComponent _s;
        /// <summary>
        /// Value
        /// 0 - 1. 0 = Darkest (black), 1 = Brightest (full colour)
        /// </summary>
        public UnitColourComponent V => _v;
        readonly UnitColourComponent _v;

        public UnitColourComponent A => _a;
        readonly UnitColourComponent _a;

        // Alias
        /// <summary>
        /// Brightness
        /// 0 - 1. 0 = Darkest (black), 1 = Brightest (full colour)
        /// </summary>
        public UnitColourComponent B => _v;

        #region Add and subtract
        public static HSVColour operator +(HSVColour a, HSVColour b)
            => new HSVColour(a.H + b.H, a.S + b.S, a.V + b.V, a.A + b.A);
        public static HSVColour operator -(HSVColour a, HSVColour b)
            => new HSVColour(a.H - b.H, a.S - b.S, a.V - b.V, a.A - b.A);
        #endregion Add and subtract

        #region Implicit and explicit casts
        public static implicit operator ColourVector(HSVColour colour) => new ColourVector(colour.H, colour.S, colour.V);

        public static explicit operator HSVColour(ColourVector vector) => new HSVColour(vector.X, vector.Y, vector.Z);
        #endregion Implicit and explicit casts
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
