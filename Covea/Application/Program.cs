using System;

namespace Covea
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Please enter a sum assured");
            int sum = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter your age");
            int age = Convert.ToInt32(Console.ReadLine());

            var lifeInsuranceCalculation = new CalculateLifeInsurance();

            var result = lifeInsuranceCalculation.CalculateInsurancePremium(sum, age);

            if (result.ErrorMessage != "")
            {
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Your sum Assured is: £" + Math.Round(result.SumAssured, 2) + ", Your age is " + age + " and your gross premium payable is: £" + Math.Round(result.InsurancePremium, 2));
            }

        }
    }
}
