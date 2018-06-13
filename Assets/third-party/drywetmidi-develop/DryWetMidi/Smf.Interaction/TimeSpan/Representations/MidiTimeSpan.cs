﻿using Melanchall.DryWetMidi.Common;
using System;

namespace Melanchall.DryWetMidi.Smf.Interaction
{
    /// <summary>
    /// Represents a time span as an amount of time measured in units of the time division
    /// of a MIDI file.
    /// </summary>
    public sealed class MidiTimeSpan : ITimeSpan
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MidiTimeSpan"/>.
        /// </summary>
        public MidiTimeSpan()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MidiTimeSpan"/> with the specified
        /// time span.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is negative.</exception>
        public MidiTimeSpan(long timeSpan)
        {
            ThrowIfLengthArgument.IsNegative(nameof(timeSpan), timeSpan);

            TimeSpan = timeSpan;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the time span of the current <see cref="MidiTimeSpan"/>.
        /// </summary>
        public long TimeSpan { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the string representation of a MIDI time span to its <see cref="MidiTimeSpan"/>
        /// equivalent. A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string containing a time span to convert.</param>
        /// <param name="timeSpan">When this method returns, contains the <see cref="MidiTimeSpan"/>
        /// equivalent of the time span contained in <paramref name="input"/>, if the conversion succeeded, or
        /// null if the conversion failed. The conversion fails if the <paramref name="input"/> is null or
        /// <see cref="String.Empty"/>, or is not of the correct format. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.</param>
        /// <returns>true if <paramref name="input"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string input, out MidiTimeSpan timeSpan)
        {
            return MidiTimeSpanParser.TryParse(input, out timeSpan).Status == ParsingStatus.Parsed;
        }

        /// <summary>
        /// Converts the string representation of a MIDI time span to its <see cref="MidiTimeSpan"/>
        /// equivalent.
        /// </summary>
        /// <param name="input">A string containing a time span to convert.</param>
        /// <returns>A <see cref="MidiTimeSpan"/> equivalent to the time span contained in
        /// <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="input"/> is null or contains white-spaces only.</exception>
        /// <exception cref="FormatException"><paramref name="input"/> has invalid format.</exception>
        public static MidiTimeSpan Parse(string input)
        {
            MidiTimeSpan timeSpan;
            var parsingResult = MidiTimeSpanParser.TryParse(input, out timeSpan);
            if (parsingResult.Status == ParsingStatus.Parsed)
                return timeSpan;

            throw parsingResult.Exception;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts <see cref="long"/> to <see cref="MidiTimeSpan"/>.
        /// </summary>
        /// <param name="timeSpan"><see cref="long"/> to convert to <see cref="MidiTimeSpan"/>.</param>
        public static explicit operator MidiTimeSpan(long timeSpan)
        {
            return new MidiTimeSpan(timeSpan);
        }

        /// <summary>
        /// Converts <see cref="MidiTimeSpan"/> to <see cref="long"/>.
        /// </summary>
        /// <param name="timeSpan"><see cref="MidiTimeSpan"/> to convert to <see cref="long"/>.</param>
        public static implicit operator long(MidiTimeSpan timeSpan)
        {
            return timeSpan.TimeSpan;
        }

        /// <summary>
        /// Determines if two <see cref="MidiTimeSpan"/> objects are equal.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to compare.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to compare.</param>
        /// <returns>true if time spans are equal, false otherwise.</returns>
        public static bool operator ==(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            if (ReferenceEquals(timeSpan1, timeSpan2))
                return true;

            if (ReferenceEquals(null, timeSpan1) || ReferenceEquals(null, timeSpan2))
                return false;

            return timeSpan1.TimeSpan == timeSpan2.TimeSpan;
        }

        /// <summary>
        /// Determines if two <see cref="MidiTimeSpan"/> objects are not equal.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to compare.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to compare.</param>
        /// <returns>false if time spans are equal, true otherwise.</returns>
        public static bool operator !=(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            return !(timeSpan1 == timeSpan2);
        }

        /// <summary>
        /// Adds two specified <see cref="MidiTimeSpan"/> instances.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to add.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to add.</param>
        /// <returns>An object whose value is the sum of the values of <paramref name="timeSpan1"/> and
        /// <paramref name="timeSpan2"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpan1"/> is null. -or-
        /// <paramref name="timeSpan2"/> is null.</exception>
        public static MidiTimeSpan operator +(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan1), timeSpan1);
            ThrowIfArgument.IsNull(nameof(timeSpan2), timeSpan2);

            return new MidiTimeSpan(timeSpan1.TimeSpan + timeSpan2.TimeSpan);
        }

        /// <summary>
        /// Subtracts a specified <see cref="MidiTimeSpan"/> from another one.
        /// </summary>
        /// <param name="timeSpan1">The minuend.</param>
        /// <param name="timeSpan2">The subtrahend.</param>
        /// <returns>An object whose value is the result of the value of <paramref name="timeSpan1"/> minus
        /// the value of <paramref name="timeSpan2"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpan1"/> is null. -or-
        /// <paramref name="timeSpan2"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="timeSpan1"/> is less than <paramref name="timeSpan2"/>.</exception>
        public static MidiTimeSpan operator -(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan1), timeSpan1);
            ThrowIfArgument.IsNull(nameof(timeSpan2), timeSpan2);

