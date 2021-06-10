using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorRunner.Runner.RuntimeHandling;
using BlazorRunner.Runner;
using Xunit;

namespace BlazorRunner.Tests
{
    public class ConcurrentCallbackQueueTests
    {
        [Fact]
        public void AddWorks()
        {
            var queue = new ConcurrentCallbackQueue<int>();

            queue.Push(1);

            if (queue.TryTake(out var item))
            {
                Assert.Equal(1, item);
            }

            queue.Add(2);
            if (queue.TryTake(out item))
            {
                Assert.Equal(2, item);
            }
        }
        [Fact]
        public void RemoveWorks()
        {
            var queue = new ConcurrentCallbackQueue<int>() {
                1,2,3,4,5,6,7,8,9
            };

            queue.Remove(7);

            int[] expected = { 1, 2, 3, 4, 5, 6, 8, 9 };

            Assert.Equal(expected, queue.ToArray());
        }
    }
}
