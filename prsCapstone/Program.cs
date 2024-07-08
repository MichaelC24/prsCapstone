using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using prsCapstone.Data;
namespace prsCapstone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<prsCapstoneContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("prsCapstoneContext") ?? throw new InvalidOperationException("Connection string 'prsCapstoneContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