            if (timeSpan1.TimeSpan < timeSpan2.TimeSpan)
                throw new ArgumentException("First time span is less than second one.", nameof(timeSpan1));

            return new MidiTimeSpan(timeSpan1.TimeSpan - timeSpan2.TimeSpan);
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MidiTimeSpan"/> is less than another one.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to compare.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to compare.</param>
        /// <returns>true if the value of <paramref name="timeSpan1"/> is less than the value of
        /// <paramref name="timeSpan2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpan1"/> is null. -or-
        /// <paramref name="timeSpan2"/> is null.</exception>
        public static bool operator <(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan1), timeSpan1);
            ThrowIfArgument.IsNull(nameof(timeSpan2), timeSpan2);

            return timeSpan1.TimeSpan < timeSpan2.TimeSpan;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MidiTimeSpan"/> is greater than another one.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to compare.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to compare.</param>
        /// <returns>true if the value of <paramref name="timeSpan1"/> is greater than the value of
        /// <paramref name="timeSpan2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpan1"/> is null. -or-
        /// <paramref name="timeSpan2"/> is null.</exception>
        public static bool operator >(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan1), timeSpan1);
            ThrowIfArgument.IsNull(nameof(timeSpan2), timeSpan2);

            return timeSpan1.TimeSpan > timeSpan2.TimeSpan;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MidiTimeSpan"/> is less than or equal to
        /// another one.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to compare.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to compare.</param>
        /// <returns>true if the value of <paramref name="timeSpan1"/> is less than or equal to the value of
        /// <paramref name="timeSpan2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpan1"/> is null. -or-
        /// <paramref name="timeSpan2"/> is null.</exception>
        public static bool operator <=(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan1), timeSpan1);
            ThrowIfArgument.IsNull(nameof(timeSpan2), timeSpan2);

            return timeSpan1.TimeSpan <= timeSpan2.TimeSpan;
        }

        /// <summary>
        /// Indicates whether a specified <see cref="MidiTimeSpan"/> is greater than or equal to
        /// another one.
        /// </summary>
        /// <param name="timeSpan1">The first <see cref="MidiTimeSpan"/> to compare.</param>
        /// <param name="timeSpan2">The second <see cref="MidiTimeSpan"/> to compare.</param>
        /// <returns>true if the value of <paramref name="timeSpan1"/> is greater than or equal to the value of
        /// <paramref name="timeSpan2"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="timeSpan1"/> is null. -or-
        /// <paramref name="timeSpan2"/> is null.</exception>
        public static bool operator >=(MidiTimeSpan timeSpan1, MidiTimeSpan timeSpan2)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan1), timeSpan1);
            ThrowIfArgument.IsNull(nameof(timeSpan2), timeSpan2);

            return timeSpan1.TimeSpan >= timeSpan2.TimeSpan;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return TimeSpan.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return this == (obj as MidiTimeSpan);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return TimeSpan.GetHashCode();
        }

        #endregion

        #region ITimeSpan

        /// <summary>
        /// Adds a time span to the current one.
        /// </summary>
        /// <remarks>
        /// If <paramref name="timeSpan"/> and the current time span have the same type,
        /// the result time span will be of this type too; otherwise - of the <see cref="MathTimeSpan"/>.
        /// </remarks>
        /// <param name="timeSpan">Time span to add to the current one.</param>
        /// <param name="mode">Mode of the operation that defines meaning of time spans the
        /// operation will be performed on.</param>
        /// <returns>Time span that is a sum of the <paramref name="timeSpan"/> and the
        /// current time span.</returns>
        /// <exception cref="ArgumentException"><paramref name="mode"/> is invalid.</exception>
        public ITimeSpan Add(ITimeSpan timeSpan, TimeSpanMode mode)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan), timeSpan);

            var midiTimeSpan = timeSpan as MidiTimeSpan;
            return midiTimeSpan != null
                ? this + midiTimeSpan
                : TimeSpanUtilities.Add(this, timeSpan, mode);
        }

        /// <summary>
        /// Subtracts a time span from the current one.
        /// </summary>
        /// <remarks>
        /// If <paramref name="timeSpan"/> and the current time span have the same type,
        /// the result time span will be of this type too; otherwise - of the <see cref="MathTimeSpan"/>.
        /// </remarks>
        /// <param name="timeSpan">Time span to subtract from the current one.</param>
        /// <param name="mode">Mode of the operation that defines meaning of time spans the
        /// operation will be performed on.</param>
        /// <returns>Time span that is a difference between the <paramref name="timeSpan"/> and the
        /// current time span.</returns>
        public ITimeSpan Subtract(ITimeSpan timeSpan, TimeSpanMode mode)
        {
            ThrowIfArgument.IsNull(nameof(timeSpan), timeSpan);

            var midiTimeSpan = timeSpan as MidiTimeSpan;
            return midiTimeSpan != null
                ? this - midiTimeSpan
                : TimeSpanUtilities.Subtract(this, timeSpan, mode);
        }

        /// <summary>
        /// Stretches the current time span by multiplying its length by the specified multiplier.
        /// </summary>
        /// <param name="multiplier">Multiplier to stretch the time span by.</param>
        /// <returns>Time span that is the current time span stretched by the <paramref name="multiplier"/>.</returns>
        public ITimeSpan Multiply(double multiplier)
        {
            ThrowIfArgument.IsNegative(nameof(multiplier), multiplier, "Multiplier is negative.");

            return new MidiTimeSpan((long)Math.Round(TimeSpan * multiplier));
        }

        /// <summary>
        /// Shrinks the current time span by dividing its length by the specified divisor.
        /// </summary>
        /// <param name="divisor">Divisor to shrink the time span by.</param>
        /// <returns>Time span that is the current time span shrinked by the <paramref name="divisor"/>.</returns>
        public ITimeSpan Divide(double divisor)
        {
            ThrowIfArgument.IsNonpositive(nameof(divisor), divisor, "Divisor is zero or negative.");

            return new MidiTimeSpan((long)Math.Round(TimeSpan / divisor));
        }

        /// <summary>
        /// Clones the current time span.
        /// </summary>
        /// <returns>Copy of the current time span.</returns>
        public ITimeSpan Clone()
        {
            return new MidiTimeSpan(TimeSpan);
        }

        #endregion
    }
}
