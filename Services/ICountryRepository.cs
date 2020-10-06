using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Models;

namespace Covid19Api.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        bool CountryExists(int id);
        bool IsDuplicateCountryName(int id, string countryName);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);

        bool Save();
        ICollection<Case> GetCasesFromACountry(int countryId);

    }
}
