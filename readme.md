﻿# Gremlin.Linq for Azure Cosmos Graph DB

Gremlin.Linq is a library that enables you to write fluent queries using lambda expressions. Targeted towards Azure Cosmos Graph DB.

## Adding entities
When you add entities you can either work with pure POCO's or use classes that derives from the Vertex class.

Connecting to the server is done by adding properties to your configuration, preferably using appsettings.json.

```
var config =  new  ConfigurationBuilder()
	.AddJsonFile("appsettings.json", false)
	.AddJsonFile("appsettings.development.json", true)
	.Build();

using (var client =  new  GraphClientFactory(config).CreateGremlinGraphClient())
{
// Use your client to update or query the graph here
}
```

Your appsettings.json should then look like this.
```
{
	"gremlin": {
		"url": "yourdb.azure.com",
		"database": "<databaseName>",
		"collection": "<collectionName>",
		"password": "<access key>"
	}
}
```

Adding entities at its simplest 

```
var user = client
	.Add(new User(){})
	.SubmitAsync();
```
	
You can also add two entities and connect them. In the example below a vertex of type User will be created joined to a vertex of type login with the edge property set to "GoogleLogin"

```
var login = client
	.Add(new User())
	.AddOut(new Login(),"GoogleLogin")
	.ExecuteAsync();
```

##Querying graph
To query the graph you can use lambda expressions. In the example below we will find users with firstname *John*

```
var users = client
	.From<User>()
	.Where(user=>user.FirstName=="John")
	.SubmitAsync();
```

To select connected vertexes 

```
var logins = client
    .From<User>()
    .WhereIn(a => a.FirstName, new string[] {"test1", "test2"})
	.Out<Login>()
	.SubmitAsync();
```

To find connected vertexes the other way around

```
var user = client
	.From<Login>()
	.Where(login=>login.Provider=="Google")
	.In<User>()
	.SubmitWithSingleResultAsync();
```


To select verticies and edges
```
var courses = await _graphClient
                .From<User>().As<User>()
                .Where(a => a.SubjectId == userId)
                .OutEdge<UserLogin>().As<UserLogin>()
                .InVertex<Login>().As<Login>()
                .Select<User,Login,UserLogin>()
                .SubmitAsync();            
```


To update edge
```
var courses = await _graphClient
                .From<User>().As<User>()
                .Where(a => a.SubjectId == userId)
                .OutEdge<UserLogin>().As<UserLogin>()
                .Select<UserLogin>()
                .Set("LoginCount",3)
                .SubmitAsync();            
```


##Custom vertex labels and propertiy names

When your class and property names are not suitable to map to your graph, you can specify your own labels and names.

To change the label for your vertex, you can specify the `GremlinLabel(string)` attribute on your model class.

```
[GremlinLabel("my-vertex")]
public class MyClass {
	public string FavoriteColor {get; set; }
}
```

To use a specific property name, you can specify the `GremlinProperty(string)` attribute on your model class properties.

```
public class MyClass {
	[GremlinProperty("fav-clr")]
	public string FavoriteColor {get; set; }
}
```
