using System.Data.Common;

namespace CleanArchitecture.Infrastructure.Data.Abstractions;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}
