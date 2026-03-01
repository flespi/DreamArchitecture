using CleanArchitecture.Domain.ValueObjects;
using HotChocolate.Language;

namespace CleanArchitecture.Graph.Schema.Shared.Scalars;

public class ColourType : ScalarType<Colour, StringValueNode>
{
    public ColourType() : base(nameof(Colour))
    {
    }

    public override IValueNode ParseResult(object? resultValue) => ParseValue(resultValue);

    protected override Colour ParseLiteral(StringValueNode valueSyntax) => new(valueSyntax.Value);

    protected override StringValueNode ParseValue(Colour runtimeValue) => new(runtimeValue);
}
