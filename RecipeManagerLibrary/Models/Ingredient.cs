/*
MIT License
Copyright(c) 2021 Kyle Givler
https://github.com/JoyfulReaper
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeLibrary.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name of the Ingredient
        /// </summary>
        [Required]
        [StringLength(75)]
        public string Name {get; set;}

        /// <summary>
        /// The reciple to make the ingredeint
        /// </summary>
        //public Recipe Recipe { get; set; }

        /// <summary>
        /// Quanity of the ingredient
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Measurement of the ingredient
        /// </summary>
        public Measurement Measurement{ get; set; }

        /// <summary>
        /// Many-to-Many releationship
        /// </summary>
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
