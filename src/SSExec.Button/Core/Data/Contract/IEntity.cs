using System;

namespace SSExec.Button.Core.Data.Contract
{
    public interface IEntity<out TKey> : IEntity
    {
        TKey Id { get; }
    }


    public interface IEntity
    {
    }
}