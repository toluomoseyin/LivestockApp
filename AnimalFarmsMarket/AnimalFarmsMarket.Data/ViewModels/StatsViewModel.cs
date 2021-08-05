namespace AnimalFarmsMarket.Data.ViewModels
{
    public class StatsViewModel
    {
        public string[] PaymentDates { get; set; }
        public int[] CompletePayments { get; set; }
        public int[] IncompletePayments { get; set; }
        
        public string[] OrderDates { get; set; }
        public int[] NumOfOrders { get; set; }
    }
}