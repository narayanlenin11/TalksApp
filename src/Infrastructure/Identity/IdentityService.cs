using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateAdminAsync(string userName, string password, string email, string phoneNumber)
    {
        var user = new Admin
        {
            UserName = userName,
            Email = email,
            IsActive = true,
            PhoneNumber = phoneNumber
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    public  async Task<(Result Result, string UserId)> CreateCoporateCustomerAsync(string userName, string password, string phoneNumber, string address, string? email, string companyName, string companyAddress, string companyPhone, string fax, string companyPreferred, string shippingMethod)
    {
        var user = new CorporateCustomer
        {
            UserName = userName,
            PhoneNumber = phoneNumber,
            Address = address,
            Email = email,
            CompanyName = companyName,
            CompanyAddress = companyAddress,
            CompanyPhone = companyPhone,
            Fax= fax,
            CompanyPreferred = companyPreferred,
            ShippingMethod = shippingMethod,
            IsActive = true,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<(Result Result, string UserId)> CreateHomeOrOfficeCustomerAsync(string userName, string password, string phoneNumber, string address, string email, string hOCompanyName, string hOFax)
    {
        var user = new HomeOrOfficeCustomer
        {
            UserName = userName,
            PhoneNumber = phoneNumber,
            Address = address,
            Email = email,
            HOCompanyName = hOCompanyName,
            HOFax = hOFax,
            IsActive = true,
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<(Result Result, string UserId)> CreateStudentCustomerAsync(string userName, string password, string phoneNumber, string address, string email, string school)
    {
        var user = new StudentCustomer
        {
            UserName = userName,
            PhoneNumber = phoneNumber,
            Address = address,
            Email = email,
            School = school,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }
}
