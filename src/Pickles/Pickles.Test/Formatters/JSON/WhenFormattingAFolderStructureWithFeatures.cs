﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenFormattingAFolderStructureWithFeatures.cs" company="PicklesDoc">
//  Copyright 2011 Jeffrey Cameron
//  Copyright 2012-present PicklesDoc team and community contributors
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

using Autofac;

using NUnit.Framework;

using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.Test.Helpers;

namespace PicklesDoc.Pickles.Test.Formatters.JSON
{
    [TestFixture]
    public class WhenFormattingAFolderStructureWithFeatures : BaseFixture
    {
        private const string OutputDirectory = FileSystemPrefix + @"JSONFeatureOutput";

        public void Setup()
        {
            var rootPath = FileSystem.DirectoryInfo.FromDirectoryName(FileSystemPrefix);

            this.AddFakeFolderStructures();

            var features = Container.Resolve<DirectoryTreeCrawler>().Crawl(rootPath, new ParsingReport());

            var outputDirectory = FileSystem.DirectoryInfo.FromDirectoryName(OutputDirectory);
            if (!outputDirectory.Exists)
            {
                outputDirectory.Create();
            }

            var configuration = new Configuration
            {
                OutputFolder = FileSystem.DirectoryInfo.FromDirectoryName(OutputDirectory),
                DocumentationFormat = DocumentationFormat.Json
            };

            var jsonDocumentationBuilder = new JsonDocumentationBuilder(configuration, null, FileSystem, new LanguageServicesRegistry());
            jsonDocumentationBuilder.Build(features);
        }

        [Test]
        public void ShouldContainTheFeatures()
        {
            this.Setup();

            var content = FileSystem.File.ReadAllText(this.FileSystem.Path.Combine(OutputDirectory, JsonDocumentationBuilder.JsonFileName));
            content.AssertJsonKeyValue("Name", "Addition");
        }
    }
}
