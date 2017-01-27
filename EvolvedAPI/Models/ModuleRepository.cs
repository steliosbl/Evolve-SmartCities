namespace EvolvedAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MySql.Data.MySqlClient;
    using SDatabase.MySQL;

    public class ModuleRepository : IModuleRepository
    {
        const string ConfigFilename = "EvolvedAPI.cfg";
        const string TableName = "modules";

        private ConnectionString connectionString;
        private ConnectionData connectionData;
        private MySqlConnection connection;

        public ModuleRepository()
        {
            if (!System.IO.File.Exists(ConfigFilename))
            {
                throw new Exception("Configuration file not found." + System.IO.Directory.GetCurrentDirectory());
            }

            this.connectionData = JFI.GetObject<ConnectionData>(ConfigFilename);
            this.connectionString = new ConnectionString(this.connectionData);
            this.connection = new MySqlConnection(this.connectionString.Text);
            this.connection.Open();
        }

        public bool Add(Module module)
        {
            if (!this.Exists(module.ID))
            {
                SDatabase.MySQL.Convert.SerializeObject(this.connection, module, TableName);
                return true;
            }

            return false;
        }

        public IEnumerable<Module> GetAll()
        {
            using (var cmd = new MySqlCommand(string.Format("SELECT * FROM {0}", TableName), this.connection))
            {
                return SDatabase.MySQL.Convert.DeserializeObjects<Module>(cmd);
            }
        }

        public Module Get(int id)
        {
            if (this.Exists(id))
            {
                using (var cmd = new MySqlCommand(string.Format("SELECT * FROM {0} WHERE id=@id", TableName), this.connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);
                    return SDatabase.MySQL.Convert.DeserializeObject<Module>(cmd);
                }
            }

            return null;
        }

        public Module Get(double longitude, double latitude)
        {
            if (this.Exists(longitude, latitude))
            {
                using (var cmd = new MySqlCommand(string.Format("SELECT * FROM {0} WHERE longitude=@long AND latitude=@lat", TableName), this.connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@long", longitude);
                    cmd.Parameters.AddWithValue("@lat", latitude);
                    return SDatabase.MySQL.Convert.DeserializeObject<Module>(cmd);
                }
            }

            return null;
        }

        public IEnumerable<Module> Get(double longitude, double latitude, double radius)
        {
            using (var cmd = new MySqlCommand(string.Format("SELECT * FROM {0} WHERE longitude BETWEEN @minlong AND @maxlong AND latitude between @minlat AND @maxlat", TableName), this.connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@minlong", longitude - radius);
                cmd.Parameters.AddWithValue("@maxlong", longitude + radius);
                cmd.Parameters.AddWithValue("@minlat", latitude - radius);
                cmd.Parameters.AddWithValue("@maxlat", latitude + radius);

                return SDatabase.MySQL.Convert.DeserializeObjects<Module>(cmd);
            }
        }

        public bool Remove(int id)
        {
            if (this.Exists(id))
            {
                using (var cmd = new MySqlCommand(string.Format("DELETE FROM {0} WHERE id=@id", TableName), this.connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            return false;
        }

        public Module Update(UpdateInfo info)
        {
            if (this.Exists(info.ID))
            {
                using (var cmd = new MySqlCommand(string.Format("UPDATE {0} SET currentstate=@state,duration=@duration WHERE id=@id", TableName), this.connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", info.ID);
                    cmd.Parameters.AddWithValue("@state", info.State);
                    cmd.Parameters.AddWithValue("@duration", info.Duration.Subtract(DateTime.Now.Subtract(info.Timestamp)));
                    cmd.ExecuteNonQuery();
                }
                
                return this.Get(info.ID);
            }

            return null;
        }

        public bool Exists(int id)
        {
            using (var cmd = new MySqlCommand(string.Format("SELECT EXISTS(SELECT * FROM {0} WHERE id = @id LIMIT 1);", TableName), this.connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public bool Exists(double longitude, double latitude)
        {
            using (var cmd = new MySqlCommand(string.Format("SELECT EXISTS(SELECT * FROM {0} WHERE longitude = @long AND latitude = @lat LIMIT 1);", TableName), this.connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@long", longitude);
                cmd.Parameters.AddWithValue("@lat", latitude);
                return System.Convert.ToBoolean(cmd.ExecuteScalar());
            }
        }

        public int Count()
        {
            using (var cmd = new MySqlCommand(string.Format("SELECT COUNT(*) FROM {0}", TableName), this.connection))
            {
                return System.Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
