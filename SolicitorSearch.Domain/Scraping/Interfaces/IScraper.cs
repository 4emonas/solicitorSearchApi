using System;
using System.Collections.Generic;
using System.Text;
using SolicitorSearch.Common.Models;

namespace SolicitorSearch.Domain.Scraping.Interfaces
{
    public interface IScraper
    {
        List<Solicitor> ExtractSolocitors(string htmlSource);
        void EnrichSolicitor(Solicitor solicitor, string htmlSource);
    }
}
