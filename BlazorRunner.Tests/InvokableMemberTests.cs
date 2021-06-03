using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BlazorRunner.Runner;
using Xunit;

namespace BlazorRunner.Tests
{
    public class InvokableMemberTests
    {
        public int TestField = 1;

        public double TestProperty { get; set; } = 3d;

        [Fact]
        public void InvokeWorks()
        {

            MethodInfo info = typeof(InvokableMemberTests).GetMethod(nameof(TestMethod));

            IInvokableMember member = Factory.CreateInvokableMember(info, this);

            Assert.Equal(1, member.Invoke());

            Assert.Equal(member.Id.ToString(), member.ToString());
        }

        public int TestMethod()
        {
            return 1;
        }
    }
}
