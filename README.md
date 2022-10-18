# Objective Connect Management Tool
This .NET 6.0 Windows Forms tool utilises the [Objective Connect public API](https://secure.objectiveconnect.com/publicapi/1/swagger-ui/index.html) to provide a set of workgroup management tools that enable a higher level of management than Objective Connect allows natively.

The app is written entirely in C# and does not require any authorisation token storage, simply sign in with the credentials of a Workgroup or Enterprise administrator and begin using the functions

## Running the application
To run the program simply clone this repository, build the project within Visual Studio, and then run the resulting application from within the `/bin/debug` folder.

## Functionality
The app currently only has one function, which is to report on workspaces within the workgroup and display the last date they were accessed, combining the native workgroup and workspace audit reports. This report scans over all workspaces using the `/workspacecsv` endpoint, then fetches the individual audit report for each workspace using the `/workspaceauditcsv` end point. The last item in the audit report for each workspace is recorded as the last accessed date and added to the output CSV.
