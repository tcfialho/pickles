﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DirectoryTreeCrawler.cs" company="PicklesDoc">
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

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;

using NLog;

using PicklesDoc.Pickles.DataStructures;

namespace PicklesDoc.Pickles.DirectoryCrawler
{
    public class DirectoryTreeCrawler
    {
        private static readonly Logger Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        private readonly FeatureNodeFactory featureNodeFactory;

        private readonly IFileSystem fileSystem;

        private readonly RelevantFileDetector relevantFileDetector;

        public DirectoryTreeCrawler(RelevantFileDetector relevantFileDetector, FeatureNodeFactory featureNodeFactory, IFileSystem fileSystem)
        {
            this.relevantFileDetector = relevantFileDetector;
            this.featureNodeFactory = featureNodeFactory;
            this.fileSystem = fileSystem;
        }

        public Tree Crawl(IDirectoryInfo directory, ParsingReport parsingReport)
        {
            return this.Crawl(directory, null, parsingReport);
        }

        private Tree Crawl(IDirectoryInfo directory, INode rootNode, ParsingReport parsingReport)
        {
            var currentNode =
                this.featureNodeFactory.Create(rootNode != null ? rootNode.OriginalLocation : null, directory, parsingReport);

            if (rootNode == null)
            {
                rootNode = currentNode;
            }

            var tree = new Tree(currentNode);

            var filesAreFound = this.CollectFiles(directory, rootNode, tree, parsingReport);

            var directoriesAreFound = this.CollectDirectories(directory, rootNode, tree, parsingReport);

            if (!filesAreFound && !directoriesAreFound)
            {
                return null;
            }

            return tree;
        }

        private bool CollectDirectories(IDirectoryInfo directory, INode rootNode, Tree tree, ParsingReport parsingReport)
        {
            var collectedNodes = new List<Tree>();

            foreach (var subDirectory in directory.GetDirectories().OrderBy(di => di.Name))
            {
                var subTree = this.Crawl(subDirectory, rootNode, parsingReport);
                if (subTree != null)
                {
                    collectedNodes.Add(subTree);
                }
            }

            foreach (var node in collectedNodes)
            {
                tree.Add(node);
            }

            return collectedNodes.Count > 0;
        }

        private bool CollectFiles(IDirectoryInfo directory, INode rootNode, Tree tree, ParsingReport parsingReport)
        {
            var collectedNodes = new List<INode>();

            foreach (var file in directory.GetFiles().Where(file => this.relevantFileDetector.IsRelevant(file)))
            {
                var node = this.featureNodeFactory.Create(rootNode.OriginalLocation, file, parsingReport);
                if (node != null)
                {
                    collectedNodes.Add(node);
                }
            }

            foreach (var node in OrderFileNodes(collectedNodes))
            {
                tree.Add(node);
            }

            return collectedNodes.Count > 0;
        }

        private static IEnumerable<INode> OrderFileNodes(List<INode> collectedNodes)
        {
            var indexFiles =
                collectedNodes.Where(
                    node => node.OriginalLocation.Name.StartsWith("index", StringComparison.InvariantCultureIgnoreCase));
            var otherFiles = collectedNodes.Except(indexFiles);

            return indexFiles.OrderBy(node => node.Name).Concat(otherFiles.OrderBy(node => node.Name));
        }
    }
}
