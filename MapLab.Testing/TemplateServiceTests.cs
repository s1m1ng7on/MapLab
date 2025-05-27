using AutoMapper;
using AutoMapper.QueryableExtensions;
using MapLab.Data;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Models.Enums;
using MapLab.Data.Repositories;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Shared.Areas.Admin.Models;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Profile = MapLab.Data.Entities.Profile;

[TestFixture]
public class TemplatesServiceTests
{
    private ApplicationDbContext _context;
    private TemplatesService _templatesService;
    private Mock<IFileStorageManager> _fileStorageManagerMock;
    private Mock<IProfileService> _profileServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IMemoryCache> _memoryCacheMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private IMapper _mapper;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        AutoMapperConfiguration.RegisterMappings(null, AppDomain.CurrentDomain.GetAssemblies());
        _mapper = AutoMapperConfiguration.MapperInstance;
    }

    [SetUp]
    public void Setup()
    {
        // Initialize in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
            .Options;

        _context = new ApplicationDbContext(options);

        // Mock IHttpContextAccessor
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "profile1") // Default user ID
        }, "TestAuthType"));
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(new DefaultHttpContext { User = user });

        // Initialize mocks
        _fileStorageManagerMock = new Mock<IFileStorageManager>();
        _profileServiceMock = new Mock<IProfileService>();
        _mapperMock = new Mock<IMapper>();
        _memoryCacheMock = new Mock<IMemoryCache>();

        // Initialize repository
        var mapTemplateRepository = new DeletableEntityRepository<MapTemplate>(_context, _httpContextAccessorMock.Object);

        // Initialize service
        _templatesService = new TemplatesService(
            mapTemplateRepository,
            _mapperMock.Object,
            _profileServiceMock.Object,
            _memoryCacheMock.Object,
            _fileStorageManagerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private MapTemplate CreateMockMapTemplate(
        string id = "template1",
        string name = "TestTemplate",
        string profileId = "profile1",
        string filePath = "template/path.json",
        Region region = Region.World,
        string userName = "MapLab",
        DateTime? createdOn = null,
        Profile? profile = null)
    {
        return new MapTemplate
        {
            Id = id,
            Name = name,
            ProfileId = profileId,
            FilePath = filePath,
            Region = region,
            Profile = profile ?? new Profile { Id = profileId, UserName = userName },
            CreatedOn = createdOn ?? DateTime.Now,
            Maps = new List<Map>(),
            Likes = new List<Like<MapTemplate>>()
        };
    }

    [Test]
    public async Task GetMapTemplateAsync_ShouldReturnMapTemplateDto()
    {
        // Arrange
        var templateId = "template1";
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate = CreateMockMapTemplate(id: templateId, profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.Add(mapTemplate);
        _context.SaveChanges();

        var mapTemplateDto = new MapTemplateDto { Id = templateId, Name = "TestTemplate" };
        _mapperMock.Setup(m => m.Map<MapTemplateDto>(mapTemplate)).Returns(mapTemplateDto);

        // Act
        var result = await _templatesService.GetMapTemplateAsync(templateId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(templateId, result.Id);
        Assert.AreEqual("TestTemplate", result.Name);
    }

    [Test]
    public async Task GetMapTemplateAsync_NotFound_ShouldReturnNull()
    {
        // Act
        var result = await _templatesService.GetMapTemplateAsync("nonexistent");

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void GetMapTemplates_NoFilters_ShouldReturnAllTemplates()
    {
        // Arrange
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", profile: profile);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        // NO mock setup for _mapper.ProjectTo anymore!

        // Act
        var result = _templatesService.GetMapTemplates(null);

        // Assert
        Assert.AreEqual(2, result.Items.Count());
        Assert.AreEqual("template1", result.Items.First().Id);
    }

    [Test]
    public void GetMapTemplates_WithSearchQuery_ShouldFilterByName()
    {
        // Arrange
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", name: "Europe Map", region: Region.Europe, profile: profile);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", name: "Asia Map", region: Region.Asia, profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        var mapTemplateDtos = new List<MapTemplateDto> { new MapTemplateDto { Id = "template1", Name = "Europe Map" } };
        _mapperMock.Setup(m => m.ProjectTo<MapTemplateDto>(It.IsAny<IQueryable<MapTemplate>>(), null, It.IsAny<string[]>()))
            .Returns(mapTemplateDtos.AsQueryable());

        var filters = new MapTemplateFiltersModel { SearchQuery = "Europe" };

        // Act
        var result = _templatesService.GetMapTemplates(filters);

        // Assert
        Assert.AreEqual(1, result.Items.Count());
        Assert.AreEqual("Europe Map", result.Items.First().Name);
    }

    [Test]
    public void GetMapTemplates_WithRegionFilter_ShouldFilterByRegion()
    {
        // Arrange
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", region: Region.Europe, profile: profile);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", region: Region.Asia, profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        var mapTemplateDtos = new List<MapTemplateDto> { new MapTemplateDto { Id = "template1", Region = Region.Europe } };
        _mapperMock.Setup(m => m.ProjectTo<MapTemplateDto>(It.IsAny<IQueryable<MapTemplate>>(), null, It.IsAny<string[]>()))
            .Returns(mapTemplateDtos.AsQueryable());

        var filters = new MapTemplateFiltersModel { Region = Region.Europe };

        // Act
        var result = _templatesService.GetMapTemplates(filters);

        // Assert
        Assert.AreEqual(1, result.Items.Count());
        Assert.AreEqual(Region.Europe, result.Items.First().Region);
    }

    [Test]
    public void GetMapTemplates_WithByMapLabFilter_ShouldFilterByUserName()
    {
        // Arrange
        var profile1 = new Profile { Id = "profile1", UserName = "MapLab" };
        var profile2 = new Profile { Id = "profile2", UserName = "OtherUser" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", profile: profile1);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", profileId: "profile2", userName: "OtherUser", profile: profile2);
        _context.Users.AddRange(profile1, profile2);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        var mapTemplateDtos = new List<MapTemplateDto> { new MapTemplateDto { Id = "template1" } };
        _mapperMock.Setup(m => m.ProjectTo<MapTemplateDto>(It.IsAny<IQueryable<MapTemplate>>(), null, It.IsAny<string[]>()))
            .Returns(mapTemplateDtos.AsQueryable());

        var filters = new MapTemplateFiltersModel { ByMapLab = true };

        // Act
        var result = _templatesService.GetMapTemplates(filters);

        // Assert
        Assert.AreEqual(1, result.Items.Count());
        Assert.AreEqual("template1", result.Items.First().Id);
    }

    [Test]
    public void GetMapTemplates_WithAdminFilters_ShouldFilterByDateRange()
    {
        // Arrange
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", createdOn: DateTime.Now.AddDays(-1), region: Region.Europe, profile: profile);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", createdOn: DateTime.Now.AddDays(-10), region: Region.Asia, profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        var mapTemplateDtos = new List<MapTemplateDto> { new MapTemplateDto { Id = "template1" } };
        _mapperMock.Setup(m => m.ProjectTo<MapTemplateDto>(It.IsAny<IQueryable<MapTemplate>>(), null, It.IsAny<string[]>()))
            .Returns(mapTemplateDtos.AsQueryable());

        var filters = new AdminTemplateFiltersModel
        {
            From = DateTime.Now.AddDays(-5),
            To = DateTime.Now
        };

        // Act
        var result = _templatesService.GetMapTemplates(filters);

        // Assert
        Assert.AreEqual(1, result.Items.Count());
        Assert.AreEqual("template1", result.Items.First().Id);
    }

    [Test]
    public void GetRecentMapTemplates_ShouldReturnRecentTemplatesForProfile()
    {
        // Arrange
        var profileId = "profile1";
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", createdOn: DateTime.Now, region: Region.Europe, profile: profile);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", createdOn: DateTime.Now.AddDays(-1), region: Region.Asia, profile: profile);
        mapTemplate1.Maps.Add(new Map { Id = "map1", ProfileId = profileId, MapTemplateId = "template1" });
        _context.Users.Add(profile);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        var mapTemplateDtos = new List<MapTemplateDto> { new MapTemplateDto { Id = "template1" } };
        _mapperMock.Setup(m => m.ProjectTo<MapTemplateDto>(It.IsAny<IQueryable<MapTemplate>>(), null, It.IsAny<string[]>()))
            .Returns(mapTemplateDtos.AsQueryable());
        _profileServiceMock.Setup(p => p.GetProfileId()).Returns(profileId);

        // Act
        var result = _templatesService.GetRecentMapTemplates();

        // Assert
        Assert.AreEqual(1, result.Items.Count());
        Assert.AreEqual("template1", result.Items.First().Id);
    }

    [Test]
    public async Task GetFeaturedMapTemplates_ShouldReturnCachedTemplates()
    {
        // Arrange
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate1 = CreateMockMapTemplate(id: "template1", region: Region.Europe, profile: profile);
        var mapTemplate2 = CreateMockMapTemplate(id: "template2", region: Region.Asia, profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.AddRange(mapTemplate1, mapTemplate2);
        _context.SaveChanges();

        var mapTemplateDtos = new List<MapTemplateDto>
        {
            new MapTemplateDto { Id = "template1" },
            new MapTemplateDto { Id = "template2" }
        };
        object cachedValue = mapTemplateDtos;
        _memoryCacheMock.Setup(m => m.TryGetValue("FeaturedMapTemplates", out cachedValue)).Returns(false);
        _mapperMock.Setup(m => m.Map<IEnumerable<MapTemplateDto>>(It.IsAny<List<MapTemplate>>()))
            .Returns(mapTemplateDtos);

        // Act
        var result = await _templatesService.GetFeaturedMapTemplates();

        // Assert
        Assert.AreEqual(2, result.Items.Count());
        Assert.AreEqual("template1", result.Items.First().Id);
        _memoryCacheMock.Verify(m => m.Set("FeaturedMapTemplates", mapTemplateDtos, It.IsAny<MemoryCacheEntryOptions>()), Times.Once());
    }

    [Test]
    public async Task GetFeaturedMapTemplates_FromCache_ShouldReturnCachedTemplates()
    {
        // Arrange
        var mapTemplateDtos = new List<MapTemplateDto>
        {
            new MapTemplateDto { Id = "template1" },
            new MapTemplateDto { Id = "template2" }
        };
        object cachedValue = mapTemplateDtos;
        _memoryCacheMock.Setup(m => m.TryGetValue("FeaturedMapTemplates", out cachedValue)).Returns(true);

        // Act
        var result = await _templatesService.GetFeaturedMapTemplates();

        // Assert
        Assert.AreEqual(2, result.Items.Count());
        Assert.AreEqual("template1", result.Items.First().Id);
        _memoryCacheMock.Verify(m => m.TryGetValue("FeaturedMapTemplates", out It.Ref<object>.IsAny), Times.Once());
    }

    [Test]
    public async Task GetMapTemplateJsonAsync_ShouldReturnJson()
    {
        // Arrange
        var mapTemplateDto = new MapTemplateDto { FilePath = "template/path.json" };
        _fileStorageManagerMock.Setup(f => f.GetFileAsync(mapTemplateDto.FilePath))
            .ReturnsAsync(Encoding.UTF8.GetBytes("templateJson"));

        // Act
        var result = await _templatesService.GetMapTemplateJsonAsync(mapTemplateDto);

        // Assert
        Assert.AreEqual("templateJson", result);
    }

    [Test]
    public void GetMapTemplateJsonAsync_FileNotFound_ShouldThrow()
    {
        // Arrange
        var mapTemplateDto = new MapTemplateDto { FilePath = "template/path.json" };
        _fileStorageManagerMock.Setup(f => f.GetFileAsync(mapTemplateDto.FilePath))
            .ThrowsAsync(new FileNotFoundException());

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _templatesService.GetMapTemplateJsonAsync(mapTemplateDto));
    }

    [Test]
    public async Task UploadMapTemplateAsync_ShouldAddTemplate()
    {
        // Arrange
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplateDto = new MapTemplateDto { Id = "template1", Name = "TestTemplate", ProfileId = "profile1", FilePath = "template/path.json" };
        var mapTemplate = CreateMockMapTemplate(id: "template1", profile: profile);
        _mapperMock.Setup(m => m.Map<MapTemplate>(mapTemplateDto)).Returns(mapTemplate);
        var fileMock = new Mock<IFormFile>();
        _fileStorageManagerMock.Setup(f => f.SaveFileAsync(fileMock.Object, "MapTemplates", "File", "template1"))
            .ReturnsAsync("template/path.json");
        _context.Users.Add(profile);
        _context.SaveChanges();

        // Act
        await _templatesService.UploadMapTemplateAsync(mapTemplateDto, fileMock.Object);

        // Assert
        Assert.AreEqual(1, _context.MapTemplates.Count());
        Assert.AreEqual("template/path.json", _context.MapTemplates.First().FilePath);
        Assert.AreEqual("TestTemplate", _context.MapTemplates.First().Name);
    }

    [Test]
    public async Task DeleteMapTemplateAsync_ShouldDeleteTemplate()
    {
        // Arrange
        var templateId = "template1";
        var profile = new Profile { Id = "profile1", UserName = "MapLab" };
        var mapTemplate = CreateMockMapTemplate(id: templateId, profile: profile);
        _context.Users.Add(profile);
        _context.MapTemplates.Add(mapTemplate);
        _context.SaveChanges();

        // Act
        await _templatesService.DeleteMapTemplateAsync(templateId);

        // Assert
        Assert.AreEqual(0, _context.MapTemplates.Count());
    }

    [Test]
    public void DeleteMapTemplateAsync_NotFound_ShouldThrow()
    {
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _templatesService.DeleteMapTemplateAsync("nonexistent"));
    }
}