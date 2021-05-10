using Covea.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Covea
{
    public class CalculateLifeInsurance
    {
        /// <summary>
        /// Calculate the insurance premium
        /// </summary>
        /// <param name="sumAssured"></param>
        /// <param name="age"></param>
        /// <returns>
        /// Returns the object Calculated
        /// </returns>
        public Calculated CalculateInsurancePremium(decimal sumAssured, int age)
        {
            if(sumAssured < 25000 || sumAssured > 500000)
            {
                return new Calculated {IsInsuranceCalculated = false, 
                                       ErrorMessage = sumAssured < 25000 ? "The Sum Assured entered has to be more than or equal to £25,000" 
                                       : "The Sum Assured entered has to be less than or equal to £500,000" };
            }

            // do sum assured calc
            var insurancePremium = this.getInsurancePremium(sumAssured, age);

            while (Math.Round(insurancePremium, 2) < 2)
            {
                sumAssured = sumAssured + 5000;
                insurancePremium = this.getInsurancePremium(sumAssured, age);
            }


            return new Calculated
            {
                IsInsuranceCalculated = true,
                ErrorMessage = "",
                SumAssured = sumAssured,
                InsurancePremium = insurancePremium
            };
        }

        /// <summary>
        /// gets the insurance premium
        /// </summary>
        /// <param name="sumAssured"></param>
        /// <param name="age"></param>
        /// <returns>
        /// Returns the insurance premium
        /// </returns>
        public decimal getInsurancePremium(decimal sumAssured, int age)
        {
            var riskPremium = this.CalculateRiskPremium(sumAssured, age);
            var renewalCommission = this.CalculateRenewalCommission(riskPremium);
            var netPremium = this.CalculateNetPremium(riskPremium, renewalCommission);
            var initialCommission = this.CalculateInitialCommission(netPremium);
            var grossPremium = this.CalculateGrossPremium(netPremium, initialCommission);

            return grossPremium;
        }

        /// <summary>
        /// Caculate the risk premium
        /// </summary>
        /// <param name="riskRate"></param>
        /// <param name="sumAssured"></param>
        /// <returns>
        /// Returns the risk premium
        /// </returns>
        public decimal CalculateRiskPremium(decimal sumAssured, int age)
        {
            var bands = this.Bands();
            decimal riskRate = 0;

            if (!bands.Exists(x => x.SumAssured == sumAssured))
            {
               riskRate = this.CalculateBetweenBands(sumAssured, bands, age);
            }
            else
            {
                if(age <= 30)
                {
                    riskRate = (decimal)bands.Find(x => x.SumAssured == 25000).UnderThrityRiskRate;
                }
                else if(age > 30 && age < 51)
                {
                    riskRate = (decimal)bands.Find(x => x.SumAssured == 25000).BetweenThrityOneAndFiftyRiskRate;
                }
                else
                {
                    riskRate = (decimal)bands.Find(x => x.SumAssured == 25000).OverFiftyRiskRate;
                }
            }

            var riskPremium = riskRate * (sumAssured / 1000);

            return riskPremium;
        }

        /// <summary>
        /// Caluculate the renewal commission
        /// </summary>
        /// <param name="riskPremium">the risk premium</param>
        /// <returns>
        /// Returns the renewal commission
        /// </returns>
        public decimal CalculateRenewalCommission(decimal riskPremium)
        {
            var RenewalCommission = riskPremium / 100 * 3 ;

            return RenewalCommission;
        }        

        /// <summary>
        /// Calulate the net premium
        /// </summary>
        /// <param name="riskPremium"></param>
        /// <param name="renewalCommission"></param>
        /// <returns>
        /// Returns the net premium
        /// </returns>
        public decimal CalculateNetPremium(decimal riskPremium, decimal renewalCommission)
        {
            var netPremium = riskPremium + renewalCommission;

            return netPremium;
        }

        /// <summary>
        /// Calculate the initial commission
        /// </summary>
        /// <param name="netPremium"></param>
        /// <returns>
        /// Return the initial commission
        /// </returns>
        public decimal CalculateInitialCommission(decimal netPremium)
        {
            var initailCommission = netPremium / 100 * 205;

            return initailCommission;
        }

        /// <summary>
        /// Calculate the gross premium
        /// </summary>
        /// <param name="netPremium"></param>
        /// <param name="initialCommission"></param>
        /// <returns>
        /// Return the gross premium
        /// </returns>
        public decimal CalculateGrossPremium(decimal netPremium, decimal initialCommission)
        {
            var grossPremium = netPremium + initialCommission;

            return grossPremium;
        }

        /// <summary>
        /// create the list of bands
        /// </summary>
        /// <returns>
        /// the list of bands
        /// </returns>
        public List<Bands> Bands()
        {
            List<Bands> bands = new List<Bands>
            {
                new Bands { SumAssured = 25000, UnderThrityRiskRate = (decimal)0.0172, BetweenThrityOneAndFiftyRiskRate = (decimal)0.1043, OverFiftyRiskRate = (decimal)0.2677 },
                new Bands { SumAssured = 50000, UnderThrityRiskRate = (decimal)0.0165, BetweenThrityOneAndFiftyRiskRate = (decimal)0.0999, OverFiftyRiskRate = (decimal)0.2565 },
                new Bands { SumAssured = 100000, UnderThrityRiskRate = (decimal)0.0154, BetweenThrityOneAndFiftyRiskRate = (decimal)0.0932, OverFiftyRiskRate = (decimal)0.2393 },
                new Bands { SumAssured = 200000, UnderThrityRiskRate = (decimal)0.0147, BetweenThrityOneAndFiftyRiskRate = (decimal)0.0887, OverFiftyRiskRate = (decimal)0.2285 },
                new Bands { SumAssured = 300000, UnderThrityRiskRate = (decimal)0.0144, BetweenThrityOneAndFiftyRiskRate = (decimal)0.0872, OverFiftyRiskRate = null },
                new Bands { SumAssured = 500000, UnderThrityRiskRate = (decimal)0.0146, BetweenThrityOneAndFiftyRiskRate = null, OverFiftyRiskRate = null }
            };
            return bands;
        }

        /// <summary>
        /// calculated the risk rate when the sum assured is between bands
        /// </summary>
        /// <param name="sumAssured"></param>
        /// <param name="bands"></param>
        /// <param name="age"></param>
        /// <returns>
        /// the risk rate
        /// </returns>
        public decimal CalculateBetweenBands(decimal sumAssured, List<Bands> bands, int age)
        {
            decimal lowerBandRiskRate = 0;
            decimal upperBandRiskRate = 0;
            decimal upperBandSumAssured = 0;
            decimal lowerBandSumAssured = 0;

            // TODO: move this to helper function
            if (sumAssured > 25000 && sumAssured < 50000)
            {
                if (age <= 30)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 25000).UnderThrityRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 50000).UnderThrityRiskRate;
                    lowerBandSumAssured = 25000;
                    upperBandSumAssured = 50000;
                }
                else if (age > 30 && age < 51)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 25000).BetweenThrityOneAndFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 50000).BetweenThrityOneAndFiftyRiskRate;
                    lowerBandSumAssured = 25000;
                    upperBandSumAssured = 50000;
                }
                else
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 25000).OverFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 50000).OverFiftyRiskRate;
                    lowerBandSumAssured = 25000;
                    upperBandSumAssured = 50000;
                }

            }
            else if (sumAssured > 50000 && sumAssured < 100000)
            {
                if (age <= 30)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 50000).UnderThrityRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 100000).UnderThrityRiskRate;
                    lowerBandSumAssured = 50000;
                    upperBandSumAssured = 100000;
                }
                else if (age > 30 && age < 51)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 50000).BetweenThrityOneAndFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 100000).BetweenThrityOneAndFiftyRiskRate;
                    lowerBandSumAssured = 50000;
                    upperBandSumAssured = 100000;
                }
                else
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 50000).OverFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 100000).OverFiftyRiskRate;
                    lowerBandSumAssured = 50000;
                    upperBandSumAssured = 100000;
                }
            }
            else if (sumAssured > 100000 && sumAssured < 200000)
            {
                if (age <= 30)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 100000).UnderThrityRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 200000).UnderThrityRiskRate;
                    lowerBandSumAssured = 100000;
                    upperBandSumAssured = 200000;
                }
                else if (age > 30 && age < 51)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 100000).BetweenThrityOneAndFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 200000).BetweenThrityOneAndFiftyRiskRate;
                    lowerBandSumAssured = 100000;
                    upperBandSumAssured = 200000;
                }
                else
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 100000).OverFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 200000).OverFiftyRiskRate;
                    lowerBandSumAssured = 100000;
                    upperBandSumAssured = 200000;
                }
            }
            else if (sumAssured > 200000 && sumAssured < 300000)
            {
                if (age <= 30)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 200000).UnderThrityRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 300000).UnderThrityRiskRate;
                    lowerBandSumAssured = 200000;
                    upperBandSumAssured = 300000;
                }
                else if (age > 30 && age < 51)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 200000).BetweenThrityOneAndFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 300000).BetweenThrityOneAndFiftyRiskRate;
                    lowerBandSumAssured = 200000;
                    upperBandSumAssured = 300000;
                }
                else
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 200000).OverFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 300000).OverFiftyRiskRate;
                    lowerBandSumAssured = 200000;
                    upperBandSumAssured = 300000;
                }
            }
            else if (sumAssured > 300000 && sumAssured < 500000)
            {
                if (age <= 30)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 300000).UnderThrityRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 500000).UnderThrityRiskRate;
                    lowerBandSumAssured = 300000;
                    upperBandSumAssured = 500000;
                }
                else if (age > 30 && age < 51)
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 300000).BetweenThrityOneAndFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 500000).BetweenThrityOneAndFiftyRiskRate;
                    lowerBandSumAssured = 300000;
                    upperBandSumAssured = 500000;
                }
                else
                {
                    lowerBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 300000).OverFiftyRiskRate;
                    upperBandRiskRate = (decimal)bands.Find(x => x.SumAssured == 500000).OverFiftyRiskRate;
                    lowerBandSumAssured = 300000;
                    upperBandSumAssured = 500000;
                }
            }

            decimal riskRate = ((sumAssured - lowerBandSumAssured)
                / (upperBandSumAssured - lowerBandSumAssured)
                * upperBandRiskRate + (upperBandSumAssured - sumAssured)
                / (upperBandSumAssured - lowerBandSumAssured)
                * lowerBandRiskRate);

            return riskRate;
        }
    }
}
