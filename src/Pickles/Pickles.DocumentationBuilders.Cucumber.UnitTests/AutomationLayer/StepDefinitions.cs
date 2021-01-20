﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StepDefinitions.cs" company="PicklesDoc">
//  Copyright 2017 Dmitry Grekov
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


using System;
using System.IO;
using System.Linq;

using Autofac;

using NFluent;

using PicklesDoc.Pickles;
using PicklesDoc.Pickles.DataStructures;
using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Cucumber;
using PicklesDoc.Pickles.Test;

using TechTalk.SpecFlow;

namespace Pickles.DocumentationBuilders.Cucumber.UnitTests.AutomationLayer
{
    [Binding]
    [Scope(Tag = "cucumber")]
    public sealed class StepDefinitions : BaseFixture /* God object antipattern */
    {
        private Tree nodes;

        [Given("I have this feature description")]
        public void IHaveThisFeatureDescription(string featureDescription)
        {
            var configuration = this.Configuration;
            var parser = new FeatureParser(configuration);

            var feature = parser.Parse(new StringReader(featureDescription));

            this.nodes = new Tree(new FeatureNode(this.FileSystem.DirectoryInfo.FromDirectoryName(@"c:\output\"), string.Empty, feature));
        }

        [When(@"I generate the documentation")]
        public void WhenIGenerateTheJsonDocumentation()
        {
            var configuration = this.Configuration;
            configuration.OutputFolder = this.FileSystem.DirectoryInfo.FromDirectoryName(@"c:\output\");
            var jsonDocumentationBuilder = this.Container.Resolve<CucumberDocumentationBuilder>();

            jsonDocumentationBuilder.Build(this.nodes);
        }

        [Then("the JSON file should contain")]
        public void ThenTheResultShouldBe(string expectedResult)
        {
            var actualResult = this.FileSystem.File.ReadAllText(@"c:\output\cucumberResult.json");

            //standardize newlines across various environments
            actualResult = actualResult.Replace("\r\n", "\n");
            expectedResult = expectedResult.Replace("\r\n", "\n");
            Check.That(actualResult).Contains(expectedResult);
        }
    }
}
