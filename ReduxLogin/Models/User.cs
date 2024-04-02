using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ReduxLogin.Models
{
	public class User
	{
		[Key]
		public int  UserId { get; set; }
		[Required]
        public string? UserName { get; set; }
		[Required]
        public string? Email { get; set; }
		[Required]
        [JsonIgnore] public string? Password{ get; set; }
		//public string? Role { get; set; }
	}
}

