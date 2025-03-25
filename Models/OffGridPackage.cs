using EcoPowerHub.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoPowerHub.Models
{
    public class OffGridPackage :BasePackage
    {
        public decimal BatteryCapacity { get; set; }
        public int BatteryLifespan { get; set; }
    }
}
