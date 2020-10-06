using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Api.Models
{
    public class Case
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool WithChronicDiseases { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public string Status { get; set; }

        /*public enum status
        {
            Confirmed, Recovered, Death
        }*/
        public Country Country { get; set; }
    }
}
