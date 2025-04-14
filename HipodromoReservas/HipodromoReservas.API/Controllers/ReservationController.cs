using HipodromoReservas.Domain.Interfaces.IService;
using HipodromoReservas.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using HipodromoReservas.Application.Utils;
using HipodromoReservas.Domain.Enums;
using HipodromoReservas.Domain.Constans;

namespace HipodromoReservas.API.Controllers
{
    [ApiController]
    [Route("api/reservation/")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservations = await _reservationService.GetReservations();
                return Ok(reservations.Select(r => new ReservationListItemDto
                {
                    Id = r.Id,
                    TableId = r.TableId,
                    ReservationDateTime = DateHelper.BuildReservationDate(r.ReservationDate, r.TimeSlot),
                    ClientId = r.ClientId
                }).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequestDto request)
        {
            try 
            {
                // Se busca al cliente.
                var client = ClientConstants.GetClientById(request.ClientId);
                
                // Se valida que la fecha sea correcta.
                var reservationDateTime = DateHelper.BuildReservationDate(request.ReservationDate, request.TimeSlot);
 
                // Se valida que el cliente pueda solicitar esa fecha.
                _reservationService.ValidateClientCouldReserve(client, request.ReservationDate, request.TimeSlot);

                // Se intenta crear la reserva.
                var reservation = await _reservationService.TryCreateReservation(request.ReservationDate, request.TimeSlot, (TableCapacityEnum)request.TableCapacity, client.Id);

                return Ok(new CreateReservationResponseDto 
                    { 
                        WaitingList = reservation == null,
                        TableId = reservation?.TableId ?? null
                    }
                );
            }
            catch (Exception ex) 
            { 
                return BadRequest(new { message = ex.Message});
            }
        }

        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            try 
            { 
                var reservation = await _reservationService.CancelReservation(reservationId);

                return Ok(reservation.Id);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}