﻿using System.ComponentModel.DataAnnotations;

namespace CodeRoom.ViewModels
{
    public class LoginVM
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
