using DAL;
using DAL.Models;
using MediatR;

namespace BLL.Services.Halls
{
    public class Delete
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
                Console.WriteLine("\n\n\n");
                var hall = await _context.Halls.FindAsync(request.Id);
                foreach (var item in hall.Activities)
                {
                    _context.Remove(item);
                }
                _context.Remove(hall);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}