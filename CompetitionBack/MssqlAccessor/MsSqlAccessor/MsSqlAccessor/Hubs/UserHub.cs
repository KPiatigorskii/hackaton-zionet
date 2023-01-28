using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Models;
using Task = System.Threading.Tasks.Task;

namespace MsSqlAccessor.Hubs
{
    public class UserHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private const string GetAllPolicy = "manager";
        private const string GetOnePolicy = "manager";
        private const string GetUserByEmail = "participant";
        private const string UpdatePolicy = "participant";
        private const string CreatePolicy = "admin";
        private const string RegistrationPolicy = "participant";
        private const string DeletePolicy = "admin";
        private const string ForceDeletePolicy = "admin";


        private readonly GenDbController<Tmodel, TmodelDTO> _dbController;
        private readonly AuthUserDbController _dbAuthUserController;

        public UserHub(GenDbController<Tmodel, TmodelDTO> dbController, AuthUserDbController dbAuthUserController)
        {
            _dbController = dbController;
            _dbAuthUserController = dbAuthUserController;
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

        [HubMethodName("GetOneByEmail")]
        [Authorize(Policy = GetUserByEmail)]
        public async Task GetOneByEmail(string Email)
        {
            UserDTO dtoItem;
            try
            {
                dtoItem = await _dbAuthUserController.GetUserByEmail(Email);
            }
            catch (Exception ex)
            {
				throw new HubException(ex.Message);
			}
            //await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveGetOneByEmail", dtoItem);
        }

        [HubMethodName("Update")]
        [Authorize(Policy = UpdatePolicy)]
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
        [Authorize(Policy = CreatePolicy)]
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

        [HubMethodName("RegisterMe")]
        [Authorize(Policy = RegistrationPolicy)]
        public async Task RegisterMe()
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;

			var dtoNewUser = new UserDTO
			{
				Email = userEmail,
				RoleId = (int)RoleEnm.Participant,
				Login = Context.User.Claims.FirstOrDefault(e => e.Type == "name").Value,
				FirstName = Context.User.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value,
				LastName = Context.User.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value,
			};

			UserDTO dtoItemResult;

            try
            {
                dtoItemResult = await _dbAuthUserController.Create(dtoNewUser, userEmail);
            }
            catch (Exception ex)
            {
				throw new HubException(ex.Message);
			}

            await Clients.Caller.SendAsync("ReceiveRegister", dtoItemResult);
			await Clients.All.SendAsync("DataHasChanged");
		}

        [HubMethodName("Delete")]
        [Authorize(Policy = DeletePolicy)]
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
        [Authorize(Policy = ForceDeletePolicy)]
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
