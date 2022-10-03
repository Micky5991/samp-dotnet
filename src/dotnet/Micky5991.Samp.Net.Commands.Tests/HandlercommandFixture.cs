using System;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Micky5991.Samp.Net.Framework.Interfaces.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class HandlercommandFixture
    {
        private Mock<IPlayer> playerMock;

        private Mock<IAuthorizationFacade> authorization;

        private HandlerCommand handlerCommand;

        private object[] passedArguments;

        private Action<object[]> fakeExecutor;

        private CommandAttribute attribute;

        [TestInitialize]
        public void Setup()
        {
            this.playerMock = new Mock<IPlayer>();
            this.authorization = new Mock<IAuthorizationFacade>();

            this.passedArguments = Array.Empty<object>();
            this.fakeExecutor = _ => { };
            this.attribute = new CommandAttribute("grouped", "command");

            this.authorization.Setup(
                                            x => x.AuthorizeAsync(
                                                                  It.IsAny<ClaimsPrincipal>(),
                                                                  It.IsAny<object>(),
                                                                  It.IsAny<AuthorizeAttribute[]>()))
                .ReturnsAsync(AuthorizationResult.Success());

            this.handlerCommand = new HandlerCommand(this.authorization.Object, this.attribute, Array.Empty<AuthorizeAttribute>(), Array.Empty<string>(), new []
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null),
                new ParameterDefinition("allow", typeof(bool), false, null),
                new ParameterDefinition("message", typeof(string), true, "cool"),
            },
                                                     x =>
                                                     {
                                                         this.fakeExecutor(x);
                                                         this.passedArguments = x;

                                                         return Task.CompletedTask;
                                                     });
        }

        [TestMethod]
        public void PassingInvalidConstructorAuthorizationArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(null!, this.attribute, Array.Empty<AuthorizeAttribute>(), Array.Empty<string>(), new[]
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null)
            },
                                                  _ => Task.CompletedTask);

            act.Should().Throw<ArgumentNullException>().WithMessage("*authorization*");
        }

        [TestMethod]
        public void PassingInvalidConstructorAttributeArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(this.authorization.Object,  null!, Array.Empty<AuthorizeAttribute>(), Array.Empty<string>(), new[]
            {
                new ParameterDefinition("player", typeof(IPlayer), false, null)
            },
                                                  _ => Task.CompletedTask);

            act.Should().Throw<ArgumentNullException>().WithMessage("*attribute*");
        }

        [TestMethod]
        public void PassingInvalidConstructorParametersArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  this.authorization.Object,
                                                  this.attribute,
                                                  Array.Empty<AuthorizeAttribute>(),
                                                  Array.Empty<string>(),
                                                  null!,
                                                  _ => Task.CompletedTask);

            act.Should().Throw<ArgumentNullException>().WithMessage("*parameters*");
        }

        [TestMethod]
        public void PassingInvalidConstructorExecutorArgumentThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  this.authorization.Object,
                                                  this.attribute,
                                                  Array.Empty<AuthorizeAttribute>(),
                                                  Array.Empty<string>(),
                                                  new[]
                                                  {
                                                      new ParameterDefinition("player", typeof(IPlayer), false, null)
                                                  },
                                                  null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*executor*");
        }

        [TestMethod]
        public void PassingParameterDefinitionWithNoDefinitionsThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  this.authorization.Object,
                                                  this.attribute,
                                                  Array.Empty<AuthorizeAttribute>(),
                                                  Array.Empty<string>(),
                                                  Array.Empty<ParameterDefinition>(),
                                                  null!);

            act.Should().Throw<ArgumentException>().WithMessage("*parameters*");
        }

        [TestMethod]
        public void PassingParameterDefinitionWithNoPlayerParameterThrowsException()
        {
            Action act = () => new HandlerCommand(
                                                  this.authorization.Object,
                                                  this.attribute,
                                                  Array.Empty<AuthorizeAttribute>(),
                                                  Array.Empty<string>(),
                                                  new[]
                                                  {
                                                      new ParameterDefinition("amount", typeof(int), false, null)
                                                  },
                                                  null!);

            act.Should().Throw<ArgumentException>().WithMessage("*parameters*");
        }

        [TestMethod]
        public async Task CallingCommandExecutesExecutor()
        {
            var result = await this.handlerCommand.TryExecuteAsync(this.playerMock.Object, new object[]
            {
                true,
                "ok"
            }, false);

            result.Status.Should().Be(CommandExecutionStatus.Ok);
            result.Message.Should().BeEmpty();
            this.passedArguments.Should().ContainInOrder(this.playerMock.Object, true, "ok");
        }

        [TestMethod]
        public async Task PassingInvalidArgumentTypesThrowsException()
        {
            var result = await this.handlerCommand.TryExecuteAsync(this.playerMock.Object, new object[]
            {
                true,
                123
            }, false);

            result.Status.Should().Be(CommandExecutionStatus.ArgumentTypeMismatch);
            this.passedArguments.Should().BeEmpty();
        }

        [TestMethod]
        public async Task PassingNotEnoughArgumentsWithoutDefaultArgumentsThrowsException()
        {
            var result = await this.handlerCommand.TryExecuteAsync(this.playerMock.Object, Array.Empty<object>(), false);

            result.Status.Should().Be(CommandExecutionStatus.MissingArgument);
            this.passedArguments.Should().BeEmpty();
        }

        [TestMethod]
        public async Task PassingTooManyArgumentsReturnsError()
        {
            var result = await this.handlerCommand.TryExecuteAsync(this.playerMock.Object, new object[]
            {
                true,
                "ok",
                1,
                2
            }, false);

            result.Status.Should().Be(CommandExecutionStatus.TooManyArguments);
            this.passedArguments.Should().BeEmpty();
        }

        [TestMethod]
        public async Task ThrowingAnyExceptionInsideCommandReturnsError()
        {
            this.fakeExecutor = _ => throw new Exception("Any message");

            var result = await this.handlerCommand.TryExecuteAsync(this.playerMock.Object, new object[]
            {
                true,
                "ok",
            }, false);

            result.Status.Should().Be(CommandExecutionStatus.Exception);
            result.Message.Should().Be("Any message");
            this.passedArguments.Should().BeEmpty();
        }
    }
}
