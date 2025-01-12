using System;
using UniRx;

namespace _App.Runtime.UI.Facts
{
    public interface IFactsPresenter : IDisposable
    {
        IReadOnlyReactiveCollection<BreedModel> Facts { get; }
        IReadOnlyReactiveProperty<bool> IsLoading { get; }
        IReadOnlyReactiveProperty<string> ErrorMessage { get; }
        void Initialize(IFactsView view);
    }
}