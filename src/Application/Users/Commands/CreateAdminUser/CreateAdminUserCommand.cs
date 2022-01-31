using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Users.Commands;


public class CreateAdminUserCommand : IRequest<(Result Result, string UserId)>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string CustomerType { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}
public class CreateAdminUserCommandHandler : IRequestHandler<CreateAdminUserCommand, (Result Result, string UserId)>
{
    private readonly IIdentityService _identityService;
    public CreateAdminUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<(Result Result, string UserId)> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
    {

       return  await _identityService.CreateAdminAsync(request.UserName, request.Password, request.Email, request.PhoneNumber);
       
    }
}