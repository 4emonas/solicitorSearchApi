using MediatR;
using SolicitorSearch.Common.Models;
using SolicitorSearch.Domain.Implementation.Responses;

namespace SolicitorSearch.Domain.Implementation.Requests
{
    public class UpdateSolicitorNotesRequest : IRequest<UpdateSolicitorNotesResponse>
    {
        public UpdateSolicitorNotesRequest(Solicitor solicitor)
        {
            Solicitor = solicitor;
        }

        public Solicitor Solicitor { get; }
    }
}
