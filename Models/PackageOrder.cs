using EcoPowerHub.Helpers;

namespace EcoPowerHub.Models
{
    public class PackageOrder //Proprty
    {
        public int OrderId { get; set; }
        public int PackageId { get; set; }
        public PropertyType Type { get; set; }
        public string Location { get; set; }
        public decimal SurfaceArea { get; set; }
        public decimal PackagePrice { get; set; }
        public PackageType Package {  get; set; }
        public BasePackage package { get; set; }
        public decimal[] ElectricityUsage { get; set; } = new decimal[6];

        public decimal ElectricityUsageAverage => ElectricityUsage.Length > 0 ? ElectricityUsage.Average() : 0;
        public decimal PricePerYear => ElectricityUsageAverage * 12;

        public float ROIYears => PricePerYear > 0 ? (float)(PackagePrice / PricePerYear) : 0;
        public float TotalYearsGuarantee { get; set; }

        public decimal SavingCost => TotalYearsGuarantee > ROIYears ? PricePerYear * ((decimal)TotalYearsGuarantee - (decimal)ROIYears) : 0;

        //public void InputBills()
        //{
        //    Console.WriteLine("Enter the amount for last 6 months:");
        //    for (int i = 0; i < ElectricityUsage.Length; i++)
        //    {
        //        decimal amount;
        //        while (true)
        //        {
        //            Console.Write($"Month {i + 1}: ");
        //            if (decimal.TryParse(Console.ReadLine(), out amount) && amount >= 0)
        //            {
        //                ElectricityUsage[i] = amount;
        //                break;
        //            }
        //            Console.WriteLine("Please enter a valid positive number.");
        //        }
        //    }
        //}

    }
}
