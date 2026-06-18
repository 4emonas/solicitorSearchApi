using MediatR;
using SolicitorSearch.Common.Models;
using SolicitorSearch.DataAccess.Contexts.Abstract;
using SolicitorSearch.DataAccess.Entities;
using SolicitorSearch.Domain.Implementation.Responses;
using SolicitorSearch.Domain.Scraping.Interfaces;
using Microsoft.EntityFrameworkCore;
using SolicitorSearch.Utilities.HttpAccess.Interfaces;
using SolicitorSearch.Domain.Implementation.Requests;

namespace SolicitorSearch.Domain.Implementation.Handlers
{
    public class UpdateSolicitorNotesHandler : IRequestHandler<UpdateSolicitorNotesRequest, UpdateSolicitorNotesResponse>
    {
        private readonly IPostgresContext _context;
        public UpdateSolicitorNotesHandler(IPostgresContext postgresContext)
        {
            _context = postgresContext;
        }

        public async Task<UpdateSolicitorNotesResponse> Handle(UpdateSolicitorNotesRequest request, CancellationToken cancellationToken)
        {
            var response = new UpdateSolicitorNotesResponse();
            var existingSolicitor = await _context.Solicitors.FirstOrDefaultAsync(_ => _.Name == request.Solicitor.Name, cancellationToken);
            if (existingSolicitor == null)
            {
                return response;
            }

            existingSolicitor.Notes = request.Solicitor.Notes;
            await _context.SaveChangesAsync(cancellationToken);

            response.Success = true;
            return response;
        }
    }
}
