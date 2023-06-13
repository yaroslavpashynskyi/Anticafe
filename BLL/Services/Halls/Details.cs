using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Halls
{
    public class Details
    {
        public class Query : IRequest<HallDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, HallDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<HallDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var hall = await _context.Halls
                .Include(a => a.Activities)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

                var hallToReturn = _mapper.Map<HallDto>(hall);
                return hallToReturn;
            }
        }
    }
}

