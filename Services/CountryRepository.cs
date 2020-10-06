using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Covid19Api.Services
{
    public class CountryRepository : ICountryRepository
    {
        private Covid19Context _context;

        public CountryRepository(Covid19Context context)
        {
            _context = context;
        }
        public bool CountryExists(int id)
        {
           return _context.Countries.Any(c => c.id == id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();

        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(a => a.Name).ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.id == id).FirstOrDefault(); 
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }
        public bool IsDuplicateCountryName(int countryId, string countryName)
        {

            var country = _context.Countries.Where(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper()
                                                && c.id != countryId).FirstOrDefault();

            return country == null ? false : true;
        }
        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }

        public ICollection<Case> GetCasesFromACountry(int countryId)
        {
           return _context.Cases.Where(c => c.id == countryId).ToList();
        }
    }
}
