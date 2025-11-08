namespace DeliFHery.Domain;

public class Customer(int id, string userName, int? authId, IEnumerable<Email> mailContacts, IEnumerable<PhoneNr> phoneContacts)
{
    public int Id { get; set; } = id;
    public string UserName { get; set; } =
        userName ?? throw new ArgumentNullException(nameof(userName));
    public int? AuthId { get; set; } = authId;
    
    public IEnumerable<Email> EmailContacts { get; set; } = mailContacts;
    public IEnumerable<PhoneNr> PhoneContacts { get; set; } = phoneContacts;
}