# Migrate-assets-from-RM-server-to-VSTS

## Migrate assets from RM server to Team Foundation Server and Team Services ##

|Branch|Health|
|------|------|
|master|![](https://almrangers.visualstudio.com/DefaultCollection/_apis/public/build/definitions/7f3cfb9a-d1cb-4e66-9d36-1af87b906fe9/83/badge)|

> **NOTICE** - We have deprecated this tool.It will continue to serve as an open source sample solution.

### What ###
As Bob,the Release Manager, I would like to migrate my assets from 2015 or 2013 U4 RM server to Visual Studio Team Foundation Server or Team Services, and start using the Release hub in Team Web Access. 

### What about TFS? ###

The release of Team Foundation Server 2015 Update 2, or higher, includes the new Release Management (RM) features. This solution targets Visual Studio Team Services and Team Foundation Server 2015 Update 2, or higher.

### Why ###
Customers have so far been interacting with RM Server using the WPF client, configuring the release templates and managing their pipelines using the XAML based workflow system. The new release management capabilities, that are available with VS Team Services and are going to be a part of TFS in a future version of TFS, are based on a light-weight workflow system.

The agent-based release templates authored using the WPF client used a set of fine-grained tasks that were available with the RM Server. The new workflow system does not provide all those tasks out of the box, making it harder to upgrade the templates to the new capabilities.

However, since both of them leverage Powershell heavily, it is possible to partially migrate the data from one to the other. This project aims to provide the guidance and tooling to help you with this migration.

### Find out more ###

[![](./doc/Images/demo.png) video](https://channel9.msdn.com/Series/Visual-Studio-ALM-Rangers-Demos/Project-Demo-Migration-of-RM-assets-from-RM-server-to-TFS)

**Overview**

- [Introduction to the Release Management service in Visual Studio Team Services](doc/Intro-to-Release-Managment-VisualStudioTeamServices.md)
- [Announcement: Moving from the earlier version of Release management service to the new one in Visual Studio Team Services](http://blogs.msdn.com/b/visualstudioalm/archive/2015/11/19/moving-from-the-earlier-version-of-release-management-service-to-the-new-one-in-visual-studio-team-services.aspx)
- [Release Management for VSTS preview version](https://msdn.microsoft.com/Library/vs/alm/Release/overview-rmpreview)
- [Migrating Release Management 2013/2015 to Release Management Service](http://incyclesoftware.com/2015/11/migrating-release-management-20132015-to-release-management-service/)
- [Architecture differences between Release Management 2013 and Release Management 2015 and Release Management (RM vNext) for Visual Studio for Team Services](doc/RMArchitecture-comparison-between-RM2013-2015-and-RMvNext-for-VSTS.md)

**Guides**

- [RM Workflow Migrator Guide](doc/RM-Workflow-Migrator-Guide.md)

**Walk-throughs**

- [Lab 1 - Using the RMWorkFlowMigrator to create release scripts](doc/Lab-1-Using-the-RMWorkFlowMigrator-to-create-release-scripts.md)
- [Lab 2 - Using the resulting release scripts in a release pipeline](doc/Lab-2-Using-the-resulting-release-scripts-in-a-release-pipeline.md)

### Collection of Information ###

We do not collect any personal information when you use the migration tool. When you use the migration tool with Internet connectivity, basic usage metrics is collected to help improve our products and services, and for statistical analysis. Consult the [RM Workflow Migrator Guide](doc/RM-Workflow-Migrator-Guide.md) for options of disabling the metrics collection at run-time.

### The team ###
Daniel Mann, Dave McKinstry, David Pitcher, Derrick Cawthon, Josh Garverick, Josh Sommer, Niel Zeeman, Richard Albrecht, Richard Fennell, Sergio Romero, Shashank Bansal, Shaun Mullis, Stawinski Fabio, Vladimir Gusarov, William Salazar

### Notices ###
Notices for certain third party software included in this solution are provided here: [Third Party Notice](ThirdPartyNotices.txt).

###Contribute
Contributions to Print Cards are welcome. Here is how you can contribute to Print Cards:  

- Submit bugs and help us verify fixes  
- Submit pull requests for bug fixes and features and discuss existing proposals   

Please refer to [Contribution guidelines](.github/CONTRIBUTING.md) and the [Code of Conduct](.github/COC.md) for more details.
