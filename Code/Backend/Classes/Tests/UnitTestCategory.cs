using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using ReMarket.Utilities;

namespace Tests;

public class CategoryTests : IDisposable
{
    private MySqlConnection _connection;

    public CategoryTests()
    {
        var connectionString = new MySqlConnectionStringBuilder()
        {
            Server = "remarket-se2project-ania-f1cd.j.aivencloud.com",
            Port = 21633,
            Database = "ReMarket",
            UserID = "avnadmin",
            Password = "test",
            SslMode = MySqlSslMode.VerifyCA,
            CertificateFile = "/Users/ola/desktop/ca.pem"
        }.ConnectionString;

        _connection = new MySqlConnection(connectionString);


        _connection.Open();
    }

    public void Dispose()
    {
        _connection.Close();
    }

    [Fact]
    public void CreateCategory_ValidData_ReturnsCategory()
    {
        var category = Category.Create("Electronics");
        Assert.Equal("Electronics", category.Name);
        Assert.Equal(0, category.Id);
    }

    [Fact]
    public void SaveToDatabase_NewCategory_SetsId()
    {
        var category = Category.Create("Books");
        category.SaveToDatabase(_connection);
        Assert.True(category.Id > 0);


        Category.Delete(_connection, category.Id);
    }

    [Fact]
    public void SaveToDatabase_ExistingCategory_UpdatesName()
    {
        var category = Category.Create("TempCat");
        category.SaveToDatabase(_connection);
        int id = category.Id;

        category.Name = "UpdatedCat";
        category.SaveToDatabase(_connection);

        var updated = Category.LoadById(_connection, id);
        Assert.Equal("UpdatedCat", updated?.Name);

        Category.Delete(_connection, id);
    }

    [Fact]
    public void LoadById_ValidId_ReturnsCategory()
    {
        var category = Category.Create("Gadgets");
        category.SaveToDatabase(_connection);

        var loaded = Category.LoadById(_connection, category.Id);
        Assert.NotNull(loaded);
        Assert.Equal(category.Id, loaded?.Id);
        Assert.Equal("Gadgets", loaded?.Name);


        Category.Delete(_connection, category.Id);
    }

    [Fact]
    public void LoadById_InvalidId_ReturnsNull()
    {
        var category = Category.LoadById(_connection, -99);
        Assert.Null(category);
    }

    [Fact]
    public void Delete_ValidId_RemovesCategory()
    {
        var category = Category.Create("ToDelete");
        category.SaveToDatabase(_connection);
        int id = category.Id;

        Category.Delete(_connection, id);
        var deleted = Category.LoadById(_connection, id);
        Assert.Null(deleted);
    }

    [Fact]
    public void LoadAll_ReflectsInsertAndDelete()
    {
        int beforeCount = Category.LoadAll(_connection).Count;

        var category = Category.Create("CountTest");
        category.SaveToDatabase(_connection);

        int afterInsert = Category.LoadAll(_connection).Count;
        Assert.Equal(beforeCount + 1, afterInsert);

        Category.Delete(_connection, category.Id);

        int afterDelete = Category.LoadAll(_connection).Count;
        Assert.Equal(beforeCount, afterDelete);
    }

    [Fact]
    public void SaveToDatabase_EmptyName_AllowsInsertButShouldBeValidated()
    {
        var category = Category.Create("");
        category.SaveToDatabase(_connection);

        Assert.True(category.Id > 0);
        Assert.Equal("", category.Name);

        Category.Delete(_connection, category.Id);
    }
}