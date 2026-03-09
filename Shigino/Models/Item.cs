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
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PaymentMethodId { get; set; }
        public int CategoryId { get; set; }
        public double Balance { get; set; } = 0;
        public PaymentMethod PaymentMethod { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
