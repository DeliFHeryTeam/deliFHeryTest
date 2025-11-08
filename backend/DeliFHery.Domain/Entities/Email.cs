namespace DeliFHery.Domain;

public class Email(int participantId, string email)
{
    public int ParticipantId { get; set; } = participantId;
    public string email { get; set; } = email;
}