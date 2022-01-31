using CleanArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Identity;

public abstract class ApplicationUser : IdentityUser
{
    public string? CustomerType { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }

}
public class CorporateCustomer: ApplicationUser
{
    public string? CompanyName { get; set;}
    public string? CompanyAddress { get; set; }
    public string? CompanyPhone { get; set; }
    public string? CompanyPreferred { get; set; }
    public string? ShippingMethod { get; set; }
    public string? Fax { get; set; }

}
public class HomeOrOfficeCustomer : ApplicationUser
{
    public string? HOCompanyName { get; set; }
    public string? HOFax { get; set; }
}
public class StudentCustomer : ApplicationUser
{
    public string? School { get; set; }
}
public class Admin: ApplicationUser
{

}
