Azure_HTTPTriggerFunctionSample
===============================

### Description
  This is a sample application which uses a `HTTP trigger` function to process a message. I have created this for my learning purpose and to improve my technical skils and wanted to implemented what i have learnt in some crash course.
  
### Features

- POST message to cloud
- All request to the triggered are Audited
- Once message is processed the similar information can be posted in azure queue for further process
- Azure queue are completely configurable from `localSettings.json` file
- Supports both Service bus queue and Storage queue
- Queue can be created dynamicaly on the fly

### Technology Used

- Azure Function (HTTP Trigger)
- .Net Core
- Entity Framework Core
- Azure storage
- Azure service bus

### Concepts Covered

- Dependency Injection
- Delegates
- Static
- Interface implementation
- SOLID Principle
- Linq

### Tools used (needed to work on this)

- Visual Studion 2019

### How to configure

  Once the repo gets cloned to your local machine and open the solution file from your loacl visual studio IDE. Bfore run and explore there are few configuraions has to be modified according to your `development` environment

#### Connection Strings

  This the primay settings we need to take care of and below are varios typers of `connection string` we have used in this project

  ##### Data base
   As this application tracking each request and process the request we have been making entries in data base. please change your connection string in the settings json file
       
     "SampleApp_ConnectionString": 
      "Data Source=<DB serever>;Initial Catalog=SampleApp;Integrated Security=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"    
    
  ##### Storage Account
   As this application send the mesage to the storage account queue based on configuraion. please change your connection string in the settings json file
       
     "SampleApp_StorageAccount_ConnectionString": "<place your storage account connection string>"    
    
   ##### Service Bus
   As this application send the mesage to the Service bus queue based on configuraion. please change your connection string in the settings json file
   
         "SampleApp_ServiceBus_ConnectionString": <place your connection string>
        
#### Queue Name
   This application provide option to configure the queue name based on need, we have inrtroduced this configuration. Make sure the queue name must be in lower case to avoid failure
   
         "SampleApp_ServiceBus_QueueName": <Service bus queue>,
         "SampleApp_StorageQueueName": <storage queue>,
        
#### Configuration which decides to use Azure service bus OR Storage account
   Based on this setting application post the message to either service bus or storage account
   
         "AzureQueueAssets": "ServiceBus|Queue" OR "StorageAccount|Queue"
    
#### Data base set up
   SQL DDL scripts are created and available under solution folder.
   
    .    
    ├── SampleFunctionApp
    │   └── SampleApp.Shared
    │       └── SQLScripts
    │         └── DDL Script.sql
    
      
  ##### Sample message format
   Since this app is using HTTP trigger the sample post request body is available under SampleFunctionApp\SampleApp.Shared\SQLScripts (Message)
      
    {
      "UserName": "palkalaislevand",
      "FirstName": "Palkalaiselvan",
      "LastName": "Dhamotharan",
      "EmailAddress": "palkalaiselvand@outlook.com",
      "Department": "Information Technology"
    }
    
