using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
   public  interface IContentSlidersRepository
    {
        List<ContentSliders> GetContentSliders(string tenantId);
        ContentSliders GetContentSlider(string id);
        void UpsertContentSlider(ContentSliders input);
        void MoveUpContentSlider(string id);
        void MoveDownContentSlider(string id);
    }
}
