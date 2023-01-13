using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;

namespace MsSqlAccessor.DbControllers
{
    public class AuthUserDbController
    {
        private readonly CompetitionBdTestContext _context;

        public AuthUserDbController(CompetitionBdTestContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> GetUserByEmail(string Email)
        {
            var dbItem = await _context.Set<User>()
                .IncludeVirtualProperties(new User { })
                .Where(e => e.StatusId == (int)StatusEnm.Active)
                .FirstOrDefaultAsync(e => e.Email == Email);

            if (dbItem == null)
            {
                throw new Exception(Errors.ItemNotFound);
            }

            var dtoItem = dbItem.ConvertToDto<User, UserDTO>();

            return dtoItem;
        }

        public async Task<UserDTO> Create(UserDTO dtoItem, string userEmail)
        {
            int userId = await GetUserIdByEmail(userEmail);

            User dbItem = dtoItem.ConvertFromDto<User, UserDTO>();

            dbItem.StatusId = (int)StatusEnm.Active;
            dbItem.GetType().GetProperty("CreateDate")?.SetValue(dbItem, DateTime.Now);
            dbItem.GetType().GetProperty("UpdateDate")?.SetValue(dbItem, DateTime.Now);
            dbItem.GetType().GetProperty("CreateUserId")?.SetValue(dbItem, userId);
            dbItem.GetType().GetProperty("UpdateUserId")?.SetValue(dbItem, userId);

            _context.Set<User>().Add(dbItem);
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

            UserDTO dtoItemResult = dbItem.ConvertToDto<User, UserDTO>();

            return dtoItemResult;
        }
        private async Task<int> GetUserIdByEmail(string userEmail)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == userEmail);
            if (user == null)
            {   
                return Constants.systemUser.Id;
            }
            throw new Exception(Errors.BadRequest);
        }

        private bool isItemExists(int id)
        {
            return _context.Set<User>().Any(e => (e.Id == id) && e.StatusId == (int)StatusEnm.Active);
        }
    }
}
