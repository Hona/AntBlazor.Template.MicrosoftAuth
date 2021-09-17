using AntBlazor.Template.MicrosoftAuth.Core.Repositories;
using Marten;
using Marten.Linq;

namespace AntBlazor.Template.MicrosoftAuth.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IDocumentStore Store;

    protected IMartenQueryable<T> Queryable(IQuerySession session) => session.Query<T>();

    protected GenericRepository(IDocumentStore store)
    {
        Store = store;
    }

    public async Task AddAsync(T model)
    {
        using var session = Store.LightweightSession();

        session.Insert(model);

        await session.SaveChangesAsync();
    }

    public async Task UpdateAsync(T model)
    {
        using var session = Store.LightweightSession();

        session.Update(model);

        await session.SaveChangesAsync();

    }

    public async Task AddOrUpdateAsync(T model)
    {
        using var session = Store.LightweightSession();

        session.Store(model);

        await session.SaveChangesAsync();
    }

    public async Task DeleteAsync(T model)
    {
        using var session = Store.LightweightSession();

        session.Delete(model);

        await session.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        using var session = Store.QuerySession();

        return await Queryable(session).ToListAsync();
    }
}