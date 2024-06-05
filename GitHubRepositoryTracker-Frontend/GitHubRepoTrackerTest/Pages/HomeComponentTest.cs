
using GitHubRepoTrackerFE_Blazor.Interfaces;
using GitHubRepoTrackerFE_Blazor.Models;
using GitHubRepoTrackerFE_Blazor.Pages;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GitHubRepoTrackerTest.Pages

{
    public class HomeComponentTest : TestContext
    {

        /// <summary>
        /// Tests the RepoService,LanguageService and TopicService injection
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task TestServiceInjection()
        {

            //Arrange

            var mockRepoService = new Mock<IRepoService>();
            mockRepoService.Setup(service => service.GetAllRepos())
                           .ReturnsAsync(new List<Repository>());

            var mockLanguageService = new Mock<ILanguageService>();
            mockLanguageService.Setup(service => service.GetAllLanguages())
                               .ReturnsAsync(new List<Language>());

            var mockTopicService = new Mock<ITopicService>();
            mockTopicService.Setup(service => service.GetAllTopics())
                            .ReturnsAsync(new List<Topic>());

            Services.AddSingleton(mockRepoService.Object);
            Services.AddSingleton(mockLanguageService.Object);
            Services.AddSingleton(mockTopicService.Object);

            //Act

            var cut = RenderComponent<Home>();

            //Assert
            Assert.NotNull(cut.Instance.RepositoryListModel.languages);
            Assert.NotNull(cut.Instance.RepositoryListModel.topics);
            Assert.NotNull(cut.Instance.RepositoryListModel.repositories);


        }

        /// <summary>
        /// Test the filter by topic. If the filter is correct, the repositories on the table should only be 2
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task FilterByTopic_ShouldFilterRepositoriesCorrectly()
        {
            // Arrange
            var mockRepoService = new Mock<IRepoService>();
            var mockLanguageService = new Mock<ILanguageService>();
            var mockTopicService = new Mock<ITopicService>();

            var topics = new List<Topic>
            {
                new Topic { topicName = "Topic1" },
                new Topic { topicName = "Topic2" },
                new Topic { topicName = "Topic3" },
                new Topic { topicName = "Topic4" }
            };

            var repositories = new List<Repository>
            {
                new Repository
                {
                    repositoryName = "Repo1",
                    description = "Description for Repo1",
                    url = "https://github.com/repo1",
                    language = new Language { languageName = "C#" },
                    repositoryTopics = new Topic[]
                    {
                        new Topic { topicName = "Topic1" },
                        new Topic { topicName = "Topic2" }
                    },
                    updatedAt = DateTime.Now.AddDays(-1),
                    forksCount = 10,
                    stargazersCount = 20
                },

                new Repository
                {
                repositoryName = "Repo2",
                description = "Description for Repo2",
                url = "https://github.com/repo2",
                language = new Language { languageName = "JavaScript" },
                repositoryTopics = new Topic[]
                {
                    new Topic { topicName = "Topic1" },
                    new Topic { topicName = "Topic4" }
                },
                updatedAt = DateTime.Now.AddDays(-2),
                forksCount = 15,
                stargazersCount = 25
                },
                new Repository
                {
                repositoryName = "Repo3",
                description = "Description for Repo3",
                url = "https://github.com/rep32",
                language = new Language { languageName = "JavaScript" },
                repositoryTopics = new Topic[]
                {
                    new Topic { topicName = "Topic3" },
                    new Topic { topicName = "Topic4" }
                },
                updatedAt = DateTime.Now.AddDays(-2),
                forksCount = 19,
                stargazersCount = 20
                }
            };


            mockRepoService.Setup(service => service.GetAllRepos()).ReturnsAsync(repositories);
            mockLanguageService.Setup(service => service.GetAllLanguages()).ReturnsAsync(new List<Language>());
            mockTopicService.Setup(service => service.GetAllTopics()).ReturnsAsync(topics);

            Services.AddSingleton(mockRepoService.Object);
            Services.AddSingleton(mockLanguageService.Object);
            Services.AddSingleton(mockTopicService.Object);

            var cut = RenderComponent<Home>();

            // Act
            var topicDropdown = cut.Find("#select_topic");
            topicDropdown.Change("Topic1"); // Simulate selecting Topic1

            // Assert
            cut.WaitForState(() => cut.FindAll("tbody tr").Count == 2); // Wait until rendering is complete
            Assert.Equal(2, cut.FindAll("tbody tr").Count); // Ensure that only repositories with Topic1 are displayed
        }


    }
}