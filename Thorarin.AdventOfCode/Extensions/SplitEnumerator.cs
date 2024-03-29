﻿namespace Thorarin.AdventOfCode.Extensions
{
    public ref struct SplitEnumerator
    {
        private ReadOnlySpan<char> _remaining;
        private ReadOnlySpan<char> _current;
        private readonly ReadOnlySpan<char> _split;
        private bool _isEnumeratorActive;

        internal SplitEnumerator(ReadOnlySpan<char> buffer, ReadOnlySpan<char> split)
        {
            _remaining = buffer;
            _split = split;
            _current = default;
            _isEnumeratorActive = true;
        }

        /// <summary>
        /// Gets the line at the current position of the enumerator.
        /// </summary>
        public ReadOnlySpan<char> Current => _current;

        /// <summary>
        /// Returns this instance as an enumerator.
        /// </summary>
        public SplitEnumerator GetEnumerator() => this;

        /// <summary>
        /// Advances the enumerator to the next line of the span.
        /// </summary>
        /// <returns>
        /// True if the enumerator successfully advanced to the next line; false if
        /// the enumerator has advanced past the end of the span.
        /// </returns>
        public bool MoveNext()
        {
            if (!_isEnumeratorActive)
            {
                return false; // EOF previously reached or enumerator was never initialized
            }

            int idx = _remaining.IndexOf(_split);
            if (idx >= 0)
            {
                _current = _remaining.Slice(0, idx);
                _remaining = _remaining.Slice(idx + _split.Length);
            }
            else
            {
                // We've reached EOF, but we still need to return 'true' for this final
                // iteration so that the caller can query the Current property once more.
                _current = _remaining;
                _remaining = default;
                _isEnumeratorActive = false;
            }

            return true;
        }
    }
}
