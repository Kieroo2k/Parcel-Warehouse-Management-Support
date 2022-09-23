using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ParcelWarehouseManagementSupport
{
    public class Initializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ParcelWarehouseDBContext>>();
            using (var context = new ParcelWarehouseDBContext(options))
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
