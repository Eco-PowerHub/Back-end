using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPowerHub.Models
{
    public class OffGridPackage :BasePackage
    {
        public override string Name { get; set; }
        public decimal BatteryCapacity { get; set; }
        public int BatteryLifespan { get; set; }
     
    }
}
