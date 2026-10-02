namespace DungeonCrawler.Domain.Abstractions
{
    /// <summary>
    /// Raised when an operation would break an <see cref="IBusinessRule"/>.
    /// </summary>
    public sealed class BusinessRuleViolationException : DomainException
    {
        public BusinessRuleViolationException(IBusinessRule brokenRule)
            : base(Guard.AgainstNull(brokenRule, nameof(brokenRule)).Message)
        {
            BrokenRule = brokenRule;
        }

        public IBusinessRule BrokenRule { get; }
    }
}
