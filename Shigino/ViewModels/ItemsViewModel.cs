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
    public partial class ItemsViewModel : ObservableObject
    {
        private ShiginoContext context;

        [ObservableProperty]
        private ObservableCollection<Item> items;

        public ItemsViewModel()
        {
            context = new();

            context.EnsureCreatedDatabase();

            Items = [];
        }

        /// <summary>
        /// Loads the database.
        /// </summary>
        /// <returns>The task to manage this operation.</returns>
        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Items.Clear();

            await context.SaveChangesAsync();

            List<Item> items =
                context.Items
                .OrderByDescending(item => item.DateAndTime)
                .ThenBy(item => item.Id)
                .ToList();

            foreach (var item in items)
                Items.Add(item);
        }

        /// <summary>
        /// Adds a new item to the database.
        /// </summary>
        /// <returns>A task to manage the addition operation.</returns>
        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task AddAsync()
        {
            Item newItem = new()
            {
                CategoryId = 1,
                PaymentMethodId = 1,
            };

            context.Add(newItem);

            await LoadAsync();
        }

        /// <summary>
        /// Removes the item from the database.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>An instance of <see cref="Task"/> to manage this operation.</returns>
        /// <exception cref="ArgumentNullException">When the item is null.</exception>
        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(ItemExists))]
        public async Task RemoveAsync(Item item)
        {
            ArgumentNullException.ThrowIfNull(item);

            context.Remove(item);

            await LoadAsync();
        }

        public bool ItemExists(Item item)
        {
            if (item == null) return false;

            EntityEntry<Item> entry = context.Entry(item);

            if (entry == null ||
                entry.State == EntityState.Detached ||
                entry.State == EntityState.Deleted)
                return false;

            return true;
        }
    }
}
