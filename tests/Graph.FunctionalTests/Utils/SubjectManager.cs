using Orca;

namespace CleanArchitecture.Graph.FunctionalTests;

public class SubjectManager
{
    private readonly ISubjectStore _subjectStore;

    public SubjectManager(ISubjectStore subjectStore)
    {
        _subjectStore = subjectStore;
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

        var subjectRoles = roles.Select(roleName => new Role
        {
            Name = roleName
        });

        await CreateEntriesAsync(subject, subjectRoles);

        return sub;
    }

    private async Task CreateEntriesAsync(Subject subject, IEnumerable<Role> roles)
    {
        var result = await _subjectStore.CreateAsync(subject);

        if (!result.Succeeded)
        {
            throw new ApplicationException("An error occurred while creating the user.");
        }

        foreach (var role in roles)
        {
            result = await _subjectStore.AddRoleAsync(subject, role);

            if (!result.Succeeded)
            {
                throw new ApplicationException("An error occurred while assigning the role.");
            }
        }
    }
}
