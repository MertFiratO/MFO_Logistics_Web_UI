using MFO_Logistics_Web_UI.Models.DTOs;
using MFO_Logistics_Web_UI.Models.Requests;

namespace MFO_Logistics_Web_UI.Models.ViewModels
{
    public class ReceiptSearchViewModel
    {
        public ReceiptFilterRequest Filter { get; set; } = new();

        public List<ReceiptSearchDTO> Receipts { get; set; } = new();

        public bool IsSearched { get; set; }

    }
}
