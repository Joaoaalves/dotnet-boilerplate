namespace Project.Domain.SeedWork
{
    /// <summary>
    /// Attribute used to exclude a field or property from equality and hash code calculations in value objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class IgnoreMemberAttribute : Attribute
    {
    }
}
