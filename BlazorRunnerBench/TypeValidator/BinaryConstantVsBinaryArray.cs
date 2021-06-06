using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BlazorRunner.Runner;

namespace BlazorRunnerBench
{
    public class BinaryConstantVsBinaryArray
    {
        public bool Array()
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

            byte instance1 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance1, typeof(sbyte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(byte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(short)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(int)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance1, typeof(decimal)));

            short instance2 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance2, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance2, typeof(byte)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance2, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance2, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance2, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance2, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance2, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance2, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance2, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance2, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance2, typeof(decimal)));

            ushort instance3 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance3, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance3, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance3, typeof(short)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(int)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance3, typeof(decimal)));

            int instance4 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance4, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance4, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance4, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance4, typeof(ushort)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance4, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance4, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance4, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance4, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance4, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance4, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance4, typeof(decimal)));

            uint instance5 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance5, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance5, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance5, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance5, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance5, typeof(int)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance5, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance5, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance5, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance5, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance5, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance5, typeof(decimal)));

            long instance6 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(uint)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance6, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance6, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance6, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance6, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance6, typeof(decimal)));

            ulong instance7 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance7, typeof(long)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance7, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance7, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance7, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance7, typeof(decimal)));

            float instance8 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(ulong)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance8, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance8, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance8, typeof(decimal)));

            double instance9 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(float)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance9, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance9, typeof(decimal)));

            decimal instance10 = 123;

            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(float)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance10, typeof(double)));
            Assert.True(TypeValidator.IsImplicitlyCastable(instance10, typeof(decimal)));

            object instance11 = new object();

            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(sbyte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(byte)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(short)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(ushort)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(int)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(uint)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(long)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(ulong)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(float)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(double)));
            Assert.False(TypeValidator.IsImplicitlyCastable(instance11, typeof(decimal)));

            return true;
        }

        //public bool Switch()
        //{
        //    sbyte instance = 123;

        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(byte)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(ushort)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance, typeof(decimal)));

        //    byte instance1 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(sbyte)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(byte)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(short)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(ushort)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(int)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(long)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance1, typeof(decimal)));

        //    short instance2 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(byte)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(ushort)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance2, typeof(decimal)));

        //    ushort instance3 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(short)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(ushort)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(int)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(long)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance3, typeof(decimal)));

        //    int instance4 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(ushort)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance4, typeof(decimal)));

        //    uint instance5 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(int)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(long)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance5, typeof(decimal)));

        //    long instance6 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(uint)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance6, typeof(decimal)));

        //    ulong instance7 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(uint)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(long)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance7, typeof(decimal)));

        //    float instance8 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(uint)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(ulong)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(double)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance8, typeof(decimal)));

        //    double instance9 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(uint)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(ulong)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(float)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(double)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance9, typeof(decimal)));

        //    decimal instance10 = 123;

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(uint)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(ulong)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(float)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(double)));
        //    Assert.True(TypeValidator.IsImplicitlyCastableAlt2(instance10, typeof(decimal)));

        //    object instance11 = new object();

        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(sbyte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(byte)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(short)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(ushort)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(int)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(uint)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(long)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(ulong)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(float)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(double)));
        //    Assert.False(TypeValidator.IsImplicitlyCastableAlt2(instance11, typeof(decimal)));

        //    return true;
        //}

        [Params(typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(bool))]
        public Type T { get; set; }

        [Benchmark]
        public bool Control()
        {
            int a = 12;

            try
            {
                Convert.ChangeType(a, T);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        [Benchmark]
        public bool Test()
        {
            int a = 12;

            return TypeValidator.IsImplicitlyCastable(a, T);
        }

        public static class Assert
        {
            public static bool True(bool b)
            {
                if (b is false)
                {
                    throw new Exception("Failed Test Exception");
                }

                return true;
            }

            public static bool False(bool b)
            {
                if (b)
                {
                    throw new Exception("Failed Test Exception");
                }

                return true;
            }
        }
    }
}
