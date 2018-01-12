using HisRoyalRedness.com.ColourConstants;
using System;
using System.Collections.Generic;
using System.Text;

/*
    Conversion routines to convert between various colour spaces and colour primitives

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

    public static class ColourConversions
    {
        #region RGB <--> HSV
        public static HSVColour ToHSV(this SRGBColour srgb)
        {
            var r = srgb.R.ToUnitColour().Value;
            var g = srgb.G.ToUnitColour().Value;
            var b = srgb.B.ToUnitColour().Value;
            var max = (ColourPrimitive)Math.Max(r, (ColourPrimitive)Math.Max(g, b));
            var min = (ColourPrimitive)Math.Min(r, (ColourPrimitive)Math.Min(g, b));
            var chroma = max - min;


            // https://en.wikipedia.org/wiki/HSL_and_HSV#Hue_and_chroma
            // https://en.wikipedia.org/wiki/HSL_and_HSV#Saturation
            // https://en.wikipedia.org/wiki/HSL_and_HSV#Lightness

            if (chroma == ColourSpaceConstants.ZERO)
                return new HSVColour(0, 0, max, srgb.A.ToUnitColour());

            else
            {
                var h_prime = max == r
                    ? (g - b) / chroma
                    : (max == g
                        ? (b - r) / chroma + ColourSpaceConstants.TWO
                        : (r - g) / chroma + ColourSpaceConstants.FOUR);
                return new HSVColour(
                    ColourSpaceConstants.SIXTY * h_prime,
                    chroma / max,
                    max,
                    srgb.A.ToUnitColour());
            }
        }

        public static SRGBColour ToSRGB(this HSVColour hsv)
        {
            var r = new UnitColourComponent(ColourSpaceConstants.ZERO);
            var g = new UnitColourComponent(ColourSpaceConstants.ZERO);
            var b = new UnitColourComponent(ColourSpaceConstants.ZERO);

            // https://en.wikipedia.org/wiki/HSL_and_HSV#From_HSV
            // http://www.easyrgb.com/en/math.php

            if (hsv.S == ColourSpaceConstants.ZERO)
            {
                var vb = hsv.V.ToByteColour();
                return new SRGBColour(vb, vb, vb, hsv.A.ToByteColour());
            }

            // Calcs simplified from Wikipedia and verified to match calcs at EasyRGB
            var h_unit = hsv.H.Value / ColourSpaceConstants.SIXTY;
            while (h_unit >= ColourSpaceConstants.SIX)
                h_unit -= ColourSpaceConstants.SIX;
            var h_int = Math.Floor(h_unit);

            var v = hsv.V.ToByteColour();
            var var_1 = ((UnitColourComponent)(hsv.V.Value * (ColourSpaceConstants.ONE - hsv.S.Value))).ToByteColour();                                 // 0 + m
            var var_2 = ((UnitColourComponent)(hsv.V.Value * (ColourSpaceConstants.ONE - hsv.S.Value * (h_unit - h_int)))).ToByteColour();              // m + X (1<=H<=2, 3<=H<=4, 5<=H<=6)
            var var_3 = ((UnitColourComponent)(hsv.V.Value * (ColourSpaceConstants.ONE - hsv.S.Value * (1 - (h_unit - h_int))))).ToByteColour();        // m + X (0<=H<=1, 2<=H<=3, 4<=H<=5)

            switch (h_int)
            {
                case 0: return new SRGBColour(v, var_3, var_1, hsv.A.ToByteColour());
                case 1: return new SRGBColour(var_2, v, var_1, hsv.A.ToByteColour());
                case 2: return new SRGBColour(var_1, v, var_3, hsv.A.ToByteColour());
                case 3: return new SRGBColour(var_1, var_2, v, hsv.A.ToByteColour());
                case 4: return new SRGBColour(var_3, var_1, v, hsv.A.ToByteColour());
                default: return new SRGBColour(v, var_1, var_2, hsv.A.ToByteColour());
            }
        }
        #endregion RGB <--> HSV

        #region RGB <--> HSL
        public static HSLColour ToHSL(this SRGBColour srgb)
        {
            var r = srgb.R.ToUnitColour().Value;
            var g = srgb.G.ToUnitColour().Value;
            var b = srgb.B.ToUnitColour().Value;
            var max = (ColourPrimitive)Math.Max(r, (ColourPrimitive)Math.Max(g, b));
            var min = (ColourPrimitive)Math.Min(r, (ColourPrimitive)Math.Min(g, b));
            var chroma = max - min;
            var l = (max + min) / ColourSpaceConstants.TWO;

            // https://en.wikipedia.org/wiki/HSL_and_HSV#Hue_and_chroma
            // https://en.wikipedia.org/wiki/HSL_and_HSV#Saturation
            // https://en.wikipedia.org/wiki/HSL_and_HSV#Lightness

            if (chroma == ColourSpaceConstants.ZERO)
                return new HSLColour(0, 0, l, srgb.A.ToUnitColour());

            else
            {
                var h_prime = max == r
                    ? (g - b) / chroma
                    : (max == g
                        ? (b - r) / chroma + ColourSpaceConstants.TWO
                        : (r - g) / chroma + ColourSpaceConstants.FOUR);
                return new HSLColour(
                    ColourSpaceConstants.SIXTY * h_prime,
                    chroma / (ColourSpaceConstants.ONE - Math.Abs(max + min - ColourSpaceConstants.ONE)),
                    l,
                    srgb.A.ToUnitColour());
            }
        }

        public static SRGBColour ToSRGB(this HSLColour hsl)
        {
            var r = new UnitColourComponent(ColourSpaceConstants.ZERO);
            var g = new UnitColourComponent(ColourSpaceConstants.ZERO);
            var b = new UnitColourComponent(ColourSpaceConstants.ZERO);

            // https://en.wikipedia.org/wiki/HSL_and_HSV#From_HSV
            // http://www.easyrgb.com/en/math.php

            if (hsl.S == ColourSpaceConstants.ZERO)
            {
                var lb = hsl.L.ToByteColour();
                return new SRGBColour(lb, lb, lb, hsl.A.ToByteColour());
            }

            // Calcs simplified from Wikipedia and verified to match calcs at EasyRGB
            var h_unit = hsl.H.Value / ColourSpaceConstants.SIXTY;
            while (h_unit >= ColourSpaceConstants.SIX)
                h_unit -= ColourSpaceConstants.SIX;
            var h_int = Math.Floor(h_unit);

            var c = hsl.S * (ColourSpaceConstants.ONE - Math.Abs(ColourSpaceConstants.TWO * hsl.L - ColourSpaceConstants.ONE));

            // X for 1st = C * (h_unit - h_int)
            // X for 2nd = C * (1 - (h_unit + h_int))

            switch (h_int)
            {
                case 0: return new SRGBColour(v, var_3, var_1, hsl.A.ToByteColour());
                case 1: return new SRGBColour(var_2, v, var_1, hsl.A.ToByteColour());
                case 2: return new SRGBColour(var_1, v, var_3, hsl.A.ToByteColour());
                case 3: return new SRGBColour(var_1, var_2, v, hsl.A.ToByteColour());
                case 4: return new SRGBColour(var_3, var_1, v, hsl.A.ToByteColour());
                default: return new SRGBColour(v, var_1, var_2, hsl.A.ToByteColour());
            }
        }
        #endregion RGB <--> HSL

        #region HSV <--> HSL
        public static HSLColour ToHSL(this HSVColour hsv)
        {
            if (hsv.S == ColourSpaceConstants.ZERO)
                return new HSLColour(hsv.H, hsv.V, hsv.V, hsv.A);

            var s = 0;
            var l = 0;
            return new HSLColour(hsv.H, s, l, hsv.A);
        }

        public static HSVColour ToHSV(this HSLColour hsl)
        {
            if (hsl.S == ColourSpaceConstants.ZERO)
                return new HSVColour(hsl.H, hsl.L, hsl.L, hsl.A);

            var s = 0;
            var v = 0;
            return new HSVColour(hsl.H, s, v, hsl.A);
        }
        #endregion HSV <--> HSL
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
