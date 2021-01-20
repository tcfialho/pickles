//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HtmlDocumentationBuilderTests.cs" company="PicklesDoc">
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
using PicklesDoc.Pickles.DocumentationBuilders.Html;

namespace PicklesDoc.Pickles.Test.Formatters
{
    [TestFixture]
    public class HtmlDocumentationBuilderTests : BaseFixture
    {
        private const string RootPath = FileSystemPrefix + @"EmptyFolderTests";

        [Test]
        public void ShouldNotBlowUpWHenParsingEmptyFolder()
        {
            var rootPath = FileSystem.DirectoryInfo.FromDirectoryName(RootPath);

            this.AddFakeFolderStructures();

            var configuration = this.Configuration;
            configuration.OutputFolder = this.FileSystem.DirectoryInfo.FromDirectoryName(FileSystemPrefix);
            var features = Container.Resolve<DirectoryTreeCrawler>().Crawl(rootPath, new ParsingReport());
            var builder = Container.Resolve<HtmlDocumentationBuilder>();

            builder.Build(features);
        }
    }
}
