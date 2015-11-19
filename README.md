## Migrate assets from RM server to VSO ##

> **PREVIEW NOTICE**
> This project is still *UNDER CONSTRUCTION*. Tooling and
> documentation are supplied as an early preview; and may 
> be changed without prior warning. We value your candid 
> feedback to help us identify issues and improve the 
> product!

### What ###
As Bob,the Release Manager, I would like to migrate my assets from 2015 or 2013 U4 RM server to VSO, and start using the Release hub in Team Web Access. 

### What about TFS? ###
This solution currently targets VSO, but will be applicable to migration to the new RM features in TFS in a future version of TFS.

### Why ###
As the new release management capabilities are going to part of TFS, and as customers are currently using the WPF client along with RM server, there is a need to help them migrate their assets into the new model. Most of these customers are using agent-based release templates. 

Their release templates are scripted using the RM server’s XAML based workflow system. The next generation of release management features in TFS are based on a light-weight workflow system and do not have all the fine-grain tasks that were otherwise available in RM server.

However, since both of them leverage Powershell heavily, it is possible to at least partially migrate the data from one to the other. This project aims to provide the guidance and tooling to help you with this migration.

### Find out more ###
- [RM Workflow Migrator Guide](doc/RM-Workflow-Migrator-Guide.md)
- [Lab 1 - Using the RMWorkFlowMigrator to create release scripts](doc/Lab-1-Using-the-RMWorkFlowMigrator-to-create-release-scripts.md)
- [Lab 2 - Using the resulting release scripts in a release pipeline](doc/Lab-2-Using-the-resulting-release-scripts-in-a-release-pipeline.md)
- [Intoduction to RM vNext](TBD) *COMING SOON*
- [RM 2015 New Features](TBD) *COMING SOON*

### The team ###
Daniel Mann, Dave McKinstry, David Pitcher, Derrick Cawthon, Etienne Tremblay, Hosam Kamel, John Bergman, Josh Garverick, Josh Sommer, Niel Zeeman, Richard Albrecht, Richard Fennell, Sergio Romero, Shashank Bansal , Shaun Mullis, Stawinski Fabio, Vladimir Gusarov, William Salazar

### Notices ###
Notices for certain third party software included in this solution are provided here: [Third Party Notice](ThirdPartyNotices.txt).
