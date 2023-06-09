namespace GetsBets.Models
{
    public class Extraction
    {
        public DateOnly ExtractionDate { get; set; }
        public TimeOnly ExtractionTime { get; set; }
        public string Numbers { get; set; }
        public string Bonus { get; set; }
    }
}
