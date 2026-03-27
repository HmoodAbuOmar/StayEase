using Microsoft.AspNetCore.Identity.UI.Services;
using StayEase.BLL.Service;
using StayEase.DAL.Repository;
using StayEase.DAL.Utilits;

namespace StayEase.PL
{
    public static class AppConfiguration
    {
        public static void Configuration(IServiceCollection Services)
        {
            Services.AddScoped<IHotelRepository, HotelRepository>();
            Services.AddScoped<IHotelService, HotelService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            Services.AddScoped<IRoomTypeService, RoomTypeService>();
            Services.AddScoped<IRoomRepository, RoomRepository>();
            Services.AddScoped<IRoomService, RoomService>();
            Services.AddScoped<IReservationRepository, ReservationRepository>();
            Services.AddScoped<IReservationService, ReservationService>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<ICheckoutService, CheckoutService>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();
            Services.AddScoped<IReviewService, ReviewService>();



        }
    }
}
