using System;
using FluentAssertions;
using Micky5991.Samp.Net.Commands.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Micky5991.Samp.Net.Commands.Tests
{
    [TestClass]
    public class CommandAttributeFixture
    {
        [TestMethod]
        public void CreatingAttributeWithNullNameThrowsException()
        {
            Action act = () => new CommandAttribute(null!);

            act.Should().Throw<ArgumentNullException>().WithMessage("*name*");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        public void CreatingAttributeWithInvalidNameThrowsException(string name)
        {
            Action act = () => new CommandAttribute(name);

            act.Should().Throw<ArgumentException>().WithMessage("*name*");
        }

        [TestMethod]
        public void CreatingCorrectNameAttributeSetsValuesCorrectly()
        {
            var attribute = new CommandAttribute("name");

            attribute.Name.Should()
                     .NotBeNull()
                     .And.Be("name");

            attribute.Group.Should()
                     .BeNullOrEmpty();

            attribute.Help.Should()
                     .BeNullOrEmpty();
        }

        [TestMethod]
        public void CreatingCorrectNameAndGroupAttributeSetsValuesCorrectly()
        {
            var attribute = new CommandAttribute("create", "veh");

            attribute.Name.Should()
                     .NotBeNull()
                     .And.Be("create");

            attribute.Group.Should().Be("veh");

            attribute.Help.Should()
                     .BeNullOrEmpty();
        }

        [TestMethod]
        public void CreatingCorrectNameGroupAndHelpAttributeSetsValuesCorrectly()
        {
            var attribute = new CommandAttribute("create", "veh")
            {
                Help = "Help me!"
            };

            attribute.Name.Should()
                     .NotBeNull()
                     .And.Be("create");

            attribute.Group.Should().Be("veh");

            attribute.Help.Should().Be("Help me!");
        }

        [TestMethod]
        public void ChangingHelpTextChangesItInAttribute()
        {
            var attribute = new CommandAttribute("create", "veh")
            {
                Help = "Help me!"
            };

            attribute.Help.Should().Be("Help me!");

            attribute.Help = "Or not";

            attribute.Help.Should().Be("Or not");
        }
    }
}
