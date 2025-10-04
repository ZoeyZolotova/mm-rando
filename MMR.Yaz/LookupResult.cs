using System.Runtime.CompilerServices;

namespace MMR.Yaz
{
    /// <summary>
    /// Contains state for result of a lookup operation.
    /// </summary>
    public readonly struct LookupResult
    {
        /// <summary>
        /// Offset of lookup result, or -1 if none found.
        /// </summary>
        public readonly int Offset;

        /// <summary>
        /// Length of lookup result.
        /// </summary>
        public readonly int Length;

        /// <summary>
        /// Whether or not to skip the current byte in favor of the <see cref="LookupResult"/> for the following byte. Used with the <see cref="LookupScheme.LookAhead"/> scheme.
        /// </summary>
        public readonly bool SkipByte;

        /// <summary>
        /// Whether or not the lookup offset is valid.
        /// </summary>
        public readonly bool Found => Offset >= 0;

        /// <summary>
        /// Construct a <see cref="LookupResult"/> with an offset and length.
        /// </summary>
        /// <param name="offset">Result offset.</param>
        /// <param name="length">Result length.</param>
        /// <param name="skipByte">Whether or not to skip current byte.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LookupResult(int offset, int length, bool skipByte = false) => (Offset, Length, SkipByte) = (offset, length, skipByte);

        /// <summary>
        /// Return an identical <see cref="LookupResult"/> with a specific SkipByte value.
        /// </summary>
        /// <param name="skipByte">SkipByte value.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly LookupResult WithSkipByte(bool skipByte) => new LookupResult(this.Offset, this.Length, skipByte);

        /// <summary>
        /// Return a <see cref="LookupResult"/> indicating nothing was found.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LookupResult NoneFound() => new LookupResult(-1, 0);

        /// <summary>
        /// Get cleared <see cref="LookupResult"/> depending on if byte was skipped.
        /// </summary>
        /// <param name="result">Existing <see cref="LookupResult"/>.</param>
        /// <returns></returns>
        /// <remarks>Using <c>ref</c> for input due to it being faster when inlined?</remarks>
        public static LookupResult ClearIfNotSkipped(ref LookupResult result)
        {
            if (!result.SkipByte)
            {
                // If current byte was not skipped, clear result.
                return LookupResult.NoneFound();
            }
            else
            {
                // If current byte was skipped, save lookup result for use during next loop.
                return result.WithSkipByte(false);
            }
        }
    }
}
