using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class UserEntity : Entity<int?>
    {
        public string Name { get; set; }

        public int? DefaultPhotoId { get; set; }
    }
}