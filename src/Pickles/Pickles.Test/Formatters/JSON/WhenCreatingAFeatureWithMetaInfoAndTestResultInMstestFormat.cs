//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenCreatingAFeatureWithMetaInfoAndTestResultInMstestFormat.cs" company="PicklesDoc">
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

using System.Linq;

using Autofac;

using Newtonsoft.Json.Linq;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.DirectoryCrawler;
using PicklesDoc.Pickles.DocumentationBuilders.Json;
using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test.Helpers;
using PicklesDoc.Pickles.TestFrameworks.MsTest;

namespace PicklesDoc.Pickles.Test.Formatters.JSON
{
    public class WhenCreatingAFeatureWithMetaInfoAndTestResultInMstestFormat : BaseFixture
    {
        public string Setup()
        {
            const string OutputDirectoryName = FileSystemPrefix + @"JSONFeatureOutput";
            var rootPath = FileSystem.DirectoryInfo.FromDirectoryName(FileSystemPrefix + @"JSON\Features");

            const string TestResultFilePath = FileSystemPrefix + @"JSON\results-example-failing-and-pasing-mstest.trx";

            var filePath = FileSystem.Path.Combine(OutputDirectoryName, JsonDocumentationBuilder.JsonFileName);

            this.AddFakeFolderAndFiles("JSON", new[] { "results-example-failing-and-pasing-mstest.trx" });
            this.AddFakeFolderAndFiles(
                @"JSON\Features",
                new[]
                {
                    "OneScenarioTransferingMoneyBetweenAccountsFailing.feature",
                    "TransferBetweenAccounts_WithSuccess.feature",
                    "TwoScenariosTransferingFundsOneFailingOneSuccess.feature",
                    "TwoScenariosTransferingMoneyBetweenAccoutsWithSuccess.feature",
                });

            var resultFile = RetrieveContentOfFileFromResources(ResourcePrefix + "JSON.results-example-failing-and-pasing-mstest.trx");
            FileSystem.AddFile(TestResultFilePath, resultFile);

            var features = Container.Resolve<DirectoryTreeCrawler>().Crawl(rootPath, new ParsingReport());

            var outputDirectory = FileSystem.DirectoryInfo.FromDirectoryName(OutputDirectoryName);
            if (!outputDirectory.Exists)
            {
                outputDirectory.Create();
            }

            var configuration = new Configuration
            {
                OutputFolder = FileSystem.DirectoryInfo.FromDirectoryName(OutputDirectoryName),
                DocumentationFormat = DocumentationFormat.Json,
                TestResultsFormat = TestResultsFormat.MsTest,
                SystemUnderTestName = "SUT Name",
                SystemUnderTestVersion = "SUT Version"
            };
            configuration.AddTestResultFile(FileSystem.FileInfo.FromFileName(TestResultFilePath));

            ITestResults testResults = new MsTestResults(configuration, new MsTestSingleResultLoader(), new MsTestScenarioOutlineExampleMatcher());
            var jsonDocumentationBuilder = new JsonDocumentationBuilder(configuration, testResults, FileSystem, new LanguageServicesRegistry());
            jsonDocumentationBuilder.Build(features);
            var content = FileSystem.File.ReadAllText(filePath);

            return content;
        }

        [Test]
        public void ItShouldContainResultKeysInTheJsonDocument()
        {
            var content = this.Setup();

            content.AssertJsonContainsKey("Result");
        }

        [Test]
        public void ItShouldContainTheSutInfoInTheJsonDocument()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            var configuration = jsonObj["Configuration"];

            Check.That(configuration["SutName"].ToString()).IsEqualTo("SUT Name");
            Check.That(configuration["SutVersion"].ToString()).IsEqualTo("SUT Version");
        }

        [Test]
        public void ItShouldIndicateWasSuccessfulIsTrue()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            var featureJsonElement = from feat in jsonObj["Features"]
                                     where
                                         feat["Feature"]["Name"].Value<string>().Equals(
                                             "Two more scenarios transfering funds between accounts")
                                     select feat;

            Check.That(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>()).IsTrue();
        }

        [Test]
        public void ItShouldIndicateWasSuccessfulIsTrueForTheOtherSuccessFeature()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            var featureJsonElement = from feat in jsonObj["Features"]
                                     where
                                         feat["Feature"]["Name"].Value<string>().Equals(
                                             "Transfer funds between accounts")
                                     select feat;

            Check.That(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>()).IsTrue();
        }

        [Test]
        public void ItShouldIndicateWasSuccessfulIsFalseForFailingScenario()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            var featureJsonElement = from feat in jsonObj["Features"]
                                     where
                                         feat["Feature"]["Name"].Value<string>().Equals(
                                             "Transfer funds between accounts onc scenario and FAILING")
                                     select feat;

            Check.That(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>()).IsFalse();
        }

        [Test]
        public void ItShouldIndicateWasSuccessfulIsFalseForAnotherFailingScenario()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            var featureJsonElement = from feat in jsonObj["Features"]
                                     where
                                         feat["Feature"]["Name"].Value<string>().Equals(
                                             "Two more scenarios transfering funds between accounts - one failng and one succeding")
                                     select feat;

            Check.That(featureJsonElement.ElementAt(0)["Result"]["WasSuccessful"].Value<bool>()).IsFalse();
        }

        [Test]
        public void ItShouldContainWasSuccessfulKeyInJsonDocument()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            Check.That(jsonObj["Features"][0]["Result"]["WasSuccessful"].ToString()).IsNotEmpty();
        }

        [Test]
        public void ItShouldWasSuccessfulFalseForFeatureXJsonDocument()
        {
            var content = this.Setup();

            var jsonObj = JObject.Parse(content);

            Check.That(jsonObj["Features"][0]["Result"]["WasSuccessful"].ToString()).IsNotEmpty();
        }
    }
}
