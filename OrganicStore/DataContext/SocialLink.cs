namespace OrganicStore.DataContext
{
    public class SocialLink : BaseEntity
    {
        public required string Platform { get; set; }
        public string Url { get; set; } = "#";
        public required string Icon { get; set; }
    }
}