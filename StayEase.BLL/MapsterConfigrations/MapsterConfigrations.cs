using Mapster;
using StayEase.DAL.DTO.Response;
using StayEase.DAL.Models;

namespace StayEase.BLL.MapsterConfigurations
{
    public static class MapsterConfigurations
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Hotel, HotelResponse>.NewConfig()
                .Map(dest => dest.Translations,
                    src => src.Translations
                        .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                        .Adapt<List<HotelTranslationsResponse>>());

            TypeAdapterConfig<Room, RoomResponse>.NewConfig()
                .Map(dest => dest.HotelName,
                    src => src.Hotel.Translations
                        .FirstOrDefault(t => t.Language == MapContext.Current.Parameters["lang"].ToString())!.Name)
                .Map(dest => dest.RoomTypeName,
                    src => src.RoomType.Name);

            TypeAdapterConfig<Reservation, UserReservationResponse>.NewConfig()
           .Map(dest => dest.RoomNumber,
         src => src.Room.RoomNumber);

            TypeAdapterConfig<Reservation, AdminReservationResponse>.NewConfig()
    .Map(dest => dest.RoomNumber,
         src => src.Room.RoomNumber)
    .Map(dest => dest.RoomPricePerNight,
         src => src.Room.PricePerNight)
    .Map(dest => dest.UserFullName,
         src => src.ApplicationUser.FullName)
    .Map(dest => dest.UserEmail,
         src => src.ApplicationUser.Email);

        }
    }
}