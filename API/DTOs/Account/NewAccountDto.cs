﻿using API.Utilities.Validation;

namespace API.DTOs.Account;

public class NewAccountDto
{
    public Guid Guid { get; set; }

    [PasswordPolicy]
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public int? OTP { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? ExpiredTime { get; set; }
}
