﻿using Microsoft.AspNetCore.Identity;

namespace Core.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<Request> Requests { get; set; }
}
