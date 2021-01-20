﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenAddingAScenarioToAWorksheet.cs" company="PicklesDoc">
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

using System.Collections.Generic;

using Autofac;

using ClosedXML.Excel;

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;

namespace PicklesDoc.Pickles.DocumentationBuilders.Excel.UnitTests
{
    [TestFixture]
    public class WhenAddingAScenarioToAWorksheet : BaseFixture
    {
        [Test]
        public void ThenSingleScenarioAddedSuccessfully()
        {
            var excelScenarioFormatter = Container.Resolve<ExcelScenarioFormatter>();
            var scenario = new Scenario
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature",
                Tags = { "tag1", "tag2" }
            };

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("SHEET1");
                var row = 3;
                excelScenarioFormatter.Format(worksheet, scenario, ref row);

                Check.That(worksheet.Cell("B3").Value).IsEqualTo(scenario.Name);
                Check.That(worksheet.Cell("C4").Value).IsEqualTo("tag1, tag2");
                Check.That(worksheet.Cell("C5").Value).IsEqualTo(scenario.Description);
                Check.That(row).IsEqualTo(6);
            }
        }

        [Test]
        public void ThenSingleScenarioWithStepsAddedSuccessfully()
        {
            var excelScenarioFormatter = Container.Resolve<ExcelScenarioFormatter>();
            var scenario = new Scenario
            {
                Name = "Test Feature",
                Description =
                    "In order to test this feature,\nAs a developer\nI want to test this feature"
            };
            var given = new Step { NativeKeyword = "Given", Name = "a precondition" };
            var when = new Step { NativeKeyword = "When", Name = "an event occurs" };
            var then = new Step { NativeKeyword = "Then", Name = "a postcondition" };
            scenario.Steps = new List<Step>(new[] { given, when, then });

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("SHEET1");
                var row = 3;
                excelScenarioFormatter.Format(worksheet, scenario, ref row);

                Check.That(worksheet.Cell("B3").Value).IsEqualTo(scenario.Name);
                Check.That(worksheet.Cell("C4").Value).IsEqualTo(scenario.Description);
                Check.That(row).IsEqualTo(8);
            }
        }
    }
}
