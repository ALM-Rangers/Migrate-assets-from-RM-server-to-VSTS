Architecture differences between Release Management 2013 and Release Management 2015 and Release Management (RM vNext) for Visual Studio for Team Services 
===========================================================================================================================================================

Release Management (RM vNext) for Visual Studio Team Services is the next version of the release management tool for Visual Studio. The current version of the Release Management service has been available as part of Visual Studio Online since November 2014.

This release was limited with

-   No browser-based interface integrated into Visual Studio Online

-   Deploy only to Microsoft Azure, and not to on-premises servers

RM vNext for Visual Studio for Team Services addresses these and other limitations, and offers many more improvements. It is not just a change of user interface from the old WPF client to the new web interface. Instead, it is based on a new architecture, and a new set of simplified concepts. You will find that this service:

-   Is easy to use

-   Works for all of your apps – Windows and Linux, Java and .Net

-   Provides traceability between various ALM entities such as builds, environments, work items, etc.

The improvements include:

-   Familiar web based interface that is aligned with the rest of VSO. Getting started, authoring a release definition, and creating releases can all be done more easily in the web interface than in the current product

-   The Release hub is integrated into Team projects.

-   **Fewer concepts.** “Release Paths” and “Release Templates” in the current version have been replaced by a single concept called *Release Definitions*.

Along with RM 2015 (server /client) edition, the new RM services (vNext) have been made available for VSTS users. *RM services (vNext) support for on premise TFS will be available in a future release.* [1]

**The main differences between Release Management 2013 and Release Management (RM vNext) for Visual Studio for Team Services**

-   Release management 2013 and 2015 RTM (Server and Client) is designed to support integration with on premise/azure environments via a WPF interface. It was based on XAML workflows and tasks that were hard to extend and maintain.

-   RM vNext for Visual Studio Team Services is based on the same distributed task execution infrastructure as Build vNext. All the tasks in your Build and RM flows execute on a pool of agents. Build and RM share the same agent.

-   The security infrastructure of RM vNext is different from the current version in that it does not manage its own groups and permissions. New permissions are introduced in VSO for RM vNext, such as “Create release definitions”, “Create releases”, and “Manage approvers”

Both Release Management 2013 and RM vNext for Visual Studio for Team Services support the new build services (Build vNext) on premise TFS and VSTS .

Comparison across the versions and features, requirements

|                                                     | Release Management 2013                                            | Release Management 2015                                                   | RM vNext Service for Visual Studio Team Services                       |
|-----------------------------------------------------|--------------------------------------------------------------------|---------------------------------------------------------------------------|------------------------------------------------------------------------|
| Integrates with Visual Studio Team Services         | Yes                                                                | Yes                                                                       | Yes                                                                    |
| Requires server installation                        | Yes                                                                | Yes                                                                       | No                                                                     |
| Requires client installation                        | Yes                                                                | Yes                                                                       | No                                                                     |
| Integration with on premise hardware (environments) | Yes                                                                | Yes                                                                       | No                                                                     |
| Azure integration                                   | Yes                                                                | Yes                                                                       | Yes                                                                    |
| Additional references                               | [Automate deployments with Release Management](https://msdn.microsoft.com/library/dn217874%28v%3Dvs.120%29.aspx) | [Release Management for Visual Studio 2015 and TFS](https://www.visualstudio.com/en-us/get-started/release/rm-for-vs2015-vs) | [Release Management for VSTS preview version](https://msdn.microsoft.com/Library/vs/alm/release/overview-rmpreview) |

For additional information on Release Management in Visual Studio Team Services, refer to [Release Management vNext Plans](http://blogs.msdn.com/b/visualstudioalm/archive/2015/08/26/release-management-vnext-plans.aspx).

[1] [Release Management vNext Plans](../customXml/item1.xml).
