//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Windows.Media;

///*
//    Defines a colour instance using the sRGB colour space
//    https://en.wikipedia.org/wiki/SRGB

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

//    #region SRGB
//    [DebuggerDisplay("R: {R}, G: {G}, B: {B}, A: {A}, {Hex32}")]
//    public struct SRGBColour
//    {
//        public SRGBColour(RGBColourComponent r, RGBColourComponent g, RGBColourComponent b)
//            : this(r, g, b, new RGBColourComponent(255))
//        { }

//        public SRGBColour(byte r, byte g, byte b)
//            : this(new RGBColourComponent(r), new RGBColourComponent(g), new RGBColourComponent(b))
//        { }

//        public SRGBColour(byte r, byte g, byte b, byte a)
//            : this(new RGBColourComponent(r), new RGBColourComponent(g), new RGBColourComponent(b), new RGBColourComponent(a))
//        { }

//        public SRGBColour(RGBColourComponent r, RGBColourComponent g, RGBColourComponent b, RGBColourComponent a)
//        {
//            R = r;
//            G = g;
//            B = b;
//            A = a;
//        }

//        public string Hex24 => $"#{(byte)R:X2}{(byte)G:X2}{(byte)B:X2}";
//        public string Hex32 => $"#{(byte)A:X2}{(byte)R:X2}{(byte)G:X2}{(byte)B:X2}";
//        public override string ToString() => Hex32;

//        /// <summary>
//        /// Red
//        /// </summary>
//        public RGBColourComponent R { get; set; }
//        /// <summary>
//        /// Green
//        /// </summary>
//        public RGBColourComponent G { get; set; }
//        /// <summary>
//        /// Blue
//        /// </summary>
//        public RGBColourComponent B { get; set; }
//        /// <summary>
//        /// Alpha
//        /// 0 - 255. 0 = Transparent, 255 = Opaque
//        /// </summary>
//        public RGBColourComponent A { get; set; }

//        public static implicit operator Color(SRGBColour colour) => Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
//        public static implicit operator SRGBColour(Color colour) => new SRGBColour(colour.R, colour.G, colour.B, colour.A);
//        public static implicit operator ColourVector(SRGBColour colour) => new ColourVector(colour.R, colour.G, colour.B);

//    }
//    #endregion SRGB

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
