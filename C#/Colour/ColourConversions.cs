//using HisRoyalRedness.com.ColourConstants;
//using System;
//using System.Collections.Generic;
//using System.Text;

///*
//    Conversion routines to convert between various colour spaces and colour primitives

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

//    public static class ColourConversions
//    {
//        #region RGB <--> HSV
//        public static HSVColour ToHSV(this SRGBColour srgb)
//        {
//            var r = srgb.R.ToUnitColour().Value;
//            var g = srgb.R.ToUnitColour().Value;
//            var b = srgb.R.ToUnitColour().Value;
//            var max = (ColourPrimitive)Math.Max(r, (ColourPrimitive)Math.Max(g, b));
//            var min = (ColourPrimitive)Math.Min(r, (ColourPrimitive)Math.Min(g, b));
//            var chroma = max - min;

//            var hue = new DegreeColourComponent(ColourSpaceConstants.ZERO);
//            var sat = new UnitColourComponent(ColourSpaceConstants.ZERO);
//            var val = new UnitColourComponent(max);

//            // https://en.wikipedia.org/wiki/HSL_and_HSV#Hue_and_chroma
//            // https://en.wikipedia.org/wiki/HSL_and_HSV#Saturation
//            // https://en.wikipedia.org/wiki/HSL_and_HSV#Lightness
//            if (chroma != ColourSpaceConstants.ZERO)
//            {
//                var h_prime = max == r
//                    ? (g - b) / chroma
//                    : (max == g
//                        ? (b - r) / chroma + ColourSpaceConstants.TWO
//                        : (r - g) / chroma + ColourSpaceConstants.FOUR);
//                hue.Value = ColourSpaceConstants.SIXTY * h_prime;
//                sat.Value = chroma / max;
//            }
//            return new HSVColour(hue, sat, val, srgb.A.ToUnitColour());
//        }

//        public static SRGBColour ToSRGB(this HSVColour hsv)
//        {
//            var r = new UnitColourComponent(ColourSpaceConstants.ZERO);
//            var g = new UnitColourComponent(ColourSpaceConstants.ZERO);
//            var b = new UnitColourComponent(ColourSpaceConstants.ZERO);

//            var chroma = hsv.V.Value * hsv.S.Value;
//            var min = hsv.V.Value - chroma;
//            // https://en.wikipedia.org/wiki/HSL_and_HSV#From_HSV
//            if (chroma != ColourSpaceConstants.ZERO)
//            {
                
//                var h_prime = hsv.H.Value / ColourSpaceConstants.SIXTY;
//            }
//            //var scaledRGB = (ColourVector)srgb / ColourSpaceConstants.TWO_FIVE_FIVE;
//            //var max = scaledRGB.Max();
//            //var min = scaledRGB.Min();
//            //var chroma = max - min;

//            //var hue = new DegreeColourComponent(ColourSpaceConstants.ZERO);
//            //var sat = new UnitColourComponent(ColourSpaceConstants.ZERO);
//            //var val = new UnitColourComponent(max);

//            //// https://en.wikipedia.org/wiki/HSL_and_HSV#Hue_and_chroma
//            //// https://en.wikipedia.org/wiki/HSL_and_HSV#Saturation
//            //// https://en.wikipedia.org/wiki/HSL_and_HSV#Lightness
//            //if (chroma != 0)
//            //{
//            //    var h_prime = max == scaledRGB.R
//            //        ? (scaledRGB.G - scaledRGB.B) / chroma
//            //        : (max == scaledRGB.G
//            //            ? (scaledRGB.B - scaledRGB.R) / chroma + ColourSpaceConstants.TWO
//            //            : (scaledRGB.R - scaledRGB.G) / chroma + ColourSpaceConstants.FOUR);
//            //    hue.Value = ColourSpaceConstants.SIXTY * h_prime;
//            //    sat.Value = chroma / max;
//            //}
//            return new SRGBColour(r.ToRGBColour(), g.ToRGBColour(), b.ToRGBColour(), hsv.A.ToRGBColour());
//        }
//        #endregion RGB <--> HSV
//    }
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
