using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;
using NuGet.Configuration;

namespace MsSqlAccessor.Hubs
{
    public class MsSQLHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private readonly CompetitionBdTestContext _context;
        //const string _header = "User";
        //const string str = typeof(Tmodel).Name;


        public MsSQLHub(CompetitionBdTestContext context)
        {
            _context = context;
            //_header = typeof(Tmodel).Name;
        }

        [HubMethodName("GetAll")]
        public async System.Threading.Tasks.Task GetAll()
        {
            var dtoItems = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Select(e => e.ConvertToDto<Tmodel, TmodelDTO>())
                .ToListAsync();

            await Clients.Caller.SendAsync("ReceiveGetAll", dtoItems);
        }

        [HubMethodName("GetOne")]
        public async System.Threading.Tasks.Task GetOne(int id)
        {
            var dbItem = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (dbItem == null)
            {
                await Clients.Caller.SendAsync("ReceiveGetOne", new TmodelDTO());
                return;
            }
            var dtoItem = dbItem.ConvertToDto<Tmodel, TmodelDTO>();

            await Clients.Caller.SendAsync("ReceiveGetOne", dtoItem);
        }

        [HubMethodName("Update")]
        public async System.Threading.Tasks.Task Update(int id, TmodelDTO dtoItem)
        {
            Tmodel dbItem;
            if (id != dtoItem.Id)
            {
                await Clients.Caller.SendAsync("ReceiveUpdate", "Bad Request");
                return;
            }

            dbItem = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .FirstOrDefaultAsync(e => e.Id == id);

            dbItem = dbItem.MakeChangesFromDto<Tmodel, TmodelDTO>(dtoItem);

            _context.Entry(dbItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!isItemExists(id))
                {
                    await Clients.Caller.SendAsync("ReceiveUpdate", new TmodelDTO());
                    return;
                }
                else
                {
                    throw;
                }
            }

            var dbItemResult = await _context.Set<Tmodel>().IncludeVirtualProperties(new Tmodel { }).FirstOrDefaultAsync(e => e.Id == id);
            TmodelDTO dtoItemResult = dbItemResult.ConvertToDto<Tmodel, TmodelDTO>();
            await Clients.Caller.SendAsync("ReceiveUpdate", dtoItemResult);
        }

        [HubMethodName("Create")]
        public async System.Threading.Tasks.Task Create(TmodelDTO dtoItem)
        {
            Tmodel dbItem = dtoItem.ConvertFromDto<Tmodel, TmodelDTO>();

            dbItem.StatusId = (int)StatusEnm.Active;

            _context.Set<Tmodel>().Add(dbItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (isItemExists(dbItem.Id))
                {
                    await Clients.Caller.SendAsync("ReceiveCreate", "Conflict");
                    return;
                }
                else
                {
                    throw;
                }
            }

            TmodelDTO dtoItemResult = dbItem.ConvertToDto<Tmodel, TmodelDTO>();

            await Clients.Caller.SendAsync("ReceiveCreate", dtoItemResult);
        }

        [HubMethodName("Delete")]
        public async System.Threading.Tasks.Task Delete(int id)
        {
            var dbItem = await _context.Set<Tmodel>().FindAsync(id);
            if (dbItem == null)
            {
                await Clients.Caller.SendAsync("ReceiveDelete", "not found");
                return;
            }

            _context.Set<Tmodel>().Remove(dbItem);
            await _context.SaveChangesAsync();

            await Clients.Caller.SendAsync("ReceiveDelete", new TmodelDTO());
            return;
        }

        private bool isItemExists(int id)
        {
            return _context.Set<Tmodel>().Any(e => e.Id == id);
        }
    }
}
