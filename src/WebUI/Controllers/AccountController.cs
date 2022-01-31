using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using CleanArchitecture.Application.Users.Commands;
using CleanArchitecture.Application.Users.Commands.CreateStudentUser;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.WebUI.Controllers;
public class AccountController : ApiControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public AccountController(UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             ILogger<AccountController> logger,
                             IConfiguration configuration, IApplicationDbContext context,
                             ICurrentUserService currentUserService, 
                             IIdentityService identityService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _configuration = configuration;
        _context = context;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    [AllowAnonymous]
    [HttpPost("/api/authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var identityUser = await _userManager.FindByNameAsync(authenticationRequest.UserName);

        if (identityUser == null || identityUser.IsActive == false)
            return Unauthorized();

        //this will sign the User in with a Cookie
        //var result = await _signInManager.PasswordSignInAsync(loginVM.PhoneNumber, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);

        //this is used if we don't need to issue a cookie 
        var result = await _signInManager.CheckPasswordSignInAsync(identityUser, authenticationRequest.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
            return Unauthorized();

        List<Claim> userClaims = await ConstructUserClaimsAsync(identityUser);

        JwtSecurityToken token = GenerateJwtToken(userClaims);

        var tokenResult = new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        };

        _logger.LogInformation("User logged in.");

        return Ok(tokenResult);
    }

    [AllowAnonymous]
    [HttpPost("/api/users/admin")]
    public async Task<IActionResult> CreateAdminUser(CreateAdminUserCommand command)
    {

        (Result result, string userId ) = await Mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
    [AllowAnonymous]
    [HttpPost("/api/users/corporateCustomer")]
    public async Task<IActionResult> CreateCorporateUser(CreateCorporateUserCommand command)
    {

        (Result result, string userId) = await Mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
    [AllowAnonymous]
    [HttpPost("/api/users/homeOrOfficeCustomer")]
    public async Task<IActionResult> CreateHomeorOfficeUser(CreateHomeOrOfficeUserCommand command)
    {

        (Result result, string userId) = await Mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
    [AllowAnonymous]
    [HttpPost("/api/users/studentCustomer")]
    public async Task<IActionResult> CreatStudentUser(CreateStudentUserCommand command)
    {

        (Result result, string userId) = await Mediator.Send(command);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return new StatusCodeResult(StatusCodes.Status201Created);
    }
    private JwtSecurityToken GenerateJwtToken(List<Claim> userClaims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:JwtKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _configuration["Tokens:JwtIssuer"],
            audience: _configuration["Tokens:JwtAudience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Tokens:JwtValidMinutes"])),
            signingCredentials: creds
            );

        return token;
    }
    public class AuthenticationRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    private async Task<List<Claim>> ConstructUserClaimsAsync(ApplicationUser identityUser)
    {
        var roles = await _userManager.GetRolesAsync(identityUser);
        List<Claim> roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

        // other claims of User if present
        List<Claim> userClaims = (await _userManager.GetClaimsAsync(identityUser)).ToList();

        userClaims = userClaims.Union(roleClaims).ToList();

        userClaims = new List<Claim>(userClaims)
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, identityUser.UserName),
                new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                new Claim("CustomerType", identityUser.CustomerType)
            };

        return userClaims;
    }
    public class  CreateApplicationUserCommand
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CustomerType { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string Address { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyPreferred { get; set; }
        public string? ShippingMethod { get; set; }
        public string? Fax { get; set; }
        public string? HOCompanyName { get; set; }
        public string? HOFax { get; set; }
        public string? School { get; set; }
    }
}
