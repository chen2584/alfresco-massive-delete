# alfresco-massive-delete
As **afaust** said in https://community.alfresco.com/thread/239770-problem-with-delete-big-folder

> It is never a good idea to try and delete (or modify) more than a few hundred elements in a single operation. Your deletion should be implemented as an incremental operation instead of trying to delete the whole folder in one go...


We should not delete whole folder that contains more than hundred of nodes in a single operation, So i create this script for delete any nodes that matchs what i want to delete. The script will iterations delete every single node that matchs in search query.


## My Test Case
Tested with **750k nodes**, It run perfectly and Alfresco not crash.


## USAGE
1. Download zip from Release and Extra the zip.
2. Change **appsettings.json** and save it.
```
{
    "Alfresco": {
        "BaseUrl": "http://localhost:8080/alfresco", // Your alfresco base url
        "UserName": "admin", // Your Alfresco's Username
        "Password": "admin" // Your Alfresco's Password
    },
    "SearchQuery": "TYPE: content AND PATH: '/app:company_home/app:shared//.'", // Search query that matchs what you want to delete
    "WorkerNumber": 20, // Number of worker
    "MaxSearchItem": 1000, // Max search output per time.
    "SearchDelay": 10 // Delay to search from Alfresco
}
```
3. Type: `dotnet MassiveDelete.dll`
