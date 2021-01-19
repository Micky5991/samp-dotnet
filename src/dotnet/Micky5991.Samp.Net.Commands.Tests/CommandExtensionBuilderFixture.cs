using System;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Interfaces;
using Micky5991.Samp.Net.Commands.Services;
using Micky5991.Samp.Net.Commands.Tests.Fakes;
using Micky5991.Samp.Net.Framework.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class CommandExtensionBuilderFixture
    {
        private readonly (Type Service, Type Implementation, Type FakeImplementation)[] services =
        {
            CreatePair<IExtensionStarter, CommandExtensionStarter, FakeCommandExtensionStarter>(),
            CreatePair<ICommandFactory, CommandFactory, FakeCommandFactory>(),
            CreatePair<ICommandService, CommandService, FakeCommandService>(),
        };

        private static (Type Service, Type Implementation, Type FakeImplementation) CreatePair<TService, TImplementation, TFake>()
            where TImplementation : TService
            where TFake : TService
        {
            return (typeof(TService), typeof(TImplementation), typeof(TFake));
        }

        [TestMethod]
        public void CommandExtensionBuilderAddsAllNeededServices()
        {
            var collection = new ServiceCollection();
            var builder = new CommandExtensionBuilder();

            builder.Register(collection);

            foreach (var (service, implementation, _) in this.services)
            {
                collection.Should()
                          .Contain(x => x.ServiceType == service && x.ImplementationType == implementation);
            }
        }

        [TestMethod]
        public void AddingProfileToBuilderRegistersToServiceCollection()
        {
            var builder = new CommandExtensionBuilder();

            builder.AddProfilesInAssembly<CommandExtensionBuilderFixture>();

            builder.ScannableAssemblies.Should().Contain(typeof(CommandExtensionBuilderFixture).Assembly);
        }

        [TestMethod]
        public void AddingServicesWillFailIfAlreadyRegistered()
        {
            var collection = new ServiceCollection();
            var builder = new CommandExtensionBuilder();

            foreach (var (service, _, fake) in this.services)
            {
                collection.AddSingleton(service, fake);
            }

            builder.Register(collection);

            foreach (var (service, implementation, fake) in this.services)
            {
                collection.Should()
                          .Contain(x => x.ServiceType == service && x.ImplementationType == fake)
                          .And.Contain(x => x.ServiceType != service && x.ImplementationType != implementation);
            }
        }
    }
}
