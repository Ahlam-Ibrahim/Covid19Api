using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Api.Dtos
{
    public class CaseDto
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool WithChronicDiseases { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public string Status { get; set; }
    }
}
