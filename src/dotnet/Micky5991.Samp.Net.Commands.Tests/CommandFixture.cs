using System;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class CommandFixture
    {
        [TestMethod]
        public void VerifyTestCommandInheritance()
        {
            typeof(TestCommand).BaseType.Should().Be(typeof(Command));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("grouped")]
        public void CreatingCommandWithCorrectParametersCreatesInstance(string groupName)
        {
            var command = new TestCommand("command", groupName, new ParameterDefinition[]
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null)
            });

            command.Name.Should().Be("command");
            command.Group.Should().Be(groupName);
            command.Parameters.Should()
                   .OnlyContain(x => x.Equals(new ParameterDefinition("player", typeof(IPlayer), false, null)));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void CreatingCommandWithInvalidNameArgumentThrowsException(string name)
        {
            Action act = () => new TestCommand(name, "groupName", new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null)
            });

            act.Should().Throw<ArgumentException>().WithMessage("*name*");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void CreatingCommandWithInvalidGroupArgumentThrowsException(string group)
        {
            Action act = () => new TestCommand("name", group, new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null)
            });

            act.Should().Throw<ArgumentException>().WithMessage("*group*");
        }

        [TestMethod]
        public void PassingNullAsParameterDefinitionThrowsException()
        {
            Action act = () => new TestCommand("name", null, null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*parameter*");
        }

        [TestMethod]
        public void CommandRequiresPlayerAsFirstArgumentInDefinitionType()
        {
            Action act = () => new TestCommand("name", null, new ParameterDefinition[]
            {
                new ("player", typeof(int), false, null)
            });

            act.Should().Throw<ArgumentException>().WithMessage("*parameter*").WithMessage("*player*");
        }
    }
}
