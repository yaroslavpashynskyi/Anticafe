using DAL;
using DAL.Models;
using MediatR;

namespace BLL.Services.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid HallId { get; set; }
            public Activity Activity { get; set; }
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
                var activity = request.Activity;
                activity.HallId = request.HallId;
                activity.Hall = await _context.Halls.FindAsync(request.HallId);

                _context.Activities.Add(activity);

                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
