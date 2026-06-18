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
    public class GetSolicitorsHandler : IRequestHandler<GetSolicitorsRequest, GetSolicitorsResponse>
    {
        private readonly IHttpProxy _httpProxy;
        private readonly IScraper _scraper;
        private readonly IPostgresContext _context;
        public GetSolicitorsHandler(IHttpProxy httpProxy, IScraper scraper, IPostgresContext postgresContext)
        {
            _httpProxy = httpProxy;
            _scraper = scraper;
            _context = postgresContext;
        }

        public async Task<GetSolicitorsResponse> Handle(GetSolicitorsRequest request, CancellationToken cancellationToken)
        {
            var handlerResponse = new GetSolicitorsResponse();
            var htmlResponse = await _httpProxy.GetHtmlResponseAsync($"https://www.solicitors.com/conveyancing+{request.Location}.html", cancellationToken);
            handlerResponse.Solicitors.AddRange(_scraper.ExtractSolocitors(htmlResponse));
            
            GroupSameSolicitorOccurence(handlerResponse);
            
            await UseAndUpdateDbRecordsAsync(request, handlerResponse);

            return await Task.FromResult(handlerResponse);
        }

        /// <summary>
        /// Group solicitors by name and merge their addresses into a single string
        /// </summary>
        private void GroupSameSolicitorOccurence(GetSolicitorsResponse handlerResponse)
        {
            var group = handlerResponse.Solicitors
                .GroupBy(s => (s.Name ?? string.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g =>
                {
                    var addresses = g
                        .Select(x => x.Address)
                        .Where(a => !string.IsNullOrWhiteSpace(a))
                        .Select(a => a.Trim())
                        .Distinct();

                    return new Solicitor
                    {
                        Name = string.IsNullOrEmpty(g.Key) ? null : g.Key,
                        Address = string.Join(" | ", addresses),
                        PhoneNumber = g.Select(x => x.PhoneNumber).FirstOrDefault(p => !string.IsNullOrWhiteSpace(p)),
                        Website = g.Select(x => x.Website).FirstOrDefault(w => !string.IsNullOrWhiteSpace(w)),
                        ExtraInfoPage = g.Select(x => x.ExtraInfoPage).FirstOrDefault(e => !string.IsNullOrWhiteSpace(e)),
                        IsSmall = g.Any(x => x.IsSmall),
                        Rating = g.Select(x => x.Rating).FirstOrDefault(),
                        NumberOfReviews = g.Select(x => x.NumberOfReviews).FirstOrDefault(),
                    };
                })
                .ToList();

            handlerResponse.Solicitors = group;
        }

        /// <summary>
        /// Flags new records in the website that are not in the database.
        /// Also records the new records for the next time
        /// </summary>
        private async Task UseAndUpdateDbRecordsAsync(GetSolicitorsRequest request, GetSolicitorsResponse response)
        {
            var existingDict = await GetExistingSolicitorsAsync(request);
            var toAdd = new List<SolicitorEntity>();
            foreach (var solicitor in response.Solicitors)
            {
                var name = (solicitor.Name ?? string.Empty).Trim();
                if (existingDict.TryGetValue(name, out var notes))
                {
                    // Fetch notes from existing record
                    solicitor.Notes = notes;
                    solicitor.IsNew = false;
                }
                else
                {
                    // store new record
                    solicitor.IsNew = true;
                    var newSolicitorEntity = new SolicitorEntity(solicitor)
                    {
                        City = request.Location
                    };
                    toAdd.Add(newSolicitorEntity);

                    existingDict[name] = null; //dont use the same name again
                }
            }

            if (toAdd.Count > 0)
            {
                _context.Solicitors.AddRange(toAdd);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<Dictionary<string, string?>> GetExistingSolicitorsAsync(GetSolicitorsRequest request)
        {
            var existing = await _context.Solicitors
                .Where(s => s.City == request.Location)
                .Select(s => new
                {
                    Name = s.Name,
                    Notes = s.Notes
                })
                .ToListAsync();

            var existingDict = existing
                .GroupBy(x => x.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Notes, StringComparer.OrdinalIgnoreCase);
            return existingDict;
        }
    }
}
