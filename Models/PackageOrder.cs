namespace EcoPowerHub.Models
{
    public class PackageOrder //PackageOrder Relation
    {
        public int OrderId { get; set; }
        public int PackageId { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal[] ElectricityUsage { get; set; } = new decimal[6];
        public decimal ElectricityUsageAverage => ElectricityUsage.Average();
        public decimal PricePerYear => ElectricityUsageAverage * 12; // متوسط استهلاك الست شهور في السنه كلها 
        public float ROIYears  => (float)PackagePrice / (float)PricePerYear; //فترة سنين الاسترداد
        public float TotalYearsGuarantee { get; set; } // الباكدج مفترض تعيش كام سنه 
        public decimal SavingCost => PricePerYear * ((decimal)TotalYearsGuarantee - (decimal)ROIYears); //المبلغ ال هيتوفر

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
