using CommunityToolkit.Mvvm.ComponentModel;
using Shigino.Contexts;
using Shigino.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.ViewModels
{
    public partial class ItemViewModel : ObservableObject
    {
        private readonly ShiginoContext shiginoContext;
        private Item item;

        public DateTimeOffset Date
        {
            get => item.DateAndTime.Date;
            set => SetProperty(item.DateAndTime.Date, value, item, SetDateAndTime);
        }

        public TimeSpan Time
        {
            get => item.DateAndTime.TimeOfDay;
            set => SetProperty(item.DateAndTime.TimeOfDay, value, item, SetDateAndTime);
        }

        private static void SetDateAndTime(Item model, DateTimeOffset value)
        {
            TimeSpan time = model.DateAndTime.TimeOfDay;
            DateTime dateTime = value.Date;
            model.DateAndTime = dateTime.Add(time);
        }

        private static void SetDateAndTime(Item model, TimeSpan value)
        {
            DateTime dateTime = model.DateAndTime.Date;
            model.DateAndTime = dateTime.Add(value);
        }

        public string ItemName
        {
            get => item.Name;
            set => SetProperty(item.Name, value, item, (m, v) => m.Name = v);
        }

        public string Description
        {
            get => item.Description;
            set => SetProperty(item.Description, value, item, (m, v) => m.Description = v);
        }

        public Category Category
        {
            get => item.Category;
            set => SetProperty(item.Category.Id, value.Id, item, (m, v) => m.Category = shiginoContext.Categories.Find(v));
        }

        public PaymentMethod PaymentMethod
        {
            get => item.PaymentMethod;
            set => SetProperty(item.PaymentMethod.Id, value.Id, item, (m, v) => m.PaymentMethod = shiginoContext.PaymentMethods.Find(v));
        }

        public ObservableCollection<Category> Categories => [.. shiginoContext.Categories.ToList()];
        public ObservableCollection<PaymentMethod> PaymentMethods => [.. shiginoContext.PaymentMethods.ToList()];

        public ItemViewModel()
        {
            shiginoContext = new();
            item = new()
            {
                Category = shiginoContext.Categories.First(),
                PaymentMethod = shiginoContext.PaymentMethods.First(),
            };
        }

        public async Task InitializeForExistingAsync(int id)
        {
            item = await shiginoContext.Items.FindAsync(id);
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Time));
            OnPropertyChanged(nameof(ItemName));
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(PaymentMethod));
            OnPropertyChanged(nameof(Expense));
            OnPropertyChanged(nameof(Income));
        }

        public double Expense
        {
            get => item.Expense;
            set => SetProperty(item.Expense, value, item, (m, v) => m.Expense = (int)v);
        }

        public double Income
        {
            get => item.Income;
            set => SetProperty(item.Income, value, item, (m, v) => m.Income = (int)v);
        }
    }
}
