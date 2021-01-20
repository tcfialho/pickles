﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="WhenDeterminingTheSignatureOfAnXUnitExampleRow.cs" company="PicklesDoc">
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

using NFluent;

using NUnit.Framework;

using PicklesDoc.Pickles.ObjectModel;
using PicklesDoc.Pickles.Test;
using PicklesDoc.Pickles.TestFrameworks.XUnit;

namespace PicklesDoc.Pickles.TestFrameworks.UnitTests.XUnit.XUnit1
{
    [TestFixture]
    public class WhenDeterminingTheSignatureOfAnXUnitExampleRow : BaseFixture
    {
        [Test]
        public void ThenCanSuccessfullyMatch()
        {
            var scenarioOutline = new ScenarioOutline { Name = "Adding several numbers" };
            var exampleRow = new[] { "40", "50", "90" };

            var signatureBuilder = new XUnitExampleSignatureBuilder();
            var signature = signatureBuilder.Build(scenarioOutline, exampleRow);

            var isMatch = signature.IsMatch("Pickles.TestHarness.xUnit.AdditionFeature.AddingSeveralNumbers(firstNumber: \"40\", secondNumber: \"50\", result: \"90\", exampleTags: System.String[])".ToLowerInvariant());
            Check.That(isMatch).IsTrue();
        }
    }
}
