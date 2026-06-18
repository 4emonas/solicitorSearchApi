using SolicitorSearch.Common.Models;

namespace SolicitorSearch.Domain.Implementation.Responses
{
    public sealed class GetSolicitorsResponse
    {
        public List<Solicitor> Solicitors { get; set; } = new List<Solicitor>();
    }
}
