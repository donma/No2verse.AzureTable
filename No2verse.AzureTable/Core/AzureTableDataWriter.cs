using Microsoft.Azure.Cosmos.Table;
using No2verse.AzureTable.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace No2verse.AzureTable.Core
{
    internal class AzureTableDataWriter
    {
        internal static void Update(DTableEntity data, ref CloudTable cloudTable, bool isWithEtag = false)
        {
            try
            {
                if (!isWithEtag)
                {
                    var upsertOperation = TableOperation.InsertOrReplace(data);

                    var _ = cloudTable.Execute(upsertOperation);
                }
                else
                {
                    var upsertOperation = TableOperation.Replace(data);

                    var _ = cloudTable.Execute(upsertOperation);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        internal static bool DeleteData(string pk, string rk, ref CloudTable cloudTable, string eTag = "*")
        {
            try
            {
                var res = AzureTableDataReader.IsDataExisted(pk, rk, ref cloudTable);
                if (!res)
                {
                    return true;
                }
                var deleteReult = cloudTable.Execute(TableOperation.Delete(new TableEntity { RowKey = rk, PartitionKey = pk, ETag = eTag }));
                return true;

            }
            catch (Microsoft.Azure.Cosmos.Table.StorageException ex) when (ex.Message.Contains("Not Found"))
            {
                return true;
            }
            catch (Microsoft.Azure.Cosmos.Table.StorageException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> source, int pageCount)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / pageCount)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        internal static bool DeleteDataByPK(string pk, ref CloudTable cloudTable)
        {
            try
            {

                var queryAllRowKeysByPK = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                 QueryComparisons.Equal, pk)).Select(new[] { "RowKey" });


                //抓回所有 PartitionKey = CLASSA 的結果
                var entities = cloudTable.ExecuteQuery(queryAllRowKeysByPK);

                // 因為TableBatchOperation 不能加入超過100
                //所以要分組 每100個為一組
                var split100Entities = Split(entities, 100);
                foreach (var split100Entry in split100Entities)
                {
                    var batch = new TableBatchOperation();
                    //將100加入到 TableBatchOperation  中 
                    foreach (var wannaDeleteData in split100Entry)
                    {
                        batch.Add(TableOperation.Delete(wannaDeleteData));
                    }
                    try
                    {
                        cloudTable.ExecuteBatch(batch);

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }




                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal static bool DeleteDataByRK(string rk, ref CloudTable cloudTable)
        {
            try
            {


                var queryAllRowKeysByPK = new TableQuery().Where(TableQuery.GenerateFilterCondition("RowKey",
              QueryComparisons.Equal, rk)).Select(new[] { "PartitionKey" });


                //抓回所有 PartitionKey = CLASSA 的結果
                var entities = cloudTable.ExecuteQuery(queryAllRowKeysByPK);

                // 因為TableBatchOperation 不能加入超過100
                //所以要分組 每100個為一組

                if (entities != null && entities.Count() > 0)
                {

                    foreach (var data in entities)
                    {
                        DeleteData(data.PartitionKey, data.RowKey, ref cloudTable);
                    }



                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
