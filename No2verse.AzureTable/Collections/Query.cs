using No2verse.AzureTable.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace No2verse.AzureTable.Collections
{
    public class Query<T> : IAzureTableQuery<T> where T : DTableEntity, new()
    {
        private readonly AzureTableRole _Role = null;

        private CloudStorageAccount CloudStorageAccount { get; set; }

        private CloudTableClient CloudTableClient { get; set; }

        private CloudTable CloudTable;
        public string TableName { get; set; }


        public bool IsAutoGCOnDtor { get; set; }

        public AzureTableRole Role
        {
            get { return _Role; }
        }


        #region Ctor

        public Query(AzureTableRole role, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("TableName cant be null.");
            }
            if (role == null)
            {
                throw new ArgumentNullException("AzureTableRole cant be null.");
            }
            _Role = role;
            CloudStorageAccount = CloudStorageAccount.Parse(role.AZSettings.ConnectionString);
            TableName = tableName.ToLower();

            CloudTableClient = CloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());

            CloudTable = CloudTableClient.GetTableReference(tableName);
            var res = CloudTable.CreateIfNotExists();

        }

        #endregion



        public IEnumerable<T> All()
        {
            return CloudTable.ExecuteQuery(new TableQuery<T>(), null);

        }

        public T[] AllDatas()
        {
            var res = All();
            if (res != null)
            {
                return res.ToArray();
            }
            return null;
        }

        public List<T> AllDatasList()
        {
            var res = All();
            if (res == null)
            {
                return new List<T>();
            }
            return res.ToList();
        }




        public string[] AllPartitionKeys()
        {
            TableQuery qPK = new TableQuery() { SelectColumns = new List<string> { "PartitionKey" } };

            var res = CloudTable.ExecuteQuery(qPK, null);

            if (res != null)
            {
                var ids = res.Select(x => x.PartitionKey).Distinct().ToArray();

                return ids;
            }

            return null;
        }

        public string[] AllRowKeysByPartitionKey(string partitionKey)
        {
            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            var pkFilter = new TableQuery<TableEntity>()
               .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                 QueryComparisons.Equal, partitionKey));
            pkFilter.SelectColumns = new List<string>();
            pkFilter.SelectColumns.Add("RowKey");
            var entities = CloudTable.ExecuteQuery<TableEntity>(pkFilter);

            if (entities != null)
            {

                return entities.Select(x => x.RowKey).Distinct().ToArray();
            }
            return null;
        }

        public T DataByPRKey(string partitionKey, string rowKey)
        {


            return CloudTable.Execute(TableOperation.Retrieve<T>(partitionKey, rowKey)).Result as T;
        }

        public int DataCount()
        {
            var queryResult = CloudTable.ExecuteQuery(new TableQuery<TableEntity>(), null);

            if (queryResult == null)
            {
                return 0;
            }
            return queryResult.Count();
        }

        public int DataCountByPartitionKey(string partitionKey)
        {
            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            var pkFilter = new TableQuery<TableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                  QueryComparisons.Equal, partitionKey));
            pkFilter.SelectColumns = new List<string>();
            pkFilter.SelectColumns.Add("RowKey");
            var entities = CloudTable.ExecuteQuery<TableEntity>(pkFilter);

            if (entities != null)
            {
                return entities.Count();
            }
            return 0;
        }

        public List<T> DataListByKeys(params PRPair[] prpairs)
        {
            if (prpairs == null)
            {
                return new List<T>();
            }

            ConcurrentBag<T> r = new ConcurrentBag<T>();
            Parallel.ForEach(prpairs, (p) =>
            {
                try
                {
                    var t = DataByPRKey(p.PartitionKey, p.RowKey);
                    if (t != null)
                    {
                        r.Add(t);

                    }
                }
                catch
                {

                }
            });

            return r.ToList();

        }





        public bool IsDataExist(string partitionKey, string rowKey)
        {
            return (DataByPRKey(partitionKey, rowKey) != null);
        }

        public List<T> DatasByPartitionKey(string partitionKey)
        {
            if (string.IsNullOrEmpty(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            var pkFilter = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey",
                  QueryComparisons.Equal, partitionKey));
            var entities = CloudTable.ExecuteQuery<T>(pkFilter);

            if (entities != null)
            {
                return entities.ToList();
            }
            return new List<T>();
        }

        public List<T> DatasByRowKey(string rowKey)
        {
            if (string.IsNullOrEmpty(rowKey))
            {
                throw new ArgumentNullException(nameof(rowKey));
            }

            var pkFilter = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("RowKey",
                  QueryComparisons.Equal, rowKey));
            var entities = CloudTable.ExecuteQuery<T>(pkFilter);

            if (entities != null)
            {
                return entities.ToList();
            }
            return new List<T>();
        }
    }
}
