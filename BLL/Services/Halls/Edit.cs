using AutoMapper;
using DAL;
using DAL.Models;
using MediatR;

namespace BLL.Services.Halls
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Hall Hall { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var hall = await _context.Halls.FindAsync(request.Hall.Id);

                request.Hall.Activities = hall.Activities;

                _mapper.Map(request.Hall, hall);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}