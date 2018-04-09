namespace Fittify.Test.Core
{
    public static class StaticFields
    {
        public static string MsSqlTestDbConnectionStringWork => @"Server=.\SQLEXPRESS;Database=Fittify;User Id=seifert-1;Password=merlin;";
        public static string TestDbFittifyConnectionString => @"Server=.\SQLEXPRESS;Database=FittifyTestDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        internal static string DbMasterConnectionString => @"Server=.\SQLEXPRESS;Database=master;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
