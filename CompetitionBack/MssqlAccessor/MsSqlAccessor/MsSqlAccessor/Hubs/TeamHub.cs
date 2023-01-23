using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Models;
using Task = System.Threading.Tasks.Task;

namespace MsSqlAccessor.Hubs
{
    public class TeamHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private const string GetAllRoles = "admin,manager,participant";
        private const string GetOneRoles = "admin,manager,participant";
        private const string UpdateRoles = "admin";
        private const string CreateRoles = "admin";
        private const string DeleteRoles = "admin";
        private const string ForceDeleteRoles = "admin";


        private readonly GenDbController<Tmodel, TmodelDTO> _dbController;

        public TeamHub(GenDbController<Tmodel, TmodelDTO> dbController)
        {
            _dbController = dbController;
        }

        [HubMethodName("GetAll")]
        [Authorize(Roles = GetAllRoles)]
        public async Task GetAll()
        {
            var dtoItems = await _dbController.GetAll();

            await Clients.Caller.SendAsync("ReceiveGetAll", dtoItems);
        }

        [HubMethodName("GetAllWithConditions")]
        [Authorize(Roles = GetAllRoles)]
        public async Task GetAllWithConditions(Dictionary<string, object> filters)
        {
            var dtoItems = await _dbController.GetAllWithConditions(filters);

            await Clients.Caller.SendAsync("ReceiveGetAll", dtoItems);
        }

        [HubMethodName("GetOne")]
        [Authorize(Roles = GetOneRoles)]
        public async Task GetOne(int id)
        {
            TmodelDTO dtoItem;
            try
            {
                dtoItem = await _dbController.GetOne(id);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
            //await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveGetOne", dtoItem);
        }

        [HubMethodName("GetOneWithConditions")]
        [Authorize(Roles = GetOneRoles)]
        public async Task GetOneWithConditions(Dictionary<string, object> filters)
        {
            TmodelDTO dtoItem;
            try
            {
                dtoItem = await _dbController.GetOneWithConditions(filters);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
            //await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveGetOne", dtoItem);
        }

        [HubMethodName("Update")]
        [Authorize(Roles = UpdateRoles)]
        public async Task Update(int id, TmodelDTO dtoItem)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;

            TmodelDTO dtoItemResult;

            try
            {
                dtoItemResult = await _dbController.Update(id, dtoItem, userEmail);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }

            await Clients.Caller.SendAsync("ReceiveUpdate", dtoItemResult);
			await Clients.All.SendAsync("DataHasChanged");
		}

        [HubMethodName("Create")]
        [Authorize(Roles = CreateRoles)]
        public async Task Create(TmodelDTO dtoItem)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;

            TmodelDTO dtoItemResult;

            try
            {
                dtoItemResult = await _dbController.Create(dtoItem, userEmail);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }

            await Clients.Caller.SendAsync("ReceiveCreate", dtoItemResult);
			await Clients.All.SendAsync("DataHasChanged");
		}

        [HubMethodName("Delete")]
        [Authorize(Roles = DeleteRoles)]
        public async Task Delete(int id)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;

            TmodelDTO dtoItemResult;

            try
            {
                dtoItemResult = await _dbController.Delete(id, userEmail);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }

            await Clients.Caller.SendAsync("ReceiveDelete", dtoItemResult);
			await Clients.All.SendAsync("DataHasChanged");
		}

        [HubMethodName("ForceDelete")]
        [Authorize(Roles = ForceDeleteRoles)]
        public async Task ForceDelete(int id)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;

            TmodelDTO dtoItemResult;

            try
            {
                dtoItemResult = await _dbController.ForceDelete(id, userEmail);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }


            await Clients.Caller.SendAsync("ReceiveForceDelete", new TmodelDTO());
			await Clients.All.SendAsync("DataHasChanged");
			return;
        }

    }
}
