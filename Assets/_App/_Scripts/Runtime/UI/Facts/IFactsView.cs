using System;

namespace _App.Runtime.UI.Facts
{
    public interface IFactsView
    {
        IObservable<FactScrollElementView> OnElementClicked { get; }
        void Add(BreedModel fact);
        void Remove(BreedModel fact);
        void Clear();
        void SetLoading(bool isLoading);
        void SetError(string errorMessage);
    }
}