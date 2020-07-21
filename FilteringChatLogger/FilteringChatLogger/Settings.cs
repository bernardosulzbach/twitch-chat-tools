using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FilteringChatLogger
{
    public class Settings
    {
        [Required]
        [MinLength(1)]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^oauth:.{30}$", ErrorMessage = @"The OAuth (password) must start with ""oauth:"" and be followed by exactly 30 characters.")]
        [JsonPropertyName("password")]
        public string OAuth { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("path-to-chat-files")]
        public string ChatPath { get; set; }

        [Required]
        [MinLength(1)]
        [JsonPropertyName("path-to-log-files")]
        public string LogPath { get; set; }

        public static Settings FromJson(string json)
        {
            var settings = JsonSerializer.Deserialize<Settings>(json);
            var context = new ValidationContext(settings, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(settings, context, results, true);
            if (!isValid) throw new ArgumentException(string.Join(", ", results.Select(result => result.ErrorMessage)));

            return settings;
        }
    }
}