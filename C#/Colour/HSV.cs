//using HisRoyalRedness.com.ColourConstants;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;

///*
//    Defines a colour instance using the HSV colour space
//    https://en.wikipedia.org/wiki/HSL_and_HSV

//    Keith Fletcher
//    Jan 2018

//    This file is Unlicensed.
//    See the foot of the file, or refer to <http://unlicense.org>
//*/

//namespace HisRoyalRedness.com
//{
//#if COLOUR_SINGLE
//    using ColourPrimitive = Single;
//#else
//    using ColourPrimitive = Double;
//#endif

//    #region HSVColour
//    [DebuggerDisplay("{DisplayString}")]
//    public struct HSVColour
//    {
//        public HSVColour(float h, float s, float v)
//            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v))
//        { }

//        public HSVColour(float h, float s, float v, float a)
//            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v), new UnitColourComponent(a))
//        { }

//        public HSVColour(double h, double s, double v)
//            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v))
//        { }

//        public HSVColour(double h, double s, double v, double a)
//            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v), new UnitColourComponent(a))
//        { }

//        public HSVColour(DegreeColourComponent h, UnitColourComponent s, UnitColourComponent v)
//            : this(h, s, v, new UnitColourComponent(ColourSpaceConstants.ONE))
//        { }

//        public HSVColour(DegreeColourComponent h, UnitColourComponent s, UnitColourComponent v, UnitColourComponent a)
//        {
//            H = h;
//            S = s;
//            V = v;
//            A = a;
//        }

//        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//        string DisplayString => $"H: {(ColourPrimitive)H:0.0°}, S: {(ColourPrimitive)S:0.000}, V: {(ColourPrimitive)V:0.000}, A: {(ColourPrimitive)A:0.000}";
//        public override string ToString() => $"H: {(ColourPrimitive)H}, S: {(ColourPrimitive)S}, V: {(ColourPrimitive)V}, A: {(ColourPrimitive)A}";

//        /// <summary>
//        /// Hue
//        /// 0° - 360°. 0° = Red, 120° = Green, 240° = Blue
//        /// </summary>
//        public DegreeColourComponent H { get; set; }
//        /// <summary>
//        /// Saturation
//        /// 0 - 1. 0 = No saturation (greyscale), 1 = Full saturation (full colour)
//        /// </summary>
//        public UnitColourComponent S { get; set; }
//        /// <summary>
//        /// Value
//        /// 0 - 1. 0 = Full colour, 1 = Black
//        /// </summary>
//        public UnitColourComponent V { get; set; }
//        public UnitColourComponent A { get; set; }

//        public static implicit operator ColourVector(HSVColour colour) => new ColourVector(colour.H, colour.S, colour.V);
//    }
//    #endregion HSVColour
//}

///*
//This is free and unencumbered software released into the public domain.

//Anyone is free to copy, modify, publish, use, compile, sell, or
//distribute this software, either in source code form or as a compiled
//binary, for any purpose, commercial or non-commercial, and by any
//means.

//In jurisdictions that recognize copyright laws, the author or authors
//of this software dedicate any and all copyright interest in the
//software to the public domain. We make this dedication for the benefit
//of the public at large and to the detriment of our heirs and
//successors. We intend this dedication to be an overt act of
//relinquishment in perpetuity of all present and future rights to this
//software under copyright law.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

//For more information, please refer to <http://unlicense.org>
//*/
