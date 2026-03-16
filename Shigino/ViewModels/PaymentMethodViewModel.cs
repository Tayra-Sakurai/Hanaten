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
    public partial class PaymentMethodViewModel : ObservableObject
    {
        private readonly ShiginoContext context;
        private PaymentMethod paymentMethod;

        public PaymentMethodViewModel()
        {
            context = new();
            paymentMethod = new();
        }

        public double Balance => context
            .Entry(paymentMethod)
            .Collection(e => e.Items)
            .Query()
            .Sum(i => i.Income - i.Expense);

        public string Name
        {
            get => paymentMethod.Name;
            set => SetProperty(paymentMethod.Name, value, paymentMethod, (m, v) => m.Name = v);
        }

        public async Task InitializeForExistingValue(int id)
        {
            paymentMethod = await context.PaymentMethods.FindAsync(id);
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Balance));
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
