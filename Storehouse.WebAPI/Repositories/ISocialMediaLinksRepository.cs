using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface ISocialMediaLinksRepository
    {
        List<SocialMediaLinks> GetSocialMediaLinks(string tenantId);
        SocialMediaLinks GetSocialMediaLink(string id);
        void UpsertSocialMediaLink(SocialMediaLinks input);
    }
}
