using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Users.Commands;

public class CreateCorporateUserCommand : IRequest<(Result Result, string UserId)>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string CustomerType { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyPhone { get; set; }
    public string CompanyPreferred { get; set; }
    public string ShippingMethod { get; set; }
    public string Fax { get; set; }
}
public class CreateCorporateUserCommandHandler : IRequestHandler<CreateCorporateUserCommand, (Result Result, string UserId)>
{
    private readonly IIdentityService _identityService;
    public CreateCorporateUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<(Result Result, string UserId)> Handle(CreateCorporateUserCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.CreateCoporateCustomerAsync( request.UserName,
                                                                   request.Password,
                                                                   request.PhoneNumber,
                                                                   request.Address,
                                                                   request.Email,
                                                                   request.CompanyName,
                                                                   request.CompanyAddress,
                                                                   request.CompanyPhone,
                                                                   request.Fax,
                                                                   request.CompanyPreferred,
                                                                   request.ShippingMethod);

    }
}
