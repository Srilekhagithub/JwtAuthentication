using System;
using System.Collections.Generic;

namespace JwtAuthentication.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime? CreateDate { get; set; }= DateTime.Now;

    public string? Roles { get; set; }
}
