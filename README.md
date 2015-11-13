## Migrate assets from RM server to TFS ##

> **PREVIEW NOTICE**
> This project is still *UNDER CONSTRUCTION*. Tooling and
> documentation are supplied as an early preview; and may 
> be changed without prior warning. We value your candid 
> feedback to help us identify issues and improve the 
> product!

### What ###
As Bob,the Release Manager, I would like to migrate my assets from 2015 or 2013 U4 RM server to 2015 TFS server, and start using the Release hub in Team Web Access.

### Why ###
As the new release management capabilities are going to part of TFS, and as customers are currently using the WPF client along with RM server, there is a need to help them migrate their assets into the new model. Most of these customers are using agent-based release templates. 

Their release templates are scripted using the RM server’s XAML based workflow system. The next generation of release management features in TFS are based on a light-weight workflow system and do not have all the fine-grain tasks that were otherwise available in RM server.

However, since both of them leverage Powershell heavily, it is possible to at least partially migrate the data from one to the other. This project aims to provide the guidance and tooling to help you with this migration.

### Find out more ###
- [Migration Overview](@) **COMING SOON**
- [PowerShell Requirements and Setup](@) **COMING SOON**
- [Migration tooling output](@) **COMING SOON**
- [Command line walk-through](@) **COMING SOON**
- [PowerShell Script usage walk-through](@) **COMING SOON**

### Notices ###

Notices for certain third party software included in this extension are provided here: [Third Party Notice](ThirdPartyNotices.txt).
