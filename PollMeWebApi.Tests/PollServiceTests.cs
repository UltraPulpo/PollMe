using FluentAssertions;
using PollMeWebApi.Models;
using PollMeWebApi.Services;
using System.Text.Json;

namespace PollMeWebApi.Tests
{
    public class PollServiceTests
    {
        private string _testFilePath;
        private PollService _sut;

        [SetUp]
        public void SetUp()
        {
            _testFilePath = Path.GetTempFileName();
            var mockJson = JsonSerializer.Serialize(new[]
            {
                new { Id = 1, Name = "poll-test-1", Description = "This is a test poll" },
                new { Id = 2, Name = "poll-test-2", Description = "This is another test poll" }
            });
            File.WriteAllText(_testFilePath, mockJson);

            _sut = new PollService(_testFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the temporary file
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Test]
        public void CreatePoll_ShouldAddNewPollToList()
        {
            // Arrange
            var newPoll = new Poll
            {
                Name = "New Poll",
                Description = "This is a new poll"
            };

            // Act
            var createdPoll = _sut.CreatePoll(newPoll);

            // Assert
            createdPoll.Should().NotBeNull("because the service should return the created poll");
            createdPoll.Id.Should().Be(3, "because it should be the next available ID");
            createdPoll.Name.Should().Be("New Poll3", "because the service appends the ID to the name");
            createdPoll.Description.Should().Be("This is a new poll");

            var polls = _sut.GetPolls();
            polls.Should().HaveCount(3, "because a new poll was added");
            polls[2].Should().BeEquivalentTo(createdPoll, "because the created poll should match the one in the list");
        }

        [Test]
        public void DeletePoll_ShouldRemovePollFromList()
        {
            // Arrange
            var pollIdToDelete = 1;

            // Act
            var isDeleted = _sut.DeletePoll(pollIdToDelete);

            // Assert
            isDeleted.Should().BeTrue("because the poll with the given ID exists and should be deleted");

            var polls = _sut.GetPolls();
            polls.Should().HaveCount(1, "because one poll was deleted");
            polls.Should().NotContain(p => p.Id == pollIdToDelete, "because the poll with the given ID should no longer exist");
        }

        [Test]
        public void GetPolls_ShouldReturnStaticPollList()
        {
            // Act
            List<Poll> polls = _sut.GetPolls();

            // Assert
            polls.Should().NotBeNull("because the service should return a valid list");
            polls.Should().HaveCount(2, "because we hard-coded two poll entries in the service");

            // Validate the content of the first poll.
            polls[0].Id.Should().Be(1);
            polls[0].Name.Should().Be("poll-test-1");
            polls[0].Description.Should().Be("This is a test poll");

            // Validate the content of the second poll.
            polls[1].Id.Should().Be(2);
            polls[1].Name.Should().Be("poll-test-2");
            polls[1].Description.Should().Be("This is another test poll");
        }
    }
}
