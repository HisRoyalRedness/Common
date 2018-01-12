using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;
using HisRoyalRedness.com.ColourConstants;

/*
    A generated file, from a template, that builds test code common to all colour components

    Keith Fletcher
    Jan 2018

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com.Tests
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    #region ByteColourComponent_Test
    [TestClass]
    public partial class ByteColourComponent_Test
    {
        // Constants that are just out, on, or just in the range of acceptable values
        internal const int MIN = (int)ByteColourComponent.MIN_VAL;
        internal const int MAX = (int)ByteColourComponent.MAX_VAL;
        internal const int STEP = (MAX - MIN) / 100;
        internal const int ONE_BELOW_MIN = MIN - STEP;
        internal const int ONE_ABOVE_MIN = MIN + STEP;
        internal const int MID = (MAX - MIN) / 2;
        internal const int ONE_BELOW_MAX = MAX - STEP;
        internal const int ONE_ABOVE_MAX = MAX + STEP;
        
        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Min_and_Max_generated()
        {
            // There should be a manually written test that checks the min and max against hard-coded values
            ByteColourComponent.MinValue.Should().Be(MIN, $"the minimum value for ByteColourComponent should be {MIN}");
            ByteColourComponent.MaxValue.Should().Be(MAX, $"the maximum value for ByteColourComponent should be {MAX}");
        } // Test_ByteColourComponent_Min_and_Max_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_IsWrappingValue_generated()
        {
            ByteColourComponent.IsWrappingValue.Should().BeFalse("ByteColourComponent is not a wrapping value type");
        } // Test_ByteColourComponent_IsWrappingValue_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Construction_default_value_generated()
        {
            new ByteColourComponent().Should().Be(MIN, $"the default value for the default constructor should be {MIN}");
        } // Test_ByteColourComponent_Construction_default_value_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Construction_different_arg_types_generated()
        {
            new ByteColourComponent((int)ONE_ABOVE_MIN).Should().Be((byte)(int)ONE_ABOVE_MIN, $"ByteColourComponent should construct with a int constructor argument");
            new ByteColourComponent((int)MID).Should().Be((byte)(int)MID, $"ByteColourComponent should construct with a int constructor argument");
            new ByteColourComponent((int)ONE_BELOW_MAX).Should().Be((byte)(int)ONE_BELOW_MAX, $"ByteColourComponent should construct with a int constructor argument");
            new ByteColourComponent((float)ONE_ABOVE_MIN).Should().Be((byte)(float)ONE_ABOVE_MIN, $"ByteColourComponent should construct with a float constructor argument");
            new ByteColourComponent((float)MID).Should().Be((byte)(float)MID, $"ByteColourComponent should construct with a float constructor argument");
            new ByteColourComponent((float)ONE_BELOW_MAX).Should().Be((byte)(float)ONE_BELOW_MAX, $"ByteColourComponent should construct with a float constructor argument");
            new ByteColourComponent((double)ONE_ABOVE_MIN).Should().Be((byte)(double)ONE_ABOVE_MIN, $"ByteColourComponent should construct with a double constructor argument");
            new ByteColourComponent((double)MID).Should().Be((byte)(double)MID, $"ByteColourComponent should construct with a double constructor argument");
            new ByteColourComponent((double)ONE_BELOW_MAX).Should().Be((byte)(double)ONE_BELOW_MAX, $"ByteColourComponent should construct with a double constructor argument");
        } // Test_ByteColourComponent_Construction_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Implicit_cast_different_arg_types_generated()
        {
            ((ByteColourComponent)(int)ONE_ABOVE_MIN).Should().Be((byte)(int)ONE_ABOVE_MIN, $"ByteColourComponent should construct from a cast from int");
            ((ByteColourComponent)(int)MID).Should().Be((byte)(int)MID, $"ByteColourComponent should construct from a cast from int");
            ((ByteColourComponent)(int)ONE_BELOW_MAX).Should().Be((byte)(int)ONE_BELOW_MAX, $"ByteColourComponent should construct from a cast from int");
            ((ByteColourComponent)(float)ONE_ABOVE_MIN).Should().Be((byte)(float)ONE_ABOVE_MIN, $"ByteColourComponent should construct from a cast from float");
            ((ByteColourComponent)(float)MID).Should().Be((byte)(float)MID, $"ByteColourComponent should construct from a cast from float");
            ((ByteColourComponent)(float)ONE_BELOW_MAX).Should().Be((byte)(float)ONE_BELOW_MAX, $"ByteColourComponent should construct from a cast from float");
            ((ByteColourComponent)(double)ONE_ABOVE_MIN).Should().Be((byte)(double)ONE_ABOVE_MIN, $"ByteColourComponent should construct from a cast from double");
            ((ByteColourComponent)(double)MID).Should().Be((byte)(double)MID, $"ByteColourComponent should construct from a cast from double");
            ((ByteColourComponent)(double)ONE_BELOW_MAX).Should().Be((byte)(double)ONE_BELOW_MAX, $"ByteColourComponent should construct from a cast from double");
        } // Test_ByteColourComponent_Implicit_cast_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Implicit_cast_value_clipping_generated()
        {
            ((ByteColourComponent)(int)ONE_BELOW_MIN).Should().Be(MIN, $"ByteColourComponent should clip to {MIN} when constructed with a value below minimum");
            ((ByteColourComponent)(int)MIN).Should().Be(MIN, $"ByteColourComponent should clip to {MIN} when constructed with a value equal to minimum");
            ((ByteColourComponent)(int)MAX).Should().Be(MAX, $"ByteColourComponent should clip to {MAX} when constructed with a value equal maximum");
            ((ByteColourComponent)(int)ONE_ABOVE_MAX).Should().Be(MAX, $"ByteColourComponent should clip to {MAX} when constructed with a value above maximum");
            ((ByteColourComponent)(float)ONE_BELOW_MIN).Should().Be(MIN, $"ByteColourComponent should clip to {MIN} when constructed with a value below minimum");
            ((ByteColourComponent)(float)MIN).Should().Be(MIN, $"ByteColourComponent should clip to {MIN} when constructed with a value equal to minimum");
            ((ByteColourComponent)(float)MAX).Should().Be(MAX, $"ByteColourComponent should clip to {MAX} when constructed with a value equal maximum");
            ((ByteColourComponent)(float)ONE_ABOVE_MAX).Should().Be(MAX, $"ByteColourComponent should clip to {MAX} when constructed with a value above maximum");
            ((ByteColourComponent)(double)ONE_BELOW_MIN).Should().Be(MIN, $"ByteColourComponent should clip to {MIN} when constructed with a value below minimum");
            ((ByteColourComponent)(double)MIN).Should().Be(MIN, $"ByteColourComponent should clip to {MIN} when constructed with a value equal to minimum");
            ((ByteColourComponent)(double)MAX).Should().Be(MAX, $"ByteColourComponent should clip to {MAX} when constructed with a value equal maximum");
            ((ByteColourComponent)(double)ONE_ABOVE_MAX).Should().Be(MAX, $"ByteColourComponent should clip to {MAX} when constructed with a value above maximum");
        } // Test_ByteColourComponent_Implicit_cast_value_clipping_generated

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Implicit_casting_generated()
        {
            ((byte)new ByteColourComponent(ONE_ABOVE_MIN)).Should().Be((byte)ONE_ABOVE_MIN);
            ((byte)new ByteColourComponent(MID)).Should().Be((byte)MID);
            ((byte)new ByteColourComponent(ONE_BELOW_MAX)).Should().Be((byte)ONE_BELOW_MAX);

        } // Test_ByteColourComponent_Implicit_casting_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Addition_generated()
        {
            byte sum = (byte)MID + (byte)STEP;
            (new ByteColourComponent(MID) + new ByteColourComponent(STEP)).Should().Be(sum, $"should add two ByteColourComponent's");

        } // Test_ByteColourComponent_Addition_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Subtraction_generated()
        {
            byte diff = (byte)MID - (byte)STEP;
            (new ByteColourComponent(MID) - new ByteColourComponent(STEP)).Should().Be(diff, $"should subtract two ByteColourComponent's");

        } // Test_ByteColourComponent_Subtraction_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Addition_with_clipping_generated()
        {
            (new ByteColourComponent(MID) + new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add two ByteColourComponent's and clip to MAX if the sum exceeds MaxValue");

        } // Test_ByteColourComponent_Addition_with_clipping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(ByteColourComponent))]
        public void Test_ByteColourComponent_Subtraction_with_clipping_generated()
        {
            (new ByteColourComponent(MID) - new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract two ByteColourComponent's and clip to MIN if the difference falls below MinValue");

        } // Test_ByteColourComponent_Subtraction_with_clipping_generated()


    } // public partial class ByteColourComponent_Test
    #endregion ByteColourComponent_Test

    #region UnitColourComponent_Test
    [TestClass]
    public partial class UnitColourComponent_Test
    {
        // Constants that are just out, on, or just in the range of acceptable values
        internal const ColourPrimitive MIN = (ColourPrimitive)UnitColourComponent.MIN_VAL;
        internal const ColourPrimitive MAX = (ColourPrimitive)UnitColourComponent.MAX_VAL;
        internal const ColourPrimitive STEP = (MAX - MIN) / 100;
        internal const ColourPrimitive ONE_BELOW_MIN = MIN - STEP;
        internal const ColourPrimitive ONE_ABOVE_MIN = MIN + STEP;
        internal const ColourPrimitive MID = (MAX - MIN) / 2;
        internal const ColourPrimitive ONE_BELOW_MAX = MAX - STEP;
        internal const ColourPrimitive ONE_ABOVE_MAX = MAX + STEP;
        
        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Min_and_Max_generated()
        {
            // There should be a manually written test that checks the min and max against hard-coded values
            UnitColourComponent.MinValue.Should().Be(MIN, $"the minimum value for UnitColourComponent should be {MIN}");
            UnitColourComponent.MaxValue.Should().Be(MAX, $"the maximum value for UnitColourComponent should be {MAX}");
        } // Test_UnitColourComponent_Min_and_Max_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_IsWrappingValue_generated()
        {
            UnitColourComponent.IsWrappingValue.Should().BeFalse("UnitColourComponent is not a wrapping value type");
        } // Test_UnitColourComponent_IsWrappingValue_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Construction_default_value_generated()
        {
            new UnitColourComponent().Should().Be(MIN, $"the default value for the default constructor should be {MIN}");
        } // Test_UnitColourComponent_Construction_default_value_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Construction_different_arg_types_generated()
        {
            new UnitColourComponent((float)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(float)ONE_ABOVE_MIN, $"UnitColourComponent should construct with a float constructor argument");
            new UnitColourComponent((float)MID).Should().Be((ColourPrimitive)(float)MID, $"UnitColourComponent should construct with a float constructor argument");
            new UnitColourComponent((float)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(float)ONE_BELOW_MAX, $"UnitColourComponent should construct with a float constructor argument");
            new UnitColourComponent((double)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(double)ONE_ABOVE_MIN, $"UnitColourComponent should construct with a double constructor argument");
            new UnitColourComponent((double)MID).Should().Be((ColourPrimitive)(double)MID, $"UnitColourComponent should construct with a double constructor argument");
            new UnitColourComponent((double)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(double)ONE_BELOW_MAX, $"UnitColourComponent should construct with a double constructor argument");
        } // Test_UnitColourComponent_Construction_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Implicit_cast_different_arg_types_generated()
        {
            ((UnitColourComponent)(float)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(float)ONE_ABOVE_MIN, $"UnitColourComponent should construct from a cast from float");
            ((UnitColourComponent)(float)MID).Should().Be((ColourPrimitive)(float)MID, $"UnitColourComponent should construct from a cast from float");
            ((UnitColourComponent)(float)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(float)ONE_BELOW_MAX, $"UnitColourComponent should construct from a cast from float");
            ((UnitColourComponent)(double)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(double)ONE_ABOVE_MIN, $"UnitColourComponent should construct from a cast from double");
            ((UnitColourComponent)(double)MID).Should().Be((ColourPrimitive)(double)MID, $"UnitColourComponent should construct from a cast from double");
            ((UnitColourComponent)(double)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(double)ONE_BELOW_MAX, $"UnitColourComponent should construct from a cast from double");
        } // Test_UnitColourComponent_Implicit_cast_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Implicit_cast_value_clipping_generated()
        {
            ((UnitColourComponent)(int)ONE_BELOW_MIN).Should().Be(MIN, $"UnitColourComponent should clip to {MIN} when constructed with a value below minimum");
            ((UnitColourComponent)(int)MIN).Should().Be(MIN, $"UnitColourComponent should clip to {MIN} when constructed with a value equal to minimum");
            ((UnitColourComponent)(int)MAX).Should().Be(MAX, $"UnitColourComponent should clip to {MAX} when constructed with a value equal maximum");
            ((UnitColourComponent)(int)ONE_ABOVE_MAX).Should().Be(MAX, $"UnitColourComponent should clip to {MAX} when constructed with a value above maximum");
            ((UnitColourComponent)(float)ONE_BELOW_MIN).Should().Be(MIN, $"UnitColourComponent should clip to {MIN} when constructed with a value below minimum");
            ((UnitColourComponent)(float)MIN).Should().Be(MIN, $"UnitColourComponent should clip to {MIN} when constructed with a value equal to minimum");
            ((UnitColourComponent)(float)MAX).Should().Be(MAX, $"UnitColourComponent should clip to {MAX} when constructed with a value equal maximum");
            ((UnitColourComponent)(float)ONE_ABOVE_MAX).Should().Be(MAX, $"UnitColourComponent should clip to {MAX} when constructed with a value above maximum");
            ((UnitColourComponent)(double)ONE_BELOW_MIN).Should().Be(MIN, $"UnitColourComponent should clip to {MIN} when constructed with a value below minimum");
            ((UnitColourComponent)(double)MIN).Should().Be(MIN, $"UnitColourComponent should clip to {MIN} when constructed with a value equal to minimum");
            ((UnitColourComponent)(double)MAX).Should().Be(MAX, $"UnitColourComponent should clip to {MAX} when constructed with a value equal maximum");
            ((UnitColourComponent)(double)ONE_ABOVE_MAX).Should().Be(MAX, $"UnitColourComponent should clip to {MAX} when constructed with a value above maximum");
        } // Test_UnitColourComponent_Implicit_cast_value_clipping_generated

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Implicit_casting_generated()
        {
            ((ColourPrimitive)new UnitColourComponent(ONE_ABOVE_MIN)).Should().Be((ColourPrimitive)ONE_ABOVE_MIN);
            ((ColourPrimitive)new UnitColourComponent(MID)).Should().Be((ColourPrimitive)MID);
            ((ColourPrimitive)new UnitColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)ONE_BELOW_MAX);

        } // Test_UnitColourComponent_Implicit_casting_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Addition_generated()
        {
            ColourPrimitive sum = (ColourPrimitive)MID + (ColourPrimitive)STEP;
            (new UnitColourComponent(MID) + new UnitColourComponent(STEP)).Should().Be(sum, $"should add two UnitColourComponent's");

        } // Test_UnitColourComponent_Addition_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Subtraction_generated()
        {
            ColourPrimitive diff = (ColourPrimitive)MID - (ColourPrimitive)STEP;
            (new UnitColourComponent(MID) - new UnitColourComponent(STEP)).Should().Be(diff, $"should subtract two UnitColourComponent's");

        } // Test_UnitColourComponent_Subtraction_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Addition_with_clipping_generated()
        {
            (new UnitColourComponent(MID) + new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add two UnitColourComponent's and clip to MAX if the sum exceeds MaxValue");

        } // Test_UnitColourComponent_Addition_with_clipping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(UnitColourComponent))]
        public void Test_UnitColourComponent_Subtraction_with_clipping_generated()
        {
            (new UnitColourComponent(MID) - new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract two UnitColourComponent's and clip to MIN if the difference falls below MinValue");

        } // Test_UnitColourComponent_Subtraction_with_clipping_generated()


    } // public partial class UnitColourComponent_Test
    #endregion UnitColourComponent_Test

    #region DegreeColourComponent_Test
    [TestClass]
    public partial class DegreeColourComponent_Test
    {
        // Constants that are just out, on, or just in the range of acceptable values
        internal const ColourPrimitive MIN = (ColourPrimitive)DegreeColourComponent.MIN_VAL;
        internal const ColourPrimitive MAX = (ColourPrimitive)DegreeColourComponent.MAX_VAL;
        internal const ColourPrimitive STEP = (MAX - MIN) / 100;
        internal const ColourPrimitive ONE_BELOW_MIN = MIN - STEP;
        internal const ColourPrimitive ONE_ABOVE_MIN = MIN + STEP;
        internal const ColourPrimitive MID = (MAX - MIN) / 2;
        internal const ColourPrimitive ONE_BELOW_MAX = MAX - STEP;
        internal const ColourPrimitive ONE_ABOVE_MAX = MAX + STEP;
        
        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Min_and_Max_generated()
        {
            // There should be a manually written test that checks the min and max against hard-coded values
            DegreeColourComponent.MinValue.Should().Be(MIN, $"the minimum value for DegreeColourComponent should be {MIN}");
            DegreeColourComponent.MaxValue.Should().Be(MAX, $"the maximum value for DegreeColourComponent should be {MAX}");
        } // Test_DegreeColourComponent_Min_and_Max_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_IsWrappingValue_generated()
        {
            DegreeColourComponent.IsWrappingValue.Should().BeTrue("DegreeColourComponent is a wrapping value type");
        } // Test_DegreeColourComponent_IsWrappingValue_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Construction_default_value_generated()
        {
            new DegreeColourComponent().Should().Be(MIN, $"the default value for the default constructor should be {MIN}");
        } // Test_DegreeColourComponent_Construction_default_value_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Construction_different_arg_types_generated()
        {
            new DegreeColourComponent((int)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(int)ONE_ABOVE_MIN, $"DegreeColourComponent should construct with a int constructor argument");
            new DegreeColourComponent((int)MID).Should().Be((ColourPrimitive)(int)MID, $"DegreeColourComponent should construct with a int constructor argument");
            new DegreeColourComponent((int)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(int)ONE_BELOW_MAX, $"DegreeColourComponent should construct with a int constructor argument");
            new DegreeColourComponent((float)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(float)ONE_ABOVE_MIN, $"DegreeColourComponent should construct with a float constructor argument");
            new DegreeColourComponent((float)MID).Should().Be((ColourPrimitive)(float)MID, $"DegreeColourComponent should construct with a float constructor argument");
            new DegreeColourComponent((float)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(float)ONE_BELOW_MAX, $"DegreeColourComponent should construct with a float constructor argument");
            new DegreeColourComponent((double)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(double)ONE_ABOVE_MIN, $"DegreeColourComponent should construct with a double constructor argument");
            new DegreeColourComponent((double)MID).Should().Be((ColourPrimitive)(double)MID, $"DegreeColourComponent should construct with a double constructor argument");
            new DegreeColourComponent((double)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(double)ONE_BELOW_MAX, $"DegreeColourComponent should construct with a double constructor argument");
        } // Test_DegreeColourComponent_Construction_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Implicit_cast_different_arg_types_generated()
        {
            ((DegreeColourComponent)(int)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(int)ONE_ABOVE_MIN, $"DegreeColourComponent should construct from a cast from int");
            ((DegreeColourComponent)(int)MID).Should().Be((ColourPrimitive)(int)MID, $"DegreeColourComponent should construct from a cast from int");
            ((DegreeColourComponent)(int)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(int)ONE_BELOW_MAX, $"DegreeColourComponent should construct from a cast from int");
            ((DegreeColourComponent)(float)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(float)ONE_ABOVE_MIN, $"DegreeColourComponent should construct from a cast from float");
            ((DegreeColourComponent)(float)MID).Should().Be((ColourPrimitive)(float)MID, $"DegreeColourComponent should construct from a cast from float");
            ((DegreeColourComponent)(float)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(float)ONE_BELOW_MAX, $"DegreeColourComponent should construct from a cast from float");
            ((DegreeColourComponent)(double)ONE_ABOVE_MIN).Should().Be((ColourPrimitive)(double)ONE_ABOVE_MIN, $"DegreeColourComponent should construct from a cast from double");
            ((DegreeColourComponent)(double)MID).Should().Be((ColourPrimitive)(double)MID, $"DegreeColourComponent should construct from a cast from double");
            ((DegreeColourComponent)(double)ONE_BELOW_MAX).Should().Be((ColourPrimitive)(double)ONE_BELOW_MAX, $"DegreeColourComponent should construct from a cast from double");
        } // Test_DegreeColourComponent_Implicit_cast_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Construction_value_wrapping_generated()
        {
            var wrapMinInt = DegreeColourComponent.MAX_VAL + (ColourPrimitive)(int)ONE_BELOW_MIN;
            var wrapMaxInt = (ColourPrimitive)(int)ONE_ABOVE_MAX - DegreeColourComponent.MAX_VAL;
            new DegreeColourComponent((int)ONE_BELOW_MIN).Should().Be(wrapMinInt, $"DegreeColourComponent should wrap to {wrapMinInt} when constructed with a value below minimum");
            new DegreeColourComponent((int)MIN).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal to minimum");
            new DegreeColourComponent((int)MAX).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal maximum");
            new DegreeColourComponent((int)ONE_ABOVE_MAX).Should().Be(wrapMaxInt, $"DegreeColourComponent should wrap to {wrapMaxInt} when constructed with a value above maximum");
            var wrapMinFloat = DegreeColourComponent.MAX_VAL + (ColourPrimitive)(float)ONE_BELOW_MIN;
            var wrapMaxFloat = (ColourPrimitive)(float)ONE_ABOVE_MAX - DegreeColourComponent.MAX_VAL;
            new DegreeColourComponent((float)ONE_BELOW_MIN).Should().Be(wrapMinFloat, $"DegreeColourComponent should wrap to {wrapMinFloat} when constructed with a value below minimum");
            new DegreeColourComponent((float)MIN).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal to minimum");
            new DegreeColourComponent((float)MAX).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal maximum");
            new DegreeColourComponent((float)ONE_ABOVE_MAX).Should().Be(wrapMaxFloat, $"DegreeColourComponent should wrap to {wrapMaxFloat} when constructed with a value above maximum");
            var wrapMinDouble = DegreeColourComponent.MAX_VAL + (ColourPrimitive)(double)ONE_BELOW_MIN;
            var wrapMaxDouble = (ColourPrimitive)(double)ONE_ABOVE_MAX - DegreeColourComponent.MAX_VAL;
            new DegreeColourComponent((double)ONE_BELOW_MIN).Should().Be(wrapMinDouble, $"DegreeColourComponent should wrap to {wrapMinDouble} when constructed with a value below minimum");
            new DegreeColourComponent((double)MIN).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal to minimum");
            new DegreeColourComponent((double)MAX).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal maximum");
            new DegreeColourComponent((double)ONE_ABOVE_MAX).Should().Be(wrapMaxDouble, $"DegreeColourComponent should wrap to {wrapMaxDouble} when constructed with a value above maximum");
        } // Test_DegreeColourComponent_Construction_value_wrapping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Implicit_cast_value_wrapping_generated()
        {
            var wrapMinInt = DegreeColourComponent.MAX_VAL + (ColourPrimitive)(int)ONE_BELOW_MIN;
            var wrapMaxInt = (ColourPrimitive)(int)ONE_ABOVE_MAX - DegreeColourComponent.MAX_VAL;
            ((DegreeColourComponent)(int)ONE_BELOW_MIN).Should().Be(wrapMinInt, $"DegreeColourComponent should wrap to {wrapMinInt} when constructed with a value below minimum");
            ((DegreeColourComponent)(int)MIN).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal to minimum");
            ((DegreeColourComponent)(int)MAX).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal maximum");
            ((DegreeColourComponent)(int)ONE_ABOVE_MAX).Should().Be(wrapMaxInt, $"DegreeColourComponent should wrap to {wrapMaxInt} when constructed with a value above maximum");
            var wrapMinFloat = DegreeColourComponent.MAX_VAL + (ColourPrimitive)(float)ONE_BELOW_MIN;
            var wrapMaxFloat = (ColourPrimitive)(float)ONE_ABOVE_MAX - DegreeColourComponent.MAX_VAL;
            ((DegreeColourComponent)(float)ONE_BELOW_MIN).Should().Be(wrapMinFloat, $"DegreeColourComponent should wrap to {wrapMinFloat} when constructed with a value below minimum");
            ((DegreeColourComponent)(float)MIN).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal to minimum");
            ((DegreeColourComponent)(float)MAX).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal maximum");
            ((DegreeColourComponent)(float)ONE_ABOVE_MAX).Should().Be(wrapMaxFloat, $"DegreeColourComponent should wrap to {wrapMaxFloat} when constructed with a value above maximum");
            var wrapMinDouble = DegreeColourComponent.MAX_VAL + (ColourPrimitive)(double)ONE_BELOW_MIN;
            var wrapMaxDouble = (ColourPrimitive)(double)ONE_ABOVE_MAX - DegreeColourComponent.MAX_VAL;
            ((DegreeColourComponent)(double)ONE_BELOW_MIN).Should().Be(wrapMinDouble, $"DegreeColourComponent should wrap to {wrapMinDouble} when constructed with a value below minimum");
            ((DegreeColourComponent)(double)MIN).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal to minimum");
            ((DegreeColourComponent)(double)MAX).Should().Be(MIN, $"DegreeColourComponent should be {MIN} when constructed with a value equal maximum");
            ((DegreeColourComponent)(double)ONE_ABOVE_MAX).Should().Be(wrapMaxDouble, $"DegreeColourComponent should wrap to {wrapMaxDouble} when constructed with a value above maximum");
        } // Test_DegreeColourComponent_Implicit_cast_value_wrapping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Implicit_casting_generated()
        {
            ((ColourPrimitive)new DegreeColourComponent(ONE_ABOVE_MIN)).Should().Be((ColourPrimitive)ONE_ABOVE_MIN);
            ((ColourPrimitive)new DegreeColourComponent(MID)).Should().Be((ColourPrimitive)MID);
            ((ColourPrimitive)new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)ONE_BELOW_MAX);

        } // Test_DegreeColourComponent_Implicit_casting_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Addition_generated()
        {
            ColourPrimitive sum = (ColourPrimitive)MID + (ColourPrimitive)STEP;
            (new DegreeColourComponent(MID) + new DegreeColourComponent(STEP)).Should().Be(sum, $"should add two DegreeColourComponent's");

        } // Test_DegreeColourComponent_Addition_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Subtraction_generated()
        {
            ColourPrimitive diff = (ColourPrimitive)MID - (ColourPrimitive)STEP;
            (new DegreeColourComponent(MID) - new DegreeColourComponent(STEP)).Should().Be(diff, $"should subtract two DegreeColourComponent's");

        } // Test_DegreeColourComponent_Subtraction_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Addition_with_wrapping_generated()
        {
            (new DegreeColourComponent(MID) + new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be(MID + ONE_BELOW_MAX - MAX, $"should add two DegreeColourComponent's and wrap if the sum exceeds MaxValue");

        } // Test_DegreeColourComponent_Addition_with_wrapping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory(nameof(DegreeColourComponent))]
        public void Test_DegreeColourComponent_Subtraction_with_wrapping_generated()
        {
            (new DegreeColourComponent(MID) - new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be(MID - ONE_BELOW_MAX + MAX, $"should subtract two DegreeColourComponent's and wrap if the difference falls below MinValue");

        } // Test_DegreeColourComponent_Subtraction_with_wrapping_generated()

    } // public partial class DegreeColourComponent_Test
    #endregion DegreeColourComponent_Test

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