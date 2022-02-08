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


### Create Table

```C#

            


```

### Create Data

```C#

            


```



### Delete Data

```C#

            


```





