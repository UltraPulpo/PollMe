using FluentAssertions;
using PollMeWebApi.Models;
using PollMeWebApi.Services;
using System.Text.Json;

namespace PollMeWebApi.Tests
{
    public class PollServiceTests
    {
        private string _testFilePath;
        private PollService _pollService;

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

            _pollService = new PollService(_testFilePath);
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
        public void GeneratePolls_ShouldReturnStaticPollList()
        {
            // Act
            List<Poll> polls = _pollService.GeneratePolls();

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
