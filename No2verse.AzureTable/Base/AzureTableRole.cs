using Microsoft.Azure.Cosmos.Table;
using System;

namespace No2verse.AzureTable.Base
{
    public class AzureTableRole : IRole
    {
        public string RoleKey { get; set; }
        public IPathSettings PathSettings { get; set; }

        public AzureStorageSettings AZSettings
        {
            get
            {
                return PathSettings as AzureStorageSettings;
            }
        }

        public AzureTableRole(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception("TableName cannot be empty.");
            }

            RoleKey = tableName.ToLower();

        }

        public AzureTableRole(string roleKey, AzureStorageSettings pathSettings) : this(roleKey)
        {

            if (pathSettings == null)
            {
                throw new Exception("PathSetting cannot be null.");
            }

            PathSettings = pathSettings;

            InitRole();
        }


        /// <summary>
        /// Test DataBase
        /// </summary>
        /// <returns></returns>
        protected internal bool InitRole()
        {
            var storageAccount = CloudStorageAccount.Parse(AZSettings.ConnectionString);

            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            var res = tableClient.TableClientConfiguration.CosmosExecutorConfiguration.CurrentRegion;

            return true;
        }

    }
}
