using Microsoft.Azure.Cosmos.Table;
using No2verse.AzureTable.Base;
using System;

namespace No2verse.AzureTable.Collections
{
    public class Operator : IAzureTableOperator
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="tableName"></param>
        public Operator(AzureTableRole role, string tableName, bool needToCheckExisted = false)
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

            if (needToCheckExisted)
            {
                var res = CloudTable.CreateIfNotExists();
            }
        }



        #endregion


        public bool Delete(string partitionKey, string rowKey)
        {
            if (string.IsNullOrEmpty(partitionKey) && string.IsNullOrEmpty(rowKey))
            {
                throw new ArgumentNullException("PK and RK cant be null both.");
            }


            // if (string.IsNullOrEmpty(partitionKey)) throw new ArgumentNullException(nameof(partitionKey));
            //   if (string.IsNullOrEmpty(rowKey)) throw new ArgumentNullException(nameof(rowKey));
            return Core.AzureTableDataWriter.DeleteData(partitionKey, rowKey, ref CloudTable);
        }

        public bool DeleteByPK(string partitionKey)
        {
            //   if (string.IsNullOrEmpty(partitionKey)) throw new ArgumentNullException(nameof(partitionKey));
            return Core.AzureTableDataWriter.DeleteDataByPK(partitionKey, ref CloudTable);
        }

        public bool DeleteByRK(string rowKey)
        {
            //  if (string.IsNullOrEmpty(rowKey)) throw new ArgumentNullException(nameof(rowKey));
            return Core.AzureTableDataWriter.DeleteDataByRK(rowKey, ref CloudTable);
        }

        public bool DeleteWithEtag(string partitionKey, string rowKey, string eTag)
        {
            //if (string.IsNullOrEmpty(partitionKey)) throw new ArgumentNullException(nameof(partitionKey));
            //if (string.IsNullOrEmpty(rowKey)) throw new ArgumentNullException(nameof(rowKey));
            if (string.IsNullOrEmpty(eTag)) throw new ArgumentNullException(nameof(eTag));

            return Core.AzureTableDataWriter.DeleteData(partitionKey, rowKey, ref CloudTable, eTag);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>ETag</returns>
        public string Insert(DTableEntity data)
        {
            var upsertOperation = TableOperation.Insert(data);
            var res = CloudTable.Execute(upsertOperation);
            return res.Etag;
        }

        public bool IsDataExist(string partitionKey, string rowKey)
        {
            return Core.AzureTableDataReader.IsDataExisted(partitionKey, rowKey, ref CloudTable);
        }

        public Operator Update(DTableEntity data)
        {
            data.ETag = "*";
            Core.AzureTableDataWriter.Update(data, ref CloudTable, false);
            return this;
        }

        public Operator UpdateWithEtag(DTableEntity data)
        {
            Core.AzureTableDataWriter.Update(data, ref CloudTable, true);
            return this;
        }
    }
}
