using OrganicStore.DataContext;

namespace OrganicStore.Models
{
    public class FooterViewModel
    {
        public string? LogoUrl { get; set; }
        public List<SocialLink> SocialLinks { get; set; } = new();
        public List<ContactInfo> ContactInfos { get; set; } = new();
    }
}