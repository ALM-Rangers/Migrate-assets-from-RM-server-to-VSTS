## Migrate assets from RM server to Visual Studio Team Services ##

> **PREVIEW NOTICE**
> This project is still *UNDER CONSTRUCTION*. Tooling and
> documentation are supplied as an early preview; and may 
> be changed without prior warning. We value your candid 
> feedback to help us identify issues and improve the 
> product!

### What ###
As Bob,the Release Manager, I would like to migrate my assets from 2015 or 2013 U4 RM server to Visual Studio Team Services, and start using the Release hub in Team Web Access. 

### What about TFS? ###
This solution currently targets Visual Studio Team Services, but will be applicable to migration to the new RM features in TFS in a future version of TFS.

### Why ###
Customers have so far been interacting with RM Server using the WPF client, configuring the release templates and managing their pipelines using the XAML based workflow system. The new release management capabilities, that are available with VS Team Services and are going to be a part of TFS in a future version of TFS, are based on a light-weight workflow system.

The agent-based release templates authored using the WPF client used a set of fine-grained tasks that were available with the RM Server. The new workflow system does not provide all those tasks out of the box, making it harder to upgrade the templates to the new capabilities.

However, since both of them leverage Powershell heavily, it is possible to partially migrate the data from one to the other. This project aims to provide the guidance and tooling to help you with this migration.

### Find out more ###

**General**

- [Announcement: Moving from the earlier version of Release management service to the new one in Visual Studio Team Services](http://blogs.msdn.com/b/visualstudioalm/archive/2015/11/19/moving-from-the-earlier-version-of-release-management-service-to-the-new-one-in-visual-studio-team-services.aspx)
- [Release Management for VSTS preview version](https://msdn.microsoft.com/Library/vs/alm/Release/overview-rmpreview)
- [Migrating Release Management 2013/2015 to Release Management Service](http://incyclesoftware.com/2015/11/migrating-release-management-20132015-to-release-management-service/)

**Guides**

- [RM Workflow Migrator Guide](doc/RM-Workflow-Migrator-Guide.md)

**Walk-throughs**

- [Lab 1 - Using the RMWorkFlowMigrator to create release scripts](doc/Lab-1-Using-the-RMWorkFlowMigrator-to-create-release-scripts.md)
- [Lab 2 - Using the resulting release scripts in a release pipeline](doc/Lab-2-Using-the-resulting-release-scripts-in-a-release-pipeline.md)

### The team ###
Daniel Mann, Dave McKinstry, David Pitcher, Derrick Cawthon, Josh Garverick, Josh Sommer, Niel Zeeman, Richard Albrecht, Richard Fennell, Sergio Romero, Shashank Bansal, Shaun Mullis, Stawinski Fabio, Vladimir Gusarov, William Salazar

### Notices ###
Notices for certain third party software included in this solution are provided here: [Third Party Notice](ThirdPartyNotices.txt).
