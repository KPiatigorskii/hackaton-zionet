﻿using System;
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
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Enums;
using MsSqlAccessor.Helpers;
using MsSqlAccessor.Models;
using MsSqlAccessor.Services;
using NuGet.Configuration;
using NuGet.Protocol;
using Task = System.Threading.Tasks.Task;

namespace MsSqlAccessor.Hubs
{
    public class UserHub<Tmodel, TmodelDTO> : Hub where Tmodel : class, IdModel, new() where TmodelDTO : class, IdModel, new()
    {
        private const string GetAllRoles = "admin,manager,paritcipant";
        private const string GetOneRoles = "admin,manager,paritcipant";
        private const string UpdateRoles = "admin";
        private const string CreateRoles = "admin";
        private const string DeleteRoles = "admin";
        private const string ForceDeleteRoles = "admin";


        private readonly GenDbController<Tmodel, TmodelDTO> _dbController;

        public UserHub(GenDbController<Tmodel, TmodelDTO> dbController)
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
                    throw new HubException(Errors.ItemNotFound);
                }
                else
                {
                    throw new HubException(Errors.General);
                }
            }

            await Clients.Caller.SendAsync("ReceiveCreate", dtoItemResult);
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
                    throw new HubException(Errors.ItemNotFound);
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
                    throw new HubException(Errors.ItemNotFound);
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
