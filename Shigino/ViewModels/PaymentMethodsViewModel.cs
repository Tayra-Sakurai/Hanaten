using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
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
    public partial class PaymentMethodsViewModel : ObservableObject
    {
        private readonly ShiginoContext context;

        [ObservableProperty]
        private ObservableCollection<PaymentMethod> paymentMethods;

        public PaymentMethodsViewModel()
        {
            context = new();

            context.EnsureCreatedDatabase();

            PaymentMethods = [];
        }

        public async Task LoadAsync()
        {
            await context.SaveChangesAsync();

            PaymentMethods.Clear();

            List<PaymentMethod> paymentMethods = await context.PaymentMethods.Include(e => e.Items).ToListAsync();
            foreach (var paymentMethod in paymentMethods)
                PaymentMethods.Add(paymentMethod);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task AddAsync()
        {
            PaymentMethod newMethod = new();

            await context.AddAsync(newMethod);
            await LoadAsync();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRemove))]
        public async Task RemoveAsync(PaymentMethod paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(paymentMethod);
            context.Remove(paymentMethod);

            await LoadAsync();
        }

        public static bool CanRemove(PaymentMethod paymentMethod)
        {
            return paymentMethod is not null;
        }
    }
}
