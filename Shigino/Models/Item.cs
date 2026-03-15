// SPDX-FileCopyrightText: 2026 Tayra Sakurai
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.Models
{
    public class Item
    {
        public int Id { get; set; }
        public DateTime DateAndTime { get; set; } = DateTime.Now;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PaymentMethodId { get; set; }
        public int CategoryId { get; set; }
        public int Expense { get; set; } = 0;
        public int Income { get; set; } = 0;
        public PaymentMethod PaymentMethod { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
