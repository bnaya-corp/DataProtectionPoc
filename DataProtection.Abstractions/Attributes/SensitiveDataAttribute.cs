using Microsoft.Extensions.Compliance.Classification;

namespace DataProtection.Abstractions;

public class SensitiveDataAttribute : DataClassificationAttribute
{
    public SensitiveDataAttribute() : base(DataTaxonomy.Sensitive)
    {
    }
}
