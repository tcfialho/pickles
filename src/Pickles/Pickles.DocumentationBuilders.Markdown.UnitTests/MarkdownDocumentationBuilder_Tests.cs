﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MarkdownDocumentationBuilder_Tests.cs" company="PicklesDoc">
//  Copyright 2018 Darren Comeau
//  Copyright 2018-present PicklesDoc team and community contributors
//
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;

using Autofac;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.ObjectModel;

namespace PicklesDoc.Pickles.DocumentationBuilders.Markdown.UnitTests
{
    [TestFixture]
    public class MarkdownDocumentationBuilder_Tests
    {
        [Test]
        public void New_MarkdownDocumentationBuilder_Is_An_Implementation_Of_IDocumentationBuilder()
        {
            IFileSystem filesystem = new MockFileSystem();
            IConfiguration configuration = new Configuration();

            var documentationBuilder = new MarkdownDocumentationBuilder(filesystem, configuration);

            Assert.IsInstanceOf<IDocumentationBuilder>(documentationBuilder);
        }

        [Test]
        public void Autofac_Can_Resolve_MarkdownDocumentationBuilder_Request()
        {
            var container = BuildContainer();

            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();

            Assert.IsNotNull(markdownDocumentationBuilder);
        }

        [Test]
        public void When_I_Build_Documentation_A_File_Is_Created()
        {
            var outputFolder = @"c:\output";
            var defaultOutputFile = @"c:\output\features.md";

            var container = BuildContainer();
            var configuration = container.Resolve<IConfiguration>();
            var fileSystem = (MockFileSystem)container.Resolve<IFileSystem>();
            configuration.OutputFolder = fileSystem.DirectoryInfo.FromDirectoryName(outputFolder);
            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();

            markdownDocumentationBuilder.Build(null);

            Assert.IsTrue(fileSystem.FileExists(defaultOutputFile));
        }

        [Test]
        public void When_I_Build_Documentation_Icon_Files_Are_Created()
        {
            var outputFolder = @"c:\output";

            var container = BuildContainer();
            var configuration = container.Resolve<IConfiguration>();
            var fileSystem = (MockFileSystem)container.Resolve<IFileSystem>();
            configuration.OutputFolder = fileSystem.DirectoryInfo.FromDirectoryName(outputFolder);
            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();

            markdownDocumentationBuilder.Build(null);

            Assert.IsTrue(fileSystem.FileExists(@"c:\output\pass.png"));
            Assert.IsTrue(fileSystem.FileExists(@"c:\output\fail.png"));
            Assert.IsTrue(fileSystem.FileExists(@"c:\output\inconclusive.png"));
        }

        [Test]
        public void With_A_Null_Tree_The_Output_Has_Default_Content()
        {
            var outputFolder = @"c:\output";
            var defaultOutputFile = @"c:\output\features.md";
            var expectedFile = new string[]
            {
                "# Features",
                "",
                "Generated on:"
            };

            var container = BuildContainer();
            var configuration = container.Resolve<IConfiguration>();
            var fileSystem = (MockFileSystem)container.Resolve<IFileSystem>();
            configuration.OutputFolder = fileSystem.DirectoryInfo.FromDirectoryName(outputFolder);
            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();

            markdownDocumentationBuilder.Build(null);

            var actualFile = fileSystem.File.ReadAllLines(defaultOutputFile);

            for (var i = 0; i < expectedFile.Length; ++i)
            {
                Check.That(actualFile[i]).Contains(expectedFile[i]);
            }
        }

        [Test]
        public void With_A_Simple_Feature_The_Output_Has_Default_Content()
        {
            var outputFolder = @"c:\output";
            var defaultOutputFile = @"c:\output\features.md";
            var expectedFile = new string[]
            {
                "# Features",
                "",
                "Generated on:",
                "",
                "### Simple Feature",
                ""
            };

            var container = BuildContainer();
            var configuration = container.Resolve<IConfiguration>();
            var fileSystem = (MockFileSystem)container.Resolve<IFileSystem>();
            configuration.OutputFolder = fileSystem.DirectoryInfo.FromDirectoryName(outputFolder);
            var markdownDocumentationBuilder = container.Resolve<MarkdownDocumentationBuilder>();

            var simpleFeature = new Feature
            {
                Name = "Simple Feature"
            };
            var relPath = "fakedir";
            var location = fileSystem.FileInfo.FromFileName(@"c:\");
            var newNode = new FeatureNode(location, relPath, simpleFeature);
            var featureTree = new Tree(new FolderNode(location, relPath))
            {
                newNode
            };

            markdownDocumentationBuilder.Build(featureTree);

            var actualFile = fileSystem.File.ReadAllLines(defaultOutputFile);

            for (var i = 0; i < expectedFile.Length; ++i)
            {
                Check.That(actualFile[i]).Contains(expectedFile[i]);
            }
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PicklesModule>();
            builder.Register<MockFileSystem>(_ => CreateMockFileSystem()).As<IFileSystem>().SingleInstance();

            var container = builder.Build();

            return container;
        }

        private MockFileSystem CreateMockFileSystem()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.Directory.SetCurrentDirectory(Assembly.GetExecutingAssembly().Location);
            return mockFileSystem;
        }
    }
}
