// SPDX-FileCopyrightText: 2026 Tayra Sakurai
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Shigino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.Services
{
    public interface ISearchService
    {
        Task<IList<float>> GetVectorAsync(Item item);
        Task<IList<float>> GetVectorAsync(Category category);
        Task<IList<float>> GetVectorAsync(string categoryName);
        Task<IList<float>> GetVectorAsync(string title, string document);
        Task<IList<Item>> SearchAndReorderAsync(ICollection<Item> items, string query);
        Task<IList<Category>> SuggestCategoryAsync(ICollection<Category> categories, Item item);
    }
}
