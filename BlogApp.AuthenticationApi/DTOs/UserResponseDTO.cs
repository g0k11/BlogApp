using BlogApp.AuthenticationApi.Entities;

namespace BlogApp.AuthenticationApi.DTOs
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } 

        public static UserResponseDTO FromEntity(UserEntity user)
        {
            return new UserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
            };
        }
    }
}