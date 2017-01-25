namespace EvolvedAPI.Models
{
    using System;
    using System.Collections.Generic;

    public interface IModuleRepository
    {
        void Add(Module module);
        IEnumerable<Module> GetAll();
        Module Find(int id);
        Module Remove(int id);
        void Update(Module module);
    }
}
