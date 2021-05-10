using System;
using System.Collections.Generic;
using System.Text;

namespace Covea.Domain
{
    public class Bands
    {
        /// <summary>
        /// Gets or sets the sum assured
        /// </summary>
        public int SumAssured { get; set; }

        /// <summary>
        /// Gets or sets the risk rate for people under 30
        /// </summary>
        public decimal? UnderThrityRiskRate { get; set; }

        /// <summary>
        /// Gets or sets the risk rate for people between 31 and 50
        /// </summary>
        public decimal? BetweenThrityOneAndFiftyRiskRate { get; set; }

        /// <summary>
        /// Gets or sets the risk rate for people over 50
        /// </summary>
        public decimal? OverFiftyRiskRate { get; set; }
    }
}
