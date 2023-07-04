using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoBackend.Settings
{
    public class MongoDBSettings
    {
        public string? Password { get; set; }

        public string ConnectionString
        {
            get
            {
                //Use this connection string to connect with mongodb atlas cluster
                return $"mongodb+srv://leonardoapd:{Password}@clusterwebdev.hgis9.mongodb.net/RoommatesBD?retryWrites=true&w=majority";
                //return $"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}