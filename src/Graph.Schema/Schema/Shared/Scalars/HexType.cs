using System.Text.Json;
using CleanArchitecture.Domain.Types;
using HotChocolate.Features;
using HotChocolate.Language;
using HotChocolate.Text.Json;

namespace CleanArchitecture.Graph.Schema.Shared.Scalars;

public class HexType : ScalarType<Hex, StringValueNode>
{
    public HexType() : base(nameof(Hex))
    {
    }

    protected override Hex OnCoerceInputLiteral(StringValueNode valueLiteral)
        => valueLiteral.Value;

    protected override Hex OnCoerceInputValue(JsonElement inputValue, IFeatureProvider context)
        => inputValue.GetString()!;

    protected override void OnCoerceOutputValue(Hex runtimeValue, ResultElement resultValue)
        => resultValue.SetStringValue((string)runtimeValue);

    protected override StringValueNode OnValueToLiteral(Hex runtimeValue)
        => new(runtimeValue.ToString());
}
