using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Graph.Schema.Shared.Objects;

public class AuditabilityType : ObjectType<Auditability>
{
    protected override void Configure(IObjectTypeDescriptor<Auditability> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(x => x.Created);

        descriptor.Field(x => x.CreatedBy);

        descriptor.Field(x => x.LastModified);

        descriptor.Field(x => x.LastModifiedBy);
    }
}
