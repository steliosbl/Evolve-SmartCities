namespace EvolvedAPI.Models
{
    using System;
    using System.Collections.Generic;

    public interface IModuleRepository
    {
        bool Add(Module module);
        IEnumerable<Module> GetAll();
        Module Get(int id);
        Module Get(double longitude, double latitude);
        IEnumerable<Module> Get(double longitude, double latitude, double radius);
        bool Remove(int id);
        Module Update(int id, string state, DateTime timestamp, TimeSpan duration);
    }
}
