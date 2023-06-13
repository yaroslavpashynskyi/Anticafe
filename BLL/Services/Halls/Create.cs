using DAL;
using DAL.Models;
using MediatR;

namespace BLL.Services.Halls
{
    public class Create
    {
        public class Command : IRequest
        {
            public Hall Hall { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Halls.Add(request.Hall);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
