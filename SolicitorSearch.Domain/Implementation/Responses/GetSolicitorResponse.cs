
using SolicitorSearch.Common.Models;

namespace SolicitorSearch.Domain.Implementation.Responses
{
    public sealed class GetSolicitorResponse
    {
        public GetSolicitorResponse(Solicitor solicitor)
        {
            Solicitor = solicitor;
        }
        public Solicitor Solicitor { get; set; }
    }
}
