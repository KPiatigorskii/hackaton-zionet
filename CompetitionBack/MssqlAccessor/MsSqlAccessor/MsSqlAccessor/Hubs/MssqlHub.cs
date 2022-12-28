using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MssqlHub<T> : Hub where T : class
{

    [HubMethodName("getAll")]
    public async Task<List<T>> GetAll()
    {
        using (var context = new CompetitionBdTestContext())
        {
            return await context.Set<T>().ToListAsync();
        }
    }

    [HubMethodName("create")]
    public async Task<T> Create(T item)
    {
        using (var context = new CompetitionBdTestContext())
        {
            context.Set<T>().Add(item);
            await context.SaveChangesAsync();
            return item;
        }
    }

    [HubMethodName("update")]
    public async Task<T> Update(T item)
    {
        using (var context = new CompetitionBdTestContext())
        {
            context.Set<T>().Update(item);
            await context.SaveChangesAsync();
            return item;
        }
    }

/*    [HubMethodName("delete")]
    public async Task<T> Delete(int id)
    {
        using (var context = new CompetitionBdTestContext())
        {
            var item = await context.Set<T>().FindAsync(id);
            context.Set<T>().Remove(item);
            await context.SaveChangesAsync();
        }
    }*/
}