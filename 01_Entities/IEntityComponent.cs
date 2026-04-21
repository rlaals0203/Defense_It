using System;
using _01_Script.Entities;

namespace _01_Script._01_Entities
{
    public interface IEntityComponent
    {
        void Initialize(Entity entity);
    }
} 