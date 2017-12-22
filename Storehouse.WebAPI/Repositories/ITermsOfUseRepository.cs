using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface ITermsOfUseRepository
    {
        TermsofUse GetTermsOfUse(string id);
        void UpsertTermsOfUse(TermsofUse input);
    }
}
