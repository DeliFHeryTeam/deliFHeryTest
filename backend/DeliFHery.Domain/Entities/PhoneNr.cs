namespace DeliFHery.Domain;

public class PhoneNr(int participantId, string phoneNr )
{
    public int ParticipantId { get; set; } = participantId;
    public string Number { get; set; } = phoneNr;
}