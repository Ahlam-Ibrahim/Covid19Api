using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Models;

namespace Covid19Api.Services
{
    public interface ICaseRepository
    {
        ICollection<Case> GetCases();
        Case GetCase(int caseId);
        bool CaseExists(int caseId);
        bool CreateCase(Case _case);
        bool UpdateCase(Case _case);
        bool DeleteCase(Case _case);
        bool Save();
    }
}
