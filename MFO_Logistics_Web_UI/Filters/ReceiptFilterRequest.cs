namespace MFO_Logistics_Web_UI.Filters
{
    public class ReceiptFilterRequest
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? CreateDate2 { get; set; }
        public string? ReceiptCode { get; set; }
        public string? DepositorName { get; set; }
        public string? LogisticName { get; set; }
    }
}
