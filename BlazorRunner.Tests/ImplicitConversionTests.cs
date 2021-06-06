using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BlazorRunner;
using BlazorRunner.Runner;

namespace BlazorRunner.Tests
{
    public class ImplicitConversionTests
    {
        [Fact]
        public void Sbyte()
        {
            sbyte instance = 123;

            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Byte()
        {
            byte instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Short()
        {
            short instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void UShort()
        {
            ushort instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Int()
        {
            int instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));

        }

        [Fact]
        public void UInt()
        {
            uint instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Long()
        {
            long instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void ULong()
        {
            ulong instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Float()
        {
            float instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Double()
        {
            double instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Decimal()
        {
            decimal instance = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void Object()
        {
            object instance = new object();
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }

        [Fact]
        public void NullNeverImplicit()
        {
            object instance = null;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(float)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance, typeof(decimal)));
        }
    }
}
