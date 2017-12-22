using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface IContentRepository
    {
        List<Contents> GetContents(string parentId);
        Contents GetContent(string id);
        void UpsertContent(Contents input);
    }
}
