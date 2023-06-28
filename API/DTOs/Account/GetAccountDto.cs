namespace API.DTOs.Account;

public class GetAccountDto
{
    public Guid Guid { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsUsed { get; set; }

    public int OTP { get; set; }

    public DateTime? ExpiredTime { get; set; }
}
