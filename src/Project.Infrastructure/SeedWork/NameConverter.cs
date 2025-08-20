using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.Domain.SharedKernel.Users;

namespace Project.Infrastructure.SeedWork
{
    /// <summary>
    /// Converts between the domain-specific <see cref="Name"/> type and its string representation for persistence.
    /// </summary>
    public sealed class NameConverter(ConverterMappingHints? mappingHints = null)
        : ValueConverter<Name, string>(
            name => name.Value,
            value => new Name(value),
            mappingHints)
    { }
}
