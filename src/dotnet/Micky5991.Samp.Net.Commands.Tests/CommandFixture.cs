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
            var command = new TestCommand("command", Array.Empty<string>(), groupName, null, new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null),
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
            Action act = () => new TestCommand(name, Array.Empty<string>(), "groupName", null, new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null),
            });

            act.Should().Throw<ArgumentException>().WithMessage("*name*");
        }

        [TestMethod]
        public void CreatingCommandWithInvalidAliasNamesArgumentThrowsException()
        {
            Action act = () => new TestCommand("name", null!, "groupName", null, new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null),
            });

            act.Should().Throw<ArgumentException>().WithMessage("*alias*");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void CreatingCommandWithInvalidGroupArgumentThrowsException(string group)
        {
            Action act = () => new TestCommand("name", Array.Empty<string>(), group, null, new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null),
            });

            act.Should().Throw<ArgumentException>().WithMessage("*group*");
        }

        [TestMethod]
        public void PassingNullAsParameterDefinitionThrowsException()
        {
            Action act = () => new TestCommand("name", Array.Empty<string>(), null, null, null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*parameter*");
        }

        [TestMethod]
        public void CommandRequiresPlayerAsFirstArgumentInDefinitionType()
        {
            Action act = () => new TestCommand("name", Array.Empty<string>(), null, null, new ParameterDefinition[]
            {
                new ("player", typeof(int), false, null),
            });

            act.Should().Throw<ArgumentException>().WithMessage("*parameter*").WithMessage("*player*");
        }

        [TestMethod]
        public void CommandRequiresOptionalParametersToComeLast()
        {
            Action act = () => new TestCommand("name", Array.Empty<string>(), null, null, new ParameterDefinition[]
            {
                new ("player", typeof(IPlayer), false, null),
                new ("color1", typeof(int), true, null),
                new ("color2", typeof(int), false, null),
            });

            act.Should().Throw<ArgumentException>().WithMessage("*default*");
        }

        [TestMethod]
        public void CommandHelpStringWillBeGeneratedCorrectlyNoGroup()
        {
            var testCommand = new TestCommand(
                                              "name",
                                              Array.Empty<string>(),
                                              null,
                                              null,
                                              new ParameterDefinition[]
                                              {
                                                  new("player", typeof(IPlayer), false, null),
                                                  new("vehicle", typeof(int), false, null),
                                              });

            testCommand.HelpSignature.Should().Be("/name [vehicle]");
        }

        [TestMethod]
        public void CommandHelpStringWillBeGeneratedCorrectlyWithGroup()
        {
            var testCommand = new TestCommand(
                                              "create",
                                              Array.Empty<string>(),
                                              "veh",
                                              null,
                                              new ParameterDefinition[]
                                              {

                                                  new("player", typeof(IPlayer), false, 5),
                                                  new("vehicle", typeof(int), false, null),
                                              });

            testCommand.HelpSignature.Should().Be("/veh create [vehicle]");
        }

        [TestMethod]
        public void CommandHelpStringWillBeGeneratedCorrectlyWithoutParameter()
        {
            var testCommand = new TestCommand(
                                              "create",
                                              Array.Empty<string>(),
                                              "veh",
                                              null,
                                              new ParameterDefinition[]
                                              {
                                                  new("player", typeof(IPlayer), false, 5),
                                              });

            testCommand.HelpSignature.Should().Be("/veh create");
        }

        [TestMethod]
        public void CommandHelpStringWillBeGeneratedCorrectlyWithSingleDefaultParameter()
        {
            var testCommand = new TestCommand(
                                              "create",
                                              Array.Empty<string>(),
                                              "veh",
                                              null,
                                              new ParameterDefinition[]
                                              {
                                                  new("player", typeof(IPlayer), false, 5),
                                                  new("vehicle", typeof(int), true, null),
                                              });

            testCommand.HelpSignature.Should().Be("/veh create <vehicle>");
        }

        [TestMethod]
        public void CommandHelpStringWillBeGeneratedCorrectlyWithSingleDefaultParameterMultipleParameters()
        {
            var testCommand = new TestCommand(
                                              "create",
                                              Array.Empty<string>(),
                                              "veh",
                                              null,
                                              new ParameterDefinition[]
                                              {
                                                  new("player", typeof(IPlayer), false, null),
                                                  new("vehicle", typeof(int), false, 5),
                                                  new("color", typeof(int), true, 5),
                                              });

            testCommand.HelpSignature.Should().Be("/veh create [vehicle] <color>");
        }

        [TestMethod]
        public void CommandParameterNamesHaveToBeUnique()
        {
            Action act = () => new TestCommand(
                                              "create",
                                              Array.Empty<string>(),
                                              "veh",
                                              null,
                                              new ParameterDefinition[]
                                              {
                                                  new("player", typeof(IPlayer), false, null),
                                                  new("vehicle", typeof(int), false, 5),
                                                  new("vehicle", typeof(string), false, 5),
                                              });

            act.Should().Throw<ArgumentException>().WithMessage("*unique*");
        }
    }
}
