using MediatR;
using SolicitorSearch.Domain.Implementation.Responses;
using SolicitorSearch.Common.Models;

namespace SolicitorSearch.Domain.Implementation.Requests
{
    public class GetSolicitorRequest : IRequest<GetSolicitorResponse>
    {
        public GetSolicitorRequest(Solicitor solicitor)
        {
            Solicitor = solicitor;
        }

        public Solicitor Solicitor{ get; }
        public string Location { get;set; }
    }
}
