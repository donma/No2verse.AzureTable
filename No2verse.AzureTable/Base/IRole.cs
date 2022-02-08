namespace No2verse.AzureTable.Base
{
    public interface IRole
    {
        string RoleKey { get; set; }
        IPathSettings PathSettings { get; }
    }


}
