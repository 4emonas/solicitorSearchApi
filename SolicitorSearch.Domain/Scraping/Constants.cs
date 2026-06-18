using System;
using System.Collections.Generic;
using System.Text;

namespace SolicitorSearch.Domain.Scraping
{
    internal static class Constants
    {
        public const string ResultItem = @"<div class=""result-item";
        public const string LinksHolder = @"<div class=""links-holder"">";
        public const string Div = "</div>";
        public const string HttpsHref = @"<a href=""https:";
        public const string NameClass = @"<span class=""h2"">";
        public const string AddressOpen = "<address>";
        public const string AddressClose = "</address>";
        public const string AHref = "<a href=\"/";
        public const string ItemSmall = "item-small";
        public const string PhoneTag = @"href=""tel:";
        public const string WebsiteTagOpen = @"<a target=""_blank"" href=""";
        public const string WebsiteTagClose= @""" rel=""nofollow""";
    };
}
