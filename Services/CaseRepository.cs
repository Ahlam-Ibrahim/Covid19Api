using Covid19Api.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Api.Services
{
    public class CaseRepository : ICaseRepository
    {
        Covid19Context _context;

        public CaseRepository(Covid19Context context)
        {
            _context = context;
        }
        public bool CaseExists(int caseId)
        {
            return _context.Cases.Any(c => c.id == caseId);
        }

        public bool CreateCase(Case _case)
        {
            _context.Add(_case);
            return Save();
        }

        public bool DeleteCase(Case _case)
        {
            _context.Remove(_case);
            return Save();
        }

        public Case GetCase(int caseId)
        {
            return _context.Cases.Where(c => c.id == caseId).FirstOrDefault();
        }

        public ICollection<Case> GetCases()
        {
            return _context.Cases.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public bool UpdateCase(Case _case)
        {
            _context.Update(_case);
            return Save();
        }
    }
}
