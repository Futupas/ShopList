namespace ShopList.Models;

public record LoginRequest
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
}
