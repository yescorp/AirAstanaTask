using Application.Abstractions.Cqrs;
using Application.Common.Models;
using Application.Dtos.Outgoing;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Commands.ChangeFlightStatus
{
    public class ChangeFlightStatusCommandHandler : ICommandHandler<ChangeFlightStatusCommand, FlightResponse>
    {
        private IFlightsRepository _flightsRepository;
        private IUnitOfWork _unitOfWork;

        public ChangeFlightStatusCommandHandler(IFlightsRepository flightsRepository, IUnitOfWork unitOfWork)
        {
            _flightsRepository = flightsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<FlightResponse>> Handle(ChangeFlightStatusCommand request, CancellationToken cancellationToken)
        {
            var flight = await _flightsRepository.GetByIdAsync(request.Id);

            if (flight == null)
            {
                return new BaseResponse<FlightResponse>(new Error("NotFound", $"Flight with id {request.Id} not found"));
            }

            flight.Status = request.Dto.Status;

            var result = await _flightsRepository.UpdateAsync(flight);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<FlightResponse>(new FlightResponse(result));
        }
    }
}
