namespace DomainFramework.Tests
{
    public class PersonWithFriendsDto
    {
        public string FirstName { get; set; }

        public FriendDto[] Friends { get; set; }
    }
}