using Mango.Services.CouponAPI.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Seeds
{
    public class DataBaseInitializer
    {
        public static void Initialize(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var service = scope.ServiceProvider;
                try
                {
                    var context = service.GetRequiredService<ApplicationDbContext>();
                    Migrate(context);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private static void Migrate(ApplicationDbContext context)
        {
            context.Database.Migrate();
            CouponSeed.Seed(context);
        }
    }
}
