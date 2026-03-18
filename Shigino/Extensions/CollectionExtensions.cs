using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shigino.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Gets the inner product with <paramref name="numbers1"/>.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number of the vectors.</typeparam>
        /// <param name="numbers">The vector to be multiplicated.</param>
        /// <param name="numbers1">The vector to multiplicate.</param>
        /// <returns>The inner product.</returns>
        /// <exception cref="ArgumentException">When the vector shapes are not suitable.</exception>
        /// <exception cref="ArgumentNullException">When one argument is null.</exception>
        public static TNumber GetInnerProduct<TNumber>(this ICollection<TNumber> numbers, ICollection<TNumber> numbers1)
            where TNumber : INumberBase<TNumber>
        {
            ArgumentNullException.ThrowIfNull(numbers);
            ArgumentNullException.ThrowIfNull(numbers1);

            if (numbers.Count != numbers1.Count)
                throw new ArgumentException("The vectors must have the same length.", nameof(numbers1));

            TNumber result = TNumber.Zero;

            foreach ((TNumber number1, TNumber number2) in numbers.Zip(numbers1))
                result += number1 * number2;

            return result;
        }
    }
}
