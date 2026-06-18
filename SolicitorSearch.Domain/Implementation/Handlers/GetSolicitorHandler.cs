using MediatR;
using Microsoft.EntityFrameworkCore;
using SolicitorSearch.DataAccess.Contexts.Abstract;
using SolicitorSearch.Domain.Implementation.Requests;
using SolicitorSearch.Domain.Implementation.Responses;
using SolicitorSearch.Domain.Scraping.Interfaces;
using SolicitorSearch.Utilities.HttpAccess.Interfaces;

namespace SolicitorSearch.Domain.Implementation.Handlers
{
    public class GetSolicitorHandler : IRequestHandler<GetSolicitorRequest, GetSolicitorResponse>
    {
        private readonly IHttpProxy _httpProxy;
        private readonly IScraper _scraper;
        private readonly IPostgresContext _context;
        public GetSolicitorHandler(IHttpProxy httpProxy, IScraper scraper, IPostgresContext postgresContext)
        {
            _httpProxy = httpProxy;
            _scraper = scraper;
            _context = postgresContext;
        }

        public async Task<GetSolicitorResponse> Handle(GetSolicitorRequest request, CancellationToken cancellationToken)
        {
            var htmlResponse = await _httpProxy.GetHtmlResponseAsync($"https://www.solicitors.com/{request.Solicitor.ExtraInfoPage}", cancellationToken);
            _scraper.EnrichSolicitor(request.Solicitor, htmlResponse);

            var handlerResponse = new GetSolicitorResponse(request.Solicitor);
            handlerResponse.Solicitor.Notes = await FetchNotesAsync(request, cancellationToken);
            return await Task.FromResult(handlerResponse);
        }

        private async Task<string> FetchNotesAsync(GetSolicitorRequest request, CancellationToken cancellationToken)
        {
            var existingSolicitorEntry = await _context.Solicitors
                .FirstOrDefaultAsync(_ => _.Name == request.Solicitor.Name
                                && _.City == request.Location, cancellationToken);

            return existingSolicitorEntry?.Notes ?? string.Empty;
        }
    }
}
