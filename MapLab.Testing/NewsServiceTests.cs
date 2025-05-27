using AutoMapper;
using MapLab.Data;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Repositories;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Testing.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using System.Text;
using Profile = MapLab.Data.Entities.Profile;

namespace MapLab.Testing
{
    [TestFixture]
    public class NewsServiceTests
    {
        private NewsService _newsService;
        private IMapper _mapper;
        private DbContextOptions<ApplicationDbContext> _dbOptions;
        private ApplicationDbContext _context;
        private IDeletableEntityRepository<NewsArticle> _newsRepo;
        private Mock<IProfileService> _mockProfileService;
        private Mock<IFileStorageManager> _mockFileStorageManager;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            AutoMapperConfiguration.RegisterMappings(null, AppDomain.CurrentDomain.GetAssemblies());
            _mapper = AutoMapperConfiguration.MapperInstance;
        }

        [SetUp]
        public void Setup()
        {
            _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "NewsDb_" + Guid.NewGuid())
                .Options;

            _context = new ApplicationDbContext(_dbOptions);

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "profile-1")
            }, "mock"));
            _mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(httpContext);

            _newsRepo = new DeletableEntityRepository<NewsArticle>(_context, _mockHttpContextAccessor.Object);
            _mockProfileService = new Mock<IProfileService>();
            _mockProfileService.Setup(x => x.GetProfileId()).Returns("profile-1");

            _mockFileStorageManager = new Mock<IFileStorageManager>();

            _newsService = new NewsService(
                _newsRepo,
                _mapper,
                _mockProfileService.Object,
                _mockFileStorageManager.Object
            );
        }

        [Test]
        public async Task CreateNewsArticleAsync_SavesArticle_WithBase64ImageConversion()
        {
            var base64Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAU=";
            var dto = new NewsArticleDto
            {
                Title = "Test",
                Content = $"<p><img src=\"{base64Image}\" /></p>",
                Thumbnail = FormFileHelper.Create("thumbnail.png", "image/png", Encoding.UTF8.GetBytes("fake-image"))
            };

            _mockFileStorageManager
                .Setup(x => x.SaveFileAsync(It.IsAny<IFormFile>(), "News", "Thumbnails", It.IsAny<string>()))
                .ReturnsAsync("/path/to/thumbnail.png");

            _mockFileStorageManager
                .Setup(x => x.SaveFileAsync(It.IsAny<string>(), "News", "Articles", It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("/path/to/saved/image.png");

            await _newsService.CreateNewsArticleAsync(dto);

            var saved = await _context.News.FirstOrDefaultAsync();
            Assert.IsNotNull(saved, "Article should be saved in DB.");
            Assert.Multiple(() =>
            {
                Assert.That(saved.Content, Does.Contain("/path/to/saved/image.png"), "Image URL should be replaced.");
                Assert.AreEqual("/path/to/thumbnail.png", saved.ThumbnailFilePath, "Thumbnail path mismatch.");
            });

            _mockFileStorageManager.Verify(x => x.SaveFileAsync(It.IsAny<IFormFile>(), "News", "Thumbnails", It.IsAny<string>()), Times.Once);
            _mockFileStorageManager.Verify(x => x.SaveFileAsync(It.IsAny<string>(), "News", "Articles", It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetNewsAsync_ReturnsPaginatedResult()
        {
            await AddProfileAsync("profile-1");
            await AddProfileAsync("profile-2");

            await _newsRepo.AddAsync(new NewsArticle
            {
                Title = "A",
                Content = "Content A",
                ProfileId = "profile-1",
                CreatedOn = DateTime.UtcNow
            });

            await _newsRepo.AddAsync(new NewsArticle
            {
                Title = "B",
                Content = "Content B",
                ProfileId = "profile-2",
                CreatedOn = DateTime.UtcNow
            });

            await _newsRepo.SaveChangesAsync();

            var result = await _newsService.GetNewsAsync(1, 10);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, result.TotalCount);
                Assert.AreEqual(2, result.Items.Count);
            });
        }

        [Test]
        public async Task GetNewsArticleAsync_ReturnsCorrectArticle()
        {
            await AddProfileAsync("profile-1");

            await _newsRepo.AddAsync(new NewsArticle
            {
                Id = "test-id",
                Title = "Sample",
                Content = "Sample content",
                ProfileId = "profile-1",
                CreatedOn = DateTime.UtcNow
            });
            await _newsRepo.SaveChangesAsync();

            var result = await _newsService.GetNewsArticleAsync("test-id");

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual("Sample", result.Title);
                Assert.AreEqual("Sample content", result.Content);
            });
        }

        [TestCase("profile-1", true)]
        [TestCase("wrong-profile", false)]
        public async Task EditNewsArticleAsync_HandlesAuthorization(string userProfileId, bool shouldSucceed)
        {
            await SeedArticleAsync("edit-id", "profile-1");

            _mockProfileService.Setup(x => x.GetProfileId()).Returns(userProfileId);

            var dto = new NewsArticleDto
            {
                Id = "edit-id",
                Title = "Updated Title",
                Content = "Updated Content"
            };

            if (shouldSucceed)
            {
                Assert.DoesNotThrowAsync(async () =>
                    await _newsService.EditNewsArticleAsync(dto, "")
                );
            }
            else
            {
                Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                    _newsService.EditNewsArticleAsync(dto, "")
                );
            }
        }

        [Test]
        public async Task DeleteNewsArticleAsync_DeletesCorrectly()
        {
            await SeedArticleAsync("to-delete", "profile-1");

            await _newsService.DeleteNewsArticleAsync("to-delete");

            var check = await _context.News.FirstOrDefaultAsync(x => x.Id == "to-delete");
            Assert.IsNull(check, "Article should be deleted.");
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private async Task AddProfileAsync(string id)
        {
            await _context.Users.AddAsync(new Profile { Id = id });
            await _context.SaveChangesAsync();
        }

        private async Task SeedArticleAsync(string id, string profileId)
        {
            await _newsRepo.AddAsync(new NewsArticle
            {
                Id = id,
                Title = "Test Title",
                Content = "Test Content",
                ProfileId = profileId
            });
            await _newsRepo.SaveChangesAsync();
        }
    }
}
