﻿using BlazorClient.DTO.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorClient.DTO.Results;

public class CreatePersonResult
{
    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public string Gender { get; set; }

    [MaxLength(80)]
    public string Address { get; set; }
    public Guid? CountryId { get; set; }

    public List<CountryModel> Countries { get; set; } = null!;
	public List<string> Genders { get; set; } = null!;
}