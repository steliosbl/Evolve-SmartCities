namespace EvolvedAPI.Models
{
    using System;
    using System.Collections.Generic;

    public interface IModuleRepository
    {
        bool Add(Module module);
        IEnumerable<Module> GetAll();
        Module Get(int id);
        bool Remove(int id);
    }
}
