namespace Gobln.Domain
{
    /// <summary>
    /// Specifies the direction in which to sort a list of items.
    /// <remarks>Used for Gobln orderby Extension</remarks>
    /// </summary>
    public enum OrderDirectionEnum
    {
        /// <summary>
        /// Sort from smallest to largest. For example, from A to Z.
        /// </summary>
        Ascending = 0,

        /// <summary>
        /// Sort from largest to smallest. For example, from Z to A.
        /// </summary>
        Descending = 1
    }
}