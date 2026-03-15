using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shigino.Contexts;
using Shigino.Extensions;
using Shigino.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        private ShiginoContext context;

        [ObservableProperty]
        private ObservableCollection<Category> categories;

        public CategoriesViewModel()
        {
            context = new();

            context.EnsureCreatedDatabase();

            Categories = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task AddAsync()
        {
            Category category = new();

            context.Add(category);

            await LoadAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CategoryExists)]
        public async Task RemoveAsync(Category category)
        {
            Categories.Remove(category);

            await LoadAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            await context.SaveChangesAsync();

            Categories.Clear();

            await foreach (Category category in
                context.Categories.AsAsyncEnumerable())
                Categories.Add(category);
        }

        public bool CategoryExists(Category category)
        {
            if (category == null) return false;

            EntityEntry<Category> entry = context.Entry(category);

            if (entry is null) return false;
            
            switch (entry.State)
            {
                case EntityState.Deleted:
                case EntityState.Detached:
                    return false;

                default:
                    return true;
            } 
        }
    }
}
