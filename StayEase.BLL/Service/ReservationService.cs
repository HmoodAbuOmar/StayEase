using Mapster;
using StayEase.DAL.DTO.Request;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;
using StayEase.DAL.Repository;

namespace StayEase.BLL.Service
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;

        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }
        public async Task<BaseResponse> CreateReservationAsync(string userId, ReservationRequest request)
        {
            if (request.CheckOutDate <= request.CheckInDate)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Check-out date must be after check-in date."
                };
            }

            if (request.CheckInDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Check-in date cannot be in the past."
                };
            }

            var room = await _roomRepository.GetByRoomNumberAsync(request.RoomNumber);

            if (room == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Room not found."
                };
            }

            if (request.NumberOfGuests > room.RoomType.MaxCapacity)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Number of guests exceeds room capacity."
                };
            }

            var hasConflict = await _reservationRepository.HasConflictAsync(
                room.Id,
                request.CheckInDate,
                request.CheckOutDate);

            if (hasConflict)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "This room is already reserved during the selected period.",
                };
            }

            var numberOfNights = (request.CheckOutDate.DayNumber - request.CheckInDate.DayNumber);
            var totalPrice = numberOfNights * room.PricePerNight;

            var reservation = request.Adapt<Reservation>();
            reservation.RoomId = room.Id;
            reservation.TotalPrice = totalPrice;
            reservation.Status = ReservationStatus.Pending;
            reservation.UserId = userId;
            reservation.CreatedBy = userId;
            reservation.CreatedAt = DateTime.Now;

            await _reservationRepository.CreateReservationAsync(reservation);

            return new BaseResponse
            {
                Success = true,
                Message = "Reservation created successfully.",
            };
        }

        public async Task<BaseResponse> DeleteReservationAsync(string userId, int id)
        {
            var result = await _reservationRepository.GetMyReservationByIdAsync(userId, id);

            if (result is null)
            {
                return new BaseResponse
                {
                    Message = "No Reservation Avallaibly For This Id",
                    Success = false,
                    Errors = new List<string>() { "No Avallaivbale Reservaions" }

                };
            }
            await _reservationRepository.DeleteReservationAsync(result);

            return new BaseResponse
            {
                Success = true,
                Message = "Reservation Deleted Succesfuly",
            };
        }



        public async Task<List<UserReservationResponse>> GetMyReservationsAsync(string userId)
        {
            var reservations = await _reservationRepository.GetMyReservationsAsync(userId);

            if (reservations == null || !reservations.Any()) // reservations == null → الليست نفسها مش موجودة
                return null;                                 //!reservations.Any() → الليست موجودة لكن فاضية

            var response = reservations.Adapt<List<UserReservationResponse>>();
            return response;
        }

        public async Task<BaseResponse> UpdateReservationAsync(int id, string userId, ReservationRequest request)
        {
            var reservation = await _reservationRepository.GetMyReservationByIdAsync(userId, id);

            if (reservation is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "No reservation found with this id"
                };
            }

            if (request.CheckOutDate <= request.CheckInDate)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Check-out date must be after check-in date."
                };
            }

            if (request.CheckInDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Check-in date cannot be in the past."
                };
            }

            var room = await _roomRepository.GetByRoomNumberAsync(request.RoomNumber);

            if (room is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Room not found."
                };
            }

            if (request.NumberOfGuests > room.RoomType.MaxCapacity)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Number of guests exceeds room capacity."
                };
            }

            var hasConflict = await _reservationRepository.HasConflictAsync(
                room.Id,
                request.CheckInDate,
                request.CheckOutDate,
                reservation.Id);

            if (hasConflict)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "This room is already reserved during the selected period."
                };
            }

            var numberOfNights = request.CheckOutDate.DayNumber - request.CheckInDate.DayNumber;
            var totalPrice = numberOfNights * room.PricePerNight;

            request.Adapt(reservation);

            reservation.RoomId = room.Id;
            reservation.TotalPrice = totalPrice;
            reservation.UserId = userId;
            reservation.UpdatedBy = userId;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _reservationRepository.UpdateReservationAsync(reservation);

            return new BaseResponse
            {
                Success = true,
                Message = "Reservation updated successfully"
            };
        }


        // For Admin

        public async Task<List<AdminReservationResponse>> GetAllReservationsForAdminAsync()
        {
            var reservations = await _reservationRepository.GetAllReservationsForAdminAsync();

            if (reservations == null || !reservations.Any())
                return new List<AdminReservationResponse>();
            return reservations.Adapt<List<AdminReservationResponse>>();

        }

        public async Task<AdminReservationResponse> GetAllReservationsByIdForAdminAsync(int id)
        {
            var reservations = await _reservationRepository.GetReservationByIdForAdminAsync(id);
            if (reservations is null)
                return null;

            return reservations.Adapt<AdminReservationResponse>();
        }

        public async Task<BaseResponse> ConfirmReservationForAdminAsync(int id)
        {
            var reservations = await _reservationRepository.GetReservationByIdForAdminAsync(id);
            if (reservations is null)
                return new BaseResponse
                {
                    Message = "reservations Falid Confirmed",
                    Success = false,
                };

            await _reservationRepository.ConfirmReservationForAdminAsync(id);

            return new BaseResponse
            {
                Message = "reservations confirm successfuly",
                Success = true,
            };
        }

        public async Task<BaseResponse> CancelReservationForAdminAsync(int id)
        {
            var reservations = await _reservationRepository.GetReservationByIdForAdminAsync(id);
            if (reservations is null)
                return new BaseResponse
                {
                    Message = "reservations Falid Cancelled",
                    Success = false,
                };

            await _reservationRepository.CancelledReservationForAdminAsync(id);

            return new BaseResponse
            {
                Message = "reservations Cancelled successfuly",
                Success = true,
            };
        }
    }
}