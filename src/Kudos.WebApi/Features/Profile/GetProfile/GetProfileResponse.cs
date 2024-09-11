namespace Kudos.WebApi.Features.Profile.GetProfile;

public record GetProfileResponse(
    Guid Id,
    string FirstName,
    string LastName,
    int KudosCount,
    int SycoinCount);