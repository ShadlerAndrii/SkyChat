namespace Skype.Data
{
    public interface IChatRecord
    {
        int Id { get; } // Record Id
        int ChatId { get; } // Chat Id
        int OwnerId { get; } // User`s Id
        DateTime InitDate { get; } // Date of creating a record
    }
}
