using System;
using System.Collections.Generic;
using FluentAssertions;
using JetBrains.Annotations;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class HandlercommandFixture
    {
        private Mock<ICommandHandler> commandHandlerMock;

        private Mock<IPlayer> playerMock;

        private HandlerCommand handlerCommand;

        private object[] passedArguments;

        private Action<object[]> fakeExecutor;

        [TestInitialize]
        public void Setup()
        {
            this.passedArguments = Array.Empty<object>();
            this.commandHandlerMock = new Mock<ICommandHandler>();
            this.playerMock = new Mock<IPlayer>();
            this.fakeExecutor = _ => { };

            this.handlerCommand = new HandlerCommand(new NullLogger<HandlerCommand>(), "grouped", "command", Array.Empty<string>(),  null, new []
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null),
                new ParameterDefinition("allow", typeof(bool), false, null),
                new ParameterDefinition("message", typeof(string), true, "cool"),
            },
                                                     this.commandHandlerMock.Object,
                                                     x =>
                                                     {
                                                         this.fakeExecutor(x);

                                                         return this.passedArguments = x;
                                                     });
        }

        [TestMethod]
        public void PassingInvalidConstructorLoggerArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(null!, null, "a", Array.Empty<string>(), null, new[]
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null)
            },
                                                  this.commandHandlerMock.Object,
                                                  _ => null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
        }

        [TestMethod]
        public void PassingInvalidConstructorNameArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(new NullLogger<HandlerCommand>(), null, null!, Array.Empty<string>(), null, new[]
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null)
            },
                                                  this.commandHandlerMock.Object,
                                                  _ => null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*name*");
        }

        [TestMethod]
        public void PassingInvalidConstructorParametersArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  new NullLogger<HandlerCommand>(),
                                                  null,
                                                  "ok",
                                                  Array.Empty<string>(),
                                                  null,
                                                  null!,
                                                  this.commandHandlerMock.Object,
                                                  _ => null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*parameters*");
        }

        [TestMethod]
        public void PassingInvalidConstructorCommandHandlerArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  new NullLogger<HandlerCommand>(),
                                                  null,
                                                  "ok",
                                                  Array.Empty<string>(),
                                                  null,
                                                  new[]
                                                  {
                                                      new ParameterDefinition("player", typeof(IPlayer), false, null)
                                                  },
                                                  null!,
                                                  _ => null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*commandHandler*");
        }

        [TestMethod]
        public void PassingInvalidConstructorExecutorArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  new NullLogger<HandlerCommand>(),
                                                  null,
                                                  "ok",
                                                  Array.Empty<string>(),
                                                  null,
                                                  new[]
                                                  {
                                                      new ParameterDefinition("player", typeof(IPlayer), false, null)
                                                  },
                                                  this.commandHandlerMock.Object,
                                                  null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*executor*");
        }

        [TestMethod]
        public void PassingParameterDefinitionWithNoDefinitionsThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  new NullLogger<HandlerCommand>(),
                                                  null,
                                                  "ok",
                                                  Array.Empty<string>(),
                                                  null,
                                                  Array.Empty<ParameterDefinition>(),
                                                  this.commandHandlerMock.Object,
                                                  null!);

            act.Should().Throw<ArgumentException>().WithMessage("*parameters*");
        }

        [TestMethod]
        public void PassingParameterDefinitionWithNoPlayerParameterThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  new NullLogger<HandlerCommand>(),
                                                  null,
                                                  "ok",
                                                  Array.Empty<string>(),
                                                  null,
                                                  new[]
                                                  {
                                                      new ParameterDefinition("amount", typeof(int), false, null)
                                                  },
                                                  this.commandHandlerMock.Object,
                                                  null!);

            act.Should().Throw<ArgumentException>().WithMessage("*parameters*");
        }

        [TestMethod]
        public void CallingCommandExecutesExecutor()
        {
            var result = this.handlerCommand.TryExecute(this.playerMock.Object, new object[]
            {
                true,
                "ok"
            }, out var errorMessage);

            result.Should().Be(CommandExecutionStatus.Ok);
            errorMessage.Should().BeEmpty();
            this.passedArguments.Should().ContainInOrder(this.playerMock.Object, true, "ok");
        }

        [TestMethod]
        public void PassingInvalidArgumentTypesThrowsException()
        {
            var result = this.handlerCommand.TryExecute(this.playerMock.Object, new object[]
            {
                true,
                123
            }, out var errorMessage);

            result.Should().Be(CommandExecutionStatus.ArgumentTypeMismatch);
            this.passedArguments.Should().BeEmpty();
        }

        [TestMethod]
        public void PassingNotEnoughArgumentsWithoutDefaultArgumentsThrowsException()
        {
            var result = this.handlerCommand.TryExecute(this.playerMock.Object, new object[]
            {
            }, out var errorMessage);

            result.Should().Be(CommandExecutionStatus.MissingArgument);
            this.passedArguments.Should().BeEmpty();
        }

        [TestMethod]
        public void PassingTooManyArgumentsReturnsError()
        {
            var result = this.handlerCommand.TryExecute(this.playerMock.Object, new object[]
            {
                true,
                "ok",
                1,
                2
            }, out var errorMessage);

            result.Should().Be(CommandExecutionStatus.TooManyArguments);
            this.passedArguments.Should().BeEmpty();
        }

        [TestMethod]
        public void ThrowingAnyExceptionInsideCommandReturnsError()
        {
            this.fakeExecutor = _ => throw new Exception("Any message");

            var result = this.handlerCommand.TryExecute(this.playerMock.Object, new object[]
            {
                true,
                "ok",
            }, out var errorMessage);

            result.Should().Be(CommandExecutionStatus.Exception);
            errorMessage.Should().Be("Any message");
            this.passedArguments.Should().BeEmpty();
        }
    }
}
