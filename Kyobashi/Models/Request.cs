// SPDX-FileCopyrightText: 2026 Tayra Sakurai
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyobashi.Models
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public decimal RequestedAmount { get; set; } = 0;
        public decimal Balance { get; set; } = 0;
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = null;
        public bool IsAccepted { get; set; } = false;
    }
}
