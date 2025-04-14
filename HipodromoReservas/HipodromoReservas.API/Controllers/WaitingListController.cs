using HipodromoReservas.Application.Dtos;
using HipodromoReservas.Application.Services;
using HipodromoReservas.Application.Utils;
using HipodromoReservas.Domain.Constans;
using HipodromoReservas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HipodromoReservas.API.Controllers
{
    [ApiController]
    [Route("api/waitingList/")]
    public class WaitingListController : ControllerBase
    {
        private readonly IWaitingListService _clientWaitingListService;

        public WaitingListController(IWaitingListService reservationService)
        {
            _clientWaitingListService = reservationService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var waitingList = await _clientWaitingListService.GetWaitingList();
                return Ok(waitingList.Select(wle => new WaitingListEntryResponseDto
                    {
                        ClientName = ClientConstants.Clients.Where(x => x.Id == wle.ClientId).First().Name,
                        ReservationDateTime = DateHelper.BuildReservationDate(wle.ReservationDate, wle.TimeSlot)
                }
                ).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("addClientToWaitingList")]
        public async Task<IActionResult> AddClientToWaitingList([FromBody] WaitingListEntryRequestDto request)
        {
            var client = ClientConstants.GetClientById(request.ClientId);

            var entry = await _clientWaitingListService.CreateEntry(client, (TableCapacityEnum)request.TableCapacity, request.ReservationDate, request.TimeSlot);

            return Ok(new WaitingListEntryResponseDto
                {
                    ClientName = client.Name,
                    ReservationDateTime = DateHelper.BuildReservationDate(entry.ReservationDate, entry.TimeSlot),
                }
            );
        }
    }
}