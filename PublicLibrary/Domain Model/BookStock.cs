// <copyright file="BookStock.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PublicLibrary.Domain_Model
{
    /// <summary>
    /// The BookStock entity.
    /// </summary>
    public class BookStock
    {
        /// <summary>
        /// Gets or sets stock code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets amount.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets lecture room amount.
        /// </summary>
        public int LectureRoomAmount { get; set; }
    }
}