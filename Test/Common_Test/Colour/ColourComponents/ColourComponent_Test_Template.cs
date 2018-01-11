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

namespace HisRoyalRedness.com
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
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Min_and_Max_generated()
        {
            // There should be a manually written test that checks the min and max against hard-coded values
            ByteColourComponent.MinValue.Should().Be(MIN, $"the minimum value for ByteColourComponent should be {MIN}");
            ByteColourComponent.MaxValue.Should().Be(MAX, $"the maximum value for ByteColourComponent should be {MAX}");
        } // Test_ByteColourComponent_Min_and_Max_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_IsWrappingValue_generated()
        {
            ByteColourComponent.IsWrappingValue.Should().BeFalse("ByteColourComponent is not a wrapping value type");
        } // Test_ByteColourComponent_IsWrappingValue_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Construction_default_value_generated()
        {
            new ByteColourComponent().Should().Be(MIN, $"the default value for the default constructor should be {MIN}");
        } // Test_ByteColourComponent_Construction_default_value_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Construction_different_arg_types_generated()
        {
            new ByteColourComponent((int)ONE_ABOVE_MIN).Should().Be((int)ONE_ABOVE_MIN, $"ByteColourComponent should construct with a int constructor argument");
            new ByteColourComponent((int)MID).Should().Be((int)MID, $"ByteColourComponent should construct with a int constructor argument");
            new ByteColourComponent((int)ONE_BELOW_MAX).Should().Be((int)ONE_BELOW_MAX, $"ByteColourComponent should construct with a int constructor argument");
            new ByteColourComponent((float)ONE_ABOVE_MIN).Should().Be((float)ONE_ABOVE_MIN, $"ByteColourComponent should construct with a float constructor argument");
            new ByteColourComponent((float)MID).Should().Be((float)MID, $"ByteColourComponent should construct with a float constructor argument");
            new ByteColourComponent((float)ONE_BELOW_MAX).Should().Be((float)ONE_BELOW_MAX, $"ByteColourComponent should construct with a float constructor argument");
            new ByteColourComponent((double)ONE_ABOVE_MIN).Should().Be((double)ONE_ABOVE_MIN, $"ByteColourComponent should construct with a double constructor argument");
            new ByteColourComponent((double)MID).Should().Be((double)MID, $"ByteColourComponent should construct with a double constructor argument");
            new ByteColourComponent((double)ONE_BELOW_MAX).Should().Be((double)ONE_BELOW_MAX, $"ByteColourComponent should construct with a double constructor argument");
        } // Test_ByteColourComponent_Construction_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Implicit_cast_different_arg_types_generated()
        {
            ((ByteColourComponent)(int)ONE_ABOVE_MIN).Should().Be((int)ONE_ABOVE_MIN, $"ByteColourComponent should construct from a cast from int");
            ((ByteColourComponent)(int)MID).Should().Be((int)MID, $"ByteColourComponent should construct from a cast from int");
            ((ByteColourComponent)(int)ONE_BELOW_MAX).Should().Be((int)ONE_BELOW_MAX, $"ByteColourComponent should construct from a cast from int");
            ((ByteColourComponent)(float)ONE_ABOVE_MIN).Should().Be((float)ONE_ABOVE_MIN, $"ByteColourComponent should construct from a cast from float");
            ((ByteColourComponent)(float)MID).Should().Be((float)MID, $"ByteColourComponent should construct from a cast from float");
            ((ByteColourComponent)(float)ONE_BELOW_MAX).Should().Be((float)ONE_BELOW_MAX, $"ByteColourComponent should construct from a cast from float");
            ((ByteColourComponent)(double)ONE_ABOVE_MIN).Should().Be((double)ONE_ABOVE_MIN, $"ByteColourComponent should construct from a cast from double");
            ((ByteColourComponent)(double)MID).Should().Be((double)MID, $"ByteColourComponent should construct from a cast from double");
            ((ByteColourComponent)(double)ONE_BELOW_MAX).Should().Be((double)ONE_BELOW_MAX, $"ByteColourComponent should construct from a cast from double");
        } // Test_ByteColourComponent_Implicit_cast_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
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
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Implicit_casting_generated()
        {
            ((byte)new ByteColourComponent(ONE_ABOVE_MIN)).Should().Be((byte)ONE_ABOVE_MIN);
            ((byte)new ByteColourComponent(MID)).Should().Be((byte)MID);
            ((byte)new ByteColourComponent(ONE_BELOW_MAX)).Should().Be((byte)ONE_BELOW_MAX);

        } // Test_ByteColourComponent_Implicit_casting_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Addition_generated()
        {
            byte sum = (byte)MID + (byte)STEP;
            (new ByteColourComponent(MID) + new ByteColourComponent(STEP)).Should().Be(sum, $"should add two ByteColourComponent's");

            (new ByteColourComponent(MID) + (int)STEP).Should().Be((byte)((byte)MID + (int)STEP), $"should add a ByteColourComponent to a int");
            ((int)MID + new ByteColourComponent(STEP)).Should().Be((byte)((int)MID + (byte)STEP), $"should add a int to a ByteColourComponent");
            (new ByteColourComponent(MID) + (float)STEP).Should().Be((byte)((byte)MID + (float)STEP), $"should add a ByteColourComponent to a float");
            ((float)MID + new ByteColourComponent(STEP)).Should().Be((byte)((float)MID + (byte)STEP), $"should add a float to a ByteColourComponent");
            (new ByteColourComponent(MID) + (double)STEP).Should().Be((byte)((byte)MID + (double)STEP), $"should add a ByteColourComponent to a double");
            ((double)MID + new ByteColourComponent(STEP)).Should().Be((byte)((double)MID + (byte)STEP), $"should add a double to a ByteColourComponent");

        } // Test_ByteColourComponent_Addition_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Subtraction_generated()
        {
            byte diff = (byte)MID - (byte)STEP;
            (new ByteColourComponent(MID) - new ByteColourComponent(STEP)).Should().Be(diff, $"should subtract two ByteColourComponent's");

            (new ByteColourComponent(MID) - (int)STEP).Should().Be((byte)((byte)MID - (int)STEP), $"should subtract a ByteColourComponent from a int");
            ((int)MID - new ByteColourComponent(STEP)).Should().Be((byte)((int)MID - (byte)STEP), $"should subtract a int from a ByteColourComponent");
            (new ByteColourComponent(MID) - (float)STEP).Should().Be((byte)((byte)MID - (float)STEP), $"should subtract a ByteColourComponent from a float");
            ((float)MID - new ByteColourComponent(STEP)).Should().Be((byte)((float)MID - (byte)STEP), $"should subtract a float from a ByteColourComponent");
            (new ByteColourComponent(MID) - (double)STEP).Should().Be((byte)((byte)MID - (double)STEP), $"should subtract a ByteColourComponent from a double");
            ((double)MID - new ByteColourComponent(STEP)).Should().Be((byte)((double)MID - (byte)STEP), $"should subtract a double from a ByteColourComponent");

        } // Test_ByteColourComponent_Subtraction_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Addition_with_clipping_generated()
        {
            (new ByteColourComponent(MID) + new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add two ByteColourComponent's and clip to MAX if the sum exceeds MaxValue");

            (new ByteColourComponent(MID) + (int)ONE_BELOW_MAX).Should().Be(MAX, $"should add a ByteColourComponent to a int and clip to MAX if the sum exceeds MaxValue");
            ((int)MID + new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add a int to a ByteColourComponent and clip to MAX if the sum exceeds MaxValue");
            (new ByteColourComponent(MID) + (float)ONE_BELOW_MAX).Should().Be(MAX, $"should add a ByteColourComponent to a float and clip to MAX if the sum exceeds MaxValue");
            ((float)MID + new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add a float to a ByteColourComponent and clip to MAX if the sum exceeds MaxValue");
            (new ByteColourComponent(MID) + (double)ONE_BELOW_MAX).Should().Be(MAX, $"should add a ByteColourComponent to a double and clip to MAX if the sum exceeds MaxValue");
            ((double)MID + new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add a double to a ByteColourComponent and clip to MAX if the sum exceeds MaxValue");

        } // Test_ByteColourComponent_Addition_with_clipping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Subtraction_with_clipping_generated()
        {
            (new ByteColourComponent(MID) - new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract two ByteColourComponent's and clip to MIN if the difference falls below MinValue");

            (new ByteColourComponent(MID) - (int)ONE_BELOW_MAX).Should().Be(MIN, $"should subtract a ByteColourComponent from a int and clip to MIN if the difference falls below MinValue");
            ((int)MID - new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract a int from a ByteColourComponent and clip to MIN if the difference falls below MinValue");
            (new ByteColourComponent(MID) - (float)ONE_BELOW_MAX).Should().Be(MIN, $"should subtract a ByteColourComponent from a float and clip to MIN if the difference falls below MinValue");
            ((float)MID - new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract a float from a ByteColourComponent and clip to MIN if the difference falls below MinValue");
            (new ByteColourComponent(MID) - (double)ONE_BELOW_MAX).Should().Be(MIN, $"should subtract a ByteColourComponent from a double and clip to MIN if the difference falls below MinValue");
            ((double)MID - new ByteColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract a double from a ByteColourComponent and clip to MIN if the difference falls below MinValue");

        } // Test_ByteColourComponent_Subtraction_with_clipping_generated()


        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Equality_generated()
        {
            new ByteColourComponent(MID).Equals(new ByteColourComponent(MID)).Should().BeTrue();
            new ByteColourComponent(MID).Equals(new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeFalse();

            (new ByteColourComponent(MID) == new ByteColourComponent(MID)).Should().BeTrue();
            (new ByteColourComponent(MID) == MID).Should().BeTrue();
            (new ByteColourComponent(MID) == new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeFalse();
            (new ByteColourComponent(MID) == ONE_ABOVE_MIN).Should().BeFalse();

            (new ByteColourComponent(MID) != new ByteColourComponent(MID)).Should().BeFalse();
            (new ByteColourComponent(MID) != MID).Should().BeFalse();
            (new ByteColourComponent(MID) != new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeTrue();
            (new ByteColourComponent(MID) != ONE_ABOVE_MIN).Should().BeTrue();

        } // Test_ByteColourComponent_Equality_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_Comparison_generated()
        {
            ByteColourComponent.Compare(new ByteColourComponent(ONE_ABOVE_MIN), new ByteColourComponent(MID)).Should().Be(-1, "the first is less than the second");
            ByteColourComponent.Compare(new ByteColourComponent(MID), new ByteColourComponent(MID)).Should().Be(0, "the first is the same as the second");
            ByteColourComponent.Compare(new ByteColourComponent(MID), new ByteColourComponent(ONE_ABOVE_MIN)).Should().Be(1, "the first is greater than the second");

            new ByteColourComponent(ONE_ABOVE_MIN).CompareTo(new ByteColourComponent(MID)).Should().Be(-1, "the first is less than the second");
            new ByteColourComponent(MID).CompareTo(new ByteColourComponent(MID)).Should().Be(0, "the first is the same as the second");
            new ByteColourComponent(MID).CompareTo(new ByteColourComponent(ONE_ABOVE_MIN)).Should().Be(1, "the first is greater than the second");

            (new ByteColourComponent(ONE_ABOVE_MIN) < new ByteColourComponent(MID)).Should().BeTrue("the first is less than the second");
            (new ByteColourComponent(ONE_ABOVE_MIN) > new ByteColourComponent(MID)).Should().BeFalse("the first is less than the second");
            (new ByteColourComponent(MID) < new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeFalse("the first is greater than the second");
            (new ByteColourComponent(MID) > new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeTrue("the first is greater than the second");
            (new ByteColourComponent(MID) < new ByteColourComponent(MID)).Should().BeFalse("the first is equal to the second");
            (new ByteColourComponent(MID) > new ByteColourComponent(MID)).Should().BeFalse("the first is equal to the second");

            (new ByteColourComponent(ONE_ABOVE_MIN) <= new ByteColourComponent(MID)).Should().BeTrue("the first is less than the second");
            (new ByteColourComponent(ONE_ABOVE_MIN) >= new ByteColourComponent(MID)).Should().BeFalse("the first is less than the second");
            (new ByteColourComponent(MID) <= new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeFalse("the first is greater than the second");
            (new ByteColourComponent(MID) >= new ByteColourComponent(ONE_ABOVE_MIN)).Should().BeTrue("the first is greater than the second");
            (new ByteColourComponent(MID) <= new ByteColourComponent(MID)).Should().BeTrue("the first is equal to the second");
            (new ByteColourComponent(MID) >= new ByteColourComponent(MID)).Should().BeTrue("the first is equal to the second");


        } // Test_ByteColourComponent_Comparison_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("ByteColourComponent")]
        public void Test_ByteColourComponent_GetHashCode_generated()
        {
            new ByteColourComponent(MIN).GetHashCode().Should().Be(MIN.GetHashCode());
            new ByteColourComponent(ONE_ABOVE_MIN).GetHashCode().Should().Be(ONE_ABOVE_MIN.GetHashCode());
            new ByteColourComponent(MID).GetHashCode().Should().Be(MID.GetHashCode());
            new ByteColourComponent(ONE_BELOW_MAX).GetHashCode().Should().Be(ONE_BELOW_MAX.GetHashCode());
            new ByteColourComponent(MAX).GetHashCode().Should().Be(MAX.GetHashCode());

        } // Test_ByteColourComponent_GetHashCode_generated()

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
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Min_and_Max_generated()
        {
            // There should be a manually written test that checks the min and max against hard-coded values
            UnitColourComponent.MinValue.Should().Be(MIN, $"the minimum value for UnitColourComponent should be {MIN}");
            UnitColourComponent.MaxValue.Should().Be(MAX, $"the maximum value for UnitColourComponent should be {MAX}");
        } // Test_UnitColourComponent_Min_and_Max_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_IsWrappingValue_generated()
        {
            UnitColourComponent.IsWrappingValue.Should().BeFalse("UnitColourComponent is not a wrapping value type");
        } // Test_UnitColourComponent_IsWrappingValue_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Construction_default_value_generated()
        {
            new UnitColourComponent().Should().Be(MIN, $"the default value for the default constructor should be {MIN}");
        } // Test_UnitColourComponent_Construction_default_value_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Construction_different_arg_types_generated()
        {
            new UnitColourComponent((float)ONE_ABOVE_MIN).Should().Be((float)ONE_ABOVE_MIN, $"UnitColourComponent should construct with a float constructor argument");
            new UnitColourComponent((float)MID).Should().Be((float)MID, $"UnitColourComponent should construct with a float constructor argument");
            new UnitColourComponent((float)ONE_BELOW_MAX).Should().Be((float)ONE_BELOW_MAX, $"UnitColourComponent should construct with a float constructor argument");
            new UnitColourComponent((double)ONE_ABOVE_MIN).Should().Be((double)ONE_ABOVE_MIN, $"UnitColourComponent should construct with a double constructor argument");
            new UnitColourComponent((double)MID).Should().Be((double)MID, $"UnitColourComponent should construct with a double constructor argument");
            new UnitColourComponent((double)ONE_BELOW_MAX).Should().Be((double)ONE_BELOW_MAX, $"UnitColourComponent should construct with a double constructor argument");
        } // Test_UnitColourComponent_Construction_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Implicit_cast_different_arg_types_generated()
        {
            ((UnitColourComponent)(float)ONE_ABOVE_MIN).Should().Be((float)ONE_ABOVE_MIN, $"UnitColourComponent should construct from a cast from float");
            ((UnitColourComponent)(float)MID).Should().Be((float)MID, $"UnitColourComponent should construct from a cast from float");
            ((UnitColourComponent)(float)ONE_BELOW_MAX).Should().Be((float)ONE_BELOW_MAX, $"UnitColourComponent should construct from a cast from float");
            ((UnitColourComponent)(double)ONE_ABOVE_MIN).Should().Be((double)ONE_ABOVE_MIN, $"UnitColourComponent should construct from a cast from double");
            ((UnitColourComponent)(double)MID).Should().Be((double)MID, $"UnitColourComponent should construct from a cast from double");
            ((UnitColourComponent)(double)ONE_BELOW_MAX).Should().Be((double)ONE_BELOW_MAX, $"UnitColourComponent should construct from a cast from double");
        } // Test_UnitColourComponent_Implicit_cast_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
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
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Implicit_casting_generated()
        {
            ((ColourPrimitive)new UnitColourComponent(ONE_ABOVE_MIN)).Should().Be((ColourPrimitive)ONE_ABOVE_MIN);
            ((ColourPrimitive)new UnitColourComponent(MID)).Should().Be((ColourPrimitive)MID);
            ((ColourPrimitive)new UnitColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)ONE_BELOW_MAX);

        } // Test_UnitColourComponent_Implicit_casting_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Addition_generated()
        {
            ColourPrimitive sum = (ColourPrimitive)MID + (ColourPrimitive)STEP;
            (new UnitColourComponent(MID) + new UnitColourComponent(STEP)).Should().Be(sum, $"should add two UnitColourComponent's");

            (new UnitColourComponent(MID) + (float)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID + (float)STEP), $"should add a UnitColourComponent to a float");
            ((float)MID + new UnitColourComponent(STEP)).Should().Be((ColourPrimitive)((float)MID + (ColourPrimitive)STEP), $"should add a float to a UnitColourComponent");
            (new UnitColourComponent(MID) + (double)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID + (double)STEP), $"should add a UnitColourComponent to a double");
            ((double)MID + new UnitColourComponent(STEP)).Should().Be((ColourPrimitive)((double)MID + (ColourPrimitive)STEP), $"should add a double to a UnitColourComponent");

        } // Test_UnitColourComponent_Addition_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Subtraction_generated()
        {
            ColourPrimitive diff = (ColourPrimitive)MID - (ColourPrimitive)STEP;
            (new UnitColourComponent(MID) - new UnitColourComponent(STEP)).Should().Be(diff, $"should subtract two UnitColourComponent's");

            (new UnitColourComponent(MID) - (float)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID - (float)STEP), $"should subtract a UnitColourComponent from a float");
            ((float)MID - new UnitColourComponent(STEP)).Should().Be((ColourPrimitive)((float)MID - (ColourPrimitive)STEP), $"should subtract a float from a UnitColourComponent");
            (new UnitColourComponent(MID) - (double)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID - (double)STEP), $"should subtract a UnitColourComponent from a double");
            ((double)MID - new UnitColourComponent(STEP)).Should().Be((ColourPrimitive)((double)MID - (ColourPrimitive)STEP), $"should subtract a double from a UnitColourComponent");

        } // Test_UnitColourComponent_Subtraction_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Addition_with_clipping_generated()
        {
            (new UnitColourComponent(MID) + new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add two UnitColourComponent's and clip to MAX if the sum exceeds MaxValue");

            (new UnitColourComponent(MID) + (float)ONE_BELOW_MAX).Should().Be(MAX, $"should add a UnitColourComponent to a float and clip to MAX if the sum exceeds MaxValue");
            ((float)MID + new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add a float to a UnitColourComponent and clip to MAX if the sum exceeds MaxValue");
            (new UnitColourComponent(MID) + (double)ONE_BELOW_MAX).Should().Be(MAX, $"should add a UnitColourComponent to a double and clip to MAX if the sum exceeds MaxValue");
            ((double)MID + new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MAX, $"should add a double to a UnitColourComponent and clip to MAX if the sum exceeds MaxValue");

        } // Test_UnitColourComponent_Addition_with_clipping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Subtraction_with_clipping_generated()
        {
            (new UnitColourComponent(MID) - new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract two UnitColourComponent's and clip to MIN if the difference falls below MinValue");

            (new UnitColourComponent(MID) - (float)ONE_BELOW_MAX).Should().Be(MIN, $"should subtract a UnitColourComponent from a float and clip to MIN if the difference falls below MinValue");
            ((float)MID - new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract a float from a UnitColourComponent and clip to MIN if the difference falls below MinValue");
            (new UnitColourComponent(MID) - (double)ONE_BELOW_MAX).Should().Be(MIN, $"should subtract a UnitColourComponent from a double and clip to MIN if the difference falls below MinValue");
            ((double)MID - new UnitColourComponent(ONE_BELOW_MAX)).Should().Be(MIN, $"should subtract a double from a UnitColourComponent and clip to MIN if the difference falls below MinValue");

        } // Test_UnitColourComponent_Subtraction_with_clipping_generated()


        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Equality_generated()
        {
            new UnitColourComponent(MID).Equals(new UnitColourComponent(MID)).Should().BeTrue();
            new UnitColourComponent(MID).Equals(new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeFalse();

            (new UnitColourComponent(MID) == new UnitColourComponent(MID)).Should().BeTrue();
            (new UnitColourComponent(MID) == MID).Should().BeTrue();
            (new UnitColourComponent(MID) == new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeFalse();
            (new UnitColourComponent(MID) == ONE_ABOVE_MIN).Should().BeFalse();

            (new UnitColourComponent(MID) != new UnitColourComponent(MID)).Should().BeFalse();
            (new UnitColourComponent(MID) != MID).Should().BeFalse();
            (new UnitColourComponent(MID) != new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeTrue();
            (new UnitColourComponent(MID) != ONE_ABOVE_MIN).Should().BeTrue();

        } // Test_UnitColourComponent_Equality_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_Comparison_generated()
        {
            UnitColourComponent.Compare(new UnitColourComponent(ONE_ABOVE_MIN), new UnitColourComponent(MID)).Should().Be(-1, "the first is less than the second");
            UnitColourComponent.Compare(new UnitColourComponent(MID), new UnitColourComponent(MID)).Should().Be(0, "the first is the same as the second");
            UnitColourComponent.Compare(new UnitColourComponent(MID), new UnitColourComponent(ONE_ABOVE_MIN)).Should().Be(1, "the first is greater than the second");

            new UnitColourComponent(ONE_ABOVE_MIN).CompareTo(new UnitColourComponent(MID)).Should().Be(-1, "the first is less than the second");
            new UnitColourComponent(MID).CompareTo(new UnitColourComponent(MID)).Should().Be(0, "the first is the same as the second");
            new UnitColourComponent(MID).CompareTo(new UnitColourComponent(ONE_ABOVE_MIN)).Should().Be(1, "the first is greater than the second");

            (new UnitColourComponent(ONE_ABOVE_MIN) < new UnitColourComponent(MID)).Should().BeTrue("the first is less than the second");
            (new UnitColourComponent(ONE_ABOVE_MIN) > new UnitColourComponent(MID)).Should().BeFalse("the first is less than the second");
            (new UnitColourComponent(MID) < new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeFalse("the first is greater than the second");
            (new UnitColourComponent(MID) > new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeTrue("the first is greater than the second");
            (new UnitColourComponent(MID) < new UnitColourComponent(MID)).Should().BeFalse("the first is equal to the second");
            (new UnitColourComponent(MID) > new UnitColourComponent(MID)).Should().BeFalse("the first is equal to the second");

            (new UnitColourComponent(ONE_ABOVE_MIN) <= new UnitColourComponent(MID)).Should().BeTrue("the first is less than the second");
            (new UnitColourComponent(ONE_ABOVE_MIN) >= new UnitColourComponent(MID)).Should().BeFalse("the first is less than the second");
            (new UnitColourComponent(MID) <= new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeFalse("the first is greater than the second");
            (new UnitColourComponent(MID) >= new UnitColourComponent(ONE_ABOVE_MIN)).Should().BeTrue("the first is greater than the second");
            (new UnitColourComponent(MID) <= new UnitColourComponent(MID)).Should().BeTrue("the first is equal to the second");
            (new UnitColourComponent(MID) >= new UnitColourComponent(MID)).Should().BeTrue("the first is equal to the second");


        } // Test_UnitColourComponent_Comparison_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("UnitColourComponent")]
        public void Test_UnitColourComponent_GetHashCode_generated()
        {
            new UnitColourComponent(MIN).GetHashCode().Should().Be(MIN.GetHashCode());
            new UnitColourComponent(ONE_ABOVE_MIN).GetHashCode().Should().Be(ONE_ABOVE_MIN.GetHashCode());
            new UnitColourComponent(MID).GetHashCode().Should().Be(MID.GetHashCode());
            new UnitColourComponent(ONE_BELOW_MAX).GetHashCode().Should().Be(ONE_BELOW_MAX.GetHashCode());
            new UnitColourComponent(MAX).GetHashCode().Should().Be(MAX.GetHashCode());

        } // Test_UnitColourComponent_GetHashCode_generated()

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
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Min_and_Max_generated()
        {
            // There should be a manually written test that checks the min and max against hard-coded values
            DegreeColourComponent.MinValue.Should().Be(MIN, $"the minimum value for DegreeColourComponent should be {MIN}");
            DegreeColourComponent.MaxValue.Should().Be(MAX, $"the maximum value for DegreeColourComponent should be {MAX}");
        } // Test_DegreeColourComponent_Min_and_Max_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_IsWrappingValue_generated()
        {
            DegreeColourComponent.IsWrappingValue.Should().BeTrue("DegreeColourComponent is a wrapping value type");
        } // Test_DegreeColourComponent_IsWrappingValue_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Construction_default_value_generated()
        {
            new DegreeColourComponent().Should().Be(MIN, $"the default value for the default constructor should be {MIN}");
        } // Test_DegreeColourComponent_Construction_default_value_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Construction_different_arg_types_generated()
        {
            new DegreeColourComponent((int)ONE_ABOVE_MIN).Should().Be((int)ONE_ABOVE_MIN, $"DegreeColourComponent should construct with a int constructor argument");
            new DegreeColourComponent((int)MID).Should().Be((int)MID, $"DegreeColourComponent should construct with a int constructor argument");
            new DegreeColourComponent((int)ONE_BELOW_MAX).Should().Be((int)ONE_BELOW_MAX, $"DegreeColourComponent should construct with a int constructor argument");
            new DegreeColourComponent((float)ONE_ABOVE_MIN).Should().Be((float)ONE_ABOVE_MIN, $"DegreeColourComponent should construct with a float constructor argument");
            new DegreeColourComponent((float)MID).Should().Be((float)MID, $"DegreeColourComponent should construct with a float constructor argument");
            new DegreeColourComponent((float)ONE_BELOW_MAX).Should().Be((float)ONE_BELOW_MAX, $"DegreeColourComponent should construct with a float constructor argument");
            new DegreeColourComponent((double)ONE_ABOVE_MIN).Should().Be((double)ONE_ABOVE_MIN, $"DegreeColourComponent should construct with a double constructor argument");
            new DegreeColourComponent((double)MID).Should().Be((double)MID, $"DegreeColourComponent should construct with a double constructor argument");
            new DegreeColourComponent((double)ONE_BELOW_MAX).Should().Be((double)ONE_BELOW_MAX, $"DegreeColourComponent should construct with a double constructor argument");
        } // Test_DegreeColourComponent_Construction_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Implicit_cast_different_arg_types_generated()
        {
            ((DegreeColourComponent)(int)ONE_ABOVE_MIN).Should().Be((int)ONE_ABOVE_MIN, $"DegreeColourComponent should construct from a cast from int");
            ((DegreeColourComponent)(int)MID).Should().Be((int)MID, $"DegreeColourComponent should construct from a cast from int");
            ((DegreeColourComponent)(int)ONE_BELOW_MAX).Should().Be((int)ONE_BELOW_MAX, $"DegreeColourComponent should construct from a cast from int");
            ((DegreeColourComponent)(float)ONE_ABOVE_MIN).Should().Be((float)ONE_ABOVE_MIN, $"DegreeColourComponent should construct from a cast from float");
            ((DegreeColourComponent)(float)MID).Should().Be((float)MID, $"DegreeColourComponent should construct from a cast from float");
            ((DegreeColourComponent)(float)ONE_BELOW_MAX).Should().Be((float)ONE_BELOW_MAX, $"DegreeColourComponent should construct from a cast from float");
            ((DegreeColourComponent)(double)ONE_ABOVE_MIN).Should().Be((double)ONE_ABOVE_MIN, $"DegreeColourComponent should construct from a cast from double");
            ((DegreeColourComponent)(double)MID).Should().Be((double)MID, $"DegreeColourComponent should construct from a cast from double");
            ((DegreeColourComponent)(double)ONE_BELOW_MAX).Should().Be((double)ONE_BELOW_MAX, $"DegreeColourComponent should construct from a cast from double");
        } // Test_DegreeColourComponent_Implicit_cast_different_arg_types_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
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
        [TestCategory("DegreeColourComponent")]
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
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Implicit_casting_generated()
        {
            ((ColourPrimitive)new DegreeColourComponent(ONE_ABOVE_MIN)).Should().Be((ColourPrimitive)ONE_ABOVE_MIN);
            ((ColourPrimitive)new DegreeColourComponent(MID)).Should().Be((ColourPrimitive)MID);
            ((ColourPrimitive)new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)ONE_BELOW_MAX);

        } // Test_DegreeColourComponent_Implicit_casting_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Addition_generated()
        {
            ColourPrimitive sum = (ColourPrimitive)MID + (ColourPrimitive)STEP;
            (new DegreeColourComponent(MID) + new DegreeColourComponent(STEP)).Should().Be(sum, $"should add two DegreeColourComponent's");

            (new DegreeColourComponent(MID) + (int)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID + (int)STEP), $"should add a DegreeColourComponent to a int");
            ((int)MID + new DegreeColourComponent(STEP)).Should().Be((ColourPrimitive)((int)MID + (ColourPrimitive)STEP), $"should add a int to a DegreeColourComponent");
            (new DegreeColourComponent(MID) + (float)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID + (float)STEP), $"should add a DegreeColourComponent to a float");
            ((float)MID + new DegreeColourComponent(STEP)).Should().Be((ColourPrimitive)((float)MID + (ColourPrimitive)STEP), $"should add a float to a DegreeColourComponent");
            (new DegreeColourComponent(MID) + (double)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID + (double)STEP), $"should add a DegreeColourComponent to a double");
            ((double)MID + new DegreeColourComponent(STEP)).Should().Be((ColourPrimitive)((double)MID + (ColourPrimitive)STEP), $"should add a double to a DegreeColourComponent");

        } // Test_DegreeColourComponent_Addition_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Subtraction_generated()
        {
            ColourPrimitive diff = (ColourPrimitive)MID - (ColourPrimitive)STEP;
            (new DegreeColourComponent(MID) - new DegreeColourComponent(STEP)).Should().Be(diff, $"should subtract two DegreeColourComponent's");

            (new DegreeColourComponent(MID) - (int)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID - (int)STEP), $"should subtract a DegreeColourComponent from a int");
            ((int)MID - new DegreeColourComponent(STEP)).Should().Be((ColourPrimitive)((int)MID - (ColourPrimitive)STEP), $"should subtract a int from a DegreeColourComponent");
            (new DegreeColourComponent(MID) - (float)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID - (float)STEP), $"should subtract a DegreeColourComponent from a float");
            ((float)MID - new DegreeColourComponent(STEP)).Should().Be((ColourPrimitive)((float)MID - (ColourPrimitive)STEP), $"should subtract a float from a DegreeColourComponent");
            (new DegreeColourComponent(MID) - (double)STEP).Should().Be((ColourPrimitive)((ColourPrimitive)MID - (double)STEP), $"should subtract a DegreeColourComponent from a double");
            ((double)MID - new DegreeColourComponent(STEP)).Should().Be((ColourPrimitive)((double)MID - (ColourPrimitive)STEP), $"should subtract a double from a DegreeColourComponent");

        } // Test_DegreeColourComponent_Subtraction_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Addition_with_wrapping_generated()
        {
            (new DegreeColourComponent(MID) + new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be(MID + ONE_BELOW_MAX - MAX, $"should add two DegreeColourComponent's and wrap if the sum exceeds MaxValue");

            (new DegreeColourComponent(MID) + (int)ONE_BELOW_MAX).Should().Be(MID + (ColourPrimitive)(int)ONE_BELOW_MAX - MAX, $"should add a DegreeColourComponent to a int and wrap if the sum exceeds MaxValue");
            ((int)MID + new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)(int)MID + ONE_BELOW_MAX - MAX, $"should add a int to a DegreeColourComponent and wrap if the sum exceeds MaxValue");
            (new DegreeColourComponent(MID) + (float)ONE_BELOW_MAX).Should().Be(MID + (ColourPrimitive)(float)ONE_BELOW_MAX - MAX, $"should add a DegreeColourComponent to a float and wrap if the sum exceeds MaxValue");
            ((float)MID + new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)(float)MID + ONE_BELOW_MAX - MAX, $"should add a float to a DegreeColourComponent and wrap if the sum exceeds MaxValue");
            (new DegreeColourComponent(MID) + (double)ONE_BELOW_MAX).Should().Be(MID + (ColourPrimitive)(double)ONE_BELOW_MAX - MAX, $"should add a DegreeColourComponent to a double and wrap if the sum exceeds MaxValue");
            ((double)MID + new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((ColourPrimitive)(double)MID + ONE_BELOW_MAX - MAX, $"should add a double to a DegreeColourComponent and wrap if the sum exceeds MaxValue");

        } // Test_DegreeColourComponent_Addition_with_wrapping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Subtraction_with_wrapping_generated()
        {
            (new DegreeColourComponent(MID) - new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be(MID - ONE_BELOW_MAX + MAX, $"should subtract two DegreeColourComponent's and wrap if the difference falls below MinValue");

            (new DegreeColourComponent(MID) - (int)ONE_BELOW_MAX).Should().Be(MID - (ColourPrimitive)(int)ONE_BELOW_MAX + MAX, $"should subtract a DegreeColourComponent from a int and wrap if the difference falls below MinValue");
            ((int)MID - new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((int)MID - ONE_BELOW_MAX + MAX, $"should subtract a int from a DegreeColourComponent and wrap if the difference falls below MinValue");
            (new DegreeColourComponent(MID) - (float)ONE_BELOW_MAX).Should().Be(MID - (ColourPrimitive)(float)ONE_BELOW_MAX + MAX, $"should subtract a DegreeColourComponent from a float and wrap if the difference falls below MinValue");
            ((float)MID - new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((float)MID - ONE_BELOW_MAX + MAX, $"should subtract a float from a DegreeColourComponent and wrap if the difference falls below MinValue");
            (new DegreeColourComponent(MID) - (double)ONE_BELOW_MAX).Should().Be(MID - (ColourPrimitive)(double)ONE_BELOW_MAX + MAX, $"should subtract a DegreeColourComponent from a double and wrap if the difference falls below MinValue");
            ((double)MID - new DegreeColourComponent(ONE_BELOW_MAX)).Should().Be((double)MID - ONE_BELOW_MAX + MAX, $"should subtract a double from a DegreeColourComponent and wrap if the difference falls below MinValue");

        } // Test_DegreeColourComponent_Subtraction_with_wrapping_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Equality_generated()
        {
            new DegreeColourComponent(MID).Equals(new DegreeColourComponent(MID)).Should().BeTrue();
            new DegreeColourComponent(MID).Equals(new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeFalse();

            (new DegreeColourComponent(MID) == new DegreeColourComponent(MID)).Should().BeTrue();
            (new DegreeColourComponent(MID) == MID).Should().BeTrue();
            (new DegreeColourComponent(MID) == new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeFalse();
            (new DegreeColourComponent(MID) == ONE_ABOVE_MIN).Should().BeFalse();

            (new DegreeColourComponent(MID) != new DegreeColourComponent(MID)).Should().BeFalse();
            (new DegreeColourComponent(MID) != MID).Should().BeFalse();
            (new DegreeColourComponent(MID) != new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeTrue();
            (new DegreeColourComponent(MID) != ONE_ABOVE_MIN).Should().BeTrue();

        } // Test_DegreeColourComponent_Equality_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_Comparison_generated()
        {
            DegreeColourComponent.Compare(new DegreeColourComponent(ONE_ABOVE_MIN), new DegreeColourComponent(MID)).Should().Be(-1, "the first is less than the second");
            DegreeColourComponent.Compare(new DegreeColourComponent(MID), new DegreeColourComponent(MID)).Should().Be(0, "the first is the same as the second");
            DegreeColourComponent.Compare(new DegreeColourComponent(MID), new DegreeColourComponent(ONE_ABOVE_MIN)).Should().Be(1, "the first is greater than the second");

            new DegreeColourComponent(ONE_ABOVE_MIN).CompareTo(new DegreeColourComponent(MID)).Should().Be(-1, "the first is less than the second");
            new DegreeColourComponent(MID).CompareTo(new DegreeColourComponent(MID)).Should().Be(0, "the first is the same as the second");
            new DegreeColourComponent(MID).CompareTo(new DegreeColourComponent(ONE_ABOVE_MIN)).Should().Be(1, "the first is greater than the second");

            (new DegreeColourComponent(ONE_ABOVE_MIN) < new DegreeColourComponent(MID)).Should().BeTrue("the first is less than the second");
            (new DegreeColourComponent(ONE_ABOVE_MIN) > new DegreeColourComponent(MID)).Should().BeFalse("the first is less than the second");
            (new DegreeColourComponent(MID) < new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeFalse("the first is greater than the second");
            (new DegreeColourComponent(MID) > new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeTrue("the first is greater than the second");
            (new DegreeColourComponent(MID) < new DegreeColourComponent(MID)).Should().BeFalse("the first is equal to the second");
            (new DegreeColourComponent(MID) > new DegreeColourComponent(MID)).Should().BeFalse("the first is equal to the second");

            (new DegreeColourComponent(ONE_ABOVE_MIN) <= new DegreeColourComponent(MID)).Should().BeTrue("the first is less than the second");
            (new DegreeColourComponent(ONE_ABOVE_MIN) >= new DegreeColourComponent(MID)).Should().BeFalse("the first is less than the second");
            (new DegreeColourComponent(MID) <= new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeFalse("the first is greater than the second");
            (new DegreeColourComponent(MID) >= new DegreeColourComponent(ONE_ABOVE_MIN)).Should().BeTrue("the first is greater than the second");
            (new DegreeColourComponent(MID) <= new DegreeColourComponent(MID)).Should().BeTrue("the first is equal to the second");
            (new DegreeColourComponent(MID) >= new DegreeColourComponent(MID)).Should().BeTrue("the first is equal to the second");


        } // Test_DegreeColourComponent_Comparison_generated()

        [TestMethod]
        [TestCategory("Template generated")]
        [TestCategory("DegreeColourComponent")]
        public void Test_DegreeColourComponent_GetHashCode_generated()
        {
            new DegreeColourComponent(MIN).GetHashCode().Should().Be(MIN.GetHashCode());
            new DegreeColourComponent(ONE_ABOVE_MIN).GetHashCode().Should().Be(ONE_ABOVE_MIN.GetHashCode());
            new DegreeColourComponent(MID).GetHashCode().Should().Be(MID.GetHashCode());
            new DegreeColourComponent(ONE_BELOW_MAX).GetHashCode().Should().Be(ONE_BELOW_MAX.GetHashCode());
            new DegreeColourComponent(MAX).GetHashCode().Should().Be(MIN.GetHashCode());

        } // Test_DegreeColourComponent_GetHashCode_generated()

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