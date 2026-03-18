// SPDX-FileCopyrightText: 2026 Tayra Sakurai
//
// SPDX-License-Identifier: AGPL-3.0-or-later

#nullable enable

using Microsoft.Extensions.AI;
using Shigino.Extensions;
using Shigino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.Services
{
    public class GeminiSearchService : ISearchService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> generator;

        public GeminiSearchService(IEmbeddingGenerator<string, Embedding<float>> generator)
        {
            this.generator = generator;
        }

        public async Task<IList<float>> GetVectorAsync(Item item)
        {
            return await GetVectorAsync(item.Name, item.Description);
        }

        public async Task<IList<float>> GetVectorAsync(Category category)
        {
            return await GetVectorAsync(category.Name);
        }

        public async Task<IList<float>> GetVectorAsync(string categoryName)
        {
            AdditionalPropertiesDictionary pairs = new(new Dictionary<string, object?>
            {
                { "TaskType", "CLASSIFICATION" },
            });
            EmbeddingGenerationOptions options = new EmbeddingGenerationOptions
            {
                AdditionalProperties = pairs,
                Dimensions = 768,
            };

            return (await generator.GenerateVectorAsync(categoryName, options))
                .ToArray()
                .ToList();
        }

        public async Task<IList<float>> GetVectorAsync(string title, string document)
        {
            AdditionalPropertiesDictionary pairs = new()
            {
                { "Title", title },
                { "TaskType", "RETRIEVAL_DOCUMENT" }
            };
            EmbeddingGenerationOptions generationOptions = new()
            {
                AdditionalProperties = pairs,
                Dimensions = 768
            };

            return (await generator.GenerateAsync(document, generationOptions))
                .Vector
                .ToArray()
                .ToList();
        }

        public async Task<IList<Item>> SearchAndReorderAsync(ICollection<Item> items, string query)
        {
            AdditionalPropertiesDictionary pairs = new()
            {
                { "TaskType", "RETRIEVAL_QUERY" },
            };
            EmbeddingGenerationOptions options = new()
            {
                AdditionalProperties = pairs,
                Dimensions = 768,
            };

            Embedding<float> embedding = await generator.GenerateAsync(query, options);

            List<float> vector = [.. embedding.Vector.ToArray()];

            List<Item> result =
                (from item in items
                 orderby item.Vector.GetInnerProduct(vector) descending
                 select item).ToList();

            return result;
        }

        public async Task<IList<Category>> SuggestCategoryAsync(ICollection<Category> categories, Item item)
        {
            item.Vector ??= [.. await GetVectorAsync(item)];

            return categories.OrderByDescending(x => x.Vector.GetInnerProduct(item.Vector))
                .ThenBy(x => x.Id)
                .ToList();
        }
    }
}
