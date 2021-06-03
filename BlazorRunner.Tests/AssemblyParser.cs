using System;
using Xunit;
using BlazorRunner.Runner;
using System.Reflection;
using BlazorRunner.Attributes;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApplication;

namespace BlazorRunner.Tests
{
    public class AssemblyBuilderTests
    {
        private readonly AssemblyBuilder Parser = new();

        [Fact]
        public void Parse()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            Assert.True(parsedAssembly.Where(x => x.Name == "Test Script").Any());
        }

        [Fact]
        public void IndexersWork()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            IScript script = parsedAssembly[0];

            Assert.NotNull(script);

            Assert.Equal("Test Script", script.Name);

            Assert.Equal(script, parsedAssembly[script.Id]);

            var main = script.EntryPoint;

            Assert.Equal(main, parsedAssembly[main.Id]);

            var field = script.Settings[0];

            Assert.Equal(field, parsedAssembly[field.Id]);

            var property = script.Settings[1];

            Assert.Equal(property, parsedAssembly[property.Id]);

        }

        [Fact]
        public void SettingsWork()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            IScript script = parsedAssembly[0];

            var property = script.Settings[0];

            parsedAssembly.SetSetting(property.Id, 37);

            Assert.Equal(37, property.Value);

            property.Value = 19;

            Assert.Equal(19, parsedAssembly.GetSetting(property.Id));
        }

        [Fact]
        public void InvokeMember()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            IScript script = parsedAssembly[0];

            var floatScript = script.MiniScripts.Where(x => x.Name == "ReturnsFloat").FirstOrDefault();

            Assert.NotNull(floatScript);

            Assert.Equal(94f, floatScript.Invoke());

            Assert.Equal(94f, parsedAssembly.InvokeMember(floatScript.Id));
        }

        [Fact]
        public void RuntimeWorks()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            IScript script = parsedAssembly[0];

            var result = script.Invoke();

            Assert.True(result.GetType() == typeof(Task));

            var setup = script.Settings.Where(x => x.Name == "SetupRan").FirstOrDefault();
            var entry = script.Settings.Where(x => x.Name == "EntryPointRan").FirstOrDefault();
            var cleanup = script.Settings.Where(x => x.Name == "CleanupRan").FirstOrDefault();

            Assert.True((bool)setup.Value);

            Assert.True((bool)entry.Value);

            Assert.True((bool)cleanup.Value);
        }

        [Fact]
        public void DisposeRan()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            IScript script = parsedAssembly[0];

            var disposeSentinel = script.Settings.Where(x => x.Name == "DisposeRan").FirstOrDefault();

            script.Dispose();

            Assert.True((bool)disposeSentinel.Value);
        }

        [Fact]
        public void GroupingWorks()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            IScript script = parsedAssembly[0];

            Assert.Equal(3, script.SettingGroups.Count);
        }

        [Fact]
        public void EmptyScriptDoesntThrow()
        {
            // load an assembly
            Assembly testAssembly = typeof(AssemblyParserTest.MyExampleScript).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            Assert.True(parsedAssembly.Where(x => x.Name == "EmptyScript").Any());
        }

        [Fact]
        public void ConsoleApplicationImports()
        {
            // load an assembly
            Assembly testAssembly = typeof(ConsoleApplication.TestClass).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            Assert.Single(parsedAssembly.Scripts);

            Assert.Equal(14, parsedAssembly[0].Invoke());
        }

        [Fact]
        public void TopLevelApplicationWorks()
        {
            // load an assembly
            Assembly testAssembly = typeof(ConsoleApplicationTopLevel.TestClass).Assembly;

            IScriptAssembly parsedAssembly = Parser.Parse(testAssembly);

            Assert.Single(parsedAssembly.Scripts);

            Assert.Equal(44, parsedAssembly[0].Invoke());
        }
    }
}

namespace AssemblyParserTest
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
#pragma warning disable CS0414
        bool SetupRan = false;

        [Setting]
        bool EntryPointRan = false;

        [Setting]
        bool CleanupRan = false;

        [Setting]
        bool DisposeRan = false;
#pragma warning restore CS0414

        [Setup]
        public void Before()
        {
            SetupRan = true;
        }

        [EntryPoint]
        public Task Main()
        {
            EntryPointRan = true;
            return new Task(() => Task.Yield());
        }

        [Cleanup]
        public void After()
        {
            CleanupRan = true;
        }

        [MiniScript]
        [Name("Alternate Message")]
        [Description("Displays a different message than hello world")]
        public void ExtraStuff()
        {

        }

        [MiniScript]
        [Name("ReturnsFloat")]
        public float Thing()
        {
            return 94f;
        }

        public void HiddenStuff()
        {
            // should not be found and displayed
        }

        public void Dispose()
        {
            DisposeRan = true;
        }
    }

    [Script]
    public class EmptyScript
    {
    }
}