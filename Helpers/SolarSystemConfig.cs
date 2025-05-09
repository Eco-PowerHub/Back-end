namespace EcoPowerHub.Helpers
{
    public class SolarSystemConfig
    {

        // مساحة اللوح الشمسي الواحد (متر مربع)
        public decimal SinglePanelArea { get; } = 2.0m;

        // متوسط ساعات الشمس اليومية
        public decimal SunlightHours { get; } = 5.5m;

        // معامل زيادة قوة العاكس
        public decimal InverterPowerFactor { get; } = 1.3m;
    }
}
