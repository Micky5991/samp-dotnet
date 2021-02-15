using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Micky5991.EventAggregator;
using Micky5991.EventAggregator.Interfaces;
using Micky5991.Samp.Net.Commands.Attributes;
using Micky5991.Samp.Net.Commands.Elements;
using Micky5991.Samp.Net.Commands.Exceptions;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Commands.Services;
using Micky5991.Samp.Net.Commands.Tests.Fakes.CommandHandlers;
using Micky5991.Samp.Net.Framework.Events.Samp;
using Micky5991.Samp.Net.Framework.Interfaces.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class CommandServiceFixture
    {
        private CommandService commandService;

        private Mock<IMapper> mapperMock;

        private Mock<IEventAggregator> eventAggregatorMock;

        private Mock<ICommandFactory> commandFactoryMock;

        private Mock<IAuthorizationService> authorizationService;

        private List<ICommandHandler> commandHandlers;

        [TestInitialize]
        public void Setup()
        {
            this.mapperMock = new Mock<IMapper>();
            this.eventAggregatorMock = new Mock<IEventAggregator>();
            this.commandFactoryMock = new Mock<ICommandFactory>();
            this.authorizationService = new Mock<IAuthorizationService>();

            this.commandHandlers = new List<ICommandHandler>();

            this.RecreateCommandService();
        }

        public void RecreateCommandService()
        {
            this.commandService = new CommandService(
                                                     this.mapperMock.Object,
                                                     this.eventAggregatorMock.Object,
                                                     this.commandFactoryMock.Object,
                                                     new NullLogger<CommandService>(),
                                                     this.commandHandlers);
        }

        [TestMethod]
        public void NullMapperArgumentInConstructorThrowsException()
        {
            Action act = () => new CommandService(
                                                  null!,
                                                  this.eventAggregatorMock.Object,
                                                  this.commandFactoryMock.Object,
                                                  new NullLogger<CommandService>(),
                                                  Array.Empty<ICommandHandler>());

            act.Should().Throw<ArgumentNullException>().WithMessage("*mapper*");
        }

        [TestMethod]
        public void NullEventAggregatorArgumentInConstructorThrowsException()
        {
            Action act = () => new CommandService(
                                                  this.mapperMock.Object,
                                                  null!,
                                                  this.commandFactoryMock.Object,
                                                  new NullLogger<CommandService>(),
                                                  Array.Empty<ICommandHandler>());

            act.Should().Throw<ArgumentNullException>().WithMessage("*eventaggregator*");
        }

        [TestMethod]
        public void NullCommandFactoryArgumentInConstructorThrowsException()
        {
            Action act = () => new CommandService(
                                                  this.mapperMock.Object,
                                                  this.eventAggregatorMock.Object,
                                                  null!,
                                                  new NullLogger<CommandService>(),
                                                  Array.Empty<ICommandHandler>());

            act.Should().Throw<ArgumentNullException>().WithMessage("*factory*");
        }

        [TestMethod]
        public void NullLoggerArgumentInConstructorThrowsException()
        {
            Action act = () => new CommandService(
                                                  this.mapperMock.Object,
                                                  this.eventAggregatorMock.Object,
                                                  this.commandFactoryMock.Object,
                                                  null!,
                                                  Array.Empty<ICommandHandler>());

            act.Should().Throw<ArgumentNullException>().WithMessage("*logger*");
        }

        [TestMethod]
        public void NullCommandHandlerArgumentInConstructorThrowsException()
        {
            Action act = () => new CommandService(
                                                  this.mapperMock.Object,
                                                  this.eventAggregatorMock.Object,
                                                  this.commandFactoryMock.Object,
                                                  new NullLogger<CommandService>(),
                                                  null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*handler*");
        }

        [TestMethod]
        public void ServiceStarterSubscribesToEventAggregator()
        {
            Expression<Func<IEventAggregator, ISubscription>> subscribeExpression = x => x.Subscribe(
                                                       It.IsAny<IEventAggregator.EventHandlerDelegate<PlayerCommandEvent>>(),
                                                       true,
                                                       EventPriority.Normal,
                                                       ThreadTarget.PublisherThread);

            this.eventAggregatorMock.Setup(subscribeExpression);

            this.commandService.Start();

            this.eventAggregatorMock.Verify(subscribeExpression, Times.Once);
        }

        [TestMethod]
        public void CommandHandlerWithNullEventThrowsException()
        {
            Action act = () => this.commandService.OnPlayerCommand(null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*event*");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(-2)]
        public void PassingInvalidAmountArgumentToArgumentSplitterThrowsException(int amount)
        {
            Action act = () => this.commandService.SplitArgumentStringToArgumentList("", amount);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [DataRow("", 0, 0)]
        [DataRow("", 1, 0)]
        [DataRow("   ", 1, 0)]
        [DataRow("test", 0, 0)]
        [DataRow("test", 1, 1)]
        [DataRow("test", 2, 1)]
        [DataRow("test cool", 0, 0)]
        [DataRow("test cool", 1, 1)]
        [DataRow("test cool", 2, 2)]
        [DataRow("test cool", 3, 2)]
        public void PassingDifferentArgumentTextsReturnsCorrectAmount(string argumentText, int argumentAmount, int expectedAmount)
        {
            var result = this.commandService.SplitArgumentStringToArgumentList(argumentText, argumentAmount);

            result.Should().HaveCount(expectedAmount).And.NotContainNulls();
        }

        [TestMethod]
        public void PassingArgumentsWithManySpacesAroundArgumentsReturnsCorrectValues()
        {
            var result = this.commandService.SplitArgumentStringToArgumentList("   test     cool    ree", 3);

            result.Should().HaveCount(3).And.ContainInOrder("test", "cool", "ree");
        }

        [TestMethod]
        public void PassingArgumentsWithManySpacesAroundArgumentsLowerThanWordsReturnsValuesWithSingleSpacesTwo()
        {
            var result = this.commandService.SplitArgumentStringToArgumentList("   test     cool    ree", 2);

            result.Should().HaveCount(2).And.ContainInOrder("test", "cool ree");
        }

        [TestMethod]
        public void PassingArgumentsWithManySpacesAroundArgumentsLowerThanWordsReturnsValuesWithSingleSpacesOne()
        {
            var result = this.commandService.SplitArgumentStringToArgumentList("   test     cool    ree", 1);

            result.Should().HaveCount(1).And.ContainInOrder("test cool ree");
        }

        [TestMethod]
        public void PassingArgumentsWithManySpacesAroundArgumentsLowerThanWordsReturnsValuesWithSingleSpacesZero()
        {
            var result = this.commandService.SplitArgumentStringToArgumentList("   test     cool    ree", 0);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void PassingInvalidArgumentsArgumentThrowsException()
        {
            Action act = () => this.commandService.MapArgumentListToTypes(null!, new List<Type>(), x => { });

            act.Should().Throw<ArgumentNullException>().WithMessage("*arguments*");
        }

        [TestMethod]
        public void PassingInvalidTypesArgumentThrowsException()
        {
            Action act = () => this.commandService.MapArgumentListToTypes(Array.Empty<string>(), null!, x => { });

            act.Should().Throw<ArgumentNullException>().WithMessage("*types*");
        }

        [TestMethod]
        public void PassingInvalidContextArgumentThrowsException()
        {
            Action act = () => this.commandService.MapArgumentListToTypes(Array.Empty<string>(), new List<Type>(), null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*context*");
        }

        [TestMethod]
        public void PassingTooManyArgumentsToMapThrowsException()
        {
            Action act = () => this.commandService.MapArgumentListToTypes(new []{ "test" }, new List<Type>(), null!);

            act.Should().Throw<ArgumentException>().WithMessage("*arguments*");
        }

        [TestMethod]
        [DataRow("group")]
        [DataRow(null)]
        public void AddingTwoHandlersWithSameCommandGroupAndNameThrowsException(string groupName)
        {
            this.commandHandlers.Add(new GroupedCommandHandler());
            this.commandHandlers.Add(new DuplicatedGroupedCommandHandler());
            var attribute = new CommandAttribute(groupName, "test");

            this.commandFactoryMock.Setup(x => x.BuildFromCommandHandler(It.IsAny<ICommandHandler>()))
                .Returns<ICommandHandler>(x =>
                                              new List<ICommand>
                                              {
                                                  new HandlerCommand(this.authorizationService.Object, attribute, Array.Empty<string>(), new ParameterDefinition[]
                                                  {
                                                      ParameterDefinition.Player(),
                                                  },
                                                                     _ => null!),
                                              });

            this.RecreateCommandService();

            Action act = () => this.commandService.Start();

            act.Should().Throw<DuplicateCommandException>();
        }

        [TestMethod]
        public void MappingArgumentsWillNotBeCalledWithOnlyPlayerParameter()
        {
            Expression<Action<IMapper>> mapperExpression = x => x.Map(
                                                                      It.IsAny<object>(),
                                                                      It.IsAny<Type>(),
                                                                      It.IsAny<Type>(),
                                                                      It.IsAny<Action<IMappingOperationOptions<object, object>>>());

            this.mapperMock.Setup(mapperExpression);

            this.commandService.Start();
            this.commandService.MapArgumentListToTypes(Array.Empty<string>(), new List<Type>(), x => { });

            this.mapperMock.Verify(mapperExpression, Times.Never);
        }

        [TestMethod]
        public void MappingArgumentsWillBeCalledCorrectly()
        {
            Expression<Action<IMapper>> stringMapperExpression = x => x.Map(
                                                                            "test",
                                                                            typeof(string),
                                                                            typeof(string),
                                                                            It.IsAny<Action<IMappingOperationOptions<object,
                                                                                object>>>());
            Expression<Action<IMapper>> intMapperExpression = x => x.Map(
                                                                         "123",
                                                                         typeof(string),
                                                                         typeof(int),
                                                                         It.IsAny<Action<IMappingOperationOptions<object,
                                                                             object>>>());
            Expression<Action<IMapper>> playerMapperExpression = x => x.Map(
                                                                         "Micky5991",
                                                                         typeof(string),
                                                                         typeof(IPlayer),
                                                                         It.IsAny<Action<IMappingOperationOptions<object,
                                                                             object>>>());

            this.mapperMock.Setup(stringMapperExpression);
            this.mapperMock.Setup(intMapperExpression);
            this.mapperMock.Setup(playerMapperExpression);

            this.commandService.Start();
            this.commandService.MapArgumentListToTypes(new []
            {
                "test",
                "123",
                "Micky5991",
            }, new List<Type>
            {
                typeof(string),
                typeof(int),
                typeof(IPlayer),
            }, x => { });

            // String mapper should be called if mapper was modified.
            this.mapperMock.Verify(stringMapperExpression, Times.Once);
            this.mapperMock.Verify(intMapperExpression, Times.Once);
            this.mapperMock.Verify(playerMapperExpression, Times.Once);
        }

        [TestMethod]
        public void MappingArgumentsReturnsValuesOfMapper()
        {
            Expression<Func<IMapper, object>> stringMapperExpression = x => x.Map(
                                                                            "test",
                                                                            typeof(string),
                                                                            typeof(string),
                                                                            It.IsAny<Action<IMappingOperationOptions<object,
                                                                                object>>>());

            Expression<Func<IMapper, object>> intMapperExpression = x => x.Map(
                                                                         "123",
                                                                         typeof(string),
                                                                         typeof(int),
                                                                         It.IsAny<Action<IMappingOperationOptions<object,
                                                                             object>>>());

            Expression<Func<IMapper, object>> playerMapperExpression = x => x.Map(
                                                                         "Micky5991",
                                                                         typeof(string),
                                                                         typeof(IPlayer),
                                                                         It.IsAny<Action<IMappingOperationOptions<object,
                                                                             object>>>());

            this.mapperMock.Setup(stringMapperExpression).Returns("TEST");
            this.mapperMock.Setup(intMapperExpression).Returns(123);
            this.mapperMock.Setup(playerMapperExpression).Returns(null);

            this.commandService.Start();
            var result = this.commandService.MapArgumentListToTypes(new []
            {
                "test",
                "123",
                "Micky5991",
            }, new List<Type>
            {
                typeof(string),
                typeof(int),
                typeof(IPlayer),
            }, x => { });

            result.Should().ContainInOrder("TEST", 123, null);
        }

        [TestMethod]
        public void ServiceFindsCorrectCommandAfterCall()
        {
            var handler = new GroupedCommandHandler();
            var attribute = new CommandAttribute("group", "verb");
            var command = new TestCommand(
                                          this.authorizationService.Object,
                                          attribute,
                                          Array.Empty<string>(),
                                          new ParameterDefinition[]
                                          {
                                              ParameterDefinition.Player(),
                                          });

            this.commandHandlers.Add(handler);
            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(handler))
                .Returns(new List<ICommand>
                {
                    command,
                });

            this.RecreateCommandService();

            this.commandService.Start();

            this.commandService.TryGetCommandFromArgumentText(
                                                              "group verb test",
                                                              false,
                                                              out var potentialCommands,
                                                              out var groupName,
                                                              out var remainingArgumentText);

            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(1)
                             .And.ContainValue(command);

            groupName.Should().Be("group");
            remainingArgumentText.Should().Be("test");
        }

        [TestMethod]
        public void ServiceFindsCorrectCommandAfterCallNoGroup()
        {
            var handler = new GroupedCommandHandler();
            var attribute = new CommandAttribute("verb");
            var command = new TestCommand(
                                          this.authorizationService.Object,
                                          attribute,
                                          Array.Empty<string>(),
                                          new ParameterDefinition[]
                                          {
                                              ParameterDefinition.Player(),
                                          });

            this.commandHandlers.Add(handler);
            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(handler))
                .Returns(new List<ICommand>
                {
                    command,
                });

            this.RecreateCommandService();

            this.commandService.Start();

            var success = this.commandService.TryGetCommandFromArgumentText(
                                                              "verb test",
                                                              false,
                                                              out var potentialCommands,
                                                              out var groupName,
                                                              out var remainingArgumentText);

            success.Should().BeTrue();
            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(1)
                             .And.ContainValue(command);

            groupName.Should().BeEmpty();
            remainingArgumentText.Should().Be("test");
        }

        [TestMethod]
        public void ServiceRequiresGroupNameToFindCommand()
        {
            var handler = new GroupedCommandHandler();
            var attribute = new CommandAttribute("group", "verb");
            var command = new TestCommand(
                                          this.authorizationService.Object,
                                          attribute,
                                          Array.Empty<string>(),
                                          new ParameterDefinition[]
                                          {
                                              ParameterDefinition.Player(),
                                          });

            this.commandHandlers.Add(handler);
            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(handler))
                .Returns(new List<ICommand>
                {
                    command,
                });

            this.commandService.Start();

            var success = this.commandService.TryGetCommandFromArgumentText(
                                                                            "verb test",
                                                                            false,
                                                                            out var potentialCommands,
                                                                            out var groupName,
                                                                            out var remainingArgumentText);

            success.Should().BeFalse();
            potentialCommands.Should().BeEmpty();
            groupName.Should().BeEmpty();
            remainingArgumentText.Should().Be("verb test");
        }

        [TestMethod]
        public void ServicePrefersGroupedCommandOverNonGrouped()
        {
            var firstHandler = new GroupedCommandHandler();
            var secondHandler = new GroupedCommandHandler();
            var attribute = new CommandAttribute("group", "verb");

            var firstCommand = new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               Array.Empty<string>(),
                                               new []
                                               {
                                                   ParameterDefinition.Player(),
                                               });

            var noGroupAttribute = new CommandAttribute("verb");
            var secondCommand = new TestCommand(
                                                this.authorizationService.Object,
                                                noGroupAttribute,
                                                Array.Empty<string>(),
                                                new[]
                                                {
                                                    ParameterDefinition.Player(),
                                                });

            this.commandHandlers.Add(firstHandler);
            this.commandHandlers.Add(secondHandler);

            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(firstHandler))
                .Returns(new List<ICommand>
                {
                    firstCommand,
                });

            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(secondHandler))
                .Returns(new List<ICommand>
                {
                    secondCommand,
                });

            this.RecreateCommandService();

            this.commandService.Start();

            var success = this.commandService.TryGetCommandFromArgumentText(
                                                                            "group verb test",
                                                                            false,
                                                                            out var potentialCommands,
                                                                            out var groupName,
                                                                            out var remainingArgumentText);

            success.Should().BeTrue();
            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(1)
                             .And.ContainValue(firstCommand);
            groupName.Should().Be("group");
            remainingArgumentText.Should().Be("test");

            success = this.commandService.TryGetCommandFromArgumentText(
                                                                            "verb test",
                                                                            false,
                                                                            out potentialCommands,
                                                                            out groupName,
                                                                            out remainingArgumentText);

            success.Should().BeTrue();
            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(1)
                             .And.ContainValue(secondCommand);
            groupName.Should().BeEmpty();
            remainingArgumentText.Should().Be("test");
        }

        [TestMethod]
        public void IfGroupHasBeenCalledFunctionReturnsAllAvailableCommands()
        {
            var firstHandler = new GroupedCommandHandler();
            var secondHandler = new GroupedCommandHandler();
            var attribute = new CommandAttribute("group", "verb");

            var firstCommand = new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               Array.Empty<string>(),
                                               new[]
                                               {
                                                   ParameterDefinition.Player(),
                                               });

            var otherGroupAttribute = new CommandAttribute("group", "othercommand");
            var secondCommand = new TestCommand(
                                                this.authorizationService.Object,
                                                otherGroupAttribute,
                                                Array.Empty<string>(),
                                                new[]
                                                {
                                                    ParameterDefinition.Player(),
                                                });

            var otherGroupCommandAttribute = new CommandAttribute("othergroup", "othercommand");
            var wrongCommand = new TestCommand(
                                               this.authorizationService.Object,
                                               otherGroupCommandAttribute,
                                               Array.Empty<string>(),
                                               new[]
                                               {
                                                   ParameterDefinition.Player(),
                                               });

            this.commandHandlers.Add(firstHandler);
            this.commandHandlers.Add(secondHandler);

            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(firstHandler))
                .Returns(new List<ICommand>
                {
                    firstCommand,
                });

            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(secondHandler))
                .Returns(new List<ICommand>
                {
                    secondCommand,
                    wrongCommand,
                });

            this.RecreateCommandService();

            this.commandService.Start();

            var success = this.commandService.TryGetCommandFromArgumentText(
                                                                            "group",
                                                                            false,
                                                                            out var potentialCommands,
                                                                            out var groupName,
                                                                            out var remainingArgumentText);

            success.Should().BeFalse();
            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(2)
                             .And.ContainValue(firstCommand)
                             .And.ContainValue(secondCommand);
            groupName.Should().Be("group");
            remainingArgumentText.Should().BeEmpty();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void InvalidArgumentTextThrowsException(string argumentText)
        {
            Action act = () => this.commandService.TryGetCommandFromArgumentText(
             argumentText,
             false,
             out _,
             out _,
             out _);

            if (argumentText == null)
            {
                act.Should().Throw<ArgumentNullException>().WithMessage("*argumentText*");
            }
            else
            {
                act.Should().Throw<ArgumentException>().WithMessage("*argumentText*");
            }
        }

        [TestMethod]
        public void BuildingSingleCommandHandlerWithMultipleAliasedCommandsInSingleGroupReturnsCorrectAmount()
        {
            var firstHandler = new GroupedCommandHandler();
            var repairAttribute = new CommandAttribute("veh", "repair");

            var firstCommand = new TestCommand(
                                               this.authorizationService.Object,
                                               repairAttribute,
                                               new[] { "r", },
                                               new[]
                                               {
                                                   ParameterDefinition.Player(),
                                               });

            var spawnAttribute = new CommandAttribute("veh", "spawn");
            var secondCommand = new TestCommand(
                                                this.authorizationService.Object,
                                                spawnAttribute,
                                                new []{ "s", },
                                                new []
                                                {
                                                    ParameterDefinition.Player(),
                                                });

            this.commandHandlers.Add(firstHandler);

            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(firstHandler))
                .Returns(new List<ICommand>
                {
                    firstCommand,
                    secondCommand,
                });

            this.RecreateCommandService();

            this.commandService.Start();

            var success = this.commandService.TryGetCommandFromArgumentText(
                                                                            "veh",
                                                                            false,
                                                                            out var potentialCommands,
                                                                            out var groupName,
                                                                            out var remainingArgumentText);

            success.Should().BeFalse();
            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(4)
                             .And.ContainValues(firstCommand, secondCommand)
                             .And.ContainKeys("veh spawn", "veh s", "veh repair", "veh r");

            groupName.Should().Be("veh");
            remainingArgumentText.Should().BeEmpty();
        }

        [TestMethod]
        public void GettingCommandsWithFilteredAliasReturnsOnlyNonAliasedNames()
        {
            var firstHandler = new GroupedCommandHandler();
            var attribute = new CommandAttribute("veh", "repair");

            var firstCommand = new TestCommand(
                                               this.authorizationService.Object,
                                               attribute,
                                               new[] { "r", },
                                               new[]
                                               {
                                                   ParameterDefinition.Player(),
                                               });

            this.commandHandlers.Add(firstHandler);

            this.commandFactoryMock
                .Setup(x => x.BuildFromCommandHandler(firstHandler))
                .Returns(new List<ICommand>
                {
                    firstCommand,
                });

            this.RecreateCommandService();

            this.commandService.Start();

            var success = this.commandService.TryGetCommandFromArgumentText(
                                                                            "veh",
                                                                            true,
                                                                            out var potentialCommands,
                                                                            out var groupName,
                                                                            out var remainingArgumentText);

            success.Should().BeFalse();
            potentialCommands.Should()
                             .NotBeNull()
                             .And.HaveCount(1)
                             .And.ContainValues(firstCommand)
                             .And.ContainKeys("veh repair");

            groupName.Should().Be("veh");
            remainingArgumentText.Should().BeEmpty();
        }
    }
}
