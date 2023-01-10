using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;
using NuGet.Configuration;
using NuGet.Protocol;
using Task = System.Threading.Tasks.Task;

namespace MsSqlAccessor.Hubs
{
    public class MsSQLHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private const string roles = "admin,user,paritcipant";
        private readonly CompetitionBdTestContext _context;
        //const string _header = "User";
        //const string str = typeof(Tmodel).Name;


        public MsSQLHub(CompetitionBdTestContext context)
        {
            _context = context;
            //_header = typeof(Tmodel).Name;
        }

        [HubMethodName("GetAll")]
        [Authorize(Roles = roles)]
        public async Task GetAll()
        {
            var dtoItems = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
                .Select(e => e.ConvertToDto<Tmodel, TmodelDTO>())
                .ToListAsync();

            await Clients.Caller.SendAsync("ReceiveGetAll", dtoItems);
        }

        [HubMethodName("GetOne")]
        [Authorize(Roles = "admin")]
        public async Task GetOne(int id)
        {
            var dbItem = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (dbItem == null)
            {
                throw new HubException(Errors.ItemNotFound);
            }
            var dtoItem = dbItem.ConvertToDto<Tmodel, TmodelDTO>();
            //await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveGetOne", dtoItem);
        }

        [HubMethodName("Update")]
        public async Task Update(int id, TmodelDTO dtoItem)
        {
            Tmodel dbItem;
            if (id != dtoItem.Id)
            {
                throw new HubException(Errors.BadRequest);
            }

            dbItem = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
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
                    throw new HubException(Errors.ItemNotFound);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            var dbItemResult = await _context.Set<Tmodel>().IncludeVirtualProperties(new Tmodel { }).FirstOrDefaultAsync(e => e.Id == id);
            TmodelDTO dtoItemResult = dbItemResult.ConvertToDto<Tmodel, TmodelDTO>();

            await Clients.Caller.SendAsync("ReceiveUpdate", dtoItemResult);
        }

        [HubMethodName("Create")]
        public async Task Create(TmodelDTO dtoItem)
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
                    throw new HubException(Errors.ConflictData);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            TmodelDTO dtoItemResult = dbItem.ConvertToDto<Tmodel, TmodelDTO>();

            await Clients.Caller.SendAsync("ReceiveCreate", dtoItemResult);
        }

        [HubMethodName("Delete")]
        public async Task Delete(int id)
        {
            var dbItem = await _context.Set<Tmodel>().FindAsync(id);
            if (dbItem == null)
            {
                throw new HubException(Errors.ItemNotFound);
            }

            dbItem.StatusId = (int)StatusEnm.NotActive;

            _context.Entry(dbItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!isItemExists(id))
                {
                    throw new HubException(Errors.ItemNotFound);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            var dbItemResult = await _context.Set<Tmodel>().IncludeVirtualProperties(new Tmodel { }).FirstOrDefaultAsync(e => e.Id == id);
            TmodelDTO dtoItemResult = dbItemResult.ConvertToDto<Tmodel, TmodelDTO>();

            await Clients.Caller.SendAsync("ReceiveDelete", dtoItemResult);
        }

        [HubMethodName("ForceDelete")]
        public async Task ForceDelete(int id)
        {
            var dbItem = await _context.Set<Tmodel>().FindAsync(id);
            if (dbItem == null)
            {
                throw new HubException(Errors.ItemNotFound);
            }

            _context.Set<Tmodel>().Remove(dbItem);
            await _context.SaveChangesAsync();

            await Clients.Caller.SendAsync("ReceiveDelete", new TmodelDTO());
            return;
        }

        //public override async Task OnConnectedAsync()
        //{
        //    var context = this.Context.GetHttpContext();
        //    // получаем кук name
        //    if (context.Request.Cookies.ContainsKey("name"))
        //    {
        //        string userName;
        //        if (context.Request.Cookies.TryGetValue("name", out userName))
        //        {
        //            Debug.WriteLine($"name = {userName}");
        //        }
        //    }

        //    // получаем юзер-агент
        //    Debug.WriteLine($"UserAgent = {context.Request.Headers["User-Agent"]}");
        //    // получаем ip
        //    Debug.WriteLine($"RemoteIpAddress = {context.Connection.RemoteIpAddress.ToString()}");

        //    await base.OnConnectedAsync();
        //}

        private bool isItemExists(int id)
        {
            return _context.Set<Tmodel>().Any(e => e.Id == id);
        }
    }
}
