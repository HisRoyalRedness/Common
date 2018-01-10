using System;
using System.Collections.Generic;
using System.Text;

/*
    Useful constants for the colour classes, typed as ColourPrimitives

    Keith Fletcher
    Jan 2018

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com.ColourConstants
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    internal static class ColourSpaceConstants
    {
        internal const ColourPrimitive ZERO = (ColourPrimitive)0.0;
        internal const ColourPrimitive ONE = (ColourPrimitive)1.0;
        internal const ColourPrimitive TWO = (ColourPrimitive)2.0;
        internal const ColourPrimitive FOUR = (ColourPrimitive)4.0;
        internal const ColourPrimitive SIXTY = (ColourPrimitive)60.0;
        internal const ColourPrimitive TWO_FIVE_FIVE = (ColourPrimitive)255.0;
        internal const ColourPrimitive THREE_SIXTY = (ColourPrimitive)360.0;

        internal const byte BYTECOLOURCOMPONENT_MIN_VALUE = 0;
        internal const byte BYTECOLOURCOMPONENT_MAX_VALUE = 255;
        internal const ColourPrimitive UNITCOLOURCOMPONENT_MIN_VALUE = ZERO;
        internal const ColourPrimitive UNITCOLOURCOMPONENT_MAX_VALUE = ONE;
        internal const ColourPrimitive DEGREECOLOURCOMPONENT_MIN_VALUE = ZERO;
        internal const ColourPrimitive DEGREECOLOURCOMPONENT_MAX_VALUE = THREE_SIXTY;
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
