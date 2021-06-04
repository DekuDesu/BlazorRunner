using System;
using BlazorRunner.Attributes;
namespace ServerTestAssembly
{
    [Script]
    [Name("Test Script")]
    [Description("My script description")]
    public class MyExampleScript : IDisposable
    {
        [Setting(Group = "Number Settings")]
        [Name("Max Count")]
        [Description("Gets; Sets; the number of times hello world should be printed")]
        public int N { get; set; }

        [Setting(Group = "Number Settings")]
        public double DummyValue = 0.0f;

        public double NotCounted { get; set; } = 2.3f;

        [Setting(Group = "Text Settings")]
        [Name("Message")]
        [Description("The message that should be displayed instead of hello world")]
        public string Text = "Hello World";

        [Setting]
        [Range(Min = "4", Max = byte.MaxValue, StepAmount = 2)]
        public byte LimitedByte = 14;

        [Setting(Group = "Number Settings")]
        public char CustomChar = '\u006A';

        [Setup]
        public void Before()
        {

        }

        [EntryPoint]
        public void Main()
        {

        }

        [Cleanup]
        public void After()
        {

        }

        [MiniScript]
        [Name("Alternate Message")]
        [Description("Displays a different message than hello world")]
        public void ExtraStuff()
        {

        }

        public void HiddenStuff()
        {
            // should not be found and displayed
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
