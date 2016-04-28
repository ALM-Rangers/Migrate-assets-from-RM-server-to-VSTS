// --------------------------------------------------------------------------------------------------------------------
// <copyright file="When_Resolving_Unique_Script_Parameters.cs" company="Microsoft Corporation">
//   Copyright Microsoft Corporation. All Rights Reserved. This code released under the terms of the MIT License (MIT, https://github.com/ALM-Rangers/Migrate-assets-from-RM-server-to-VSO/blob/master/License.txt). This is sample code only, do not use in production environments.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.ALMRangers.RMWorkflowMigrator.Tests.Unit
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.ALMRangers.RMWorkflowMigrator.DataAccess.Model;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell;
    using Microsoft.ALMRangers.RMWorkflowMigrator.Generator.PowerShell.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    // ReSharper disable InconsistentNaming
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class When_Resolving_Unique_Script_Parameters
    {
        [TestMethod]
        public void Configuration_Variable_Parameter_Names_Are_Remapped_To_Remove_Invalid_Characters()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments =
                                                   @"-Param __Hello World ~`!@#$%^&*()_+=|\__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   IsParameter
                                                                       =
                                                                       true, 
                                                                   OriginalName
                                                                       =
                                                                       @"Hello World ~`!@#$%^&*()_+=|\", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction);

            // Assert
            Assert.IsTrue(
                results.First().ConfigurationVariables.All(r => r.OriginalName == @"Hello World ~`!@#$%^&*()_+=|\"));
            Assert.IsTrue(results.First().ConfigurationVariables.All(r => r.RemappedName == "HelloWorld_"));
        }

        [TestMethod]
        public void Configuration_Variable_Replacement_Token_Names_Are_Remapped_To_Remove_Invalid_Characters()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments =
                                                   @"-Param __Hello World ~`!@#$%^&*()_+=|\__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   IsParameter
                                                                       =
                                                                       false, 
                                                                   OriginalName
                                                                       =
                                                                       @"Goodbye World ~`!@#$%^&*()_+=|\", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction);

            // Assert
            Assert.IsTrue(
                results.First().ConfigurationVariables.All(r => r.OriginalName == @"Goodbye World ~`!@#$%^&*()_+=|\"));
            Assert.IsTrue(results.First().ConfigurationVariables.All(r => r.RemappedName == "GoodbyeWorld_"));
        }

        [TestMethod]
        public void Invalid_PowerShell_Variable_Characters_Are_Removed_From_Command_Parameters()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments =
                                                   @"-Param __Hello World ~`!@#$%^&*()_+=|\__"
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction);

            // Assert
            Assert.AreEqual("-Param $HelloWorld_", results.First().Arguments);
        }

        [TestMethod]
        public void Many_Identical_And_Nonidentical_Parameters_And_Config_Values_Are_Made_Unique()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments =
                                                   @"-Param __Hello World__ -AnotherParam __Baz!__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }, 
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Config Token", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }, 
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Another Config Token@", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }, 
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Baz!", 
                                                                   Value
                                                                       =
                                                                       "A Value"
                                                               }
                                                       }
                                           }, 
                                       new ScriptAction
                                           {
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "Bar"
                                                               }, 
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Another Config Token", 
                                                                   Value
                                                                       =
                                                                       "Baz"
                                                               }
                                                       }
                                           }, 
                                       new ScriptAction
                                           {
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "Bar"
                                                               }, 
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Config Token", 
                                                                   Value
                                                                       =
                                                                       "Definitely Unique Value"
                                                               }, 
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Another Config Token", 
                                                                   Value
                                                                       =
                                                                       "Also Definitely Unique Value"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction).ToList();
            var action1 = results[0];
            var action2 = results[1];
            var action3 = results[2];

            // Assert
            Assert.AreEqual("-Param $HelloWorld -AnotherParam $Baz", action1.Arguments);

            Assert.IsTrue(action1.ConfigurationVariables.Any(cv => cv.RemappedName == "HelloWorld"));
            Assert.IsTrue(action1.ConfigurationVariables.Any(cv => cv.RemappedName == "ConfigToken"));
            Assert.IsTrue(action1.ConfigurationVariables.Any(cv => cv.RemappedName == "AnotherConfigToken"));
            Assert.IsTrue(action1.ConfigurationVariables.Any(cv => cv.RemappedName == "Baz"));

            Assert.IsTrue(action2.ConfigurationVariables.Any(cv => cv.RemappedName == "HelloWorld2"));
            Assert.IsTrue(action2.ConfigurationVariables.Any(cv => cv.RemappedName == "AnotherConfigToken3"));

            Assert.IsTrue(action3.ConfigurationVariables.Any(cv => cv.RemappedName == "HelloWorld2"));
            Assert.IsTrue(action3.ConfigurationVariables.Any(cv => cv.RemappedName == "ConfigToken4"));
            Assert.IsTrue(action3.ConfigurationVariables.Any(cv => cv.RemappedName == "AnotherConfigToken5"));
        }

        [TestMethod]
        public void Two_Identical_Parameters_That_Take_The_Different_Values_Are_Made_Unique()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           }, 
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "Bar"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction).ToList();

            // Assert
            Assert.AreEqual("-Param $HelloWorld", results[0].Arguments);
            Assert.IsTrue(results[0].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld"));
            Assert.AreEqual("-Param $HelloWorld2", results[1].Arguments);
            Assert.IsTrue(results[1].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld2"));
        }

        [TestMethod]
        public void A_Set_Of_NonSequential_Identical_Parameters_That_Take_A_Combination_Of_Different_And_Same_Values_Are_Made_Unique()
        {
            // Arrange
            var actions = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable { OriginalName = "Hello World", Value = "placeholder" }
                                                       }
                                           },
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable { OriginalName = "Hello World", Value = "Bar" }
                                                       }
                                           },
                                           new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable { OriginalName = "Hello World", Value = "placeholder" }
                                                       }
                                           },
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable { OriginalName = "Hello World", Value = "Bar" }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(actions).ToList();

            // Assert
            Assert.AreEqual("-Param $HelloWorld", results[0].Arguments);
            Assert.IsTrue(results[0].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld"));
            Assert.AreEqual("-Param $HelloWorld2", results[1].Arguments);
            Assert.IsTrue(results[1].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld2"));
            Assert.AreEqual("-Param $HelloWorld", results[2].Arguments);
            Assert.IsTrue(results[2].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld"));
            Assert.AreEqual("-Param $HelloWorld2", results[3].Arguments);
            Assert.IsTrue(results[3].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld2"));
        }

        [TestMethod]
        public void Two_Identical_Parameters_That_Take_The_Different_Values_Are_Made_Unique_When_One_Is_A_Command()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {

                                               Command = "__Hello World__.exe",
                                               Arguments = string.Empty,
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World",
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           },
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World",
                                                                   Value
                                                                       =
                                                                       "Bar"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction).ToList();

            // Assert
            Assert.AreEqual("$HelloWorld.exe", results[0].Command);
            Assert.IsTrue(results[0].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld"));
            Assert.AreEqual("-Param $HelloWorld2", results[1].Arguments);
            Assert.IsTrue(results[1].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld2"));
        }

        [TestMethod]
        public void Parameters_Are_Unique_Across_Both_Arguments_And_Commands()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {

                                               Command = "__Hello World__.exe",
                                               Arguments = string.Empty,
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World",
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           },
                                           new ScriptAction
                                           {

                                               Command = "__Hello World__.exe",
                                               Arguments = string.Empty,
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World",
                                                                   Value
                                                                       =
                                                                       "placeholder2"
                                                               }
                                                       }
                                           },
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World",
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction).ToList();

            // Assert
            Assert.AreEqual("$HelloWorld.exe", results[0].Command);
            Assert.IsTrue(results[0].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld"));
            Assert.AreEqual("$HelloWorld2.exe", results[1].Command);
            Assert.IsTrue(results[1].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld2"));
            Assert.AreEqual("-Param $HelloWorld", results[2].Arguments);
            Assert.IsTrue(results[2].ConfigurationVariables.All(cv => cv.RemappedName == "HelloWorld"));
        }

        [TestMethod]
        public void Text_That_Is_Not_A_Parameter_Token_Is_Not_Replaced()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Command = "test",
                                               Arguments =
                                                   "-SomeParameter __SomeParameter__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "SomeParameter",
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           },
                                       new ScriptAction
                                           {
                                               Command = "test",
                                               Arguments =
                                                   "-SomeParameter __SomeParameter__",
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "SomeParameter",
                                                                   Value
                                                                       =
                                                                       "placeholder2"
                                                               }
                                                       }
                                           }
                                   };
                                   
        

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction).ToList();

            // Assert
            Assert.AreEqual("-SomeParameter $SomeParameter", results[0].Arguments);
            Assert.AreEqual("-SomeParameter $SomeParameter2", results[1].Arguments);
        }


        [TestMethod]
        public void Two_Parameters_That_Take_The_Same_Value_Are_Untouched()
        {
            // Arrange
            var singleAction = new List<ScriptAction>
                                   {
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           }, 
                                       new ScriptAction
                                           {
                                               Arguments = @"-Param __Hello World__", 
                                               ConfigurationVariables =
                                                   new List<ConfigurationVariable>
                                                       {
                                                           new ConfigurationVariable
                                                               {
                                                                   OriginalName
                                                                       =
                                                                       "Hello World", 
                                                                   Value
                                                                       =
                                                                       "placeholder"
                                                               }
                                                       }
                                           }
                                   };

            // Act
            var results = UniquePropertyResolver.ResolveProperties(singleAction).ToList();

            // Assert
            Assert.AreEqual("-Param $HelloWorld", results[0].Arguments);
            Assert.AreEqual("-Param $HelloWorld", results[1].Arguments);
        }
    }
}