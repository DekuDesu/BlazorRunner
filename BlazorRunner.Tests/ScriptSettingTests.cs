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
    public class ScriptSettingTests
    {
        public int TestField = 1;

        public double TestProperty { get; set; } = 3d;

        [Fact]
        public void FieldSet()
        {
            FieldInfo info = typeof(ScriptSettingTests).GetField(nameof(TestField));

            IScriptSetting setting = Factory.CreateScriptSetting(info, this);

            Assert.Equal(TestField, setting.Value);

            TestField = 12;

            Assert.Equal(TestField, setting.Value);

            setting.Value = 14;

            Assert.Equal(14, TestField);

            Assert.Equal($"{setting.Id} (Field)", setting.ToString());

            Assert.True(setting.IsField);
        }

        [Fact]
        public void Propertyset()
        {
            PropertyInfo info = typeof(ScriptSettingTests).GetProperty(nameof(TestProperty));

            IScriptSetting setting = Factory.CreateScriptSetting(info, this);

            Assert.Equal(TestProperty, setting.Value);

            TestProperty = 12;

            Assert.Equal(TestProperty, setting.Value);

            setting.Value = 14;

            Assert.Equal(14, TestProperty);

            Assert.Equal($"{setting.Id} (Property)", setting.ToString());

            Assert.True(setting.IsProperty);
        }
    }
}
