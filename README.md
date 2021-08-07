# BlazorBattles
This repo is about a full end to end example of a Blazor WASM app. It includes an server API that talks to a SQL DB. 

The credit for this app goes to Patrick God @ Udemy. He has a class for walking through the Full Stack Blazor app.

Running on your Dev or Production:

  One must first create the DataBase and the Tables (there are sql scripts for that).
  One must adjust the appsettings.json and appsettings.Development.json <= the connection strings need to be updated for one's own environment!
  
 "But Scott, I don't have SQL Server..." Not a problem, one will have to make changes, of course, to the Startup.cs of the Server project.
   The app does use the repository pattern for the client, so yes, to switch providers other than SQL Server, this needs to be done only in
   the Server project.
   
   ***Hope that helps... ScottGeek
