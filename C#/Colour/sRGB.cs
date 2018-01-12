using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Media;

/*
    Defines a colour instance using the sRGB colour space
    https://en.wikipedia.org/wiki/SRGB

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

    [DebuggerDisplay("{DisplayString}")]
    public struct SRGBColour
    {
        public SRGBColour(ByteColourComponent r, ByteColourComponent g, ByteColourComponent b)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = 255;
        }

        public SRGBColour(ByteColourComponent r, ByteColourComponent g, ByteColourComponent b, ByteColourComponent a)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }

        public SRGBColour(ColourVector v, ByteColourComponent a)
        {
            _r = v.X;
            _g = v.Y;
            _b = v.Z;
            _a = a;
        }

        public string Hex24 => $"#{(byte)R:X2}{(byte)G:X2}{(byte)B:X2}";
        public string Hex32 => $"#{(byte)A:X2}{(byte)R:X2}{(byte)G:X2}{(byte)B:X2}";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"R: {R}, G: {G}, B: {B}, A: {A}, {Hex32}";
        public override string ToString() => Hex32;

        /// <summary>
        /// Red
        /// </summary>
        public ByteColourComponent R => _r;
        readonly ByteColourComponent _r;
        /// <summary>
        /// Green
        /// </summary>
        public ByteColourComponent G => _g;
        readonly ByteColourComponent _g;
        /// <summary>
        /// Blue
        /// </summary>
        public ByteColourComponent B => _b;
        readonly ByteColourComponent _b;
        /// <summary>
        /// Alpha
        /// 0 - 255. 0 = Transparent, 255 = Opaque
        /// </summary>
        public ByteColourComponent A => _a;
        readonly ByteColourComponent _a;

        #region Add and subtract
        public static SRGBColour operator +(SRGBColour a, SRGBColour b)
            => new SRGBColour((int)a.R + (int)b.R, (int)a.G + (int)b.G, (int)a.B + (int)b.B, (int)a.A + (int)b.A);
        public static SRGBColour operator -(SRGBColour a, SRGBColour b)
            => new SRGBColour((int)a.R - (int)b.R, (int)a.G - (int)b.G, (int)a.B - (int)b.B, (int)a.A - (int)b.A);
        #endregion Add and subtract

        #region Implicit and explicit casts
        public static implicit operator Color(SRGBColour colour) => Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        public static implicit operator ColourVector(SRGBColour colour) => new ColourVector(colour.R, colour.G, colour.B);

        public static explicit operator SRGBColour(Color colour) => new SRGBColour(colour.R, colour.G, colour.B, colour.A);
        public static explicit operator SRGBColour(ColourVector vector) => new SRGBColour(vector.X, vector.Y, vector.Z);
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
