using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface IHomePageSettingsRepository
    {
        HomePageSettings GetHomepageSettings(string tenantId, string id);
        void UpsertHomePageSettings(HomePageSettings input);
    }
}
