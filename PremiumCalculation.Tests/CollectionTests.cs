using PremiumCalculation.Models;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace PremiumCalculation.Tests
{
    public class CollectionTests
    {
        private List<Group> Groups = new List<Group>();

        public CollectionTests()
        {
            this.Groups.Add(new Group()
            {
                Id = "Group1",
                Name = "Group1Name",
                Users = new List<User>() { new User() { Id = "User1", Name = "User1Name" }, new User() { Id = "User2", Name = "User2Name" } }
            });
            
            this.Groups.Add(new Group()
            {
                Id = "Group2",
                Name = "Group2Name",
                Users = new List<User>() { new User() { Id = "User1", Name = "User1Name" }, new User() { Id = "User3", Name = "User3Name" } }
            });
        }

        [Fact]
        public void CheckSingleGroup()
        {
            // Act
            var sut = this.Groups.FindAll(g => g.Id == "Group1");

            // Assert
            sut.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("User1", 2)]
        [InlineData("User2", 1)]
        [InlineData("User3", 1)]
        [InlineData("Userx", 0)]
        public void CheckSingleUser(string userId, int count)
        {
            // Act
            var sut = this.Groups.FindAll(g => g.Users.Exists(u => u.Id == userId));

            // Assert
            sut.Should().HaveCount(count);
        }
    }
}
