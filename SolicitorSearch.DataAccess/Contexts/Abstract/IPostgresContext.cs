using Microsoft.EntityFrameworkCore;
using SolicitorSearch.DataAccess.Entities;

namespace SolicitorSearch.DataAccess.Contexts.Abstract
{
    public interface IPostgresContext
    {
        DbSet<SolicitorEntity> Solicitors { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
