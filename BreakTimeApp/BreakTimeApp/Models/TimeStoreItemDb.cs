namespace BreakTimeApp.Models
{
    public class TimeStoreItemDb
    {
        public string ID { get; set; }

        public string Span { get; set; }

        public bool IsRunning { get; set; }

        public int Icon { get; set; }

        public double Progress { get; set; }
        public double MaxProgress { get; set; }
    }
}
