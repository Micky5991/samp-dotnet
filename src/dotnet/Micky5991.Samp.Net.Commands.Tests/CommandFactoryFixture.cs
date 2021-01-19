using System;
using System.Linq;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Services;
using Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers;
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
            var commands = this.commandFactory.BuildFromCommandHandler(new EmptyCommandHandler());
            commands.Should().BeEmpty();
        }

        [TestMethod]
        public void CommandHandlerWithSingleCommandMethodReturnsCorrectCommand()
        {
            var commands = this.commandFactory.BuildFromCommandHandler(new EmptyCommandHandler());
            commands.Should().HaveCount(1);

            var command = commands.First();
        }
    }
}
