using Ecommerce.Product.Core.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Product.Core;

public static class DependencyInjection
{

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingCreateProductRequestProfile).Assembly);
        return services;
    }

}