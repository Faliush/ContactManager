﻿namespace BlazorClient.DTO.Results
{
    public class PersonResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string? Address { get; set; }
        public string? CountryName { get; set; }
    }
}
