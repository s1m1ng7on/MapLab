namespace MapLab.Services.Contracts
{
    public interface IProfileService
    {
        string? GetProfileId();
        bool IsAdmin();
    }
}
