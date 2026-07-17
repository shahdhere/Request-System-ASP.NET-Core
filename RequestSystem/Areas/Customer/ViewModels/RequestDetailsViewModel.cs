using System;

namespace RequestSystem.Areas.Customer.ViewModels
{
    public class RequestDetailsViewModel
    {
        public Guid Id { get; set; }
        public string RequestType { get; set; }
        public string Country { get; set; }
        public int RequestNo { get; set; }
        public string ShortDescription { get; set; }
        public string HealthImpactDescription { get; set; }
        public string SolutionMaturity { get; set; }
        public string TechnologyCategory { get; set; }
        public string MarketSize { get; set; }
        public string TargetAudience { get; set; }
        public Guid? AttachmentId { get; set; }
        public string Status { get; set; }
        public string ActionRequired { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
