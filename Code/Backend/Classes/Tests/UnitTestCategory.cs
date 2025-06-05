using System;
using Xunit;
using ReMarket.Models;
using MySql.Data.MySqlClient;
using ReMarket.Utilities;

namespace Tests;

public class UnitTestCategory
{
    public class CategoryTests
    {
    //    [Fact]
    //    public void CreateCategory_ValidData_ReturnsCategory()
    //    {
    //        string categoryName = "Electronics";

    //        var category = Category.Create(categoryName);

    //        Assert.Equal(categoryName, category.Name);
    //        Assert.Equal(0, category.Id); 
    //    }

    //    [Fact]
    //    public void SaveToDatabase_NewCategory_SavesCategory()
    //    {
    //        var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
    //        connection.Open();

    //        var category = Category.Create("Electronics");

    //        category.SaveToDatabase(connection);

    //        Assert.True(category.Id > 0);

    //        connection.Close();
    //    }

    //    [Fact]
    //    public void LoadById_ValidId_ReturnsCategory()
    //    {
    //        var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
    //        connection.Open();

    //        var category = Category.LoadById(connection, 1);

    //        Assert.NotNull(category);
    //        Assert.Equal(1, category.Id);

    //        connection.Close();
    //    }

    //    [Fact]
    //    public void LoadById_InvalidId_ReturnsNull()
    //    {
    //        var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
    //        connection.Open();

    //        var category = Category.LoadById(connection, -1); 

    //        Assert.Null(category);

    //        connection.Close();
    //    }

    //    [Fact]
    //    public void Delete_ValidId_RemovesCategory()
    //    {
    //        var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
    //        connection.Open();

    //        Category.Delete(connection, 1);

    //        var category = Category.LoadById(connection, 1);

    //        Assert.Null(category);

    //        connection.Close();
    //    }

    //    [Fact]
    //    public void LoadAll_ReturnsListOfCategories()
    //    {
    //        var connection = new MySqlConnection("server=localhost;port=3306;database=ReMarket;user=root;password=toor1234");
    //        connection.Open();

    //        var categories = Category.LoadAll(connection);

    //        Assert.NotEmpty(categories);

    //        connection.Close();
    //    }
    }
}
