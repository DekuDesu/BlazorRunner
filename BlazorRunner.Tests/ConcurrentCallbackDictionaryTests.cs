using BlazorRunner.Runner.RuntimeHandling;
using BlazorRunner.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlazorRunner.Tests
{
    public class ConcurrentCallbackDictionaryTests
    {
        [Fact]
        public void Set()
        {
            var dict = new ConcurrentCallbackDictionary<string, int>();
            dict.Add("1", 1);
            Assert.True(dict.ContainsKey("1"));
            Assert.False(dict.ContainsKey("2"));
            Assert.Single(dict);
            Assert.Equal(1, dict["1"]);
            Assert.Equal("1", dict.Keys.ToArray()[0]);
            Assert.Equal(1, dict.Values.ElementAt(0));
            dict.Add("2", 2);
            Assert.Equal(2, dict.Count);
            Assert.True(dict.TryGetValue("2", out _));
            Assert.False(dict.TryGetValue("4", out _));
            dict.UpdateValue("2", 1);
            Assert.Equal(1, dict["2"]);
            dict.Add("5", 5);
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
            Assert.True(dict.Contains(new KeyValuePair<string, int>("5", 5)));
#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection
        }
    }
}
