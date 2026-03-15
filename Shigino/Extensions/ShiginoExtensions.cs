using Shigino.Contexts;
using Shigino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.Extensions
{
    public static class ShiginoExtensions
    {
        /// <summary>
        /// Ensure the database created.
        /// Then initializes the database.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The value ehich indicates whether the database is created.</returns>
        public static bool EnsureCreatedDatabase(this ShiginoContext context)
        {
            bool isCreated = context.Database.EnsureCreated();

            if (isCreated)
            {
                context.Add(
                    new Category
                    {
                        Name = "Default",
                    });

                context.Add(
                    new PaymentMethod
                    {
                        Name = "Default",
                    });

                context.SaveChanges();
            }

            return isCreated;
        }
    }
}
