# Golf Course Mapper - API & Backend

## Description

The **Mapper API** is a REST API used for controlled access to the backend of the Golf Course Mapper. The API is built using the _Microsoft Dotnet Core Entity Framework_, using _PostGIS_ as the database backend.

## Installation

Firstly nsure that your workstation has the following dependancies installed:

* PostGIS
* Dotnet Core
* Node

You can now clone the repository to your local drive.

```
git clone https://github.com/team-recursive-recursion/mapper-api
```

Next, you must install the Node package dependancies and set up the PostGIS database through migrations.

```
cd mapper-api/MapperApi
npm install
dotnet ef database update
```

Finally you can execute the API.

```
dotnet run
```
