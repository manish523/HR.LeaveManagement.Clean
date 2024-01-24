using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Command.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _logger;
        private readonly IUserService _userService;

        public CreateLeaveRequestCommandHandler(
            IEmailSender emailSender,
            IMapper mapper,
            ILeaveTypeRepository leaveTypeRepository,
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveAllocationRepository leaveAllocationRepository,
            IAppLogger<CreateLeaveRequestCommandHandler> logger,
            IUserService userService)
        {
            _emailSender = emailSender;
            _mapper = mapper;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveAllocationRepository = leaveAllocationRepository;
            _logger = logger;
            _userService = userService;
        }
        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Request", validationResult);

            // Get requesting employee's id
            var employeeId = _userService.UserId;

            // Check on employee's allocation
            var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

            // If allocations aren't enough, return validation error with message
            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                    "You do not have any allocations for this leave type."));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            // If day requested exceeds total number of allocation days, return validation error with message
            int daysRequest = (int)(request.EndDate - request.StartDate).TotalDays;
            if (daysRequest > allocation.NumberOfDays)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.EndDate),
                    "You do not have enough days for this request."));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }


            // Create leave request
            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            leaveRequest.RequestingEmployeeId = employeeId;
            leaveRequest.DateRequested = DateTime.Now;
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            // Send confirmation email
            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty,
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                   $"has been created successfully.",
                    Subject = "Leave Request Created"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
            }

            return Unit.Value;
        }
    }
}
