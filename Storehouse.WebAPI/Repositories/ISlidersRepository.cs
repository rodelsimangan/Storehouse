using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface ISlidersRepository
    {
        List<Sliders> GetSliders(string tenantId);
        Sliders GetSlider(string id);
        void UpsertSlider(Sliders input);
    }
}
