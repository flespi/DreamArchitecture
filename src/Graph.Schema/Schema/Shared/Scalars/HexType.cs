using CleanArchitecture.Domain.Types;
using HotChocolate.Language;

namespace CleanArchitecture.Graph.Schema.Shared.Scalars;

public class HexType : ScalarType<Hex, StringValueNode>
{
    public HexType() : base(nameof(Hex))
    {
    }

    public override IValueNode ParseResult(object? resultValue) => ParseValue(resultValue);

    protected override Hex ParseLiteral(StringValueNode valueSyntax) => valueSyntax.Value;

    protected override StringValueNode ParseValue(Hex runtimeValue) => new(runtimeValue.ToString());
}
