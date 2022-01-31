using CleanArchitecture.Application.Common.Models;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateAdminAsync(string userName,string password, string email, string phoneNumber);      
    Task<(Result Result, string UserId)> CreateCoporateCustomerAsync(string userName, string password,string phoneNumber,string address, string email, string companyName, string companyAddress, string companyPhone, string fax, string companyPreferred, string shippingMethod);
    Task<(Result Result, string UserId)> CreateHomeOrOfficeCustomerAsync(string userName, string password, string phoneNumber, string address, string email,string hOCompanyName, string hOFax);
    Task<(Result Result, string UserId)> CreateStudentCustomerAsync(string userName, string password, string phoneNumber, string address, string email,string school);

    Task<Result> DeleteUserAsync(string userId);
}
