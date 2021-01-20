using System;
using System.Linq;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Services;
using Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class CommandFactoryFixture
    {
        private CommandFactory commandFactory;

        [TestInitialize]
        public void Setup()
        {
            this.commandFactory = new CommandFactory();
        }

        [TestMethod]
        public void InvalidFactoryParametersThrowException()
        {
            Action act = () => this.commandFactory.BuildFromCommandHandler(null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*handler*");
        }

        [TestMethod]
        public void CommandHandlerWithNoDefinedMethodsReturnsEmptyCollection()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new EmptyCommandHandler());
            commands.Should().BeEmpty();
        }

        [TestMethod]
        public void CommandHandlerWithNoCommandMethodsReturnsEmptyCollection()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new NoCommandMethodCommandHandler());
            commands.Should().BeEmpty();
        }

        [TestMethod]
        public void CommandHandlerWithSingleCommandMethodReturnsCorrectCommand()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new SingleCommandHandler());
            commands.Should().HaveCount(1);

            var command = commands.First();
            command.Should().BeOfType<HandlerCommand>();

            command.Name.Should().Be(SingleCommandHandler.CommandName);
            command.Group.Should().Be(SingleCommandHandler.CommandGroup);
            command.Parameters.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void GroupedCommandHandlerWithSingleCommandMethodReturnsCorrectCommand()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new GroupedCommandHandler());
            commands.Should().HaveCount(1);

            var command = commands.First();
            command.Should().BeOfType<HandlerCommand>();

            command.Name.Should().Be("verb");
            command.Group.Should().Be("group");
            command.Parameters.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void MultipleCommandHandlerWillBeBuilt()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new MultipleCommandsHandler());
            commands.Should().HaveCount(2);

            var command1 = commands.ElementAt(0);
            command1.Should().BeOfType<HandlerCommand>();

            command1.Name.Should().Be("command1");
            command1.Group.Should().Be("grouped");
            command1.Parameters.Should().NotBeNull().And.BeEmpty();

            var command2 = commands.ElementAt(1);
            command2.Should().BeOfType<HandlerCommand>();

            command2.Name.Should().Be("command2");
            command2.Group.Should().BeNull();
            command2.Parameters.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void TypedCommandHandlerWillBeBuilt()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new TypedParameterCommandHandler());
            commands.Should().HaveCount(1);

            var command = commands.ElementAt(0);

            command.Should().BeOfType<HandlerCommand>();

            command.Name.Should().Be("veh");
            command.Group.Should().Be("grouped");
            command.Parameters.Should()
                   .HaveCount(3)
                   .And.ContainInOrder(
                                       new ParameterDefinition("player", typeof(IPlayer), false, null),
                                       new ParameterDefinition("number", typeof(string), false, null),
                                       new ParameterDefinition("type", typeof(int), false, 123)
                                      );
        }

        [TestMethod]
        public void TypedCommandHandlerWillBeBuiltWithDefault()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new DefaultValuesCommandHandler());
            commands.Should().HaveCount(1);

            var command = commands.ElementAt(0);

            command.Should().BeOfType<HandlerCommand>();

            command.Name.Should().Be("test");
            command.Group.Should().Be("grouped");
            command.Parameters.Should()
                   .HaveCount(3)
                   .And.ContainInOrder(
                                       new ParameterDefinition("player", typeof(IPlayer), false, null),
                                       new ParameterDefinition("test", typeof(string), false, null),
                                       new ParameterDefinition("provided", typeof(int), true, 123)
                                      );
        }
    }
}
