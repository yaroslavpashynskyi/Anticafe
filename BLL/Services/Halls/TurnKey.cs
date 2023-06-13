using DAL;
using DAL.Models;
using MediatR;

namespace BLL.Services.Halls
{
    public class Turnkey
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                var hall = await _context.Halls.FindAsync(request.Id);

                Console.WriteLine(hall.Turnkey);
                if (hall.Turnkey)
                {
                    hall.Turnkey = false;
                }
                else
                {
                    hall.Turnkey = true;
                }

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}