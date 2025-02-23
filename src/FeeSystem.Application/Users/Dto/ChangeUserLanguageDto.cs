using System.ComponentModel.DataAnnotations;

namespace FeeSystem.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}