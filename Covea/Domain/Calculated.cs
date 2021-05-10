using System;
using System.Collections.Generic;
using System.Text;

namespace Covea.Domain
{
    public class Calculated
    {
        /// <summary>
        /// Gets or sets the calculated value
        /// </summary>
        public bool IsInsuranceCalculated { get; set; }

        /// <summary>
        /// Gets or sets the error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the insurance premium
        /// </summary>
        public decimal InsurancePremium { get; set; }

        /// <summary>
        /// Gets or sets the age
        /// </summary>
        public decimal SumAssured { get; set; }
    }
}
