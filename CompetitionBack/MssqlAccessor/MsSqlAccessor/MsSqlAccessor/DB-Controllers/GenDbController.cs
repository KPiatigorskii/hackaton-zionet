using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MsSqlAccessor;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Hubs;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;
using Newtonsoft.Json.Linq;

namespace MsSqlAccessor.DbControllers
{
    public class GenDbController<Tmodel, TmodelDTO> where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private readonly CompetitionBdTestContext _context;

        public GenDbController(CompetitionBdTestContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TmodelDTO>> GetAll()
        {
            var dtoItems = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
                .Select(e => e.ConvertToDto<Tmodel, TmodelDTO>())
                .ToListAsync();

            return dtoItems;
        }

        public async Task<TmodelDTO> GetOne(int id)
        {
            var dbItem = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (dbItem == null)
            {
                throw new Exception(Errors.ItemNotFound);
            }

            var dtoItem = dbItem.ConvertToDto<Tmodel, TmodelDTO>();

            return dtoItem;
        }

        public async Task<TmodelDTO> Update(int id, TmodelDTO dtoItem, string userEmail)
        {
            int userId = await GetUserIdByEmail(userEmail);

            if (id != dtoItem.Id)
            {
                throw new Exception(Errors.BadRequest);
            }

            var dbItem = await _context.Set<Tmodel>()
                .IncludeVirtualProperties(new Tmodel { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (dbItem == null)
            {
                throw new Exception(Errors.ItemNotFound);
            }

            dbItem = dbItem.MakeChangesFromDto<Tmodel, TmodelDTO>(dtoItem);
            dbItem.GetType().GetProperty("UpdateDate")?.SetValue(dbItem, DateTime.Now);
            dbItem.GetType().GetProperty("UpdateUserId")?.SetValue(dbItem, userId);
            //if (dbItem.GetType().GetProperty("UpdateDate") != null)
            //{
            //    dbItem.GetType().GetProperty("UpdateDate")?.SetValue(dbItem, DateTime.Now);
            //}

            _context.Entry(dbItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(Errors.General);
            }

            var dbItemResult = await _context.Set<Tmodel>().IncludeVirtualProperties(new Tmodel { }).FirstOrDefaultAsync(e => e.Id == id);
            TmodelDTO dtoItemResult = dbItemResult.ConvertToDto<Tmodel, TmodelDTO>();

            return dtoItemResult;
        }

        public async Task<TmodelDTO> Create(TmodelDTO dtoItem, string userEmail)
        {
            int userId = await GetUserIdByEmail(userEmail);

            Tmodel dbItem = dtoItem.ConvertFromDto<Tmodel, TmodelDTO>();

            dbItem.StatusId = (int)StatusEnm.Active;
            dbItem.GetType().GetProperty("CreateDate")?.SetValue(dbItem, DateTime.Now);
            dbItem.GetType().GetProperty("UpdateDate")?.SetValue(dbItem, DateTime.Now);
            dbItem.GetType().GetProperty("CreateUserId")?.SetValue(dbItem, userId);
            dbItem.GetType().GetProperty("UpdateUserId")?.SetValue(dbItem, userId);

            _context.Set<Tmodel>().Add(dbItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (isItemExists(dbItem.Id))
                {
                    throw new Exception(Errors.ConflictData);
                }
                else
                {
                    throw new Exception(Errors.General);
                }
            }

            TmodelDTO dtoItemResult = dbItem.ConvertToDto<Tmodel, TmodelDTO>();

            return dtoItemResult;
        }

        public async Task<TmodelDTO> Delete(int id, string userEmail)
        {
            int userId = await GetUserIdByEmail(userEmail);

            var dbItem = await _context.Set<Tmodel>().FindAsync(id);
            if (dbItem == null)
            {
                throw new Exception(Errors.ItemNotFound);
            }

            dbItem.StatusId = (int)StatusEnm.NotActive;
            dbItem.GetType().GetProperty("UpdateDate")?.SetValue(dbItem, DateTime.Now);
            dbItem.GetType().GetProperty("UpdateUserId")?.SetValue(dbItem, userId);

            _context.Entry(dbItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(Errors.General);
            }

            var dbItemResult = await _context.Set<Tmodel>().IncludeVirtualProperties(new Tmodel { }).FirstOrDefaultAsync(e => e.Id == id);
            TmodelDTO dtoItemResult = dbItemResult.ConvertToDto<Tmodel, TmodelDTO>();

            return dtoItemResult;
        }

        public async Task<TmodelDTO> ForceDelete(int id, string userEmail)
        {
            int userId = await GetUserIdByEmail(userEmail);

            var dbItem = await _context.Set<Tmodel>().FindAsync(id);
            if (dbItem == null)
            {
                throw new Exception(Errors.ItemNotFound);
            }

            _context.Set<Tmodel>().Remove(dbItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(Errors.General);
            }

            return new TmodelDTO();
        }

        private async Task<int> GetUserIdByEmail(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (user == null)
            {   
                throw new Exception(Errors.NotAuthorizedOnServer );
            }
            return user.Id;
        }

        private bool isItemExists(int id)
        {
            return _context.Set<Tmodel>().Any(e => (e.Id == id) && e.StatusId == (int)StatusEnm.Active);
        }
    }
}
