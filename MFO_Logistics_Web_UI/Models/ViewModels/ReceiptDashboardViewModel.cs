namespace MFO_Logistics_Web_UI.Models.ViewModels
{
    public class ReceiptDashboardViewModel
    {
        public int Completed { get; set; }
        public int Waiting { get; set; }
        public int Cancelled { get; set; }
        public int InProgress { get; set; }
    }
}
