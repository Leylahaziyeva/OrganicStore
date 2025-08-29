namespace OrganicStore.DataContext
{
    public class ContactInfo : BaseEntity
    {
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
    }
}
