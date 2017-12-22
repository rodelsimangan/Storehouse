using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storehouse.Model;

namespace Storehouse.WebAPI.Repositories
{
    public interface IFeedbacksRepository
    {
        List<Feedbacks> GetFeedbacks(string tenantId, bool isMember);
        Feedbacks GetFeedback(string id);
        void UpsertFeedback(Feedbacks input);
    }
}
