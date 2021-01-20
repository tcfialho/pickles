//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="StringExtensionsTests.cs" company="PicklesDoc">
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

using PicklesDoc.Pickles.Extensions;

namespace PicklesDoc.Pickles.Test.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ThenCanFormatTextAsWikiWordSuccessfully()
        {
            var actual = "ThisIsTheWikiWord".ExpandWikiWord();
            Check.That(actual).IsEqualTo("This Is The Wiki Word");
        }

        [Test]
        public void ThenCanFormatTextWithAcronymAndNumberAsWikiWordSuccessfully()
        {
            var actual = "ThisIsAnACRONYM1".ExpandWikiWord();
            Check.That(actual).IsEqualTo("This Is An ACRONYM1");
        }

        [Test]
        public void ThenCanFormatTextWithAcronymAsWikiWordSuccessfully()
        {
            var actual = "ThisIsAnACRONYM".ExpandWikiWord();
            Check.That(actual).IsEqualTo("This Is An ACRONYM");
        }

        [Test]
        public void ThenCanFormatTextWithLongNumbersAsWikiWordSuccessfully()
        {
            var actual = "ThisIsThe5000thWikiWord".ExpandWikiWord();
            Check.That(actual).IsEqualTo("This Is The 5000th Wiki Word");
        }

        [Test]
        public void ThenCanFormatTextWithNumberFollowedByCapitalAsWikiWordSuccessfully()
        {
            var actual = "001FeatureOne".ExpandWikiWord();
            Check.That(actual).IsEqualTo("001 Feature One");
        }

        [Test]
        public void ThenCanFormatTextWithNumbersAsWikiWordSuccessfully()
        {
            var actual = "ThisIsThe4thWikiWord".ExpandWikiWord();
            Check.That(actual).IsEqualTo("This Is The 4th Wiki Word");
        }

        [Test]
        public void ThenCanFormatTextWithSpecialCharactersAsWikiWordSuccessfully()
        {
            var actual = "ThisIsThe_WikiWord".ExpandWikiWord();
            Check.That(actual).IsEqualTo("This Is The Wiki Word");
        }
    }
}
