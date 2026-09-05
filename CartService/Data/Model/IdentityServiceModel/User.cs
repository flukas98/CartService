using System;
using CartService.Data.ViewModel;

namespace CartService.Data.Model.IdentityServiceModel
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName => string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName)
            ? string.Empty
            : $"{FirstName} {LastName}".Trim();

        public static User FromViewModel(UserViewModel? vm)
        {
            if (vm == null) return new User();

            return new User
            {
                Id = vm.Id == Guid.Empty ? Guid.NewGuid() : vm.Id,
                Username = vm.Username,
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName
            };
        }
    }
}
