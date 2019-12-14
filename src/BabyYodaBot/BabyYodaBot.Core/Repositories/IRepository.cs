using System;
using System.Collections.Generic;

namespace BabyYodaBot.Core.Repositories
{
    public interface IRepository<T>
    {
        T Store(T item);
        IReadOnlyList<T> All();
        void Remove(Predicate<T> predicate);
        void Save();
    }
}