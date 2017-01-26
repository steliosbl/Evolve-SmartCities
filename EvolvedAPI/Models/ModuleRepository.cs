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
            //using (var cmd = new MySqlCommand(string.Format("INSERT INTO {0} VALUES (@id, @hubid, @long, @lat, @red, @orange, @green, @current);", TableName), this.connection))
            //{
            //    cmd.Prepare();
            //    cmd.Parameters.AddWithValue("@id", module.ID);
            //    cmd.Parameters.AddWithValue("@hubid", module.HubID);
            //    cmd.Parameters.AddWithValue("@long", module.Longitude);
            //    cmd.Parameters.AddWithValue("@lat", module.Latitude);
            //    cmd.Parameters.AddWithValue("@red", module.RedDuration);
            //    cmd.Parameters.AddWithValue("@orange", module.OrangeDuration);
            //    cmd.Parameters.AddWithValue("@green", module.GreenDuration);
            //    cmd.Parameters.AddWithValue("@current", module.CurrentState);
            //}

            if (!this.Exists(module.ID))
            {
                SDatabase.MySQL.Convert.SerializeObject(this.connection, module, TableName);
                return true;
            }

            return false;
        }

        public IEnumerable<Module> GetAll()
        {
            var res = new List<Module>();
            using (var cmd = new MySqlCommand(string.Format("SELECT * FROM {0}", TableName), this.connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new Module(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), reader.GetDouble(3), reader.GetString(4), reader.GetDateTime(5), reader.GetTimeSpan(6)));
                    }
                }
            }

            return res;
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
            var res = new List<Module>();
            using (var cmd = new MySqlCommand(string.Format("SELECT * FROM {0} WHERE longitude BETWEEN @minlong AND @maxlong AND latitude between @minlat AND @maxlat", TableName), this.connection))
            {
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@minlong", longitude - radius);
                cmd.Parameters.AddWithValue("@maxlong", longitude + radius);
                cmd.Parameters.AddWithValue("@minlat", latitude - radius);
                cmd.Parameters.AddWithValue("@maxlat", latitude + radius);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new Module(reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2), reader.GetDouble(3), reader.GetString(4), reader.GetDateTime(5), reader.GetTimeSpan(6)));
                    }
                }
            }

            return res;
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

        public Module Update(int id, string state, DateTime timestamp, TimeSpan duration)
        {
            if (this.Exists(id))
            {
                using (var cmd = new MySqlCommand(string.Format("UPDATE {0} SET currentstate=@state,duration=@duration WHERE id=@id", TableName), this.connection))
                {
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@duration", duration.Subtract(DateTime.Now.Subtract(timestamp)));
                    cmd.ExecuteNonQuery();
                }
                
                return this.Get(id);
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
