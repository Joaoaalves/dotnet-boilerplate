using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.SeedWork
{
    public class TypedIdValueConverter<TTypedIdValue>(ConverterMappingHints? mappingHints = null) : ValueConverter<TTypedIdValue, Guid>(
            id => id.Value,
            value => Create(value),
            mappingHints)
        where TTypedIdValue : TypedIdValueBase
    {
        private static TTypedIdValue Create(Guid id)
        {
            return (Activator.CreateInstance(typeof(TTypedIdValue), id) as TTypedIdValue)!;
        }
    }
}