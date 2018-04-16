# TransactionsManager
[![CircleCI](https://circleci.com/gh/guillermocorrea/tm.svg?style=svg)](https://circleci.com/gh/guillermocorrea/tm)

Asp .net core MVC and API to manage bank transactions.

Requirements
============
* [Dotnet core 2](https://www.microsoft.com/net/download/windows)
* [Sql Server](https://www.microsoft.com/en-us/sql-server/)
* Clone and configure [TransactionsManagerIdentityServer](https://github.com/guillermocorrea/TransactionsManagerIdentityServer) repo.

Set up the Database
===================
* Create a database named "TransactionsManager" in SQL Server.
* Run the file sql/data.sql in the database created to import the schema and the data.

Run the Application
===================
* Make sure to clone and run the [TransactionsManagerIdentityServer](https://github.com/guillermocorrea/TransactionsManagerIdentityServer) repo.
* In a cmd hit `dotnet restore` and `dotnet run src/TransactionsManager.UI/TransactionsManager.UI.csproj` or open the solution in Visual Studio 2017 and run the application from there.
* Open a browser at http://localhost:5050/

Users
=====
* (Role Assistant) Username: assistant Password: password
* (Role Manager) Username: manager Password: password 
* (Role Administrator) Username: administrator Password: password 

Web API
=======
* The API is protected with Bearer authentication, get an authentication token at http://localhost:5000/connect/token
* In a shell hit `curl -d "grant_type=client_credentials&scope=transactions-api&client_id=api-client&client_secret=secret" -X POST http://localhost:5000/connect/token`.
* Copy the token and append it to the following requests. 

| Url      |   Method      |  Sample curl |
|----------|:-------------:|-------------:|
| http://localhost:5050/api/transactions/search?isFraud=true&SearchNameDest= |  GET | `curl -i -X Get -H "Authorization: Bearer PASTE_TOKEN_VALUE_HERE" -H "Cache-Control: no-cache" "http://localhost:5050/api/transactions/search?isFraud=true&SearchNameDest="` |
| http://localhost:5050/api/transactions |    POST   |   `curl -X POST http://localhost:5050/api/transactions -H 'content-type: application/json' -H "Authorization: Bearer COPY_TOKEN_VALUE_HERE"-d '{ "step": 1, "type": "PAYMENT",	"amount": 1111111,	"nameOrig": "TEST",	"oldbalanceOrg": 1111111,	"newbalanceOrig": 1111111,	"nameDest": "TEST",	"oldbalanceDest": 1111111,	"newbalanceDest": 1111111,	"isFraud": false,	"isFlaggedFraud": false}'` |    

Unit Tests
==========
* In a cmd hit `dotnet test test/TransactionsManager.Domain.Tests/TransactionsManager.Domain.Tests.csproj`

Dataset
=======
The dataset posted at https://www.kaggle.com/ntnu-testimon/paysim1 was imported into a SQL Server table to be queried and managed.

![Transactions Table](https://raw.githubusercontent.com/guillermocorrea/TransactionsManager/master/Transactions%20Table.png)

Architecture
============

![Architecture](https://raw.githubusercontent.com/guillermocorrea/TransactionsManager/master/components-diagram.jpg)

Contact
=======

Developed by [Guillermo Correa](https://www.linkedin.com/in/luis-guillermo-correa-guti%C3%A9rrez-01620a94/). Feel free to contact me at: guillermo.correa.99@gmail.com
