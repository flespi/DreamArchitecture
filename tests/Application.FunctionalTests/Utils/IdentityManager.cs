using Orca;

namespace CleanArchitecture.Application.FunctionalTests;

public class IdentityManager
{
    private readonly IOrcaStoreAccessor _storeAccessor;

    public IdentityManager(IOrcaStoreAccessor storeAccessor)
    {
        _storeAccessor = storeAccessor;
    }

    public async Task<string> CreateSubjectAsync(string userName, string[] roles)
    {
        var sub = Guid.NewGuid().ToString();

        var subject = new Subject
        {
            Sub = sub,
            Name = userName,
            Email = userName
        };

        var result = await _storeAccessor.SubjectStore.CreateAsync(subject);

        if (!result.Succeeded)
        {
            throw new ApplicationException("An error occurred while creating the user.");
        }

        foreach (var roleName in roles)
        {
            var role = await _storeAccessor.RoleStore.FindByNameAsync(roleName);

            result = await _storeAccessor.SubjectStore.AddRoleAsync(subject, role);

            if (!result.Succeeded)
            {
                throw new ApplicationException("An error occurred while assigning the role.");
            }
        }

        return sub;
    }
}
