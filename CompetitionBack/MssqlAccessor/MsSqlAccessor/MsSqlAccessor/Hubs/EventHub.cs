using Azure.Core;
//using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MsSqlAccessor.Models;
using MsSqlAccessor.Enums;
using MsSqlAccessor.DbControllers;
using MsSqlAccessor.Services;
using Microsoft.Extensions.Logging;

namespace MsSqlAccessor.Hubs
{
    public class EventHub : Hub
    {
        private readonly EventsDbController _dbController;

        public EventHub(EventsDbController dbController)
        {
            _dbController = dbController;

        }

        public async System.Threading.Tasks.Task GetAll()
        {
            var dbItems = await _dbController.GetEvents();

            await Clients.All.SendAsync("ReceiveEvents", dbItems);
        }

        public async System.Threading.Tasks.Task GetOne(int id)
        {
            try
            {
                var dbItem = await _dbController.GetEvent(id);

                await Clients.All.SendAsync("ReceiveEvent", dbItem);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.NoData:
                        await Clients.All.SendAsync("ReceiveEvent", "Not Found");
                        break;

                    default:
                        await Clients.All.SendAsync("ReceiveEvent", "Bad Request");
                        break;
                }
            }
        }

        public async System.Threading.Tasks.Task UpdateOne(int id, EventDTO request, int userId)
        {
            try
            {
                var dbItem = await _dbController.PutEvent(id, request, userId);
                await Clients.All.SendAsync("UpdateEvent", dbItem);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.BadRequest:
                        await Clients.All.SendAsync("ReceiveEvent", "Bad Request");
                        break;

                    case AppError.NoData:
                        await Clients.All.SendAsync("ReceiveEvent", "Not Found");
                        break;

                    default:
                        await Clients.All.SendAsync("ReceiveEvent", "Bad Request");
                        break;
                }
            }        
        }

        public async System.Threading.Tasks.Task PostOne(EventDTO request, int userId)
        {
            try
            {
                var dbItem = await _dbController.PostEvent(request, userId);
                await Clients.All.SendAsync("PostEvent", dbItem);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    default:
                        await Clients.All.SendAsync("ReceiveEvent", "Bad Request");
                        break;
                }
            }
        }

        public async System.Threading.Tasks.Task DeleteOne(int id)
        {
            try
            {
                var results = await _dbController.DeleteEvent(id);
                if (results.Value) await Clients.All.SendAsync("DeleteEvent", true);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.NoData:
                        await Clients.All.SendAsync("ReceiveEvent", "Not Found");
                        break;

                    default:
                        await Clients.All.SendAsync("ReceiveEvent", "Bad Request");
                        break;
                }
            }
        }

        public async System.Threading.Tasks.Task ForceDeleteOne(int id)
        {
            try
            {
                var results = await _dbController.DeleteEventForce(id);
                if(results.Value) await Clients.All.SendAsync("ForceDeleteEvent", true);
            }
            catch (ServerError ex)
            {
                switch (ex.Error)
                {
                    case AppError.NoData:
                        await Clients.All.SendAsync("ReceiveEvent", "Not Found");
                        break;

                    default:
                        await Clients.All.SendAsync("ReceiveEvent", "Bad Request");
                        break;
                }
            }
        }
    }
}