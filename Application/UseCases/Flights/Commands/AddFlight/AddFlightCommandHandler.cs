using Application.Abstractions.Cqrs;
using Application.Common.Models;
using Application.Dtos.Outgoing;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Flights.Commands.AddFlight
{
    public class AddFlightCommandHandler : ICommandHandler<AddFlightCommand, FlightResponse>
    {
        private IFlightsRepository _flightsRepository;
        private IUnitOfWork _unitOfWork;

        public AddFlightCommandHandler(IFlightsRepository flightsRepository, IUnitOfWork unitOfWork)
        {
            _flightsRepository = flightsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<FlightResponse>> Handle(AddFlightCommand request, CancellationToken cancellationToken)
        {
            var result = await _flightsRepository.AddAsync(request.Dto.ToFlight());

            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<FlightResponse>(true, new FlightResponse(result));
        }
    }
}
