using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Halls
{
    public class List
    {
        public class Query : IRequest<List<HallDto>> { }

        public class Handler : IRequestHandler<Query, List<HallDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;

            }
            public async Task<List<HallDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var halls = await _context.Halls
                .Include(a => a.Activities)
                .ToListAsync();

                var hallsToReturn = _mapper.Map<List<HallDto>>(halls);

                return hallsToReturn;
            }
        }
    }
}