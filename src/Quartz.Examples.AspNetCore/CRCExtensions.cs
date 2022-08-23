namespace Quartz.Examples.AspNetCore
{
    public static class CRCExtensions
    {
        public static void UseQuartz(this IApplicationBuilder app)
        {
            var crcJobFactory = new CRCJobFactory(app.ApplicationServices, app.ApplicationServices.GetRequiredService<ILogger<CRCJobFactory>>());

        }
    }
}
