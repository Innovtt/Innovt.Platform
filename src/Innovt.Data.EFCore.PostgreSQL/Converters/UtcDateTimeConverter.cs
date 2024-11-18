using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Innovt.Data.EFCore.PostgreSQL.Converters;

public class UtcDateTimeConverter() : ValueConverter<DateTime, DateTime>(
    v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

public class UtcDateTimeOfConverter() : ValueConverter<DateTimeOffset, DateTimeOffset>(v =>
        v.DateTime.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(v.DateTime, DateTimeKind.Utc)
            : v.ToUniversalTime(),
    v => DateTime.SpecifyKind(v.DateTime, DateTimeKind.Utc));