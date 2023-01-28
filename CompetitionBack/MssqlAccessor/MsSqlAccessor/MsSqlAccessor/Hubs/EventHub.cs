using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Managers;
using Task = System.Threading.Tasks.Task;
using System.Security.Claims;

namespace MsSqlAccessor.Hubs
{
    public class EventHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private const string GetAllPolicy = "participant";
        private const string GetOnePolicy = "participant";
        private const string RunWithArgumentsPolicy = "participant";
        private const string UpdatePolicy = "admin";
        private const string CreatePolicy = "admin";
        private const string DeletePolicy = "admin";
        private const string ForceDeletePolicy = "admin";


        private readonly GenDbController<Tmodel, TmodelDTO> _dbController;
        private EventLogicManager _eventLogicManager;

        public EventHub(GenDbController<Tmodel, TmodelDTO> dbController, EventLogicManager eventLogicManager)
        {
            _dbController = dbController;
            _eventLogicManager = eventLogicManager;
 
        }

        [HubMethodName("GetAll")]
        [Authorize(Policy = GetAllPolicy)]
        public async Task GetAll()
        {
            var dtoItems = await _dbController.GetAll();

            await Clients.Caller.SendAsync("ReceiveGetAll", dtoItems);
        }

		[HubMethodName("GetAllWithConditions")]
		[Authorize(Policy = GetAllPolicy)]
		public async Task GetAllWithConditions(Dictionary<string, object> filters)
		{
			var dtoItems = await _dbController.GetAllWithConditions(filters);

			await Clients.Caller.SendAsync("ReceiveGetAll", dtoItems);
		}

		[HubMethodName("GetOne")]
        [Authorize(Policy = GetOnePolicy)]
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
		[Authorize(Policy = GetOnePolicy)]
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

        [HubMethodName("RunWithArguments")]
        [Authorize(Policy = RunWithArgumentsPolicy)]
        public async Task RunWithArguments(string functionName, Dictionary<string, object> arguments)
        {
            TmodelDTO dtoItem;
            try
            {
                var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value;
                _eventLogicManager.startEvent(arguments, userEmail);
                //dtoItem = await _dbController.GetOneWithConditions(filters);
            }
            catch (Exception ex)
            {
                throw new HubException(ex.Message);
            }
            //await Task.Delay(1000);
            await Clients.All.SendAsync("DataHasChanged");  
        }

        [HubMethodName("Update")]
        [Authorize(Policy = UpdatePolicy)]
        public async Task Update(int id, TmodelDTO dtoItem)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value;

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
        [Authorize(Policy = CreatePolicy)]
        public async Task Create(TmodelDTO dtoItem)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value;

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
        [Authorize(Policy = DeletePolicy)]
        public async Task Delete(int id)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value;

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
        [Authorize(Policy = ForceDeletePolicy)]
        public async Task ForceDelete(int id)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email).Value;

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
