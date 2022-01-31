using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using MediatR;

namespace CleanArchitecture.Application.Users.Commands.CreateStudentUser;

public class CreateStudentUserCommand : IRequest<(Result Result, string UserId)>
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string CustomerType { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string? School { get; set; }
}
public class CreateStudentUserCommandHandler : IRequestHandler<CreateStudentUserCommand, (Result Result, string UserId)>
{
    private readonly IIdentityService _identityService;
    public CreateStudentUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<(Result Result, string UserId)> Handle(CreateStudentUserCommand request, CancellationToken cancellationToken)
    {
         return await _identityService.CreateStudentCustomerAsync(request.UserName,
                                                              request.Password,
                                                              request.PhoneNumber,
                                                              request.Address,
                                                              request.Email,
                                                              request.School);
    }
}

