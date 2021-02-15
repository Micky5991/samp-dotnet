using System;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class CommandFixture
    {
        private Mock<IAuthorizationService> authorizationService;

        [TestInitialize]
        public void Setup()
        {
            this.authorizationService = new Mock<IAuthorizationService>();
        }

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
            var attribute = new CommandAttribute(groupName, "command");
            var command = new TestCommand(
                                          this.authorizationService.Object,
                                          attribute,
                                          Array.Empty<string>(),
                                          new ParameterDefinition[]
                                          {
                                              new ("player", typeof(IPlayer), false, null),
                                          });

            command.Name.Should().Be("command");
            command.Group.Should().Be(groupName);
            command.Parameters.Should()
                   .OnlyContain(x => x.Equals(new ParameterDefinition("player", typeof(IPlayer), false, null)));
        }

        [TestMethod]
        public void CreatingCommandWithInvalidAuthorizationServiceArgumentThrowsException()
        {
            var attribute = new CommandAttribute("groupName", "name");
            Action act = () => new TestCommand(
                                               null!,
                                               attribute,
                                               Array.Empty<string>(),
                                               new ParameterDefinition[]
                                               {
                                                   new ("player", typeof(IPlayer), false, null),
                                               });

            act.Should().Throw<ArgumentException>().WithMessage("*authorization*");
        }

        [TestMethod]
        public void CreatingCommandWithInvalidAliasNamesArgumentThrowsException()
        {
            var attribute = new CommandAttribute("groupName", "name");
            Action act = () => new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               null!,
                                               new ParameterDefinition[]
                                               {
                                                   new ("player", typeof(IPlayer), false, null),
                                               });

            act.Should().Throw<ArgumentException>().WithMessage("*alias*");
        }

        [TestMethod]
        public void PassingNullAsParameterDefinitionThrowsException()
        {
            var attribute = new CommandAttribute("name");
            Action act = () => new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               Array.Empty<string>(),
                                               null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*parameter*");
        }

        [TestMethod]
        public void CommandRequiresPlayerAsFirstArgumentInDefinitionType()
        {
            var attribute = new CommandAttribute("name");
            Action act = () => new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               Array.Empty<string>(),
                                               new ParameterDefinition[]
                                               {
                                                   new ("player", typeof(int), false, null),
                                               });

            act.Should().Throw<ArgumentException>().WithMessage("*parameter*").WithMessage("*player*");
        }

        [TestMethod]
        public void CommandRequiresOptionalParametersToComeLast()
        {
            var attribute = new CommandAttribute("name");
            Action act = () => new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               Array.Empty<string>(),
                                               new ParameterDefinition[]
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
            var attribute = new CommandAttribute("name");
            var testCommand = new TestCommand(
                                              this.authorizationService.Object,
                                              attribute,
                                              Array.Empty<string>(),
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
            var attribute = new CommandAttribute("veh", "create");
            var testCommand = new TestCommand(
                                              this.authorizationService.Object,
                                              attribute,
                                              Array.Empty<string>(),
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
            var attribute = new CommandAttribute("veh", "create");

            var testCommand = new TestCommand(
                                              this.authorizationService.Object,
                                              attribute,
                                              Array.Empty<string>(),
                                              new ParameterDefinition[]
                                              {
                                                  new("player", typeof(IPlayer), false, 5),
                                              });

            testCommand.HelpSignature.Should().Be("/veh create");
        }

        [TestMethod]
        public void CommandHelpStringWillBeGeneratedCorrectlyWithSingleDefaultParameter()
        {
            var attribute = new CommandAttribute("veh", "create");
            var testCommand = new TestCommand(
                                              this.authorizationService.Object,
                                              attribute,
                                              Array.Empty<string>(),
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
            var attribute = new CommandAttribute("veh", "create");
            var testCommand = new TestCommand(
                                              this.authorizationService.Object,
                                              attribute,
                                              Array.Empty<string>(),
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
            var attribute = new CommandAttribute("veh", "create");
            Action act = () => new TestCommand(
                                               this.authorizationService.Object,
                                              attribute,
                                              Array.Empty<string>(),
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
