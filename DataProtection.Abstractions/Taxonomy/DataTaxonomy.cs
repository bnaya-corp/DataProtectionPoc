// Ignore Spelling: Pii

using Microsoft.Extensions.Compliance.Classification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataProtection.Abstractions;
public static class DataTaxonomy
{
    public static string TaxonomyName { get; } = typeof(DataTaxonomy).FullName!;

    public static DataClassification Sensitive { get; } = new(TaxonomyName, nameof(Sensitive));
    public static DataClassification Pii { get; } = new(TaxonomyName, nameof(Pii));

    public static class Sets
    {
        public static DataClassificationSet Sensitive { get; } = new(DataTaxonomy.Sensitive);
        public static DataClassificationSet Pii { get; } = new(DataTaxonomy.Pii);
    }
}
