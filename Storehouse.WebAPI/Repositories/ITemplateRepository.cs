using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface ITemplateRepository
    {
        List<Templates> GetTemplates(string tenantId);
        Templates GetTemplate(string id);
        void UpsertTemplate(Templates input);
    }
}
