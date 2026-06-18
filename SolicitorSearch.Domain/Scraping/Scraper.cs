using SolicitorSearch.Common.Models;
using SolicitorSearch.Domain.Scraping.Interfaces;
using SolicitorSearch.Utilities.Helpers;
using System.Text.RegularExpressions;
using Constants = SolicitorSearch.Domain.Scraping.Constants;

namespace SolicitorSearch.Utilities.Scraping
{
    public class Scraper : IScraper
    {
        public List<Solicitor> ExtractSolocitors(string source)
        {
            var retList = new List<Solicitor>();
            var currentIndex = source.IndexOf(Constants.ResultItem);
            while (currentIndex >= 0)
            {
                var solicitorDiv = GetNextSolicitorDiv(source, currentIndex);
                var solicitor = GetSolicitorFromDiv(solicitorDiv);
                retList.Add(solicitor);

                currentIndex = GetNextIndex(source, currentIndex);
            }

            return retList;
        }

        public void EnrichSolicitor(Solicitor solicitor, string htmlSource)
        {
            var linksDiv = ExtractBetween(htmlSource, Constants.LinksHolder, Constants.Div);
            var website = $"https:{ExtractBetween(htmlSource, Constants.HttpsHref, "\"")}";
            solicitor.Website = website;
        }

        private int GetNextIndex(string source, int currentIndex)
        {
            return source.IndexOf(Constants.ResultItem, currentIndex + 1);
        }

        private string GetNextSolicitorDiv(string source, int currentIndex)
        {
            var closedDivIndex = GetNextIndex(source, currentIndex);

            //this will happen when we scrape the last div. If last </div> still cannot be found, return empty string
            if (closedDivIndex < 0)
            {
                closedDivIndex = source.LastIndexOf(Constants.Div);
                if (closedDivIndex < 0)
                {
                    return string.Empty;
                }
            }
            return source.Substring(currentIndex, closedDivIndex - currentIndex);
        }

        private Solicitor GetSolicitorFromDiv(string source)
        {
            var name = ExtractBetween(source, Constants.NameClass, "<").Normalise();
            var address = ExtractBetween(source, Constants.AddressOpen, Constants.AddressClose).Normalise();
            var extraInfoPage = ExtractBetween(source, Constants.AHref, "\"");
            var isSmall = source.Contains(Constants.ItemSmall);
            var phone = ExtractBetween(source, Constants.PhoneTag, @""">");
            var rating = ExtractRating(source);
            var numberOfReviews = rating == null ? null : ExtractNumberOfReviews(source);


            var solicitor = new Solicitor
            {
                Name = name,
                PhoneNumber = phone,
                Address = address,
                ExtraInfoPage = extraInfoPage,
                IsSmall = isSmall,
                Rating = rating,
                NumberOfReviews = numberOfReviews
            };

            if (!isSmall)
            {
                solicitor.Website = ExtractBetween(source, Constants.WebsiteTagOpen, Constants.WebsiteTagClose);
            }

            return solicitor;
        }

        public string ExtractBetween(string source, string start, string end)
        {
            var startIndex = source.IndexOf(start);
            if (startIndex < 0)
                return string.Empty;

            var startIndexWithOffstet = source.IndexOf(start) + start.Length;
            var endIndex = source.IndexOf(end, startIndexWithOffstet);
            if (endIndex < 0)
                return string.Empty;

            return source.Substring(startIndexWithOffstet, endIndex - startIndexWithOffstet);
        }

        private decimal? ExtractRating(string source)
        {
            var ratingString = source.Split("star-");
            var fullStars = ratingString.Count(s => s.StartsWith("full"));
            var halfStars = ratingString.Count(s => s.StartsWith("half"));

            var rating = fullStars + (halfStars * 0.5m);
            return rating > 0 ? rating : null;
        }

        private int? ExtractNumberOfReviews(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            var parenMatch = Regex.Match(source, "\\(([0-9]+)\\)");
            if (parenMatch.Success && parenMatch.Groups.Count > 1)
            {
                return int.TryParse(parenMatch.Groups[1].Value, out var v) ? v : null;
            }

            return null;
        }
    }
}
