// Ignore Spelling: Pii

using Microsoft.Extensions.Compliance.Classification;

namespace DataProtection.Abstractions;

public class PiiDataAttribute : DataClassificationAttribute
{
    public PiiDataAttribute() : base(DataTaxonomy.Pii)
    {
    }
}