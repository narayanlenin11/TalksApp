using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Users.Commands;

public class CreateHomeOrOfficeUserCommand : IRequest<(Result Result, string UserId)>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string CustomerType { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string? HOCompanyName { get; set; }
    public string? HOFax { get; set; }
}
public class CreateHomeOrOfficeUserCommandHandler : IRequestHandler<CreateHomeOrOfficeUserCommand, (Result Result, string UserId)>
{
    private readonly IIdentityService _identityService;
    public CreateHomeOrOfficeUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<(Result Result, string UserId)> Handle(CreateHomeOrOfficeUserCommand request, CancellationToken cancellationToken)
    {

        return await _identityService.CreateHomeOrOfficeCustomerAsync(request.UserName,
                                                                      request.Password,
                                                                      request.PhoneNumber,
                                                                      request.Address,
                                                                      request.Email,
                                                                      request.HOCompanyName,
                                                                      request.HOFax);
    }
}
