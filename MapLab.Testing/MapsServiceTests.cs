using AutoMapper;
using MapLab.Data;
using MapLab.Data.Entities;
using MapLab.Data.Managers.Contracts;
using MapLab.Data.Models.Enums;
using MapLab.Data.Repositories;
using MapLab.Services;
using MapLab.Services.Contracts;
using MapLab.Services.Mapping;
using MapLab.Services.Models;
using MapLab.Shared.Models.FilterModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using System.Text;

[TestFixture]
public class MapsServiceTests
{
    private ApplicationDbContext _context;
    private MapsService _mapsService;
    private Mock<IFileStorageManager> _fileStorageManagerMock;
    private Mock<ITemplatesService> _mapTemplatesServiceMock;
    private Mock<IProfileService> _profileServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;

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
            new Claim(ClaimTypes.NameIdentifier, "profile1") // Default user ID for tests
        }, "TestAuthType"));
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(new DefaultHttpContext { User = user });

        // Initialize mocks
        _fileStorageManagerMock = new Mock<IFileStorageManager>();
        _mapTemplatesServiceMock = new Mock<ITemplatesService>();
        _profileServiceMock = new Mock<IProfileService>();
        _mapperMock = new Mock<IMapper>();

        // Initialize repositories with mocked IHttpContextAccessor
        var mapRepository = new DeletableEntityRepository<Map>(_context, _httpContextAccessorMock.Object);
        var mapTemplateRepository = new DeletableEntityRepository<MapTemplate>(_context, _httpContextAccessorMock.Object);
        var mapViewsRepository = new Repository<MapView>(_context, _httpContextAccessorMock.Object);
        var mapLikesRepository = new DeletableEntityRepository<Like<Map>>(_context, _httpContextAccessorMock.Object);

        // Initialize service
        _mapsService = new MapsService(
            mapRepository,
            mapTemplateRepository,
            mapViewsRepository,
            mapLikesRepository,
            _fileStorageManagerMock.Object,
            _mapTemplatesServiceMock.Object,
            _profileServiceMock.Object,
            _mapperMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private Map CreateMockMap(
        string id = "1",
        string name = "TestMap",
        string profileId = "profile1",
        bool isPublic = true,
        string mapTemplateId = "template1",
        MapTemplate? mapTemplate = null,
        DateTime? createdOn = null,
        string? filePath = null)
    {
        return new Map
        {
            Id = id,
            Name = name,
            ProfileId = profileId,
            IsPublic = isPublic,
            MapTemplateId = mapTemplateId,
            MapTemplate = mapTemplate ?? new MapTemplate { Id = mapTemplateId },
            CreatedOn = createdOn ?? DateTime.Now,
            FilePath = filePath,
            Views = new List<MapView>(),
            Likes = new List<Like<Map>>(),
            Profile = new MapLab.Data.Entities.Profile { Id = profileId }
        };
    }

    [Test]
    public void GetMapsForProfile_ShouldReturnFilteredMapsForCurrentProfile()
    {
        // Arrange
        var profileId = "profile1";
        var map1 = CreateMockMap(id: "1", profileId: profileId, createdOn: DateTime.Now);
        var map2 = CreateMockMap(id: "2", profileId: profileId, createdOn: DateTime.Now.AddDays(-1));
        _context.Maps.AddRange(map1, map2);
        _context.SaveChanges();

        var mapDtos = new List<MapDto> { new MapDto { Id = "1", Name = "TestMap" }, new MapDto { Id = "2", Name = "TestMap" } };
        _mapperMock.Setup(m => m.Map<IEnumerable<MapDto>>(It.IsAny<IQueryable<Map>>())).Returns(mapDtos);

        // Act
        var result = _mapsService.GetMapsForProfile(profileId, true, null);

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("TestMap", result.First().Name);
    }

    [Test]
    public void GetMapsForProfile_WithFilters_ShouldReturnFilteredMaps()
    {
        // Arrange
        var profileId = "profile1";
        var mapTemplate1 = new MapTemplate { Id = "template1", Region = Region.Europe };
        var mapTemplate2 = new MapTemplate { Id = "template2", Region = Region.Asia };
        var map1 = CreateMockMap(id: "1", profileId: profileId, mapTemplateId: "template1", mapTemplate: mapTemplate1);
        var map2 = CreateMockMap(id: "2", profileId: profileId, mapTemplateId: "template2", mapTemplate: mapTemplate2);
        _context.Maps.AddRange(map1, map2);
        _context.SaveChanges();

        var mapDtos = new List<MapDto> { new MapDto { Id = "1", Name = "TestMap", MapTemplate = new MapTemplateDto { Region = Region.Europe } } };
        _mapperMock.Setup(m => m.Map<IEnumerable<MapDto>>(It.IsAny<IQueryable<Map>>())).Returns(mapDtos);

        var filters = new MapFiltersModel { Region = Region.Europe };

        // Act
        var result = _mapsService.GetMapsForProfile(profileId, true, filters);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("TestMap", result.First().Name);
    }

    [Test]
    public async Task GetMapAsync_ShouldReturnMapDto()
    {
        // Arrange
        var mapId = "1";
        var map = CreateMockMap(id: mapId);
        _context.Maps.Add(map);
        _context.SaveChanges();

        var mapDto = new MapDto { Id = mapId, Name = "TestMap" };
        _mapperMock.Setup(m => m.Map<MapDto>(map)).Returns(mapDto);

        // Act
        var result = await _mapsService.GetMapAsync(mapId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(mapId, result.Id);
        Assert.AreEqual("TestMap", result.Name);
    }

    [Test]
    public async Task GetMapAsync_MapNotFound_ShouldReturnNull()
    {
        // Act
        var result = await _mapsService.GetMapAsync("nonexistent");

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task OpenMapAsync_ShouldReturnMapAndJson()
    {
        // Arrange
        var mapId = "1";
        var profileId = "profile1";
        var map = CreateMockMap(id: mapId, profileId: profileId, filePath: "path/to/map.json");
        _context.Maps.Add(map);
        _context.SaveChanges();

        var mapDto = new MapDto { Id = mapId, Name = "TestMap", FilePath = "path/to/map.json", MapTemplate = new MapTemplateDto() };
        _mapperMock.Setup(m => m.Map<MapDto>(map)).Returns(mapDto);
        _mapTemplatesServiceMock.Setup(t => t.GetMapTemplateJsonAsync(It.IsAny<MapTemplateDto>())).ReturnsAsync("templateJson");
        _fileStorageManagerMock.Setup(f => f.GetFileAsync(mapDto.FilePath)).ReturnsAsync(Encoding.UTF8.GetBytes("mapJson"));
        _profileServiceMock.Setup(p => p.GetProfileId()).Returns(profileId);

        // Act
        var (mapResult, templateJson, mapJson) = await _mapsService.OpenMapAsync(mapId);

        // Assert
        Assert.AreEqual(mapDto, mapResult);
        Assert.AreEqual("templateJson", templateJson);
        Assert.AreEqual("mapJson", mapJson);
        Assert.AreEqual(1, _context.MapViews.Count());
        Assert.AreEqual(mapId, _context.MapViews.First().MapId);
    }

    [Test]
    public async Task GetMapJsonAsync_FileNotFound_ShouldReturnEmptyJson()
    {
        // Arrange
        var mapDto = new MapDto { FilePath = "path/to/map.json", MapTemplate = new MapTemplateDto() };
        _mapTemplatesServiceMock.Setup(t => t.GetMapTemplateJsonAsync(It.IsAny<MapTemplateDto>())).ReturnsAsync("templateJson");
        _fileStorageManagerMock.Setup(f => f.GetFileAsync(mapDto.FilePath)).ThrowsAsync(new Exception());

        // Act
        var (templateJson, mapJson) = await _mapsService.GetMapJsonAsync(mapDto);

        // Assert
        Assert.AreEqual("templateJson", templateJson);
        Assert.IsTrue(mapJson.Contains("FeatureCollection"));
        Assert.IsTrue(mapJson.Contains("legend"));
        Assert.IsTrue(mapJson.Contains("features"));
    }

    [Test]
    public async Task CreateMapAsync_ShouldAddMap()
    {
        // Arrange
        var map = new MapDto
        {
            Name = "Test Map",
            MapTemplateId = "templateId",
            ProfileId = "profileId",
            IsPublic = true
        };

        // Act
        await _mapsService.CreateMapAsync(map);

        // Assert
        var savedMap = await _context.Maps
            .FirstOrDefaultAsync(m => m.Name == map.Name && m.ProfileId == map.ProfileId);

        Assert.NotNull(savedMap);
        Assert.AreEqual(map.Name, savedMap.Name);
        Assert.AreEqual(map.MapTemplateId, savedMap.MapTemplateId);
        Assert.AreEqual(map.IsPublic, savedMap.IsPublic);
    }

    [Test]
    public async Task EditMapAsync_ShouldUpdateMap()
    {
        // Arrange
        var mapId = "1";
        var profileId = "profile1";
        var map = CreateMockMap(id: mapId, profileId: profileId, name: "OldMap");
        _context.Maps.Add(map);
        _context.SaveChanges();

        var mapDto = new MapDto { Id = mapId, Name = "NewMap", MapTemplateId = "template1", ProfileId = profileId, IsPublic = true };
        _mapperMock.Setup(m => m.Map(mapDto, map)).Callback<MapDto, Map>((dto, m) => m.Name = dto.Name);
        _profileServiceMock.Setup(p => p.GetProfileId()).Returns(profileId);

        // Act
        await _mapsService.EditMapAsync(mapDto);

        // Assert
        var updatedMap = _context.Maps.Find(mapId);
        Assert.AreEqual("NewMap", updatedMap.Name);
    }

    [Test]
    public void EditMapAsync_MapNotFound_ShouldThrow()
    {
        // Arrange
        var mapDto = new MapDto { Id = "1", Name = "TestMap", MapTemplateId = "template1", ProfileId = "profile1", IsPublic = true };
        _profileServiceMock.Setup(p => p.GetProfileId()).Returns("profile1");

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _mapsService.EditMapAsync(mapDto));
    }

    [Test]
    public void EditMapAsync_Unauthorized_ShouldThrow()
    {
        // Arrange
        var mapId = "1";
        var map = CreateMockMap(id: mapId, profileId: "profile1");
        _context.Maps.Add(map);
        _context.SaveChanges();

        var mapDto = new MapDto { Id = mapId, Name = "NewMap", MapTemplateId = "template1", ProfileId = "profile1", IsPublic = true };
        _profileServiceMock.Setup(p => p.GetProfileId()).Returns("profile2");

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => _mapsService.EditMapAsync(mapDto));
    }

    [Test]
    public async Task DeleteMapAsync_ShouldDeleteMap()
    {
        // Arrange
        var mapId = "1";
        var profileId = "profile1";
        var map = CreateMockMap(id: mapId, profileId: profileId);
        _context.Maps.Add(map);
        _context.SaveChanges();

        _profileServiceMock.Setup(p => p.GetProfileId()).Returns(profileId);

        // Act
        await _mapsService.DeleteMapAsync(mapId);

        // Assert
        Assert.AreEqual(0, _context.Maps.Count());
    }

    [Test]
    public void DeleteMapAsync_MapNotFound_ShouldThrow()
    {
        // Arrange
        _profileServiceMock.Setup(p => p.GetProfileId()).Returns("profile1");

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _mapsService.DeleteMapAsync("nonexistent"));
    }

    [Test]
    public void DeleteMapAsync_Unauthorized_ShouldThrow()
    {
        // Arrange
        var mapId = "1";
        var map = CreateMockMap(id: mapId, profileId: "profile1");
        _context.Maps.Add(map);
        _context.SaveChanges();

        _profileServiceMock.Setup(p => p.GetProfileId()).Returns("profile2");

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => _mapsService.DeleteMapAsync(mapId));
    }

    [Test]
    public async Task SaveMapAsync_ShouldUpdateFilePath()
    {
        // Arrange
        var mapId = "1";
        var updatedMapJson = "updatedJson";
        var map = CreateMockMap(id: mapId, profileId: "profile1");
        _context.Maps.Add(map);
        _context.SaveChanges();

        _fileStorageManagerMock.Setup(f => f.SaveJsonFileAsync(updatedMapJson, "Maps", "File", mapId)).ReturnsAsync("new/path.json");

        // Act
        await _mapsService.SaveMapAsync(mapId, updatedMapJson);

        // Assert
        var updatedMap = _context.Maps.Find(mapId);
        Assert.AreEqual("new/path.json", updatedMap.FilePath);
    }

    [Test]
    public async Task UploadMapTemplateAsync_ShouldAddTemplate()
    {
        // Arrange
        var mapTemplate = new MapTemplate { Id = "1" };
        var fileMock = new Mock<IFormFile>();
        _fileStorageManagerMock.Setup(f => f.SaveFileAsync(fileMock.Object, "MapTemplates", "File", "1")).ReturnsAsync("template/path.json");

        // Act
        await _mapsService.UploadMapTemplateAsync(mapTemplate, fileMock.Object);

        // Assert
        Assert.AreEqual(1, _context.MapTemplates.Count());
        Assert.AreEqual("template/path.json", _context.MapTemplates.First().FilePath);
    }

    [Test]
    public async Task ToggleLikeDislikeMapAsync_LikeExists_ShouldRemoveLike()
    {
        // Arrange
        var profileId = "profile1";
        var mapId = "1";
        var like = new Like<Map> { ProfileId = profileId, EntityId = mapId };
        _context.MapLikes.Add(like);
        _context.SaveChanges();

        // Act
        var (likesCount, isLiked) = await _mapsService.ToggleLikeDislikeMapAsync(profileId, mapId);

        // Assert
        Assert.AreEqual(0, _context.MapLikes.Count());
        Assert.AreEqual(0, likesCount);
        Assert.IsFalse(isLiked);
    }

    [Test]
    public async Task ToggleLikeDislikeMapAsync_NoLikeExists_ShouldAddLike()
    {
        // Arrange
        var profileId = "profile1";
        var mapId = "1";

        // Act
        var (likesCount, isLiked) = await _mapsService.ToggleLikeDislikeMapAsync(profileId, mapId);

        // Assert
        Assert.AreEqual(1, _context.MapLikes.Count());
        Assert.AreEqual(1, likesCount);
        Assert.IsTrue(isLiked);
        Assert.AreEqual(mapId, _context.MapLikes.First().EntityId);
        Assert.AreEqual(profileId, _context.MapLikes.First().ProfileId);
    }
}