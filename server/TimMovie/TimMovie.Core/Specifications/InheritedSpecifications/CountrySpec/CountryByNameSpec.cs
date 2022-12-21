using TimMovie.Core.Entities;
using TimMovie.SharedKernel.Specification;

namespace TimMovie.Core.Specifications.InheritedSpecifications.CountrySpec;

public class CountryByNameSpec: Specification<Country>
{
    public CountryByNameSpec(string name)
    {
        Conditional = country => country.Name == name;
    }
}