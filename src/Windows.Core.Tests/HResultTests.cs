﻿// Copyright (c) to owners found in https://github.com/AArnott/pinvoke/blob/master/COPYRIGHT.md. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using PInvoke;
using Xunit;

public class HResultTests
{
    [Fact]
    public void Ctor_Int32()
    {
        Assert.Equal(3, new HResult(3).AsInt32);
    }

    [Fact]
    public void Ctor_UInt32()
    {
        Assert.Equal(3, new HResult((uint)3).AsInt32);
    }

    [Fact]
    public void DefaultIs0()
    {
        Assert.Equal(0, default(HResult).AsInt32);
    }

    [Fact]
    public void PopularValuesPredefined()
    {
        Assert.Equal(0, HResult.S_OK.AsInt32);
        Assert.Equal(0x80004005, HResult.E_FAIL);
        Assert.Equal(1, HResult.S_FALSE.AsInt32);
    }

    [Fact]
    public void AsUInt32()
    {
        uint expectedValue = 5;
        HResult hr = expectedValue;
        uint actualValue = hr.AsUInt32;
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ImplicitCast_Int32()
    {
        int originalValue = 0x5;
        HResult hr = originalValue;
        int backToInt = hr;
        Assert.Equal(originalValue, backToInt);
    }

    [Fact]
    public void ImplicitCast_UInt32()
    {
        uint originalValue = 0x5;
        HResult hr = originalValue;
        Assert.Equal(originalValue, hr.AsUInt32);
    }

    [Fact]
    public void ExplicitCast_Int32()
    {
        int originalValue = 0x5;
        HResult hr = (HResult)originalValue;
        Assert.Equal(originalValue, (int)hr);
    }

    [Fact]
    public void ExplicitCast_UInt32()
    {
        uint originalValue = 0x5;
        HResult hr = (HResult)originalValue;
        Assert.Equal(originalValue, (uint)hr);
    }

    [Fact]
    public void ToString_FormatsNumberAsHex()
    {
        HResult hr = 0x80000000;
        Assert.Equal("0x80000000", hr.ToString());
    }

    [Fact]
    public void ToString_IsFormattable()
    {
        HResult hr = 0x10;
        Assert.Equal("0010", $"{hr:x4}");
        Assert.Equal("00000010", $"{hr:x8}");
    }

    [Fact]
    public void GetHashCodeReturnsValue()
    {
        Assert.Equal(3, new HResult(3).GetHashCode());
        Assert.Equal(4, new HResult(4).GetHashCode());
    }

    [Fact]
    public void EqualityChecks()
    {
        HResult hr3 = 3;
        HResult hr5 = 5;
        IEquatable<HResult> hr3Equatable = hr3;

        Assert.Equal(hr3, hr3);
        Assert.True(hr3.Equals(hr3));
        Assert.True(hr3Equatable.Equals(hr3));

        Assert.NotEqual(hr3, hr5);
        Assert.False(hr3.Equals(hr5));
        Assert.False(hr3Equatable.Equals(hr5));
    }

    [Fact]
    public void EqualOperators()
    {
        HResult hr3 = 3;
        HResult hr3b = 3;
        HResult hr5 = 5;

        Assert.True(hr3 != hr5);
        Assert.True(hr3 == hr3b);
        Assert.False(hr3 == hr5);
        Assert.False(hr3 != hr3b);
    }

    [Fact]
    public void DebuggerDisplay()
    {
        HResult hr = 0x10;
        var privateProperty = typeof(HResult).GetProperty("DebuggerDisplay", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        object value = privateProperty.GetMethod.Invoke(hr, null);
        Assert.Equal("0x00000010", value);
    }

    [Fact]
    public void Succeeded()
    {
        Assert.True(HResult.S_OK.Succeeded);
        Assert.True(HResult.S_FALSE.Succeeded);
        Assert.False(new HResult(-1).Succeeded);
    }

    [Fact]
    public void Failed()
    {
        Assert.False(HResult.S_OK.Failed);
        Assert.False(HResult.S_FALSE.Failed);
        Assert.True(new HResult(-1).Failed);
    }

    [Fact]
    public void Severity()
    {
        Assert.Equal((HResult.SeverityCodes)0x1, new HResult(0x80000000).Severity);
        Assert.Equal((HResult.SeverityCodes)0x0, new HResult(0x7fffffff).Severity);
    }

    [Fact]
    public void Facility()
    {
        Assert.Equal((HResult.FacilityCodes)0x7ff, new HResult(0x7ff0000).Facility);
        Assert.Equal((HResult.FacilityCodes)0x0, new HResult(0xf800ffff).Facility);
    }

    [Fact]
    public void Code()
    {
        Assert.Equal(0xffff, new HResult(0xffffffff).FacilityCode);
    }

    [Fact]
    public void ThrowOnFailure()
    {
        Assert.Throws<COMException>(() => HResult.E_FAIL.ThrowOnFailure());
        HResult.S_OK.ThrowOnFailure();
        HResult.S_FALSE.ThrowOnFailure();
    }
}
