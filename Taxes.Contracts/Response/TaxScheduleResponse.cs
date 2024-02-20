namespace Taxes.Contracts.Response
{
    public record TaxScheduleResponse
    {
        public string DateRange { get; init; } = string.Empty;
        public string Rate { get; init; } = string.Empty;
    }
}