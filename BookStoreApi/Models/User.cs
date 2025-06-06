﻿namespace BookStoreApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
