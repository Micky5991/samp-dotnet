using FluentAssertions;
using Micky5991.Samp.Net.Commands.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class ParameterDefinitionFixture
    {
        [TestMethod]
        public void ParameterDefinitionValuesWillbeSet()
        {
            var definition = new ParameterDefinition("a", typeof(int), true, null);

            definition.Name.Should().Be("a");
            definition.Type.Should().Be(typeof(int));
            definition.HasDefault.Should().BeTrue();
            definition.DefaultValue.Should().Be(null);
        }

        [TestMethod]
        public void FalseHasDefaultWillForceNullValue()
        {
            var definition = new ParameterDefinition("a", typeof(int), false, 1234);

            definition.HasDefault.Should().BeFalse();
            definition.DefaultValue.Should().Be(null);
        }

        [TestMethod]
        public void ParameterDefintionDetectsEquality()
        {
            var first = new ParameterDefinition("a", typeof(int), true, null);
            var second = new ParameterDefinition("a", typeof(int), true, null);

            first.Equals((object)second).Should().Be(true);
        }

        [TestMethod]
        [DataRow(true, true, true, false)]
        [DataRow(true, true, false, true)]
        [DataRow(true, false, true, true)]
        [DataRow(false, true, true, true)]

        [DataRow(true, false, true, false)]
        [DataRow(false, true, false, true)]
        [DataRow(false, false, true, true)]
        [DataRow(false, false, false, false)]
        public void ParameterDefintionDetectsIneqquality(bool sameName, bool sameType, bool sameHasDefault, bool sameDefaultValue)
        {
            var first = new ParameterDefinition("a", typeof(int), true, 1);
            var second = new ParameterDefinition(sameName ? "a" : "b", sameType ? typeof(int) : typeof(bool), sameHasDefault, sameDefaultValue ? 1 : 2);

            first.Equals((object)second).Should().BeFalse();
        }
    }
}
