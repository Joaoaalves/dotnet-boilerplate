using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Domain.SharedKernel.Users;

namespace Project.Infrastructure.SeedWork.Resilience
{
    public sealed class NameConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<Name, string>(
        name => name.Value,
        value => new Name(value),
        mappingHints
    )
    { }
}