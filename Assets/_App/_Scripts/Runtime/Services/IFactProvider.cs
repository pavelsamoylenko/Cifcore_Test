using System.Collections.Generic;
using System.Threading;
using _App.Runtime.UI.Facts;
using _App.Runtime.Web.DTO;
using Cysharp.Threading.Tasks;

namespace _App.Runtime.Services
{
    public interface IFactProvider
    {
        UniTask<List<BreedModel>> GetBreedsListAsync(CancellationToken cancellationToken);

        UniTask<DTOs.Breed> GetBreedByIdAsync(string id, CancellationToken cancellationToken);

    }
}