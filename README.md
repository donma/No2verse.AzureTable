# No2verse.AzureTable

A simple library for .Net developer to use [Azure Table Storage](https://azure.microsoft.com/en-us/services/storage/tables/) , dep

Dependency :

[Microsoft.Azure.Cosmos.Table](https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table)

[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)

Happy Coding :)

Tutorial
----

How To Start?
----

### Sample Data Model 

```C#

    public class User : No2verse.AzureTable.Base.DTableEntity
    {
        public class Car
        {

            public int No { get; set; }
            public string Name { get; set; }

            public DateTime BuyDate { get; set; }

            public string Color { get; set; }

        }

        public string Name { get; set; }

        public DateTime Birth { get; set; }

        public Car CarInfo { get; set; }
    }          


```


### Init

```C#

    //Init
    var role = new No2verse.AzureTable.Base.AzureTableRole("TEST1", new No2verse.AzureTable.AzureStorageSettings
    {
        //Azure Table Connection String.
        //DefaultEndpointsProtocol=https;AccountName=azureblobname;AccountKey=....
        ConnectionString = AzureConnectionString

     });
        


```


### Create Table

```C#

    
    //Create Table
    //needToCheckExisted  for check table existed and create .
    
    var operatorMain = new No2verse.AzureTable.Collections.Operator(role, "tablesample1", true);

            
```

### Create/Update Data

```C#


    //UpdateData
    var data1 = new User
    {
        PartitionKey = "PART1",
        RowKey = "DONMA",
        Birth = new DateTime(1983, 6, 21),
        Name = "DONMA HSU",
        CarInfo = new User.Car { BuyDate = DateTime.Now, Color = "RED", No = 123456, Name = "SWIFT" }
    };
    operatorMain.Update(data1);
            


```



### Delete Data

```C#


    //Delete Data
    operatorMain.Delete("PART1", "DONMA");

```


### GetData By Pa Data

```C#

    //Get Data
    var queryMain = new No2verse.AzureTable.Collections.Query<User>(role, "tablesample1");
    var data=queryMain.DataByPRKey("PART1", "DONMA");
    Console.WriteLine(JsonConvert.SerializeObject(data));

```





