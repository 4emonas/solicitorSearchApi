using MediatR;
using SolicitorSearch.Domain.Implementation.Responses;

namespace SolicitorSearch.Domain.Implementation.Requests
{
    public class GetSolicitorsRequest : IRequest<GetSolicitorsResponse>
    {
        public GetSolicitorsRequest(string location)
        {
            Location = location;
        }

        public string Location { get; }
    }
}
