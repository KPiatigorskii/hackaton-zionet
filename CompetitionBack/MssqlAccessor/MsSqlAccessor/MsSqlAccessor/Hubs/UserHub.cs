﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Models;
using Task = System.Threading.Tasks.Task;

namespace MsSqlAccessor.Hubs
{
    public class UserHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private const string GetAllRoles = "admin,manager";
        private const string GetOneRoles = "admin,manager";
        private const string GetUserByEmail = "admin, manager, participant";
        private const string UpdateRoles = "admin, manager, participant";
        private const string CreateRoles = "admin";
        private const string RegistrationRoles = "participant";
        private const string DeleteRoles = "admin";
        private const string ForceDeleteRoles = "admin";


        private readonly GenDbController<Tmodel, TmodelDTO> _dbController;
        private readonly AuthUserDbController _dbAuthUserController;

        public UserHub(GenDbController<Tmodel, TmodelDTO> dbController, AuthUserDbController dbAuthUserController)
        {
            _dbController = dbController;
            _dbAuthUserController = dbAuthUserController;
        }

        [HubMethodName("GetAll")]
        [Authorize(Roles = GetAllRoles)]
        public async Task GetAll()
        {
            var dtoItems = await _dbController.GetAll();

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
                if (ex.Message == Errors.ItemNotFound)
                {
                    throw new HubException(Errors.ItemNotFound);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }
            //await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveGetOne", dtoItem);
        }

        [HubMethodName("GetOneByEmail")]
        [Authorize(Roles = GetOneRoles)]
        public async Task GetOneByEmail(string Email)
        {
            UserDTO dtoItem;
            try
            {
                dtoItem = await _dbAuthUserController.GetUserByEmail(Email);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errors.ItemNotFound)
                {
                    throw new HubException(Errors.ItemNotFound);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }
            //await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveGetOneByEmail", dtoItem);
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
                if (ex.Message == Errors.NotAuthorizedOnServer)
                {
                    throw new HubException(Errors.NotAuthorizedOnServer);
                }
                if (ex.Message == Errors.ItemNotFound)
                {
                    throw new HubException(Errors.ItemNotFound);
                }
                if (ex.Message == Errors.BadRequest)
                {
                    throw new HubException(Errors.BadRequest);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            await Clients.Caller.SendAsync("ReceiveUpdate", dtoItemResult);
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
                if (ex.Message == Errors.NotAuthorizedOnServer)
                {
                    throw new HubException(Errors.NotAuthorizedOnServer);
                }
                if (ex.Message == Errors.ConflictData)
                {
                    throw new HubException(Errors.ConflictData);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            await Clients.Caller.SendAsync("ReceiveCreate", dtoItemResult);
        }

        [HubMethodName("Register")]
        [Authorize(Roles = CreateRoles)]
        public async Task Register(UserDTO dtoItem)
        {
            var userEmail = Context.User.Claims.FirstOrDefault(e => e.Type == "http://zionet-api/user/claims/email").Value;

            UserDTO dtoItemResult;

            try
            {
                dtoItemResult = await _dbAuthUserController.Create(dtoItem, userEmail);
            }
            catch (Exception ex)
            {
                if (ex.Message == Errors.NotAuthorizedOnServer)
                {
                    throw new HubException(Errors.NotAuthorizedOnServer);
                }
                if (ex.Message == Errors.BadRequest)
                {
                    throw new HubException(Errors.BadRequest);
                }
                if (ex.Message == Errors.ConflictData)
                {
                    throw new HubException(Errors.ConflictData);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            await Clients.Caller.SendAsync("ReceiveRegister", dtoItemResult);
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
                if (ex.Message == Errors.NotAuthorizedOnServer)
                {
                    throw new HubException(Errors.NotAuthorizedOnServer);
                }
                if (ex.Message == Errors.ConflictData)
                {
                    throw new HubException(Errors.ConflictData);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            await Clients.Caller.SendAsync("ReceiveDelete", dtoItemResult);
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
                if (ex.Message == Errors.NotAuthorizedOnServer)
                {
                    throw new HubException(Errors.NotAuthorizedOnServer);
                }
                if (ex.Message == Errors.ConflictData)
                {
                    throw new HubException(Errors.ConflictData);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }


            await Clients.Caller.SendAsync("ReceiveForceDelete", new TmodelDTO());
            return;
        }

    }
}
