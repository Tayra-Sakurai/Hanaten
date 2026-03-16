using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Shigino.Contexts;
using Shigino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.ViewModels
{
    public partial class CategoryViewModel : ObservableObject
    {
        /// <summary>
        /// The context instance.
        /// </summary>
        private readonly ShiginoContext context;

        /// <summary>
        /// The category instance.
        /// </summary>
        private Category category;

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name
        {
            get => category.Name;
            set => SetProperty(category.Name, value, category, (m, v) => m.Name = v);
        }

        public int Balance => context
            .Entry(category)
            .Collection(e => e.Items)
            .Query()
            .Sum(item => item.Income - item.Expense);

        public CategoryViewModel()
        {
            context = new();
            category = new();
        }

        /// <summary>
        /// Finds and sets the value based on the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the target.</param>
        /// <returns>An instance of <see cref="Task"/> to manage this asynchronous operation.</returns>
        public async Task InitializeForExistingValueAsync(int id)
        {
            category = await context.Categories.FindAsync(id);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Balance));
        }

        /// <summary>
        /// Save the data.
        /// </summary>
        /// <returns>An instance of <see cref="Task"/> to manage the operation.</returns>
        [RelayCommand]
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
